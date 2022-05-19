import React, {useState} from 'react';
import {post} from "@front-end/utils"
import { message } from 'antd';

function Login(props) {
    const username = useFormInput('');
    const password = useFormInput('');
    const [loading, setLoading] = useState(false);

    // handle button click of login form
    const handleLogin = (event) => {
        event.preventDefault();
        if(!username.value || !password.value){
            message.error('Vui lòng nhập đầy đủ thông tin!');
            return;     
        }
        setLoading(true);
        post("admin/login", {
                data:
                    {
                        userName: username.value,
                        password: password.value
                    }
            }
        )
            .then((res) => {
                setLoading(false);
                if (res.data) {
                    window.location.href = 'home';
                } else {
                    message.error('Đăng nhập thất bại, vui lòng đăng nhập lại!');
                    return;
                }
            }, (err) => {
                setLoading(false);
                toast.error("Lỗi")
            })
    }

    return (
        <div className="center-screen">
            <div className="login">
                <div className="title"><h3>Đăng nhập</h3></div>
                <form onSubmit={()=> handleLogin(event)}>
                    <div className="form-group">
                        <label htmlFor="username">Tên đăng nhập</label>
                        <input className="form-control" type="text" name="username"
                               placeholder="Nhập tên đăng nhập" {...username} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Mật khẩu</label>
                        <input className="form-control" type="password" name="password"
                               placeholder="Nhập mật khẩu" {...password} />
                    </div>
                    <button type="submit" className="button-login"
                            disabled={loading}>{loading ? 'Loading...' : 'Đăng nhập'}</button>
                </form>
            </div>
        </div>
    );
}

const useFormInput = initialValue => {
    const [value, setValue] = useState(initialValue);

    const handleChange = e => {
        setValue(e.target.value);
    }
    return {
        value,
        onChange: handleChange
    }
}

export default Login;