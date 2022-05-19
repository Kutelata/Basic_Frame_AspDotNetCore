import React, {useEffect, useState} from 'react';
import {get, post} from "@front-end/utils";
import moment from 'moment';
import {Modal, Button, message, Tag, Switch, InputNumber} from 'antd';
import NumberFormat from "react-number-format";
import {DefaultSettingName} from "@front-end/constants"
import PubSub from 'pubsub-js';
import {EditOutlined, SaveOutlined} from "@ant-design/icons";

const GetCustomerDetailContract = (props) => {
    const {id} = props.data;
    const [contract, setContract] = useState({});
    const [isContract, setIsContract] = useState(true);
    const [interests, setInterests] = useState([]);
    const [isModalContractVisible, setIsModalContractVisible] = useState(false);
    const [isModalRejectVisible, setIsModalRejectVisible] = useState(false);
    const [isModalEditContractVisible, setIsModalEditContractVisible] = useState(false);
    const reasonReject = useFormInput('');
    const reasonRejectId = useFormInput('1');

    const [customer, setCustomer] = useState({});
    const [templateContract, setTemplateContract] = useState('');
    const [companyName, setCompanyName] = useState('');
    const [signature, setSignature] = useState('');
    const [checkedWithdrawMoney, setCheckedWithdrawMoney] = useState(false);
    const [loading, setLoading] = useState(false);

    const [isEditAmountOfMoney, setIsEditAmountOfMoney] = useState(false);
    const [amountOfMoney, setAmountOfMoney] = useState(0);

    const [isEditInterest, setIsEditInterest] = useState(false);
    const interest = useFormInput({});


    useEffect(() => {
        getData();
        getAllInterest();
        getCustomerDetail();
        getTemplateContract();
        getCompanyName();
        getSignatureContract();
    }, []);
    const getCustomerDetail = () => {
        get('/customer/getCustomerForDetail', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                setCustomer(res.data);
            })
    }
    const getTemplateContract = () => {
        get('/setting/getSettingByName', {
                params: {
                    input: DefaultSettingName.TermsOfContract
                }
            }
        )
            .then(res => {
                if (res.data) {
                    setTemplateContract(res.data.value);
                }
            })
    }
    const getSignatureContract = () => {
        get('/customer/getSignatureContract', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                let path = `${process.env.MEDIA_DOMAIN}\\${res.data}`;
                console.log('getSignatureContract', path);
                setSignature(path);
            })
    }
    const getCompanyName = () => {
        get('/setting/getSettingByName', {
                params: {
                    input: DefaultSettingName.CompanyName
                }
            }
        )
            .then(res => {
                if (res.data) {
                    setCompanyName(res.data.value);
                }
            })
    }
    const getData = () => {
        get('/customer/getContractCustomer', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                const data = res.data;
                console.log('data', data)
                if (data == null || data === "") {
                    setIsContract(false);
                }
                if (data.amountOfMoney) {
                    setAmountOfMoney(data.amountOfMoney);
                }
                if (data.interestId) {
                    interest.onChange(data.interestId);
                }
                setCheckedWithdrawMoney(data.isWithdrawMoney)
                setContract(res.data);
            });
    }

    const getAllInterest = () => {
        get('/interest/getAllInterest'
        )
            .then(res => {
                setInterests(res.data);
            });
    }

    const onShowModalContract = () => {
        setIsModalContractVisible(true);
    }
    const handleOk = () => {
        setIsModalContractVisible(false);
    };

    const handleCancel = () => {
        setIsModalContractVisible(false);
    };

    //
    // public long ContractId { get; set; }
    // public long CustomerId { get; set; }
    // public Decimal AmountOfMoney { get; set; }
    const onApprove = () => {
        if (contract.status === 'Pending') {
            setLoading(true);
            contract.status = "Approve";
            post('/contract/approveContract', {
                    data: {
                        contractId:contract.id,
                        amountOfMoney:contract.amountOfMoney,
                        customerId: id
                    }
                }
            )
                .then(res => {
                    PubSub.publish('onApproveContract', true);
                    setLoading(false);
                });
            updateContract('Approved', '', contract.amountOfMoney, contract.interestId, contract.isWithdrawMoney);
        }
    }

    const onReject = () => {
        setIsModalRejectVisible(true);
    }
    const handleRejectOk = () => {
        let reason = "";
        if (reasonRejectId.value === "1") {
            reason = "Sai thông tin liên kết ví";
        } else if (reasonRejectId.value === "2") {
            reason = "Điểm tín dụng chưa đủ";
        } else {
            reason = reasonReject.value;
            reasonReject.onChange('');
            if (reason === null || reason === "") {
                message.error('Vui lòng nhập lý do từ chối!');
                return;
            }
        }
        updateContract(contract.status, reason, contract.amountOfMoney, contract.interestId, contract.isWithdrawMoney);
        setIsModalRejectVisible(false);
    };

    const handleRejectCancel = () => {
        reasonReject.onChange('');
        setIsModalRejectVisible(false);
    };
    const updateContract = (status, reason, amount, interestId, isWithdrawMoney) => {
        post('/contract/updateContract', {
            data: {
                id: contract.id,
                customerId: contract.customerId,
                contractCode: contract.contractCode,
                digitalSignature: contract.digitalSignature,
                createdOn: contract.createdOn,
                status: status,
                reason: reason,
                amountOfMoney: amount,
                interestId: interestId,
                isWithdrawMoney: isWithdrawMoney
            }
        }).then(res => {
            getData();
        })
    }


    const onUpdateContract = () => {
        setIsModalEditContractVisible(true);
    }
    const handleEditContractOk = () => {
        if (amountOfMoney.value == null || amountOfMoney.value === '') {
            message.error('Vui lòng nhập số tiền!');
        } else {
            updateContract(contract.status, contract.reason, amountOfMoney.value, interest.value, contract.isWithdrawMoney);
            setIsModalEditContractVisible(false);
        }
    };

    const handleEditContractCancel = () => {
        setIsModalEditContractVisible(false);
    };

    const onChangeWithdrawMoney = (checked) => {
        contract.isWithdrawMoney = checked;
        setCheckedWithdrawMoney(checked);
        post('/contract/updateContract', {
            data:
                {...contract}
        }).then(res => {
            message.info('Cập nhật trạng thái rút tiền thành công');
        })
    }
    const onChangeReason = (data) => {
        console.log('data', data.target.value, data);
    }

    const onClickEditAmountOfMoney = () => {
        setIsEditAmountOfMoney(true);
    }

    const onSaveAmountOfMoney = () => {
        contract.amountOfMoney = amountOfMoney;
        post('/contract/updateContract', {
            data:
                {...contract}
        }).then(res => {
            message.success('Cập nhật số tiền thành công');
            setIsEditAmountOfMoney(false);
        })
    }

    const onClickEditInterest = () => {
        setIsEditInterest(true);
    }

    const onSaveInterest = () => {
        contract.interestId = +interest.value;
        let interestName = (interests.find(x => x.id === contract.interestId))?.name;
        console.log('interest', interest.value, interestName)
        contract.interestName = interestName;
        post('/contract/updateContract', {
            data:
                {...contract}
        }).then(res => {
            message.success('Cập nhật thời hạn thành công');
            setIsEditInterest(false);
        })
        setIsEditInterest(false);
    }

    return (
        <div className="card h-100">
            <>
                {
                    isContract === true &&
                    <div className="card-body">
                        <div className="contract">
                            <div className="contract-left-item">Số tiền</div>
                            <div className="contract-right-item">
                                <>
                                    {
                                        isEditAmountOfMoney === false &&
                                        <div>
                                            <NumberFormat
                                                value={contract.amountOfMoney}
                                                displayType="text"
                                                thousandSeparator={true}
                                            /> VND
                                            <span className="icon-edit-account-number"
                                                  onClick={onClickEditAmountOfMoney}>
                                               <EditOutlined> </EditOutlined>
                                           </span>
                                        </div>
                                    }
                                    {
                                        isEditAmountOfMoney === true &&
                                        <div className="input-group mt-0">
                                            <InputNumber controls={false} style={{width: "80%"}} decimalSeparator=","
                                                         min="100000"
                                                         formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                                                         parser={value => value.replace(/\$\s?|(,*)/g, '')}
                                                         value={amountOfMoney}
                                                         onChange={(e) => {
                                                             setAmountOfMoney(e)
                                                         }}
                                            />
                                            
                                            <button type="button" className="btn btn-default btn-save-custom"
                                                    onClick={onSaveAmountOfMoney}>
                                                <SaveOutlined/>
                                            </button>
                                        </div>
                                    }
                                </>
                            </div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Trạng thái</div>
                            <div className="contract-right-item">
                                <>
                                    {
                                        contract.status === 'Pending' &&
                                        <Tag color='#f50' key={contract.status}>
                                            Chờ duyệt
                                        </Tag>
                                    }
                                    {
                                        contract.status === 'Approved' &&
                                        <Tag color='#108ee9' key={contract.status}>
                                            Đã duyệt
                                        </Tag>
                                    }
                                    {
                                        contract.status === 'Finished' &&
                                        <Tag color='#108ee9' key={contract.status}>
                                            Đã hoàn thành
                                        </Tag>
                                    }
                                    {
                                        contract.status === 'Reject' &&
                                        <Tag color='red' key={contract.status}>
                                            Bị từ chối
                                        </Tag>
                                    }
                                </>
                            </div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Thời hạn</div>
                            <div className="contract-right-item">
                                <>
                                    {
                                        isEditInterest === false &&
                                        <div>
                                            {contract.interestName}
                                            <span className="icon-edit-account-number"
                                                  onClick={onClickEditInterest}>
                                               <EditOutlined> </EditOutlined>
                                           </span>
                                        </div>
                                    }
                                    {
                                        isEditInterest === true &&
                                        <div className="input-group mt-0">
                                            <select name="interest" id="interest"  {...interest}
                                                    className="form-control">
                                                {
                                                    interests ? interests.map((item, index) =>
                                                        (
                                                            <option key={item.id} value={item.id}>{item.name}</option>
                                                        )) : null
                                                }
                                            </select>
                                            <button type="button" className="btn btn-default btn-save-custom"
                                                    onClick={onSaveInterest}>
                                                <SaveOutlined/>
                                            </button>
                                        </div>
                                    }
                                </>
                            </div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Mã hợp đồng</div>
                            <div className="contract-right-item">{contract.contractCode}</div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Thời gian tạo</div>
                            <div className="contract-right-item">
                                {moment(contract.createdOn).format("DD/MM/YYYY HH:mm")}
                            </div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Rút tiền</div>
                            <div className="contract-right-item">
                                <Switch checked={checkedWithdrawMoney} onChange={onChangeWithdrawMoney}/>
                            </div>
                        </div>
                        <div className="contract">
                            <div className="contract-left-item">Lý do từ chối</div>
                            <div className="contract-right-item"> {contract.reason}</div>
                        </div>
                        <div>
                            <div className="mt-3">
                                <button type="button" className="btn btn-primary m-2"
                                        onClick={() => onShowModalContract()}>Xem hợp đồng
                                </button>
                                <button type="button" className="btn btn-danger m-2" onClick={() => onReject()}>Từ chối
                                </button>
                                <button type="button" className="btn btn-primary m-2"
                                        onClick={() => onApprove()}
                                        value={loading ? 'Loading...' : 'Lưu'} disabled={loading}
                                >Cập nhật
                                </button>
                            </div>
                        </div>
                    </div>
                }
                {
                    isContract === false && <div className="card-body text-center">Hiện chưa có hợp đồng</div>
                }
            </>


            <Modal title="Từ chối hợp đồng" visible={isModalRejectVisible} onOk={handleRejectOk}
                   onCancel={handleRejectCancel}>
                <form>
                    <div className="form-group">
                        <label htmlFor="education">Lý do từ chối</label>
                        <select name="education" id="education" className="form-control" {...reasonRejectId}>
                            <option value="1">Sai thông tin liên kết ví</option>
                            <option value="2">Điểm tín dụng chưa đủ</option>
                            <option value="3">Lý do khác</option>
                        </select>
                    </div>
                    <>
                        {
                            reasonRejectId.value === "3" && <div className="form-group">
                                <label htmlFor="reason">Nhập lý do từ chối</label>
                                <textarea type="text" {...reasonReject} name={'reason'} id="reason"
                                          className="form-control" placeholder="Nhập lý do từ chối"/>
                            </div>
                        }
                    </>
                </form>
            </Modal>

            <Modal title="Chỉnh sửa" visible={isModalEditContractVisible} onOk={handleEditContractOk}
                   onCancel={handleEditContractCancel}>
                <form>
                    <div className="form-group">
                        <label htmlFor="amountOfMoney">Chỉnh sửa số tiền</label>
                        <input type="number" {...amountOfMoney} name={'amountOfMoney'} id="amountOfMoney"
                               className="form-control" placeholder="Nhập số tiền"/>
                    </div>

                    <div className="form-group">
                        <label htmlFor="interest">Chỉnh sửa số tháng</label>
                        <select name="interest" id="interest"  {...interest} className="form-control">
                            {
                                interests ? interests.map((item, index) =>
                                    (
                                        <option key={item.id} value={item.id}>{item.name}</option>
                                    )) : null
                            }
                        </select>
                    </div>
                </form>
            </Modal>

            <Modal title="Hợp đồng" visible={isModalContractVisible} onOk={handleOk} onCancel={handleCancel}
                   className='modal-large'>
                <div className='row contract'>
                    <div className="contract-header col-md-12 text-center">
                        <h5>Cộng hòa xã hội chủ nghĩa Việt Nam</h5>
                        <h5>Độc lập - Tự do - Hạnh phúc </h5>
                        <h5>--------------</h5>
                        <h5>ĐƠN VAY TIỀN</h5>
                    </div>

                    <div className="col-md-12 contract-content">
                        <p>Thông tin cơ bản về khoản vay</p>
                        <p className="mb-0">Bên A (Bên cho vay): <div className="display-inline-block"
                                                                      dangerouslySetInnerHTML={{__html: companyName}}/>
                        </p>
                        <p>Bên B (Bên vay) Ông/Bà: {customer.fullName} </p>
                        <p>Số CMNN : {customer.identityCard}</p>
                        <p>Ngày kí : {moment(contract.createdOn).format("DD/MM/YYYY HH:mm")}</p>
                        <p>Mã hợp đồng : {contract.contractCode}</p>
                        <p>Số tiền : <NumberFormat
                            value={contract.amountOfMoney}
                            displayType="text"
                            thousandSeparator={true}
                        /> VND
                        </p>
                        <p>Thời gian vay : {contract.interestName}</p>
                        <p>Lãi suất cho vay là {contract.percent} % mỗi tháng</p>
                        <div dangerouslySetInnerHTML={{__html: templateContract}}/>

                        <p className="mt-2">Người kí vay</p>
                        <img className="signature" src={signature} alt=""/>
                        <p className="mt-2">{customer.fullName}</p>
                    </div>


                </div>
            </Modal>
        </div>
    )
}

const useFormInput = initialValue => {
    const [value, setValue] = useState(initialValue);

    const handleChange = e => {
        if (e && e.target) {
            setValue(e.target.value);
        } else {
            setValue(e);
        }
    }
    return {
        value,
        onChange: handleChange,
    }
}

export default GetCustomerDetailContract;
