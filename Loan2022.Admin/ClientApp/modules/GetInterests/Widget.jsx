import React, {Fragment, useEffect, useState,} from 'react';
import {get, post} from "@front-end/utils"
import {Table, Tag, Space, Modal, message, Menu, Dropdown, Button} from 'antd';
import {SearchOutlined, SettingOutlined} from '@ant-design/icons';

const confirm = Modal.confirm;
const Interests = (props) => {
    const [interests, setInterests] = useState([]);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const [inputFilter, setFilter] = useState({
        filter: '',
        sorting: 'Id',
        pageNumber: 1,
        pageSize: 10,
    });
    const [pageSize, setPageSize] = useState(10);
    const [totalCount, setTotalCount] = useState(0);
    useEffect(() => {
        getData();
    }, [inputFilter]);

    const getData = () => {
        setLoading(true);
        post('/interest/getInterests', {
            data: {
                filter: inputFilter.filter,
                sorting: inputFilter.sorting,
                pageNumber: inputFilter.pageNumber,
                pageSize: inputFilter.pageSize
            }
        }).then((res) => {
            const {data} = res.data;
            console.log('data', data)
            setInterests([...data]);
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

    const updateInterest = (id) => {
        window.location.href = 'interests/' + id;
    }

    const deleteInterest = (record) => {
        confirm({
            title: 'Bạn có muốn xóa lãi suất này không?',
            onOk() {
                get('/interest/deleteInterest', {
                    params: {
                        id: record.id
                    }
                }).then(res => {
                    message.success('Bạn đã xóa thành công lãi suất', record.name);
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
    const onAddInterest = () => {
        window.location.href = '/interests/create';
    }

    const columns = [
        {
            title: 'STT',
            render: (text, record, index) => (page - 1) * pageSize + index + 1,
            className: "text-center width-100"
        },
        {
            title: 'Tên lãi suất',
            dataIndex: 'name',
            key: 'name',
            sorter: true,
            className: "text-center"
        },
        {
            title: 'Số tháng',
            dataIndex: 'numberOfMonth',
            key: 'numberOfMonth',
            sorter: true,
            className: "text-center"
        },
        {
            title: 'Phần trăm',
            dataIndex: 'percent',
            key: 'percent',
            sorter: true,
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
                                <Menu.Item key="1" onClick={() => updateInterest(item.id)}>Chỉnh sửa</Menu.Item>
                                <Menu.Item key="2" onClick={() => deleteInterest(item)}>Xóa</Menu.Item>
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
                                    onClick={() => onAddInterest()}>
                                Thêm
                            </button>
                        </div>
                    </div>
                    <div className="card-body table-responsive">
                        <Table
                            columns={columns}
                            dataSource={interests}
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

export default Interests;
