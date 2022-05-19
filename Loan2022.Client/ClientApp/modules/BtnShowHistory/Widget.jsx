import React, {useEffect, useState} from 'react';
import {Input, Form, message, Row, Col, InputNumber, Button, Popconfirm, Spin, notification, Modal} from 'antd'
import {useAppContext} from '@front-end/hooks';
import NumberFormat from "react-number-format";
import {post, get, formatter} from "@front-end/utils"
import style from  "./style.scss";

const Widget = (props) => {
    const {data: globalData} = useAppContext("global")
    const [modalHistory, setModalHistory] = useState(false);
    return (<>
        <a onClick={()=>{
            setModalHistory(true)
        }}>Biến động số dư</a>
        <Modal title="Lịch sử" className="history-modal" footer={[]} visible ={modalHistory} onCancel={()=>{
            setModalHistory(false)
        }}>
            {
                globalData.history?(<Row>
                    {globalData.history.map((e)=>{
                        return (<>
                            <Col span={14}>
                                {e.Description}
                            </Col>
                            <Col span={10}>
                                {`${e.Type ==="Plus"?"+":"-"} ${formatter.format(e.Amount)} VNĐ`}
                            </Col>
                        </>)
                    })}
                </Row>):null
            }
        </Modal>
    </>)
}

export default Widget