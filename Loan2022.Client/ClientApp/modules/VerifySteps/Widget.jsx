import React, {useEffect, useState} from 'react';
import {Steps, Button, message, Upload, Spin, Form, Input, Select, DatePicker, Row, Col} from 'antd';
import {post, get} from "@front-end/utils"
import Compressor from 'compressorjs';
import 'moment/locale/vi';
import moment from "moment";
import frontImage from "./images/identity-card-front.jpg"
import backImage from "./images/identity-card-back.jpg"
import avatarImage from "./images/avatar.jpg"
const Widget = (props) => {
    const [form1] = Form.useForm();
    const {data} = props;
    const [front, setFront] = useState(null);
    const frontFace = useUpload(null, "Mặt trước CMT / CCCD")
    const backFace = useUpload(null, "Mặt sau CMT / CCCD")
    const avatar = useUpload(null, "Ảnh chân dung")
    const [loadingAll, setLoadingAll] = useState(false);
    const {Step} = Steps;
    const {Option} = Select;
    const [current, setCurrent] = useState(0);
    const [banks, setBanks] = useState([]);
    const [displayGuide, setDisplayGuide] = useState(true)

    useEffect(() => {
        getBank().then((res) => {
            setBanks(res)
        })
        setCurrent(parseInt(data.currentstep))
    }, [])

    const steps = [{
        title: 'XM Tên thật',
        content: (<>
                {displayGuide ? (<>
                   <div className="text-center">
                       <h2>Hướng dẫn</h2>
                       <div style={{marginBottom: "10px"}}><h3>Mặt trước CMND/CCCD</h3><img style={{width: '100%', padding: '0 50px'}} src={frontImage} alt=""/></div>
                       <div style={{marginBottom: "10px"}}><h3>Mặt sau CMND/CCCD</h3><img style={{width: '100%', padding: '0 48px'}} src={backImage} alt=""/></div>
                       <div><h3>Ảnh chân dung</h3><img style={{width: '100%', padding: '0 100px'}} src={avatarImage} alt=""/></div>
                   </div>
                </>) : (
                    <>
                        <div className="upload-image-zone">{frontFace.Upload}</div>
                        <div className="upload-image-zone">{backFace.Upload}</div>
                        <div className="upload-image-zone">{avatar.Upload}</div>
                    </>
                )}
            </>
        )

    }, {
        title: 'Thông tin',
        content: (<>
            <Form form={form1} size="middle" onFinish={(values) => {
                const dateOfBirth = moment(`${values.day}/${values.month}/${values.year}`, "DD/MM/YYYY")
                if (!dateOfBirth.isValid() || dateOfBirth.year() < 1900 || dateOfBirth.year() > moment().year()) {
                    message.error("Ngày sinh không hợp lệ, vui lòng thử lại");
                    return
                }
                const age = moment().diff(dateOfBirth, 'years')
                debugger
                if (age < 18) {
                    message.error("Bạn phải trên 18 tuổi");
                    return
                }
                setLoadingAll(true);
                get("/api/check-identity", {
                    params: {
                        number: values['identityCard']
                    }
                }).then((res) => {
                    if (res.data) {
                        message.error("Chứng minh nhân dân / CCCD đã tồn tại")
                        setLoadingAll(false);
                    } else {
                        post("/api/update-information", {
                            data: {
                                ...values, dateOfBirth
                            }
                        }).then((x) => {
                            setCurrent(current + 1)
                            setLoadingAll(false);
                        })
                    }
                })
            }}>
                <Form.Item name="fullName" rules={[{required: true, message: "Vui lòng nhập họ tên"}, {
                    max: 50,
                    message: "Vượt quá ký tự cho phép"
                }]}>
                    <Input placeholder="Nhập họ tên"/>
                </Form.Item>
                {/*    <Form.Item*/}
                {/*    name="dateOfBirth"*/}
                {/*    rules={[{required: true, message: "Vui lòng nhập ngày sinh"}]}*/}
                {/*>*/}
                {/*    <DatePicker locale={locale} format="DD/MM/YYYY" placeholder="Chọn ngày sinh" style={{width: '100%'}}/>*/}
                {/*</Form.Item>*/}
                <div className="ant-col ant-form-item-label"><label htmlFor="birthDay"
                                                                    className="ant-form-item-required"
                                                                    title="Thu nhập">Ngày sinh</label></div>
                <Row gutter={10}>
                    <Col span={7}>
                        <Form.Item name="day">
                            <Input placeholder="Ngày"/>
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="month">
                            <Input placeholder="Tháng"/>
                        </Form.Item>
                    </Col>
                    <Col span={10}>
                        <Form.Item name="year">
                            <Input placeholder="Năm"/>
                        </Form.Item>
                    </Col>
                </Row>
                <Form.Item name="identityCard" rules={[{
                    required: true,
                    message: "Vui lòng nhập số chứng minh nhân dân / CCCD"
                }, {pattern: "^\\d+$", message: "Vui lòng nhập đúng số chứng minh nhân dân / CCCD", max: 12}]}>
                    <Input placeholder="Số chứng minh thư / CCCD"/>
                </Form.Item>
                <Form.Item name="job" rules={[{required: true, message: "Vui lòng nhập nghề nghiệp"}]}>
                    <Input placeholder="Nghề nghiệp"/>
                </Form.Item>
                <Form.Item name="address" rules={[{required: true, message: "Vui lòng nhập địa chỉ"}]}>
                    <Input placeholder="Địa chỉ số nhà / đường / phường / ...."/>
                </Form.Item>
                <Form.Item
                    name="income"
                    label="Thu nhập"
                    rules={[{required: true, message: 'Vui lòng lựa chọn thu nhập'}]}
                >
                    <Select placeholder="Lựa chọn...">
                        <Option value="1">Dưới 5.000.000</Option>
                        <Option value="2">Từ 5.000.000 - 10.000.000</Option>
                        <Option value="3">Từ 10.000.000 - 30.000.000</Option>
                        <Option value="4">Trên 30.000.000</Option>
                    </Select>
                </Form.Item>
                <Form.Item
                    name="education"
                    label="Học vấn"
                    rules={[{required: true, message: 'Vui lòng lựa chọn học vấn'}]}
                >
                    <Select placeholder="Lựa chọn...">
                        <Option value="1">Trung học cơ sở</Option>
                        <Option value="2">Trung học phổ thông</Option>
                        <Option value="3">Trung cấp</Option>
                        <Option value="4">Cao đẳng</Option>
                        <Option value="5">Đại học</Option>
                        <Option value="6">Sau đại học</Option>
                    </Select>
                </Form.Item>
                <Form.Item
                    name="marriage"
                    label="Hôn nhân"
                    rules={[{required: true, message: 'Vui lòng lựa chọn tình trạng hôn nhân'}]}
                >
                    <Select placeholder="Lựa chọn...">
                        <Option value="1">Độc thân</Option>
                        <Option value="2">Kết hôn</Option>
                        <Option value="3">Ly hôn</Option>
                    </Select>
                </Form.Item>
                <Form.Item className="text-center">
                    <Button type="primary" size="large" htmlType="submit">
                        Tiếp theo
                    </Button>
                </Form.Item>
            </Form></>)
    },
        {
            title: 'NH thụ hưởng',
            content: (<><Form size="middle" onFinish={(values) => {
                debugger
                post("/api/update-bank", {
                    data: {...values}
                }).then((res) => {
                    location.href = "/register-contract"
                }, (err) => {
                    message.error("Đã xảy ra lỗi")
                })
            }}>
                <Form.Item name="accountNumber" rules={[{
                    required: true,
                    message: "Vui lòng nhập số tài khoản ngân hàng"
                }, {pattern: "^\\d+$", message: "Vui lòng nhập đúng số tài khoản ngân hàng"}]}>
                    <Input placeholder="Số tài khoản ngân hàng"/>
                </Form.Item>
                <Form.Item name="beneficiaryOfName" rules={[{required: true, message: "Vui lòng nhập tên chủ thẻ"}]}>
                    <Input placeholder="Tên chủ thẻ"/>
                </Form.Item>
                <Form.Item
                    name="bankId"
                    rules={[{required: true, message: 'Vui lòng lựa chọn tình trạng hôn nhân'}]}>
                    <Select placeholder="Lựa chọn ngân hàng">
                        {banks ? banks.map((b) => {
                            return (<Option value={b.id}>{b.bankName}</Option>)
                        }) : null}
                    </Select>
                </Form.Item>
                <Form.Item className="text-center">
                    <Button type="primary" size="large" htmlType="submit">
                        Tiếp theo
                    </Button>
                </Form.Item>
            </Form></>)
        }]

    const getBank = () => {
        return new Promise((resolve, reject) => {
            post('/api/get-banks').then((res) => {
                const {data} = res;
                resolve(data)
            }, (err) => {
                reject(err);
            })
        })

    }

    const next = () => {
        switch (current) {
            case 0:
                if (!frontFace.value || !backFace.value || !avatar.Upload) {
                    message.error("Vui lòng chọn đủ ảnh")
                    return
                }
                setLoadingAll(true)
                const data = new FormData();
                data.append("identityCardFrontFace", frontFace.value, frontFace.value.name);
                data.append("identityCardBackFace", backFace.value, backFace.value.name);
                data.append("identityAvatar", avatar.value, avatar.value.name);
                post("/api/upload-identity-image", {
                    data: data
                }).then((res) => {
                    setLoadingAll(false)
                    setCurrent(current + 1);
                }, (err) => {
                    setLoadingAll(false)
                    message.error("Lỗi")
                })
                break
        }

    };

    return (<>
        <Spin spinning={loadingAll} size="large" wrapperClassName="container-loading">
            <Steps className="steps-container" current={current} direction="horizontal" responsive={false}
                   labelPlacement="vertical">
                {steps.map(item => (
                    <Step key={item.title} title={item.title}/>
                ))}
            </Steps>
            <div className="steps-content">{steps[current].content}</div>
            <div className="steps-action text-center">
                {(current < steps.length - 1 && current !== 1 && !displayGuide) && (
                    <Button size="large" type="primary" onClick={() => next()}>
                        Tiếp theo
                    </Button>
                )}

                {(current === 0 && displayGuide) && (
                    <Button size="large" type="primary" onClick={() => {
                        setDisplayGuide(false)
                    }}>
                        Đã hiểu
                    </Button>
                )}
            </div>
        </Spin>
    </>)
}

//Form upload
const useUpload = (initialValue, name) => {
    const [value, setValue] = useState(initialValue);
    const [imageBase64, setImageBase64] = useState(null);
    const [loading, setLoading] = useState(false);

    function getBase64(img, callback) {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
    }

    const element = (<><Upload accept="image/x-png,image/jpeg" maxCount={1} showUploadList={false}
                               beforeUpload={(r, f) => {
                                   setLoading(true)
                                   const validImageTypes = ['image/jpeg', 'image/png'];
                                   if (!validImageTypes.includes(r.type)) {
                                       message.error("Vui lòng chọn đúng định dạng ảnh")
                                       setLoading(false)
                                       return
                                   }
                                   new Compressor(r, {
                                       quality: 0.7, success(result) {
                                           setValue(result);
                                           getBase64(result, imageUrl => {
                                               setImageBase64(imageUrl);
                                               setLoading(false)
                                           })
                                       }, error(error) {

                                       }
                                   })
                                   return false;
                               }}>
            <Spin spinning={loading}>
                <div className="upload-area">
                    {!value ? <p>{name}</p> : null}
                    {imageBase64 ? <img className="preview-image" src={imageBase64} alt=""/> : null}
                </div>
            </Spin>
        </Upload></>
    )
    return {
        value,
        Upload: element,
    }
}

export default Widget