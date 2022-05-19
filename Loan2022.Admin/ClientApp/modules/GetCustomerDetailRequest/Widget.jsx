import React, {useEffect, useState} from 'react';
import {get} from "@front-end/utils";
import moment from 'moment';
import NumberFormat from "react-number-format";

const GetCustomerDetailRequest = (props) => {
    const {id} = props.data;
    const [requestsCustomer, setRequestCustomer] = useState([]);
    useEffect(() => {
        get('/customer/getWithdrawalRequestCustomer', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                setRequestCustomer(res.data);
            })
    }, []);

    return (
        <>
            <div className="card h-100">
                <div className="card-body">
                    {
                        requestsCustomer ? requestsCustomer.map((item, index) =>
                            (
                                <div className='row request-item' key={item} >
                                    <div className='col-md-6'>{item.name}</div>
                                    <div className='col-md-6 text-right'>
                                        <NumberFormat
                                            value={item.amountOfMoney}
                                            displayType="text"
                                            thousandSeparator={true}
                                        /> VND
                                    </div>
                                    <div className='col-md-6'>Thời gian</div>
                                    <div className="col-md-6 text-right">
                                        {moment(item.createdOn).format("DD/MM/YYYY HH:mm")}
                                    </div>
                                </div>
                            )) : <p className="text-center">Hiện không có yêu cầu nào</p>
                    }
                </div>
            </div>
        </>
    )
}
export default GetCustomerDetailRequest;
