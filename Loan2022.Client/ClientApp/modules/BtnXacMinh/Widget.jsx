import {get} from "@front-end/utils"
import React, {Fragment, useEffect, useState} from 'react';
import {useAppContext} from '@front-end/hooks';
import {formatter} from '@front-end/utils';
import {message, Spin, Modal, Button, Row, Col} from 'antd';
import moment from "moment";

const Widget = (props) => {
    const [modalFalse, setModalFalse] = useState(false)
    const {data: globalData} = useAppContext("global");
    const [modalHistory, setModalHistory] = useState(false)

    const data = () => {
        const e = globalData.interest;
        if (!e) return []
        return cal(globalData.contract.AmountOfMoney, e.NumberOfMonth, e.Percent, globalData.contract.CreatedOn)
    }

    const cal = (mn, month, rate, date) => {
        const months = [];
        const m = mn / month
        for (let i = 0; i < month; i++) {
            const interest = m + (mn / 100 * rate);
            // const interestRound = (Math.round(interest/1000)* 1000);
            months.push({ky: i + 1, m: interest, mt: moment(date).add(i+1, 'months')})
            mn -= m;
        }
        return months;
    }
    
    
    return (<>
        <Modal title="Thông tin hồ sơ" visible={modalFalse} onCancel={() => {
            setModalFalse(false)
        }} footer={[]}>
            <table style={{fontFamily: 'arial, sans-serif', borderCollapse: 'collapse', width: '100%'}}>
                <tr>
                    <td><strong>Họ tên</strong></td>
                    <td>{globalData.customer ? globalData.customer.FullName : ""}</td>
                </tr>
                <tr>
                    <td><strong>Ngày sinh</strong></td>
                    <td>{globalData.customer ? moment(globalData.customer.DateOfBirth).format("DD/MM/YYYY") : ""}</td>
                </tr>
                <tr>
                    <td><strong>Nghề nghiệp</strong></td>
                    <td>{globalData.customer ? globalData.customer.Job : ""}</td>
                </tr>
                <tr>
                    <td><strong>Số CMND / CCCD</strong></td>
                    <td>{globalData.customer ? globalData.customer.IdentityCard : ""}</td>
                </tr>
                <tr>
                    <td><strong>Số điện thoại</strong></td>
                    <td>{globalData.customer ? globalData.customer.PhoneNumber : ""}</td>
                </tr>
                <tr>
                    <td><strong>Địa chỉ</strong></td>
                    <td>{globalData.customer ? globalData.customer.Address : ""}</td>
                </tr>
                <tr>
                    <td><strong>Tên chủ thẻ</strong></td>
                    <td>{globalData.customer ? globalData.customer.BeneficiaryOfName : ""}</td>
                </tr>

                <tr>
                    <td><strong>Tên ngân hàng</strong></td>
                    <td>{globalData.bankName ? globalData.bankName : ""}</td>
                </tr>

                <tr>
                    <td><strong>Số tài khoản ngân hàng</strong></td>
                    <td>{globalData.customer ? globalData.customer.AccountNumber : ""}</td>
                </tr>

            </table>
        </Modal>
        <Modal title="Chi tiết trả nợ" className="history-modal" footer={[]} visible={modalHistory} onCancel={() => {
            setModalHistory(false)
        }}>
            <table>
                <tr>
                    <th>Kỳ</th>
                    <th>Số tiền</th>
                    <th>Ngày đóng</th>
                </tr>
                {
                    data().map((item) => {
                        return (<tr key={item.ky}>
                            <td>{`Kì thứ ${item.ky}`}</td>
                            <td>{formatter.format(Math.round(item.m))} VNĐ</td>
                            <td>{item.mt.format("DD-MM-YYYY")}</td>
                        </tr>)
                    })
                }
            </table>
        </Modal>
        {globalData.contract ? <button className="btn btn-outline-primary"
                                       onClick={() => {
                                           setModalHistory(true)
                                       }}>Chi tiết trả nợ</button> : null}
        <button className="btn btn-outline-primary" onClick={() => {
            if (globalData.customer.Status == "Unverified") {
                location.href = "/verify"
                return
            }
            setModalFalse(true);
        }}>Xác minh khách hàng
        </button>
    </>)
}

export default Widget;