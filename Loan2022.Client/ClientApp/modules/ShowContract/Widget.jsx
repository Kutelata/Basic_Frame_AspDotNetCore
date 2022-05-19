import React, {Fragment, useEffect, useState} from 'react';
import {message, Spin, Modal, Button, Result, Input} from 'antd';
import {useAppContext} from '@front-end/hooks';
import moment from 'moment';
import NumberFormat from "react-number-format";
import {ExclamationCircleOutlined} from '@ant-design/icons';

const Widget = (props) => {
    const {data: globalData} = useAppContext("global");
    const [isModalContractVisible, setIsModalContractVisible] = useState(false)
    const [isModalWithdraw, setIsModalWithdraw] = useState(false)
    const [modalWithdraw, setModalWithdraw] = useState(false)
    const {customer, contract, signature, companyName, templateContract, month, employee} = globalData.contract

    return (<>
        <div className="container btns">
            {
                contract ? (<div className="btn-withdraw">
                    <button className="ant-btn ant-btn-primary ant-btn-lg" onClick={() => {
                        if(!contract.IsWithdrawMoney){
                            setIsModalWithdraw(true)
                        }else {
                            location.href = "/withdraw"
                        }
                    }}>Rút tiền
                    </button>
                </div>) : null
            }
            
            <div className="btn-withdraw">
                <button className="button" onClick={() => {
                    setIsModalContractVisible(true)
                }}>Xem hợp đồng
                </button>
            </div>
            
        </div>
        <Modal className="flex-scc" onCancel={()=>{
            setIsModalWithdraw(false)
        }} footer={[<Button onClick={() => {
            setIsModalWithdraw(false)
        }}>Đóng</Button>, <Button key="1" type="primary" onClick={() => {
        }} style={{textAlign: "center", textTransform: "uppercase"}} onClick={() => {
            if(employee){
                location.href=employee.ChatId;
            }
        }}>Liên hệ thẩm định viên</Button>]} visible={isModalWithdraw}>
            {!contract || !contract.IsWithdrawMoney || contract.Status != "Approved"? (<Result icon={<ExclamationCircleOutlined />}
                status="error"
                title="Từ chối yêu cầu"
            >
                <div className="text-center">
                    <p style={{color: "red", fontSize:"16px"}}>{contract? contract.Reason: ""}</p>
                    <h3>Vui lòng ấn vào liên hệ THẨM ĐỊNH VIÊN</h3>
                </div>
            </Result>) : null}
        </Modal>
        <Modal title="Hợp đồng" visible={isModalContractVisible} onCancel={() => {
            setIsModalContractVisible(false);
        }} onOk={() => {
            setIsModalContractVisible(false);
        }} className='modal-large'>
            {
                customer && contract ? (
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
                            <p>Bên B (Bên vay) Ông/Bà: {customer.FullName} </p>
                            <p>Số CMND/CCCD : {customer.IdentityCard}</p>
                            <p>Ngày kí : {moment(contract.CreatedOn).format("DD/MM/YYYY HH:mm")}</p>
                            <p>Mã hợp đồng : {contract.ContractCode}</p>
                            <p>Số tiền : <NumberFormat
                                value={contract.AmountOfMoney}
                                displayType="text"
                                thousandSeparator={true}
                            /> VND
                            </p>
                            <p>Thời gian vay : {month.Name}</p>
                            <p>Lãi suất cho vay là {month.Percent} % mỗi tháng</p>
                            <div dangerouslySetInnerHTML={{__html: templateContract}}/>

                            <p className="mt-2">Người kí vay</p>
                            <img className="signature" src={`${process.env.MEDIA_DOMAIN}/${signature}`} alt=""/>
                            <p className="mt-2">{customer.FullName}</p>
                        </div>
                    </div>
                ) : (<>Bạn chưa ký hợp đồng</>)
            }

        </Modal>
    </>)
}

export default Widget;