import { FrontEndApp} from '@front-end/core';
import './styles/plugins/adminlte.css';
import './styles/common.scss';
import "antd/dist/antd.css";
class AdminApp extends FrontEndApp {
    moduleResolver() {
        return require.context('./modules', true, /\/index.js$/);
    }
}

new AdminApp({
    serviceWorker: true
});
