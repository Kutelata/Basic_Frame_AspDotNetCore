import loadable from '@loadable/component'
import './style.scss'
const Widget = loadable(() => import('./Widget'));

export default {
    lazyLoad: false,
    component: Widget
};