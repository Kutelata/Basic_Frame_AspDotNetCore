import React, {useEffect, useState,} from 'react';
import {get, post} from "@front-end/utils"
import {Button, DatePicker, message, Modal, Space, Table, Tag, Pagination} from 'antd';
import {SearchOutlined} from '@ant-design/icons';
import NumberFormat from "react-number-format";
import moment from "moment";

const GetCustomers = (props) => {
    const [customers, setCustomers] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [loading, setLoading] = useState(false);
    const [isModalEditContractVisible, setIsModalEditContractVisible] = useState(false);

    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());


    const [inputFilter, setFilter] = useState({
        filter: '',
        sorting: 'CreatedOn DESC',
        pageNumber: 1,
        pageSize: 10,
    });
    const {RangePicker} = DatePicker;
    const dateFormat = 'YYYY/MM/DD';

    const getAvatar = (input) => {
        return `${process.env.MEDIA_DOMAIN}\\${input}`;
    }
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const columns = [
        {
            title: 'STT',
            render: (text, record, index) => (page - 1) * pageSize + index + 1,
            className: "text-center width-100"
        },
        {
            title: 'Avatar',
            dataIndex: 'avatar',
            key: 'avatar',
            render: avatar => (
                <>
                    {avatar &&
                        <img alt="Avatar" src={getAvatar(avatar)} className='avatar-customer'/>
                    }
                    {
                        !avatar &&
                        <img alt="No Avatar" src='./default-profile-picture.png' className='avatar-customer'/>
                    }
                </>
            ),
            className: "text-center"
        },
        {
            title: 'Sales',
            dataIndex: 'salesEmployee',
            key: 'salesEmployee',
            sorter: true,
            className: "text-center"
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phoneNumber',
            key: 'phoneNumber',
            sorter: true,
            className: "text-center",
            render: (text, record) => (
                <a href={`customers/${record.id}`}>{record.phoneNumber}</a>
            )
        },
        {
            title: 'Số dư',
            dataIndex: 'totalMoney',
            key: 'totalMoney',
            sorter: true,
            className: "text-center",
            render: text => (
                text != null ?
                    <NumberFormat
                        value={text}
                        displayType="text"
                        thousandSeparator={true}
                    /> : 0
            ),
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            className: "text-center",
            render: status => (
                <>
                    {
                        status === 'Verified' &&
                        <Tag color='blue' key={status}>
                            Đã xác minh
                        </Tag>
                    }
                    {
                        status === 'Unverified' &&
                        <Tag color='red' key={status}>
                            Chưa xác minh
                        </Tag>
                    }
                </>
            ),
        },
        {
            title: 'CMND',
            dataIndex: 'identityCard',
            key: 'identityCard',
            className: "text-center",
            render: identityCard => (
                <>
                    {identityCard && identityCard !== '' ? <>{identityCard}</> : (<>Không có</>)}
                </>
            ),
        },
        {
            title: 'Hành động',
            key: 'action',
            className: 'width-150',
            render: (text, record) => (
                <Space size="middle">
                    <Button type="primary" onClick={() => changePassword(record)}>Đổi mật khẩu</Button>
                    <Button type="danger" onClick={() => deleteCustomer(record)}>Xóa</Button>
                </Space>
            )
        }
    ];

    const getData = () => {
        setLoading(true);
        console.log('getData', inputFilter.sorting, inputFilter.pageNumber, inputFilter.pageSize)
        post('/customer/getCustomers', {
            data: {
                filter: inputFilter.filter,
                sorting: inputFilter.sorting,
                pageNumber: inputFilter.pageNumber,
                pageSize: inputFilter.pageSize
            }
        }).then((res) => {
            setTotalCount(res.data.totalCount);
            const {data} = res.data;
            setCustomers([...data]);
            const result = res.data;
            setCustomers([...result.data]);
            setLoading(false);
        }, (err) => {
        })
    }

    useEffect(() => {
        getData();
    }, [inputFilter]);
    const confirm = Modal.confirm;
    const deleteCustomer = (record) => {
        confirm({
            title: 'Bạn có muốn xóa khách hàng này không?',
            onOk() {
                get('/customer/deleteCustomer', {
                    params: {
                        id: record.id
                    }
                }).then(res => {
                    message.success('Bạn đã xóa thành công khách hàng ', record.fullName);
                    getData();
                })
            },
            onCancel() {
                console.log('Cancel', record);
            },
        });
    }

    const changePassword = (record) => {
        window.location.href = '/customers/change-password/' + record.userId;
    }

    function onChange(pagination, filters, sorter, extra) {
        setPageSize(pagination.pageSize);
        if (pagination && pagination.current) {
            setPage(pagination.current);
        }
        let sort = 'CreatedOn DESC';
        if (sorter && sorter.columnKey) {
            let order = 'ASC';
            if (sorter.order === 'descend') {
                order = 'DESC';
            }
            sort = sorter.columnKey + " " + order;
        }
        setFilter({
            filter: inputFilter.filter,
            sorting: sort,
            pageSize: pagination.pageSize,
            pageNumber: pagination.current,
        });
    }

    const onSearch = () => {
        getData();
    }
    const onExportExcel = () => {
        window.location.href = `/customer/excel?startDate=${moment(startDate).format('YYYY-MM-DDTHH:mm:ss')}&endDate=${moment(endDate).format('YYYY-MM-DDTHH:mm:ss')}`;
    }

    const showExportModal = () => {
        setIsModalEditContractVisible(true);
    }
    const handleEditContractOk = () => {
        onExportExcel();
        setIsModalEditContractVisible(false);
    };

    const handleEditContractCancel = () => {
        setIsModalEditContractVisible(false);
    };
    const onChangeDate = (dates, dateString) => {
        setStartDate(dates[0].toDate());
        setEndDate(dates[1].toDate());
    };
    return (
        <>
            <div className="row">
                <div className="col-12">
                    <div className="card-header">
                        <div className="card-tools">
                            <div className="input-group mt-0">
                                <input type="text" name="table_search" className="form-control float-right"
                                       placeholder="Search" value={inputFilter.filter}
                                       onChange={(e) => {
                                           setFilter({...inputFilter, filter: e.target.value})
                                       }}/>
                                <button type="button" className="btn btn-default" onClick={onSearch}>
                                    <SearchOutlined/>
                                </button>
                            </div>
                        </div>
                        <div className="export">
                            <button type="submit" className="btn btn-primary"
                                    onClick={() => showExportModal()}>
                                Excel
                            </button>
                        </div>
                    </div>
                    <div className="card-body table-responsive">
                        <Table
                            columns={columns}
                            dataSource={customers}
                            onChange={onChange}
                            loading={loading}
                            rowKey={(record) => record.id.toString()}
                            pagination={{
                                pageSize: pageSize,
                                total: totalCount,
                                pageSizeOptions: ['10', '50', '100', '200']
                            }}
                        />
                    </div>
                </div>
            </div>
            <Modal title="Xuất Excel" visible={isModalEditContractVisible} onOk={handleEditContractOk}
                   onCancel={handleEditContractCancel}>
                <form>
                    <div className="form-group">
                        <label htmlFor="amountOfMoney">Nhập khoảng thời gian</label>
                        <RangePicker
                            className="form-control"
                            format={dateFormat}
                            onChange={onChangeDate}
                        />
                    </div>
                </form>
            </Modal>
        </>
    )
}

export default GetCustomers;
