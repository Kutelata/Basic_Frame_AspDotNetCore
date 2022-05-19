import loadable from '@loadable/component'

const Widget = loadable(() => import('./Widget'));

export default {
    lazyLoad: false,
    component: Widget
};
