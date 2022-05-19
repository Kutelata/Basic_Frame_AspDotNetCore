import { FrontEndApp} from '@front-end/core';

class ClientApp extends FrontEndApp {
    moduleResolver() {
        return require.context('./modules', true, /\/index.js$/);
    }
}

new ClientApp({
    serviceWorker: true
});
