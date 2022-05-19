import React, {useEffect, useState} from 'react';
import {get, post} from '@front-end/utils';
import { message } from 'antd';
function ChangePasswordCustomer(props) {
    const {id} = props.data;
    const password = useFormInput('');
    const rePassword = useFormInput('');
    const [loading, setLoading] = useState(false);
    const onSave = () => {
        if(password.value !== rePassword.value){
            message.error('Vui lòng nhập đúng mật khẩu lại!');
        } else {
            console.log('password', password.value);
            console.log('password', rePassword.value);
            console.log('password', id);
            
            post('/customer/changePassword', {
                data: {
                    userId: id,
                    password: password.value,
                    rePassword: rePassword.value,
                }
            }).then(res => {
                setLoading(true);
                if(res){
                    message.success('Đổi mật khẩu thành công!');
                    onCancel();
                } else {
                    message.error('Đổi mật khẩu thất bại!');
                }
            })
        }
    }
    const onCancel = () => {
        window.location.href = '/customers';
    }

    return (
        <div className="card card-primary">
            <div className="card-header">
                <h3 className="card-title">Thay đổi mật khẩu</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="form-group">
                        <label htmlFor="password">Nhập mật khẩu</label>
                        <input type="password" {...password} name={'password'} id="password"
                               className="form-control" placeholder="Nhập mật khẩu"/>
                    </div>
                    <div className="form-group">
                        <label htmlFor="rePassword">Nhập lại mật khẩu</label>
                        <input type="password" {...rePassword} className="form-control"
                               id="rePassword" placeholder="Nhập lại mật khẩu"/>
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

export default ChangePasswordCustomer;