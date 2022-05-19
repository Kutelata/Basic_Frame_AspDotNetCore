import React, { Fragment } from 'react';
import ReactDOM from 'react-dom';
import pick from 'lodash/pick';
import LazyLoad from 'react-lazyload';

import App from '../App';

export class ModuleInjection {
  #loaded = {};

  inject = (modules) => {
    modules.forEach((item) => {
      const { alias, module } = item;
      if (module && !this.#loaded[alias]) {
        this.#loaded[alias] = true;
        this.#moduleHandler(item);
      }
    });

    this.#renderApp(modules.filter(({ alias, module }) => {
      const { component, selector, portal } = module;
      const mountNode = document.querySelectorAll(selector || `[data-module="${alias}"]`);
      return !!component && (mountNode?.length > 0 || !portal)
    }));
  };

  #getRoot = () => {
    const mountNode = document.createElement('div');
    document.body.append(mountNode);
    return mountNode;
  };

  #renderApp = (modules) => {
    const mountNode = this.#getRoot();
    ReactDOM.render(
      <App>
        {modules.map((item) => {
          return this.#renderModule(item);
        })}
      </App>,
      mountNode
    );
  };

  #renderModule = (data) => {
    const { module, alias } = data;
    const { selector, component: Component, lazyLoad, loader: Loader, portal = true } = module;
    const mountNode = document.querySelectorAll(selector || `[data-module="${alias}"]`);
    const options = pick(data, ['alias', 'name', 'source']);

    if (!portal) {
      return [<Fragment key={alias}>
        <Component data={options} />
      </Fragment>];
    }

    const results = [];
    mountNode.forEach((node, index) => {
      const data = {
        ...options,
        ...(node.dataset || {}),
      };

      let component = null;
      if (lazyLoad) {
        if (Loader) {
          const placeholder = <Loader />;
          component = <LazyLoad placeholder={placeholder}><Component data={data} /></LazyLoad>
        } else {
          component = <LazyLoad><Component data={data} /></LazyLoad>
        }
      } else {
        component = <Component data={data} />;
      }

      results.push(
        <Fragment key={`${alias}-${index}`}>
          {this.#renderWithPortal(component, node)}
        </Fragment>
      );
    });

    return results;
  }

  #renderWithPortal = (component, node) => {
    return ReactDOM.createPortal(component, node);
  };

  #moduleHandler = (data) => {
    if (typeof data?.module?.handler === 'function') {
      const { module } = data;
      const { handler, selector } = module;
      const elements = selector ? document.querySelectorAll(selector) : null;

      if (!selector || elements?.length > 0) {
        const options = pick(data, ['alias', 'name', 'source']);
        handler(elements, options).then(() => {
          if (process.env.NODE_ENV !== 'production') {
            console.log(`LOADED: ${data.alias}`);
          }
        }).catch((ex) => {
          console.log(ex);
        })
      }
    }
  };
}
