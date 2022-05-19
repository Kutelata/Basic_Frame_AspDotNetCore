import React, {useEffect, useState} from 'react';
import {get} from "@front-end/utils";
import moment from 'moment';
import {MediaTypesCustomer} from "@front-end/constants"
const GetCustomerDetailMedia = (props) => {
    const {id} = props.data;
    const [images, setImages] = useState({});
    const [title, setTitle] = useState('');
    const [imageSelected, setImageSelected] = useState({});
    const [countImage, setCountImage] = useState(0);
    useEffect(() => {
        get('/customer/getMediasCustomer', {
                params: {
                    id: id
                }
            }
        )
            .then(res => {
                const data = res.data;
                data.map((item, index) => {
                    item.path = `${process.env.MEDIA_DOMAIN}\\${item.path}`;
                    item.index = index;
                })
                console.log('medias', data,data.length);
                setCountImage(data.length);
                setImages(data);
                if (data) {
                    setImageSelected(data[0]);
                    if (data[0]) {
                        getTitle(data[0].type);
                    }
                }
            })
    }, []);

    const getTitle = (input) => {
        switch (input) {
            case MediaTypesCustomer.Avatar: {
                setTitle('Ảnh chân dung');
                break;
            }
            case MediaTypesCustomer.FrontFaceIdentityCard: {
                setTitle('Ảnh CMND mặt trước');
                break;
            }
            case MediaTypesCustomer.BackFaceIdentityCard: {
                setTitle('Ảnh CMND mặt sau');
                break;
            }
        }
    }
    const previousImage = () => {
        let index = imageSelected.index;
        setImageSelected(images[index - 1]);
        getTitle(images[index - 1].type);
    }

    const nextImage = () => {
        let index = imageSelected.index;
        setImageSelected(images[index + 1]);
        getTitle(images[index + 1].type);
    }

    return (
        <>
            {
                images.length > 0 &&
                <div className="card h-100">
                    <div className="card-body">
                        <div className='d-flex'>
                            <img src={imageSelected?.path}
                                 className="elevation-2 medias-image"
                                 alt="No Image"
                            >
                            </img>
                        </div>
                        <p className="text-center mt-2">{title}</p>
                        <div>
                            <button type="button" className="btn btn-default float-left"
                                    disabled={imageSelected?.index === 0}
                                    onClick={() => previousImage()}>Trước
                            </button>
                            <button type="button" className="btn btn-default float-right"
                                    disabled={imageSelected?.index === (countImage-1)}
                                    onClick={() => nextImage()}>Sau
                            </button>
                        </div>
                    </div>
                </div>
            }
            {
                (images.length === 0 || ! images == null)  &&
                <div className="card h-100">
                    <div className="card-body">
                        <p className='text-center'>Không có ảnh</p>
                    </div>
                </div>
            }
        </>
    )
}
export default GetCustomerDetailMedia;
