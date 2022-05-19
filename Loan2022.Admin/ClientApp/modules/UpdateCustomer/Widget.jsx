import React, {useEffect, useState} from 'react';
import {get, post} from "@front-end/utils";
import {DatePicker, Tag} from "antd";
import moment from 'moment';
import 'moment/locale/zh-cn';
const UpdateCustomer = (props) => {
    const {id} = props.data;
    const [customer, setCustomer] = useState({});
    const [banks, setBanks] = useState([]);
    const fullName = useFormInput('');
    const job = useFormInput('');
    const identityCard = useFormInput('');
    const phoneNumber = useFormInput('');
    const address = useFormInput('');
    const education = useFormInput('');
    const marriage = useFormInput('');
    const income = useFormInput('');
    const accountNumber = useFormInput('');
    const bankId = useFormInput('');
    const beneficiaryOfName = useFormInput('');
    const status = useFormInput('');
    const totalMoney = useFormInput('');
    const [loading, setLoading] = useState(false);
    const [dateOfBirth, setDateOfBirth] = useState(moment(new Date()));
    
    const getAllBank = () => {
        get('/bank/getAllBank'
        )
            .then(res => {
                console.log('setBanks',res)
                setBanks(res.data);
            });
    }

    useEffect(() => {
        getCustomerDetail();
        getAllBank();
    }, []);

    const getCustomerDetail = () => {
        get('/customer/getCustomerForDetail', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                let cust = res.data;
                if(cust.dateOfBirth){
                    setDateOfBirth(moment(cust.dateOfBirth));
                } else{
                    setDateOfBirth(null);
                }
                setCustomer(res.data);
                fullName.onChange(cust.fullName);
                job.onChange(cust.job);
                identityCard.onChange(cust.identityCard);
                phoneNumber.onChange(cust.phoneNumber);
                address.onChange(cust.address);
                education.onChange(cust.education);
                marriage.onChange(cust.marriage);
                income.onChange(cust.income);
                accountNumber.onChange(cust.accountNumber);
                bankId.onChange(cust.bankId);
                beneficiaryOfName.onChange(cust.beneficiaryOfName);
                totalMoney.onChange(cust.totalMoney);
                status.onChange(cust.status);
            })
    }

    const onSave = () => {
        setLoading(true);
        post('/customer/updateCustomer', {
            data: {
                id: id,
                fullName: fullName.value,
                dateOfBirth: moment(dateOfBirth),
                job: job.value,
                identityCard: identityCard.value,
                phoneNumber: phoneNumber.value,
                address: address.value,
                education: education.value,
                marriage: marriage.value,
                income: income.value,
                accountNumber: accountNumber.value,
                bankId: bankId.value,
                beneficiaryOfName: beneficiaryOfName.value,
                totalMoney: totalMoney.value,
                status: status.value,
                userId : customer.userId,
                contractId: customer.contractId
            }
        }).then(res => {
            setLoading(false);
            onCancel();
        })
    }
    const onCancel = () => {
        window.location.href = '/customers/' + id;
    }

    const onChangeDateOfBirth =(event) => {
        setDateOfBirth(event);
    }
    return (
        <div className="card h-100">
            <div className="card-header">
                <h3 className="card-title">Chỉnh sửa thông tin khách hàng</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="row">
                        <div className="col-md-4">
                            <div className="form-group">
                                <label htmlFor="fullName">Họ và tên</label>
                                <input type="text" {...fullName} name={'fullName'} id="fullName"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="phoneNumber">Số điện thoại</label>
                                <input type="text" {...phoneNumber} name={'phoneNumber'} id="fullName"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="identityCard">Số CMND</label>
                                <input type="text" {...identityCard} name={'identityCard'} id="identityCard"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="address">Địa chỉ</label>
                                <input type="text" {...address} name={'address'} id="address"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="dateOfBirth">Ngày sinh</label>
                                <DatePicker  
                                            format="DD/MM/YYYY"
                                            {...dateOfBirth}
                                            value={dateOfBirth}
                                            placeholder="Chọn ngày sinh"
                                            style={{width: '100%'}}
                                            onChange={onChangeDateOfBirth}
                                            className="form-control"/>
                            </div>

                        </div>
                        <div className="col-md-4">
                            <div className="form-group">
                                <label htmlFor="job">Nghề nghiệp</label>
                                <input type="text" {...job} name={'job'} id="job"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="marriage">Tình trạng hôn nhân</label>
                                <select name="marriage" id="marriage" {...marriage} className="form-control">
                                    <option value="1">Độc thân</option>
                                    <option value="2">Kết hôn</option>
                                    <option value="3">Ly hôn</option>
                                </select>
                            </div>

                            <div className="form-group">
                                <label htmlFor="income">Thu nhập</label>
                                <select name="income" id="income" {...income} className="form-control">
                                    <option value="1">Dưới 5.000.000</option>
                                    <option value="2">Từ 5.000.000 - 10.000.000</option>
                                    <option value="3">Từ 10.000.000 - 30.000.000</option>
                                    <option value="4">Trên 30.000.000</option>
                                </select>
                            </div>       
                            
                            <div className="form-group">
                                <label htmlFor="education">Học vấn</label>
                                <select name="education" id="education" {...education} className="form-control">
                                    <option value="1">Trung học cơ sở</option>
                                    <option value="2">Trung học phổ thông</option>
                                    <option value="3">Trung cấp</option>
                                    <option value="4">Cao đẳng</option>
                                    <option value="5">Đại học</option>
                                    <option value="6">Sau đại học</option>
                                </select>
                            </div>
                        </div>
                        <div className="col-md-4">

                            <div className="form-group">
                                <label htmlFor="fullName">Ngân hàng</label>
                                <select name="interest" id="interest"  {...bankId}  className="form-control">
                                    {
                                        banks ? banks.map((item, index) =>
                                            (
                                                <option value={item.id}>{item.bankName}</option>
                                            )): null
                                    }
                                </select>
                            </div>

                            <div className="form-group">
                                <label htmlFor="accountNumber">Số tài khoản</label>
                                <input type="text" {...accountNumber} name={'accountNumber'} id="accountNumber"
                                       className="form-control"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="beneficiaryOfName">Tên người hưởng thụ</label>
                                <input type="text" {...beneficiaryOfName} name={'beneficiaryOfName'} id="beneficiaryOfName"
                                       className="form-control"/>

                            </div>
                        </div>
                    </div>

                </div>
                <div className="card-footer">
                    <button type="button" className="btn btn-primary float-right"
                            value={loading ? 'Loading...' : 'Login'} onClick={() => onSave()} disabled={loading}>Lưu
                    </button>
                    <button type="button" className="btn btn-default float-right mr-2" onClick={() => onCancel()}>Hủy
                        bỏ
                    </button>
                </div>
            </form>
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

export default UpdateCustomer;
