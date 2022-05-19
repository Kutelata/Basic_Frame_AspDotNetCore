import omitBy from 'lodash/omitBy';
import isNil from 'lodash/isNil';

export function objToUrlParam(obj) {
    return new URLSearchParams(omitBy(obj, isNil)).toString();
}

export * from './request';
export * from './validation';
export * from './string-utils';
export * from './actions';
export * from './external-content';
export * from './number';
export * from './image-utils';