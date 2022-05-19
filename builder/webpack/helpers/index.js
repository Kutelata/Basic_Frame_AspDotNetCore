const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlWebpackHarddiskPlugin = require('html-webpack-harddisk-plugin');
const HTMLInlineCSSWebpackPlugin = require("html-inline-css-webpack-plugin").default;


const isDev = process.env.NODE_ENV === 'development';
// const isAMP = process.env.AMP_MODE === 'true';
const isAMP = (/[Aa]mp/gm).test(process.env.ASPNETCORE_ENVIRONMENT);

const fs = require('fs');
const path = require('path');
const forEach = require('lodash/forEach');
const groupBy = require('lodash/groupBy');
const pick = require('lodash/pick');
const snakeCase = require('lodash/snakeCase');
const isNil = require('lodash/isNil');
const merge = require('lodash/merge');
const glob = require('glob');
const slugify = require('slugify');

function checkContentHaveBuilderAmp(content) {
  return (/(([ \t]*)\/[*]\s*builder:amp-css\s*[*\/])(\n|\r|.)*?([\/*]\s*endbuilder:amp-css\s*[*]\/)/gi).test(content);
}

function generateTemplate(content, { files }) {
  const { js, css } = files;
  return content
    .replace(
      /(([ \t]*)<!--\s*builder:css\s*-->)(\n|\r|.)*?(<!--\s*endbuilder:css\s*-->)/gi,
      `<!-- builder:css -->
      ${css.map((link) => `<link href="${link}" rel="stylesheet">`).join('\n')}
      <!-- endbuilder:css -->`,
    )
    .replace(
        /(([ \t]*)\/[*]\s*builder:amp-css\s*[*\/])(\n|\r|.)*?([\/*]\s*endbuilder:amp-css\s*[*]\/)/gi,
        '/* builder:amp-css */ <!-- amp:css --> /* endbuilder:amp-css */',
    )
    .replace(
      /(([ \t]*)<!--\s*builder:js\s*-->)(\n|\r|.)*?(<!--\s*endbuilder:js\s*-->)/gi,
      `<!-- builder:js -->
      ${js.map((link) => `<script defer src="${link}"></script>`).join('\n')}
      <!-- endbuilder:js -->`,
    );
}

function objectToEnv(object) {
  const env = {};

  forEach(object, (value, key) => {
    const envKey = snakeCase(key).toUpperCase();
    env[`${envKey}`] = JSON.stringify(value);
  });
  return env;
}

function getHTMLPlugins(chunks) {
  const plugins = [];
  const chunkByViews = groupBy(chunks, 'view');
  forEach(chunkByViews, (items, view) => {
    if(isAMP && !checkContentHaveBuilderAmp(fs.readFileSync(view).toString()))
      return;
    plugins.push(
      new HtmlWebpackPlugin({
        minify: false,
        inject: false,
        filename: view,
        alwaysWriteToDisk: true,
        templateContent: ({ htmlWebpackPlugin }) => {
          return generateTemplate(fs.readFileSync(view).toString(), htmlWebpackPlugin)
        },
        chunks: [
          ...items.map(({ chunk }) => chunk),
          ...(isDev ? ['hot-reload'] : []),
        ],
      }),
    );
  });


  if (isAMP) {
    plugins.push(new HTMLInlineCSSWebpackPlugin({
      replace: {
        removeTarget: true,
        target: '<!-- amp:css -->',
      },
      styleTagFactory({ style }) {
        return style ? style.replace(/@/gi, '@@') : '';
      },
    }));
  } else {
    plugins.push(new HtmlWebpackHarddiskPlugin());
  }

  return plugins;
}

function getProjects() {
  const files = glob.sync(path.resolve(process.cwd(), `*/builder.config.js`));
  return files.map((filePath) => {
    const { name } = path.parse(filePath);
    return name;
  });
}

function getProjectInfo(folder = process.env.PROJECT || 'Loan2022.Client') {
  const projectFolderPath = path.resolve(process.cwd(), `${folder}`);
  const configPath = `${projectFolderPath}/builder.config.json`;
  // const appSettingPath = `${projectFolderPath}/appsettings.json`;
  const appSettings = mergeAppConfig(projectFolderPath, process.env.ASPNETCORE_ENVIRONMENT)

  //Load project config
  const rawConfig = require(configPath); //eslint-disable-line
  // const rawAppSetting = require(appSettingPath); //eslint-disable-line
  const rawAppSetting = appSettings; //eslint-disable-line

  const { assetsFolder, outputFolder, entryPoints, appSettingKeys = [] } = rawConfig;

  //Resolve folder path
  const paths = {};
  forEach({ assetsFolder, outputFolder }, (folderName, key) => {
    paths[`${key}Path`] = path.resolve(projectFolderPath, folderName);
  });

  //Read all layouts
  const defaultViews = [
    ...glob.sync(path.resolve(projectFolderPath, `./Views/Shared/Layouts/*.cshtml`)),
    ...glob.sync(path.resolve(projectFolderPath, `./Views/Shared/Layouts/*.html`))
  ];

  const chunks = defaultViews.map((viewPath) => {
    return {
      chunk: 'common',
      view: viewPath,
    };
  })
  // default webpack entry
  const entry = {
    common: path.resolve(paths.assetsFolderPath, 'index.js'),
  };
  
  //Build entry points based pages
  forEach(entryPoints, ({ name, file, views, amp }) => {
    if (isAMP && !amp) {
      return;
    }

    const alias = slugify(name).toLowerCase();
    const filePath = path.resolve(paths.assetsFolderPath, file);

    // automatically inject script & css
    forEach(views, (viewPath) => {
      chunks.push({
        chunk: alias,
        view: path.resolve(projectFolderPath, viewPath),
      });
    });

    // webpack entry
    entry[alias] = filePath;
  });

  const envConfig = rawConfig.env || {};
  
  return {
    ...rawConfig,
    projectFolder: folder,
    projectFolderPath,
    entry,
    chunks,
    ...paths,
    ...(envConfig[process.env.NODE_ENV] || {}),
    appSettings: pick(rawAppSetting.AppSettings || {}, appSettingKeys),
    isAMP
  };
}

function mergeAppConfig(projectFolderPath, env) {
  var cfg = {};
  mergeConfig(cfg, `${projectFolderPath}/appsettings.base.json`);
  mergeConfig(cfg, `${projectFolderPath}/appsettings.${env}.json`);
  fs.writeFile(`${projectFolderPath}/appsettings.json`, JSON.stringify(cfg), function (err) {
    if (err) return console.log('Merge appsettings.json ', err);
    console.log(`Merge appsettings.json cho Enviroment ${env}`);
  });
  return cfg;
}

function mergeConfig(cfg, envFile) {
  if (isNil(cfg)) cfg = {};
  if (fs.existsSync(envFile)) {
    cfg = merge(cfg, require(envFile));
  }
  return cfg;
}

module.exports = {
  getProjectInfo,
  getProjects,
  getHTMLPlugins,
  objectToEnv,
};
