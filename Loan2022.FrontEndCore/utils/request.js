import axios from 'axios';

export const request = async (options = {}) => {
  return await axios({
    baseURL: process.env.WEB_API,
    ...options
  });
};

export const get = async (url, options = {}) => {
  return await request({
    url,
    ...options,
    method: 'GET',
  });
};

export const post = async (url, options = {}) => {
  return await request({
    url,
    ...options,
    method: 'POST',
  });
};