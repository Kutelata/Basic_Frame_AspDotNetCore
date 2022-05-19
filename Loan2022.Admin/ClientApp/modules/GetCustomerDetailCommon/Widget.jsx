import React, {useEffect, useState} from 'react';
import {get, post} from "@front-end/utils";
import moment from 'moment';
import {message, Tag} from "antd";
import {EditOutlined, SaveOutlined, SearchOutlined} from '@ant-design/icons';

const GetCustomerDetailCommon = (props) => {
    const {id} = props.data;
    const [customer, setCustomer] = useState({});
    const [isEditAccountNumber, setIsEditAccountNumber] = useState(false);
    const accountNumber = useFormInput('');
    useEffect(() => {
        getCustomerDetail();
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
                accountNumber.onChange(res.data.accountNumber);
            })
    }
    const onEditCustomer = () => {
        window.location.href = 'update/' + id;
    }

    const onVerifiedCustomer = () => {
        console.log('id', id)
        get('/customer/verifiedCustomer', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                getCustomerDetail();
            })
    }
    const getMarriageName = (id) => {
        switch (id) {
            case 1:
                return 'Độc thân';
            case 2:
                return 'Kết hôn';
            case 3:
                return 'Ly hôn';
        }
    }
    const geInComeName = (id) => {
        switch (id) {
            case 1:
                return 'Dưới 5.000.000';
            case 2:
                return 'Từ 5.000.000 - 10.000.000';
            case 3:
                return 'Từ 10.000.000 - 30.000.000';
            case 4:
                return 'Trên 30.000.000';
            default:
                return 'Chưa nhập thu nhập';
        }
    }
    const getEducationName = (id) => {
        switch (id) {
            case 1:
                return 'Trung học cơ sở';
            case 2:
                return 'Trung học phổ thông';
            case 3:
                return 'Trung cấp';
            case 4:
                return 'Cao đẳng';
            case 5:
                return 'Đại học';
            case 6:
                return 'Sau đại học';
            default:
                return 'Trung học phổ thông';
        }
    }
    const getAvatar = (input) => {
        return `${process.env.MEDIA_DOMAIN}\\${input}`;
    }
    const onClickEditAccountNumber = () => {
        setIsEditAccountNumber(true);
    }  
    
    const onSaveAccountNumber = () => {
        customer.accountNumber = accountNumber.value;
        console.log('customer', customer)
        post('/customer/updateCustomer', {
            data: {
                ...customer
            }
        }).then(res => {
            message.success("Cập nhật số tài khoản thành công");
            setIsEditAccountNumber(false);
        })
    }
    return (
        <div className="card h-100">
            <div className="card-body">
                <div className="row">
                    <div className="col-md-3 pt-5 text-center">
                        <p>
                            {
                                customer.status === "Unverified" &&
                                <Tag color='red' key={customer.status}>
                                    Chưa xác minh
                                </Tag>
                            }
                            {
                                customer.status === 'Verified' &&
                                <Tag color='blue' key={customer.status}>
                                    Đã xác minh
                                </Tag>
                            }
                        </p>
                        <>
                            {
                                customer.avatar &&
                                <img src={getAvatar(customer.avatar)} className="img-circle medias-avatar elevation-2"
                                     alt="No Image">
                                </img>
                            }

                            {
                                !customer.avatar &&
                                <img src='\default-profile-picture.png' className="img-circle medias-avatar elevation-2"
                                     alt="No Image">
                                </img>
                            }
                        </>

                        <p className="font-weight-bolder mb-0 mt-3">{customer.phoneNumber}</p>
                        <p className="text-center font-weight-bolder">{customer.fullName}</p>
                    </div>
                    <div className="col-md-5 mt-3">
                        <div className="row">
                            <div className="col-4"> Số CMND/CCCD</div>
                            <div className="col-8"> {customer.identityCard}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Địa chỉ</div>
                            <div className="col-8">{customer.address}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Ngày sinh</div>
                            <div className="col-8">
                                <>
                                    {
                                        customer.dateOfBirth && <span>{moment(customer.dateOfBirth).format("DD/MM/YYYY")}</span> 
                                    }
                                </>
                            </div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Nghề nghiệp</div>
                            <div className="col-8">{customer.job}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">TT hôn nhân</div>
                            <div className="col-8">{getMarriageName(customer.marriage)}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Số điện thoại</div>
                            <div className="col-8"> {customer.phoneNumber}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Thu nhập</div>
                            <div className="col-8">{geInComeName(customer.income)}</div>
                        </div>

                        <div className="row mt-2">
                            <div className="col-4">Học vấn</div>
                            <div className="col-8">{getEducationName(customer.education)}</div>
                        </div>

                    </div>
                    <div className="col-md-4 mt-3">
                        <h5>Thông tin người hưởng thụ</h5>
                        <div className="row">
                            <div className="col-4">Ngân hàng</div>
                            <div className="col-8">{customer.bankName}</div>
                        </div>
                        <div className="row mt-2">
                            <div className="col-4">Số tài khoản</div>
                            <div className="col-8">
                                <>
                                    {
                                        isEditAccountNumber === false &&
                                        <div>{customer.accountNumber}
                                            <span className="icon-edit-account-number"
                                                  onClick={onClickEditAccountNumber}>
                                               <EditOutlined> </EditOutlined>
                                           </span>
                                        </div>
                                    }
                                    {
                                        isEditAccountNumber === true &&
                                        <div className="input-group mt-0">
                                            <input type="text" name="table_search" className="form-control float-right" {...accountNumber}/>
                                            <button type="button" className="btn btn-default btn-save-custom" onClick={onSaveAccountNumber}>
                                                <SaveOutlined />
                                            </button>
                                        </div>

                                    }
                                </>

                            </div>
                        </div>
                        <div className="row mt-2">
                            <div className="col-4">Tên người hưởng thụ</div>
                            <div className="col-8">{customer.beneficiaryOfName}</div>
                        </div>
                    </div>
                </div>
                <div className="col-md-12">
                    <button type="button" className="btn btn-primary float-right" onClick={() => onEditCustomer()}>Chỉnh
                        sửa
                    </button>
                    <>
                        {
                            customer.status === "Unverified" &&
                            <button type="button" className="btn btn-primary float-right mr-2"
                                    onClick={() => onVerifiedCustomer()}>Xác minh
                            </button>
                        }
                    </>
                </div>
            </div>
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
export default GetCustomerDetailCommon;
