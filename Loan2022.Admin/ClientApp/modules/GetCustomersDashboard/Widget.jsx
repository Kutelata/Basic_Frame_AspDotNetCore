import React, {useEffect, useState} from 'react';
import {get} from "@front-end/utils";
import { UserAddOutlined ,TeamOutlined, UserOutlined } from '@ant-design/icons';

const GetCustomersDashboard = (props) => {
    const [customersForDashboard, setCustomersForDashboard] = useState({
        totalCustomer:  0,
        totalCustomersRegisteredToday: 0,
        totalCustomersAuthenticated : 0
    });
    useEffect(()=>{
        get('/customer/getCustomersForDashboard')
            .then(res => {
                setCustomersForDashboard(res.data);
            })
    },[]);
    return (
        <>
            <div className="col-md-3 col-sm-6 col-12">
                <div className="info-box">
                        <span className="info-box-icon bg-info">
                            <TeamOutlined />
                        </span>
                    <div className="info-box-content">
                        <span className="info-box-text">Tổng số người dùng</span>
                        <span className="info-box-number">{customersForDashboard.totalCustomer}</span>
                    </div>
                </div>
            </div>
            <div className="col-md-3 col-sm-6 col-12">
                <div className="info-box">
                        <span className="info-box-icon bg-success">
                           <UserAddOutlined />
                        </span>

                    <div className="info-box-content">
                        <span className="info-box-text">Người dùng đăng kí hôm nay</span>
                        <span className="info-box-number">{customersForDashboard.totalCustomersRegisteredToday}</span>
                    </div>
                </div>
            </div>
            <div className="col-md-3 col-sm-6 col-12">
                <div className="info-box">
                        <span className="info-box-icon bg-warning">
                            <UserOutlined />
                        </span>

                    <div className="info-box-content">
                        <span className="info-box-text">Người dùng đã xác thực hôm nay</span>
                        <span className="info-box-number">{customersForDashboard.totalCustomersAuthenticatedToday}</span>
                    </div>
                </div>
            </div>
            <div className="col-md-3 col-sm-6 col-12">
                <div className="info-box">
                        <span className="info-box-icon bg-warning">
                            <UserOutlined />
                        </span>

                    <div className="info-box-content">
                        <span className="info-box-text">Người dùng đã xác thực</span>
                        <span className="info-box-number">{customersForDashboard.totalCustomersAuthenticated}</span>
                    </div>
                </div>
            </div>
        </>
    )
}
export default GetCustomersDashboard;
