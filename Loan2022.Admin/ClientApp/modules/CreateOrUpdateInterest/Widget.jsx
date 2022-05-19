import React, {useEffect, useState} from 'react';
import {get, post} from '@front-end/utils';

function CreateOrUpdateInterest(props) {
    const {id} = props.data;
    const name = useFormInput('');
    const numberOfMonth = useFormInput('');
    const percent = useFormInput('');
    const [loading, setLoading] = useState(false);
    const [interest, setInterest] = useState({});
    const onSave = () => {
        let id = 0;
        if (interest.id) {
            id = interest.id
        }
        post('/interest/createOrUpdate', {
            data: {
                name: name.value,
                percent: percent.value,
                numberOfMonth: numberOfMonth.value,
                id: id,
            }
        }).then(res => {
            setLoading(true);
            onCancel();
        })
    }
    useEffect(() => {
        if (id) {
            get('/interest/getInterest', {
                params: {
                    id: id
                }
            }).then(res => {
                const inter = res.data;
                name.onChange(inter.name);
                numberOfMonth.onChange(inter.numberOfMonth);
                percent.onChange(inter.percent);
                setInterest(inter);
            })
        }
    }, []);
    const onCancel = () => {
        window.location.href = '/interests';
    }

    return (
        <div className="card card-primary">
            <div className="card-header">
                <h3 className="card-title">Thêm/Sửa lãi suất</h3>
            </div>
            <form>
                <div className="card-body">
                    <div className="form-group">
                        <label htmlFor="bankName">Tên lãi suất</label>
                        <input type="text" {...name} name={'name'} id="name"
                               className="form-control" placeholder="Nhập tên lãi suất"/>
                    </div> 
                    <div className="form-group">
                        <label htmlFor="numberOfMonth">Số tháng</label>
                        <input type="number" {...numberOfMonth} name={'numberOfMonth'} id="numberOfMonth"
                               className="form-control" placeholder="Nhập số tháng"/>
                    </div>
                    <div className="form-group">
                        <label htmlFor="logo">Phần trăm</label>
                        <input type="text" {...percent} className="form-control" id="logo"
                               placeholder="Nhập phầm trăm"/>
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

export default CreateOrUpdateInterest;