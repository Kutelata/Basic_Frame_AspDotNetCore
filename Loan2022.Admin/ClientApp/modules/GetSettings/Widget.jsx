import React, {Fragment, useEffect, useState,} from 'react';
import {get, post} from "@front-end/utils"
import {Table, Tag, Space, Modal, message, Menu, Dropdown, Button} from 'antd';
import {SearchOutlined, SettingOutlined} from '@ant-design/icons';

const confirm = Modal.confirm;
const Employees = (props) => {
    const [settings, setSettings] = useState([]);
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
        post('/setting/getSettings', {
            data: {
                sorting: inputFilter.sorting,
                pageNumber: inputFilter.pageNumber,
                pageSize: inputFilter.pageSize
            }
        }).then((res) => {
            const {data} = res.data;
            console.log('data', data);
            setSettings([...data]);
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

    const updateSetting = (id) => {
        window.location.href = 'settings/' + id;
    }

    const deleteSetting = (record) => {
        confirm({
            title: 'Bạn có muốn xóa cài đặt này không?',
            onOk() {
                get('/setting/deleteSetting', {
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
    const onAddSetting = () => {
        window.location.href = '/settings/create';
    }

    const columns = [
        {
            title: 'STT',
            render: (text, record, index) => (page - 1) * pageSize + index + 1,
            className: "text-center width-100"
        },
        {
            title: 'Tên Cài đặt',
            dataIndex: 'key',
            key: 'key',
            sorter: true,
            className: "text-center"
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
                                <Menu.Item key="1" onClick={() => updateSetting(item.id)}>Chỉnh sửa</Menu.Item>
                                <Menu.Item key="2" onClick={() => deleteSetting(item)}>Xóa</Menu.Item>
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
                        <div className="export">
                            <button type="submit" className="btn btn-primary"
                                    onClick={() => onAddSetting()}>
                                Thêm
                            </button>
                        </div>
                    </div>
                    <div className="card-body table-responsive">
                        <Table
                            columns={columns}
                            dataSource={settings}
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
