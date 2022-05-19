import React, {Fragment, useEffect, useState} from 'react';
import {Spin} from 'antd';
import {post} from "@front-end/utils"
import {useAppContext} from "@front-end/hooks";
import toast, {Toaster} from 'react-hot-toast';
import './style.scss'
import viewName from "./viewName";
import $ from 'jquery'
import logo from '../../assets/logo.jpg'

const Login = (props) => {
    const {data} = props;
    const {data: step, setData: setStep} = useAppContext('login-step');
    const [phone, setPhone] = useState('');
    const [password, setPassword] = useState('');
    const [rePassword, setRePassword] = useState('');
    const [lastScreen, setLastScreen] = useState('');
    const [loading, setLoading] = useState(false);
    const checkPhone = (input) => {
        const re = /^[0-9\b]{0,10}$/;
        return (input === '' || re.test(input))
    }

    const handleSubmit = function (event) {
        event.preventDefault();
        if (loading) return;
        const re = /(([03+[2-9]|05+[6|8|9]|07+[0|6|7|8|9]|08+[1-9]|09+[1-4|6-9]]){3})+[0-9]{7}\b/g;
        if (!checkPhone(phone)) {
            toast.error("Vui lòng nhập đúng số điện thoại!")
            return
        }else if(phone.length <10){
            toast.error("Số điện thoại phải đủ 10 số")
            return
        }
        setLoading(true);
        post("/api/check-phone", {data: {userName: phone}}).then((res) => {
            setLoading(false);
            if (res.data) {
                setLastScreen(step);
                setStep(viewName.LoginPassword)
            } else {
                setLastScreen(step);
                setStep(viewName.RegisterPassword)
            }
        }, (err) => {
            setLoading(false)
            console.log(err)
            toast.error("Lỗi")
        })
    }

    const handleLogin = (e) => {
        e.preventDefault()
        if (loading) return;
        setLoading(true);
        post("/api/login-by-phone", {data: {userName: phone, password: password}}).then((res) => {
            setLoading(false)
            if (res.data) {
                window.location.href = "/borrow"
            } else {
                toast.error("Sai mật khẩu");
            }

        }, (err) => {
            setLoading(false)
            console.log(err)
            toast.error("Có lỗi xảy ra, vui lòng thử lại")
        })
    }

    const handleRegister = (e) => {
        e.preventDefault()
        if (loading) return;
        setLoading(true);
        post("/api/register", {data: {userName: phone, password: password, rePassword: rePassword}}).then((res) => {
            setLoading(false)
            if (res.data) {
                window.location.href = "/borrow"
            } else {
                toast.error("Đăng ký thất bại");
            }

        }, (err) => {
            setLoading(false)
            console.log(err)
            toast.error("Có lỗi xảy ra, vui lòng thử lại")
        })
    }

    const onChangePhoneInput = (e) => {
        const re = /^[0-9\b]{0,11}$/;
        if (checkPhone(e.target.value)) {
            setPhone(e.target.value)
        }
    }

    const onChangePasswordInput = (e) => {
        setPassword(e.target.value)
    }

    const onChangeRePasswordInput = (e) => {
        setRePassword(e.target.value)
    }

    useEffect(() => {
        setStep(viewName.PhoneInputScreen);
        $('.loadingbar').delay(1500).animate({left: '0'}, 3000);
        $('.loadingBox').delay(500).animate({opacity: '1'}, 1000);
        $('.splashScreen').delay(4500).animate({top: '-100%'}, 300);
        $('.loadingCircle').delay(4500).animate({opacity: '0'}, 500);
        $('body').delay(5000).queue(function(){
            $('body').addClass("visibleSplash");
        });
    }, [])

    const getTitle = (view) => {
        let title = "";
        switch (view) {
            case viewName.PhoneInputScreen:
                title = 'Nhập số điện thoại'
                break;
            case viewName.LoginPassword:
                title = 'Đăng nhập'
                break;
            case viewName.RegisterPassword:
                title = 'Đăng ký'
                break;
        }
        return (<>{title}</>);
    }

    return (<>
        <div className="splashBody">
            <div className="splashScreen">
                <div className="loadingContainer">

                    <div className="loadingBox">
                        <img
                            src={logo}
                            className="splashLogo"/>
                            <div className="loadingBarContainer">
                                <div className="loadingbar"></div>
                            </div>
                    </div>
                </div>
            </div>
        </div>

        <Spin spinning={loading} style={{height: '50%'}}>
                <div className="center-screen">
                    <nav className="top-nav">
                        {
                            step != viewName.PhoneInputScreen ? (
                                <i className="l-icon icon-arrow-left icon-2x" onClick={() => {
                                    setStep(lastScreen)
                                }}></i>) : <></>
                        }
                        <div>
                            <h2>{getTitle(step)}</h2>
                        </div>
                    </nav>
                    <Toaster
                        position="top-center"
                        reverseOrder={false}
                    />
                    {step === viewName.PhoneInputScreen ? (
                        <section>
                            <div className="logo">
                                <img src={logo} alt=""/>
                            </div>
                            <div className="title"><h3>Nhập số điện thoại để tiến hành đăng ký</h3></div>
                            <form action="" onSubmit={handleSubmit} id="form_phone">
                                <div className="form-group">
                                    <i className="region-code">+84</i>
                                    <input className="form-control" type="text" name="phone"
                                           placeholder="Nhập số điện thoại"
                                           value={phone} onChange={onChangePhoneInput.bind(this)}/>
                                </div>
                                <button type="submit">Tiếp theo</button>
                            </form>
                        </section>) : null}
                    {step === viewName.LoginPassword ? (
                        <section>
                            <div className="logo">
                                <img src={logo} alt=""/>
                            </div>
                            <div className="title"><h3>Chào mừng bạn quay trở lại</h3></div>
                            <form action="" onSubmit={handleLogin} id="form_password">
                                <div className="form-group">
                                    <input className="form-control" value={password}
                                           onChange={onChangePasswordInput.bind(this)}
                                           type="password" name="password" placeholder="Nhập mật khẩu"/>
                                </div>
                                {/*<div className={'form-group'}>*/}
                                {/*    <input className="form-control" type="password" name="confirm" placeholder="Nhập lại mật khẩu"/>*/}
                                {/*</div>*/}
                                <button type="submit">Đăng nhập</button>
                            </form>
                        </section>) : null}
                    {step === viewName.RegisterPassword ? (
                        <section className="w-100">
                            <div className="logo">
                                <img src={logo} alt=""/>
                            </div>
                            <form action="" onSubmit={handleRegister} id="form_register">
                                <div className="form-group">
                                    <input className="form-control" type="password" name="password"
                                           placeholder="Nhập mật khẩu" onChange={onChangePasswordInput.bind(this)}/>
                                </div>
                                <div className={'form-group'}>
                                    <input className="form-control" type="password" name="confirm"
                                           onChange={onChangeRePasswordInput.bind(this)}
                                           placeholder="Nhập lại mật khẩu"/>
                                </div>
                                <button type="submit">Đăng ký</button>
                            </form>
                            <div>
                                <div><strong>Chú ý</strong></div>
                                <p>Độ dài mật khẩu từ 6-20 ký tự</p>
                                <div><strong>Ví dụ:</strong></div>
                                <p>Mật khẩu: 123456</p>
                                <p>Nhập lại mật khẩu: 123456</p>
                            </div>
                        </section>) : null}
                </div>
            </Spin>
        </>

    )
}

export default Login