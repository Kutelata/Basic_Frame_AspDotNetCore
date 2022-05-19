import React, {useEffect, useState} from 'react';
import {get, post} from '@front-end/utils';
import {Upload, message} from 'antd';
import {LoadingOutlined, PlusOutlined} from '@ant-design/icons';
import Compressor from "compressorjs";

function CreateOrUpdateBank(props) {
    const {id} = props.data;
    const bankName = useFormInput('');
    const [loading, setLoading] = useState(false);
    const [bank, setBank] = useState({});
    const [imageUrl, setImageUrl] = useState('');
    useEffect(() => {
        if (id) {
            get('/bank/getBank', {
                params: {
                    id: id
                }
            }).then(res => {
                const ba = res.data;
                bankName.onChange(ba.bankName);
                setImageUrl(ba.logo);
                setBank(ba);
            })
        }
    }, []);
    
    const onSave = () => {
        let id = 0;
        if (bank.id) {
            id = bank.id
        }
        post('/bank/createOrUpdate', {
            data: {
                bankName: bankName.value,
                logo: imageUrl,
                id: id,
            }
        }).then(res => {
            setLoading(true);
            onCancel();
        })
    }

    const onCancel = () => {
        window.location.href = '/banks';
    }

    const uploadButton = (
        <div>
            {loading ? <LoadingOutlined /> : <PlusOutlined />}
            <div style={{ marginTop: 8 }}>Upload</div>
        </div>
    );
    const getBase64 = (img, callback)  =>{
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
    }
    return (
        <div className="card card-primary">
            <div className="card-header">
                <h3 className="card-title">Thêm/Sửa ngân hàng</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="form-group">
                        <label htmlFor="bankName">Tên ngân hàng</label>
                        <input type="text" {...bankName} name={'fullName'} id="bankName"
                               className="form-control" placeholder="Nhập tên ngân hàng"/>
                    </div>
                    <div className="form-group">
                        <label htmlFor="logo">Logo</label>
                        <Upload accept="image/x-png,image/jpeg" maxCount={1} showUploadList={false}
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
                                            getBase64(result, imageUrl => {
                                                setImageUrl(imageUrl);
                                                setLoading(false)
                                            })
                                        }, error(error) {

                                        }
                                    })
                                }}
                                name="avatar"
                                listType="picture-card"
                                className="avatar-uploader"
                        >
                            {imageUrl ? <img src={imageUrl} alt="avatar" style={{width: '100%'}}/> : uploadButton}
                        </Upload>
                    </div>
                </div>
                <div className="card-footer">
                    <button type="button" className="btn btn-primary float-right"
                            value={loading ? 'Loading...' : 'Login'} onClick={() => onSave()} disabled={loading}>Lưu
                    </button>
                    <button type="button" className="btn btn-default float-right mr-2" onClick={() => onCancel()}>Hủy
                        bỏ
                    </button>
                </div>
            </form>
        </div>
    );
}

const useFormInput = initialValue => {
    const [value, setValue] = useState(initialValue);

    const handleChange = e => {
        if (e && e.target) {
            setValue(e.target.value);
        } else {
            setValue(e);
        }
    }
    return {
        value,
        onChange: handleChange,
    }
}

export default CreateOrUpdateBank;