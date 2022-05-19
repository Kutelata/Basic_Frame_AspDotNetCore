import React, {Fragment, useEffect, useState,} from 'react';
import {get, post} from "@front-end/utils"
import {Table, Tag, Space, Modal, message, Menu, Dropdown, Button} from 'antd';
import {SearchOutlined, SettingOutlined} from '@ant-design/icons';

const confirm = Modal.confirm;
const Employees = (props) => {
    const [employees, setEmployees] = useState([]);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const [inputFilter, setFilter] = useState({
        filter: '',
        sorting: 'Id',
        pageNumber: 1,
        pageSize: 10,
    });
    const [totalCount, setTotalCount] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    useEffect(() => {
        getData();
    }, [inputFilter]);

    const getData = () => {
        setLoading(true);
        post('/employee/getEmployees', {
            data: {
                filter: inputFilter.filter,
                sorting: inputFilter.sorting,
                pageNumber: inputFilter.pageNumber,
                pageSize: inputFilter.pageSize
            }
        }).then((res) => {
            const {data} = res.data;
            setEmployees([...data]);
            setTotalCount(res.data.totalCount);
            setLoading(false);
        }, (err) => {
        })
    }

    function onChange(pagination, filters, sorter, extra) {
        setPageSize(pagination.pageSize);
        if (pagination && pagination.current) {
            setPage(pagination.current);
        }
        let sort = 'Id DESC';
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

    const lockEmployee = (id) => {
        updateStatusEmployee(id, 'InActive');
    }
    const unLockEmployee = (id) => {
        updateStatusEmployee(id, 'Active');
    }
    const updateEmployee = (id) => {
        window.location.href = 'employees/' + id;
    }
    const userEmployee = (id) => {
        console.log('userEmployee', id);
    }

    const updateStatusEmployee = (id, status) => {
        get('/employee/updateStatusEmployee', {
            params: {
                id: id,
                status: status
            }
        }).then((res) => {
            getData();
        }, (err) => {
        })
    }

    const deleteEmployee = (record) => {
        confirm({
            title: 'Bạn có muốn xóa nhân viên này không?',
            onOk() {
                get('/employee/deleteEmployee', {
                    params: {
                        id: record.id
                    }
                }).then(res => {
                    message.success('Bạn đã xóa thành công khách hàng ', record.fullName);
                    getData();
                })
            },
            onCancel() {
                console.log('Cancel');
            },
        });
    }

    const onSearch = () => {
        getData();
    }
    const onAddEmployee = () => {
        window.location.href = '/employees/create';
    }

    const columns = [
        {
            title: 'STT',
            render: (text, record, index) => (page - 1) * pageSize + index + 1,
            className: "text-center width-100"
        },
        {
            title: 'Họ tên',
            dataIndex: 'fullName',
            key: 'fullName',
            sorter: true,
            className: "text-center"
        },
        // {
        //     title: 'Số điện thoại',
        //     dataIndex: 'phoneNumber',
        //     key: 'phoneNumber',
        //     sorter: true,
        //     className: "text-center",
        // },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            key: 'status',
            className: "text-center",
            render: status => (
                <>
                    {
                        status === 'Active' &&
                        <Tag color='blue' key={status}>
                            Kích hoạt
                        </Tag>
                    }
                    {
                        status === 'InActive' &&
                        <Tag color='red' key={status}>
                            Khóa
                        </Tag>
                    }
                </>
            ),
        },
        {
            title: 'Tổng',
            dataIndex: 'totalCare',
            key: 'totalCare',
            className: "text-center",
        },
        {
            title: 'Hôm nay',
            dataIndex: 'totalToday',
            key: 'totalToday',
            className: "text-center",
        },
        {
            title: 'Hành động',
            key: 'action',
            className: 'width-150',
            render: (text, item) => (
                <div>
                    <Dropdown
                        trigger={['click']}
                        overlay={
                            <Menu>
                                {item.status === 'Active' &&
                                    <Menu.Item key="1" onClick={() => lockEmployee(item.id)}>Khóa</Menu.Item>}
                                {item.status === 'InActive' &&
                                    <Menu.Item onClick={() => unLockEmployee(item.id)}>Mở khóa</Menu.Item>}
                                <Menu.Item key="2" onClick={() => updateEmployee(item.id)}>Chỉnh sửa</Menu.Item>
                                <Menu.Item key="3" onClick={() => deleteEmployee(item)}>Xóa</Menu.Item>
                                {/*<Menu.Item onClick={() => userEmployee(item.id)}>User</Menu.Item>*/}
                            </Menu>

                        }
                        placement="bottomLeft"
                    >
                        <Button type="primary">
                            Hành động
                        </Button>
                    </Dropdown>
                </div>
            )
        }
    ];


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
                                    onClick={() => onAddEmployee()}>
                                Thêm
                            </button>
                        </div>
                    </div>
                    <div className="card-body table-responsive">
                        <Table
                            columns={columns}
                            dataSource={employees}
                            onChange={onChange}
                            loading={loading}
                            rowKey={(record) => record.id.toString()}
                            pagination={{pageSize: 10, total: totalCount, pageSizeOptions: ['10', '50', '100', '200']}}
                        />
                    </div>
                </div>
            </div>
        </>
    )
}

export default Employees;
