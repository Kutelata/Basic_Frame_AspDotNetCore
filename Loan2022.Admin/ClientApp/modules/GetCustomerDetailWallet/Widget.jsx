import React, {useEffect, useState} from 'react';
import {get, post} from "@front-end/utils";
import {Modal, message, InputNumber} from 'antd';
import NumberFormat from "react-number-format";
import PubSub from 'pubsub-js';

const GetCustomerDetailWallet = (props) => {
    const {id} = props.data;
    const [totalMoney, setTotalMoney] = useState(0);
    const [history, setHistory] = useState(null);

    const reasonPlusWallet = useFormInput('Thay đổi số dư');
    const reasonMinusWallet = useFormInput('Thay đổi số dư');
    const [isModalPlusWalletVisible, setIsModalPlusWalletModalVisible] = useState(false);
    const [isModalMinusWalletVisible, setIsModalMinusWalletVisible] = useState(false);


    const [amountPlusWallet, setAmountPlusWallet] = useState(100000);
    const [amountMinusWallet, setAmountMinusWallet] = useState(100000);

    PubSub.subscribe('onApproveContract', () => {
        getTotalMoney(id);
        getHistory(id);
    });

    useEffect(() => {
        getTotalMoney(id);
        getHistory(id);
    }, []);

    const getTotalMoney = (id) => {
        get('/customer/getTotalMoneyCustomer', {
            params: {
                id: id
            }
        }).then(res => {
            setTotalMoney(res.data);
        })
    }
    const getHistory = (id) => {
        get('/customer/getCustomerWalletHistoryByCustomerId', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                setHistory(res.data);
            })
    }


    const onPlusWallet = () => {
        setIsModalPlusWalletModalVisible(true);
    }

    const handleOkPlusWallet = () => {
        post('/customer/AddCustomerWalletHistory', {
                data: {
                    name: "Cộng tiền",
                    type: "Plus",
                    amount: amountPlusWallet,
                    description: reasonPlusWallet.value,
                    customerId: id
                }
            }
        )
            .then(res => {
                if(res.data){
                    setAmountPlusWallet(100000);
                    reasonPlusWallet.onChange('Thay đổi số dư');
                    setIsModalPlusWalletModalVisible(false);
                    getTotalMoney(id);
                    getHistory(id); 
                }else {
                    message.error('Lỗi, Vui lòng thử lại!');
                }
            });
    };

    const handleCancelPlusWallet = () => {
        setIsModalPlusWalletModalVisible(false);
    };

    const onMinusWallet = () => {
        setIsModalMinusWalletVisible(true);
    }

    const handleOkMinusWallet = () => {
        post('/customer/AddCustomerWalletHistory', {
                data: {
                    name: "Trừ tiền",
                    type: "Minus",
                    amount: amountMinusWallet,
                    description: reasonMinusWallet.value,
                    customerId: id
                }
            }
        )
            .then(res => {
                if (res.data) {
                    setAmountMinusWallet(100000);
                    reasonMinusWallet.onChange('Thay đổi số dư');
                    setIsModalMinusWalletVisible(false);
                    getTotalMoney(id);
                    getHistory(id);
                } else {
                    message.error('Số tiền hiện tại trong tài khoản nhỏ hơn số tiền trừ ví!');
                }

            });
    };

    const handleCancelMinusWallet = () => {
        setIsModalMinusWalletVisible(false);
    };

    return (
        <>
            <div className="card h-100">
                <div className="card-body">
                    <p className="text-center">Ví người dùng</p>
                    <div>
                        <div>Số dư</div>
                        <div className='text-center'>
                            <NumberFormat
                                value={totalMoney}
                                displayType="text"
                                thousandSeparator={true}
                            /> VND
                        </div>
                    </div>
                    <div className='change-wallet'>
                        <button type="button" className="btn btn-default float-left" onClick={() => onMinusWallet()}>Trừ
                            ví
                        </button>
                        <button type="button" className="btn btn-default float-right"
                                onClick={() => onPlusWallet()}>Cộng ví
                        </button>
                    </div>
                    <div className="wallet-history">
                        <div>Lịch sử ví</div>
                        {
                            history ? history.map((item, index) =>
                                (
                                    <div className='history' key={item}>
                                        <div className='history-item'>{item.description}</div>
                                        <div className='history-item text-right'>
                                            {
                                                item.type === "Plus" ? "+" : "-"
                                            }
                                            <NumberFormat
                                                value={item.amount}
                                                displayType="text"
                                                thousandSeparator={true}
                                            /> VND
                                        </div>
                                    </div>
                                )) : null
                        }
                    </div>
                </div>
            </div>


            <Modal title='Nhập số tiền cộng ví' visible={isModalPlusWalletVisible} onOk={handleOkPlusWallet}
                   onCancel={handleCancelPlusWallet}>
                <form>
                    <div className="form-group">
                        <label htmlFor="amountPlusWallet">Nhập số tiền cộng</label>
                        <InputNumber controls={false} style={{width: "100%"}} decimalSeparator=","
                                     min="100000" addonAfter={<>VNĐ</>}
                                     formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                                     parser={value => value.replace(/\$\s?|(,*)/g, '')}
                                     value={amountPlusWallet}
                                     onChange={(e) => {
                                         setAmountPlusWallet(e)
                                     }}
                        />
                        <span>Nhập số tiền tối thiểu là 100,000</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="reason">Lý do</label>
                        <input type="text" {...reasonPlusWallet} className="form-control" id="reason"
                               placeholder="Nhập lý do"/>
                    </div>
                </form>
            </Modal>

            <Modal title='Nhập số tiền trừ ví' visible={isModalMinusWalletVisible} onOk={handleOkMinusWallet}
                   onCancel={handleCancelMinusWallet}>
                <form>
                    <div className="form-group">
                        <label htmlFor="amountPlusWallet">Nhập số tiền trừ ví</label>
                        <InputNumber controls={false} style={{width: "100%"}} decimalSeparator=","
                                     min="100000" addonAfter={<>VNĐ</>}
                                     formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                                     parser={value => value.replace(/\$\s?|(,*)/g, '')}
                                     value={amountMinusWallet}
                                     onChange={(e) => {
                                         setAmountMinusWallet(e)
                                     }}
                        />
                        <span>Nhập số tiền tối thiểu là 100,000</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="reason">Lý do</label>
                        <input type="text" {...reasonMinusWallet} className="form-control" id="reason"
                               placeholder="Nhập lý do"/>
                    </div>
                </form>
            </Modal>
        </>
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
export default GetCustomerDetailWallet;
