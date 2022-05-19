import React, {useEffect, useState} from 'react';
import {Row, Col, Form, Input, Radio, Button, PageHeader, Modal, message, Result} from 'antd';
import {useAppContext} from '@front-end/hooks';
import {post, formatter} from '@front-end/utils';
import MaskedInput from 'antd-mask-input'
import moment from "moment";

const Widget = (props) => {
    const [month, setMonth] = useState()
    const {data: globalData} = useAppContext("global");
    const [form1] = Form.useForm();
    const [money, setMoney] = useState("")
    const [modalDetails, setModalDetails] = useState(false)
    const [m, setM] = useState()

    useEffect(() => {
        setM(globalData.months[0]["Id"] || 1)
        setMoney(50000000)
    }, [])


    const cal = (mn, month, rate) => {
        const months = [];
        const m = mn / month
        for (let i = 0; i < month; i++) {
            const interest = m + (mn / 100 * rate);
            // const interestRound = (Math.round(interest/1000)* 1000);
            months.push({ky: i + 1, m: interest, mt: moment().add(i+1, 'months')})
            mn -= m;
        }
        return months;
    }

    const firstMonth = () => {
        const e = globalData.months.find((x) => x.Id == m);
        return cal(money, e.NumberOfMonth, e.Percent)[0]?.m || 0
    }

    const data = () => {
        const e = globalData.months.find((x) => x.Id == m);
        if (!e) return []
        return cal(money, e.NumberOfMonth, e.Percent)
    }

    return (<>
        <Modal className="modal-details" onCancel={() => {
            setModalDetails(false)
        }} visible={modalDetails} title="Chi tiết trả nợ" footer={[<Button key="1" type="primary" onClick={() => {
            setModalDetails(false)
        }} style={{textAlign: "right"}}>Ok</Button>]}>
            <table>
                <tr>
                    <th>Kỳ</th>
                    <th>Số tiền</th>
                    <th>Ngày đóng</th>
                </tr>
                { 
                    data().map((item)=>{
                        return ( <tr key={item.ky}>
                            <td>{`Kì thứ ${item.ky}`}</td>
                            <td>{formatter.format(Math.round(item.m))} VNĐ</td>
                            <td>{item.mt.format("DD-MM-YYYY")}</td>
                        </tr>)
                    })
                }
               
            </table>

        </Modal>
        <PageHeader
            className="site-page-header text-center"
            onBack={() => null}
            title="Đăng ký khoản vay"
            backIcon={false}
        />
        <div className="card">
            <div className="text-center"><h3>Hạn mức vay</h3></div>
            <Form initialValues={{
                month: globalData.months[0]["Id"] || 1,
                money: 50000000
            }} form={form1} onFinish={(values) => {
                let vl = parseInt(values['money']);
                if(vl<30000000|| vl>500000000){
                    message.error("Hạn mức vay trong khoảng 30tr đến 500tr đồng")
                    return
                }
                post("/api/update-combo", {
                    data: {...values}
                }).then((res) => {
                    location.href = "/sign-contract"
                })
            }}>
                {/*<Form.Item  labelAlign="left" label="VND">*/}
                {/*    <Input />*/}
                {/*    */}
                {/*</Form.Item>*/}
                <Form.Item name="money" rules={[{required: true, message: "Vui lòng nhập số tiền"}]}>
                    <Input style={{width: "100%"}} addonAfter="VND"
                           onChange={(e) => {
                               const newNumber = parseInt(e.target.value, 10) || '';

                               if (Number.isNaN(newNumber)) {
                                   return;
                               }
                               form1.setFieldsValue({
                                   money: newNumber
                               })
                               setMoney(newNumber);
                           }}/>
                </Form.Item>
                <Form.Item label="Chọn số tháng thanh toán khoản vay" name="month" rules={[{required: true}]}>
                    <Radio.Group style={{width: "100%"}} buttonStyle="solid" onChange={(e) => {
                        setM(e.target.value)
                    }}>
                        <Row gutter={[8, 8]}>
                            {globalData.months ? globalData.months.map((m) => {
                                return (<Col key={m.Id} span={12}>
                                    <Radio.Button style={{width: "100%"}} className="text-center" size="large"
                                                  value={m.Id}>{m.Name}</Radio.Button>
                                </Col>)
                            }) : null}
                        </Row>
                    </Radio.Group>
                </Form.Item>
                <Form.Item className="text-center">
                    <Button type="primary" size="large" htmlType="submit">
                        Tiếp theo
                    </Button>
                </Form.Item>
            </Form>
        </div>
        <div className="card info">
            <Row>
                <Col span={12}>
                    Hạn mức vay
                </Col>
                <Col span={12}>
                    {money ? (<>{`${formatter.format(money)} VNĐ`}</>) : <>0 VNĐ</>}
                </Col>
            </Row>
            <Row>
                <Col span={12}>
                    Số kỳ thanh toán
                </Col>
                <Col span={12}>
                    {(m && globalData.months) ? (<>{`${globalData.months.find((x) => x.Id == m)?.Name}`}</>) : null}
                </Col>
            </Row>
            <Row>
                <Col span={12}>
                    Trả nợ kỳ đầu
                </Col>
                <Col span={12}>
                    {(m && globalData.months) ? <>{formatter.format(Math.round(firstMonth()))} VNĐ</> : <>0 VNĐ</>}
                </Col>
            </Row>
            <Row>
                <Col span={12}>
                    Lãi suất hàng tháng
                </Col>
                <Col span={12}>
                    {(m && globalData.months) ? (<>{`${globalData.months.find((x) => x.Id == m)?.Percent} %`}</>) : null}
                </Col>
            </Row>
            <div className="text-center details">
                <a onClick={() => {
                    setModalDetails(true)
                }}>Chi tiết trả nợ</a>
            </div>
        </div>
    </>)
}


export default Widget