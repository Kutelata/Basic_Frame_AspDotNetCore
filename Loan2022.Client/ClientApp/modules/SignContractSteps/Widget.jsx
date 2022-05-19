import React, {useEffect, useState, useRef} from 'react';
import {Steps, Button, Spin, Select, PageHeader, Row, Col, Result} from 'antd';
import {post, get, formatter, dataURLtoFile} from "@front-end/utils"
import {useAppContext} from '@front-end/hooks';
import { ReactSketchCanvas } from 'react-sketch-canvas';

const Widget = (props) => {
    const {data} = props;
    const [loadingAll, setLoadingAll] = useState(false);
    const {Step} = Steps;
    const {Option} = Select;
    const [current, setCurrent] = useState(0);
    const {data: globalData} = useAppContext("global");
    const [start, setStart] = useState(false)
    const [contract, setContract] = useState(null)
    const signAreaRef = useRef(null)
    const [signImage, setSignImage] = useState(null);
    const [loading, setLoading] = useState(false)
    const [chatId, setChatId] = useState(null)
    
    useEffect(() => {
        setContract(globalData.signContract.contract || null)
        if (!globalData.signContract.contract) {
            setCurrent(0)
        }else if(globalData.signContract.contract.Status == "Pending") {
            setCurrent(1)
        }else if (globalData.signContract.contract.Status == "Approved"){
            setCurrent(2)
        }
        setChatId(globalData.signContract.chatId || null)
    }, [])

    const steps = [{
        title: 'Thông tin hợp đồng',
        content: (<>
                <div ref={signAreaRef}>
                    {
                        !start?( <div>
                            <Row>
                                <Col span={12}>
                                   <div className="text-bold">
                                       Số tiền:
                                   </div>
                                </Col>
                                <Col span={12}>
                                    {globalData.signContract.money ? (<>{formatter.format(globalData.signContract.money)} VNĐ</>) : null}
                                </Col>
                            </Row>
                            <Row>
                                <Col span={12}>
                                    <div className="text-bold">
                                    Số tháng thanh toán:
                                    </div>
                                </Col>
                                <Col span={12}>
                                    {globalData.signContract.month ? (<>{globalData.signContract.month.Name}</>) : null}
                                </Col>
                            </Row>
                            <Row>
                                <Col span={12}>
                                    <div className="text-bold">
                                    Chữ ký:
                                    </div>
                                </Col>
                                <Col span={12}>
                                    {signImage?"Đã ký":"Chưa ký"}
                                </Col>
                            </Row>
                            <Row>
                                <Col span={24}>
                                    {signImage?<img src={signImage} alt=""/>:null}
                                </Col>
                            </Row>
                        </div>):(<>
                            <div className="text-center">Ký vào khung bên dưới</div>
                            <ReactSketchCanvas ref={signAreaRef} style={{
                            border: '0.0625rem solid #9c9c9c',
                            borderRadius: '0.25rem',
                        }}
                                                        width="600"
                                                        height="250px"
                                                        strokeWidth={3}
                                                        strokeColor="black"/>
                            <a onClick={()=>{
                                if(signAreaRef.current){
                                    signAreaRef.current.clearCanvas();
                                }
                            }}>Làm mới</a></>)
                    }
                    <div className="steps-action text-center">
                        {(current === 0 && !start && !signImage) && (
                            <Button size="large" type="primary" onClick={() => {
                                setStart(true)
                            }}>
                                Ký hợp đồng
                            </Button>
                        )}
                        
                        {(current === 0 && !start && signImage) && (
                            <Button size="large" type="primary" onClick={() => {
                                const file = dataURLtoFile(signImage, "sign.png")
                                if(file){
                                    setLoading(true)
                                    const form = new FormData();
                                    form.append("file", file);
                                    post("/api/save-contract", {
                                        data: form
                                    }).then((res)=>{
                                        setContract(res.data.contract)
                                        setChatId(res.data.chatId)
                                        setLoading(false)
                                        setCurrent(1)
                                    })
                                }
                            }}>
                               Bước tiếp theo
                            </Button>
                        )}

                        {(current === 0 && start) && (
                            <Button size="large" type="primary" onClick={() => {
                                signAreaRef.current.exportImage().then((res)=>{
                                    setSignImage(res)
                                    setStart(false)
                                })
                            }}>
                                Xác nhận
                            </Button>
                        )}
                    </div>
                </div>
            </>
        )

    }, {
        title: 'Đang xét duyệt',
        content: (<>
            <div>
                <div>
                    <Result
                        title="Liên hệ thẩm định viên để xét duyệt hồ sơ"
                        extra={
                            <Button type="primary" onClick={()=>{
                                if(chatId){
                                    location.href=chatId
                                }
                            }}>
                                LIÊN HỆ THẨM ĐỊNH VIÊN
                            </Button>
                        }
                    />
                </div>
                <div className="detailt-1">
                    <div className="text-center"><h4>Chi tiết hợp đồng vay</h4></div>
                    <Row>
                        <Col span={12}>
                            <div className="text-bold">
                                Số tiền:
                            </div>
                        </Col>
                        <Col span={12}>
                            {contract ? (<>{formatter.format(contract.AmountOfMoney || contract.amountOfMoney)} VNĐ</>) : null}
                        </Col>
                    </Row>
                    <Row>
                        <Col span={12}>
                            <div className="text-bold">
                                Số tháng thanh toán:
                            </div>
                        </Col>
                        <Col span={12}>
                            {globalData.signContract.month ? (<>{globalData.signContract.month.Name}</>) : null}
                        </Col>
                    </Row>
                    <Row>
                        <Col span={12}>
                            <div className="text-bold">
                                Mã hợp đồng:
                            </div>
                        </Col>
                        <Col span={12}>
                            {contract ? (<>{contract.ContractCode || contract.contractCode}</>) : null}
                        </Col>
                    </Row>
                    <Row>
                        <Col span={12}>
                            <div className="text-bold">
                                Chữ ký:
                            </div>
                        </Col>
                        <Col span={12}>
                            Đã ký
                        </Col>
                    </Row>
                </div>
            </div>
        </>)
    },
        {
            title: 'Xét duyệt thành công',
            content: (<>
                <Result
                    status="success"
                    title="Hợp đồng đã được xét duyệt"
                    extra={[
                        <Button type="primary" onClick={()=>{
                            location.href="/"
                        }}>
                            Trở lại trang chủ
                        </Button>,
                    ]}
                /></>)
        }]


    return (<>
        <Spin spinning={loading}>
            <PageHeader
                className="site-page-header text-center"
                onBack={() => null}
                title="Đăng ký khoản vay"
                backIcon={false}
            />
            <Steps className="steps-container" current={current} direction="horizontal" responsive={false}
                   labelPlacement="vertical">
                {steps.map(item => (
                    <Step key={item.title} title={item.title}/>
                ))}
            </Steps>
            <div className="steps-content">{steps[current].content}</div>
        </Spin>
    </>)
}

export default Widget