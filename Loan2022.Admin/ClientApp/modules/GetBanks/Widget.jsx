import React, {Fragment, useEffect, useState,} from 'react';
import {get, post} from "@front-end/utils"
import {Table, Tag, Space, Modal, message, Menu, Dropdown, Button} from 'antd';
import {SearchOutlined, SettingOutlined} from '@ant-design/icons';

const confirm = Modal.confirm;
const GetBanks = (props) => {
    const [banks, setBanks] = useState([]);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const [inputFilter, setFilter] = useState({
        filter: '',
        sorting: 'Id',
        pageNumber: 1,
        pageSize: 10,
    });
    const [pageSize, setPageSize] = useState(10);
    useEffect(() => {
        getData();
    }, [inputFilter]);

    const getData = () => {
        setLoading(true);
        post('/bank/getBanks', {
            data: {
                filter: inputFilter.filter,
                sorting: inputFilter.sorting,
                pageNumber: inputFilter.pageNumber,
                pageSize: inputFilter.pageSize
            }
        }).then((res) => {
            const {data} = res.data;
            setBanks([...data]);
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
        let sort = 'Id';
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

    const updateBank = (id) => {
        window.location.href = 'banks/' + id;
    }

    const deleteBank = (record) => {
        confirm({
            title: 'Bạn có muốn xóa ngân hàng này không?',
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
    const onAddBank = () => {
        window.location.href = '/banks/create';
    }

    const columns = [
        {
            title: 'STT',
            render: (text, record, index) => (page - 1) * pageSize + index + 1,
            className: "text-center width-100"
        },
        {
            title: 'Tên ngân hàng',
            dataIndex: 'bankName',
            key: 'bankName',
            sorter: true,
            className: "text-center"
        },
        {
            title: 'Hình ảnh',
            dataIndex: 'logo',
            key: 'logo',
            className: "text-center",
            render: logo => (
                <>
                    {logo &&
                        <img alt="Logo" src={logo} className='logo-bank'/>
                    }
                    {
                        !logo && "Không có logo !"
                    }
                </>
            ),
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
                                <Menu.Item onClick={() => updateBank(item.id)}>Chỉnh sửa</Menu.Item>
                                <Menu.Item onClick={() => deleteBank(item)}>Xóa</Menu.Item>
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
                                    onClick={() => onAddBank()}>
                                Thêm
                            </button>
                        </div>
                    </div>
                    <div className="card-body table-responsive">
                        <Table
                            columns={columns}
                            dataSource={banks}
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

export default GetBanks;
