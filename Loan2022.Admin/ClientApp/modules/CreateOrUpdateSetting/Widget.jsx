import React, {useEffect, useState} from 'react';
import {get, post} from '@front-end/utils';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';


function CreateOrUpdateSetting(props) {
    const {id} = props.data;
    const key = useFormInput('');
    const valueKey = useFormInput('');
    const [loading, setLoading] = useState(false);
    const [setting, setSetting] = useState({});
    const [stateValue, setStateValue] = useState('');
    const onSave = () => {
        let id = 0;
        if (setting.id) {
            id = setting.id
        }
        post('/setting/createOrUpdate', {
            data: {
                key: key.value,
                value: stateValue,
                id: id,
            }
        }).then(res => {
            setLoading(true);
            onCancel();
        })
    }
    useEffect(() => {
        if (id) {
            get('/setting/getSetting', {
                params: {
                    id: id
                }
            }).then(res => {
                const data = res.data;
                key.onChange(data.key);
                valueKey.onChange(data.value);
                setStateValue(data.value);
                setSetting(data);
            });
        }
    }, []);
    const onCancel = () => {
        window.location.href = '/settings';
    }
   const  handleChange = (value) =>  {
       setStateValue(value);
    }
    return (
        <div className="card card-primary">
            <div className="card-header">
                <h3 className="card-title">Thêm/sửa cài đặt</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="form-group">
                        <label htmlFor="name">Tên cài đặt</label>
                        <input type="text" {...key} name={'name'} id="name"
                               className="form-control"/>
                    </div>
                    <div className="form-group">
                        <label htmlFor="value">Giá trị cài đặt</label>
                        <div className="form-group">
                            <ReactQuill
                                modules={
                                    {
                                        toolbar: [
                                            [{ 'header': '1'}, {'header': '2'}, { 'font': [] }],
                                            [{size: []}],
                                            ['bold', 'italic', 'underline', 'strike', 'blockquote'],
                                            [{'list': 'ordered'}, {'list': 'bullet'},
                                                {'indent': '-1'}, {'indent': '+1'}],
                                            ['link', 'image', 'video'],
                                            ['clean']
                                        ],
                                        clipboard: {
                                            // toggle to add extra line breaks when pasting HTML:
                                            matchVisual: false,
                                        }
                                    }
                                }
                                value={stateValue}
                                onChange={handleChange} />
                        </div>
                    </div>   
                    
                  
                </div>
                <div className="card-footer">
                    <button type="button" className="btn btn-primary float-right"
                            value={loading ? 'Loading...' : 'Lưu'} onClick={() => onSave()} disabled={loading}>Lưu
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

export default CreateOrUpdateSetting;