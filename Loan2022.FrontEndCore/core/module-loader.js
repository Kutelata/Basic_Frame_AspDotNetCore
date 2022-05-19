import kebabCase from 'lodash/kebabCase';
export class ModuleLoader {
  load = ({ context, source }) => {
    return context
      .keys()
      .map((path) => {
        const [, moduleName] = path.split('/');
        const alias = kebabCase(moduleName);
        const module = context(path);
        return {
          source,
          name: moduleName,
          alias,
          module: module.default,
        };
      });
  }
};