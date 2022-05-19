import React, {useEffect, useState} from 'react';
import {Input, Form, message, Row, Col, InputNumber, Button, Popconfirm, Spin, notification } from 'antd'
import {useAppContext} from '@front-end/hooks';
import NumberFormat from "react-number-format";
import {post, get, formatter} from "@front-end/utils"

const Widget = (props) => {
    const {data: globalData} = useAppContext("global")
    const [withdrawMoney, setWithdrawMoney] = useState(globalData.contract.TotalMoney);
    const [visible, setVisible] = React.useState(false);
    const [loading, setLoading] = React.useState(false);
    return (<>
        <div className="text-center"><h3>Số tiền muốn rút</h3></div>
            <InputNumber controls={false} addonAfter={<>VNĐ</>} style={{width: "100%"}} decimalSeparator="," max={globalData.contract.TotalMoney} min="100000"
                         formatter={value => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                         parser={value => value.replace(/\$\s?|(,*)/g, '')} 
                         onChange={(e) => {setWithdrawMoney(e)}}
                         value={globalData.contract.TotalMoney}/>
       <div className="card">
           <Row>
           <Col span={12}>
               <div>Số dư</div>
           </Col>
           <Col span={12}>
               {globalData.contract ? (<><NumberFormat
                   value={globalData.contract.TotalMoney}
                   displayType="text"
                   thousandSeparator={true}
               /> VND</>) : ""}
           </Col>

           <Col span={12}>
               <div>Số tiền muốn rút</div>
           </Col>
           <Col span={12}>
               {withdrawMoney ? (<><NumberFormat
                   value={withdrawMoney}
                   displayType="text"
                   thousandSeparator={true}
               /> VND</>) : ""}
           </Col>
       </Row>
           <div className="text-center btn-withdraw">
               <Popconfirm style={{width: "300px"}}
                   title={`Quý khách có đồng ý rút ${formatter.format(withdrawMoney)} VND`}
                   okText="Có"
                   cancelText="Không" 
                           onCancel={()=>{
                               setVisible(false)
                           }}
                   onConfirm={()=>{
                       setVisible(false)
                       setLoading(true)
                       var formData = new FormData();
                       formData.append("money", withdrawMoney)
                       post("/api/withdraw-request", {data: formData}).then((res)=>{
                           setLoading(false)
                           // notification['success']({
                           //     message: 'Tạo yêu cầu rút tiền thành công',
                           //     description: 'Yêu cầu rút tiền đang chờ xác thực, vui lòng chờ trong 2 giờ!',
                           // });
                           location.href="/withdraw-invoice"
                       })
                   }} visible = {visible}
                   onVisibleChange={() => console.log('visible change')}
               >
                   <Spin spinning={loading}>
                       <Button type="primary" size="large" onClick={()=>{
                           if(!withdrawMoney){
                               message.error("Vui lòng nhập số tiền cần rút")
                               return
                           }
                           if(withdrawMoney > globalData.contract.TotalMoney){
                               message.error("Số tiền rút phải nhỏ hơn số dư")
                               return;
                           }
                           setVisible(true)
                       }}>Rút tiền</Button>
                   </Spin>
               </Popconfirm>
               
           </div>
       </div>
     
    </>)
}

export default Widget