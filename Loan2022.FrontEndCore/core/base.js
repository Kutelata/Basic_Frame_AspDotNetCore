import * as Sentry from '@sentry/browser';

import { ModuleLoader } from './module-loader';

window.Sentry = window.Sentry || Sentry;

export class FrontEndApp {
  #moduleLoader = new ModuleLoader();

  #options = {
    autoInit: true,
    serviceWorker: true,
    sentryDSN: '',
  };

  constructor(options = {}) {
    this.#options = {
      ...this.#options,
      ...options,
    };

    if (this.#options.autoInit) {
      this.init().catch((ex) => {
        console.log(ex);
      });
    }
  }

  #initSW = () => {
    if ('serviceWorker' in navigator) {
      window.addEventListener('load', () => {
        navigator.serviceWorker
          .register('/sw.js')
          .then((registration) => {
            console.log('SW registered: ', registration);
          })
          .catch((registrationError) => {
            console.log('SW registration failed: ', registrationError);
          });
      });
    }
  };

  #loadAppModules = () => {
    if (typeof this.moduleResolver === 'function') {
      return this.#moduleLoader.load({
        context: this.moduleResolver(),
        source: 'app',
      });
    }
    return [];
  };

  #loadCoreModules = () => {
    return this.#moduleLoader.load({
      context: require.context('../modules', true, /\/index.js$/),
      source: 'app',
    });
  };

  #loadModule = async () => {
    const { ModuleInjection } = await import('./module-injection');
    const moduleInjection = new ModuleInjection();
    moduleInjection.inject([
      ...this.#loadAppModules(),
      ...this.#loadCoreModules(),
    ]);
  };

  #initSentry = () => {
    let environment = process.env.NODE_ENV;
    let sentryOptions = {
      dsn: this.#options.sentryDSN,
      //integrations: [new Integrations.BrowserTracing()],
      // We recommend adjusting this value in production, or using tracesSampler
      // for finer control
      // tracesSampleRate: 1.0,
      environment: environment,
      debug: environment.indexOf('dev') >= 0,
    };
    Sentry.init(sentryOptions);
  };

  init = async () => {
    await this.#loadModule();
    if (this.#options.serviceWorker) {
      this.#initSW();
    }

    if (this.#options.sentryDSN) {
      this.#initSentry();
    }
  };
}
