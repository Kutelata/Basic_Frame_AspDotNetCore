import React, {useEffect, useState} from 'react';
import {get, post} from '@front-end/utils';

function CreateOrUpdateEmployee(props) {
    const {id} = props.data;
    // const [fullName,setFullName] = useState(useFormInput(''));
    const fullName = useFormInput('');
    const phoneNumber = useFormInput('');
    const status = useFormInput('Active');
    const chatId = useFormInput('');
    const email = useFormInput('');
    const [loading, setLoading] = useState(false);
    const [employee, setEmployee] = useState({});
    const onSave = () => {
        let id = 0;
        let userId = '';
        if (employee.id) {
            id = employee.id
            userId = employee.userId;
        }
        
        console.log('chatId.value', chatId.value);
        
        post('/employee/createOrUpdate', {
            data: {
                id: id,
                userId: userId,
                fullName: fullName.value,
                phoneNumber: phoneNumber.value,
                status: status.value,
                chatId: chatId.value,
                email: email.value,
            }
        }).then(res => {
            setLoading(true);
            onCancel();
        })
    }
    useEffect(() => {
        if (id) {
            get('/employee/getEmployee', {
                params: {
                    id: id
                }
            }).then(res => {
                const emp = res.data;
                fullName.onChange(emp.fullName);
                phoneNumber.onChange(emp.phoneNumber);
                status.onChange(emp.status);
                chatId.onChange(emp.chatId);
                setEmployee(emp);
            })
        }
    }, []);
    const onCancel = () => {
        window.location.href = '/employees';
    }

    return (
        <div className="card card-primary">
            <div className="card-header">
                <h3 className="card-title">Thêm/Sửa nhân viên</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="form-group">
                        <label htmlFor="fullName">Họ tên</label>
                        <input type="text" {...fullName} name={'fullName'} id="fullName"
                               className="form-control" placeholder="Nhập họ tên"/>
                    </div>
                    {/*<div className="form-group">*/}
                    {/*    <label htmlFor="phoneNumber">Số điện thoại</label>*/}
                    {/*    <input type="text" {...phoneNumber} className="form-control" id="phoneNumber"*/}
                    {/*           placeholder="Nhập số điện thoại"/>*/}
                    {/*</div>   */}
                    {/*<div className="form-group">*/}
                    {/*    <label htmlFor="Email">Email</label>*/}
                    {/*    <input type="text" {...email} className="form-control" id="email"*/}
                    {/*           placeholder="Nhập email"/>*/}
                    {/*</div>*/}
                    <div className="form-group">
                        <label htmlFor="chatId">Chat ID</label>
                        <input type="text" {...chatId} className="form-control" id="chatId" placeholder="Nhập mã chat"/>
                    </div>
                    <div className="form-group">
                        <label htmlFor="exampleSelectBorder">Trạng thái hoạt động</label>
                        <select className="custom-select" id="exampleSelectBorder" {...status}>
                            <option value="Active">Đang hoạt động</option>
                            <option value="InActive">Khóa</option>
                        </select>
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
    );
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

export default CreateOrUpdateEmployee;