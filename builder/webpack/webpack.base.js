/**
 * COMMON WEBPACK CONFIGURATION
 */

const webpack = require('webpack');
const path = require('path');
const WebpackBar = require('webpackbar');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const excludeNodeModulesExcept = require('./helpers/exclude');

const helpers = require('./helpers');
const { name, publicPath, outputFolderPath, chunks, assetsFolderPath, appSettings, isAMP } = helpers.getProjectInfo();

const postcssOptions = {
  plugins: [
    [
      'autoprefixer',
      {
        // Options
      },
    ],
  ],
};

process.noDeprecation = true;
// const { GenerateSW } = require('workbox-webpack-plugin');

module.exports = (options) => {
  const webpackOptions = {
    mode: options.mode,
    entry: options.entry,
    output: Object.assign(
      {
        // Compile into js/build.js
        path: `${outputFolderPath}`,
        // publicPath: `${publicPath}/`,
        publicPath: `${appSettings.AssertsDomain}${publicPath}/`,
      },
      options.output
    ), // Merge with env dependent settings
    optimization: options.optimization,
    module: {
      rules: [
        {
          test: /\.(js|jsx)$/, // Transform all .js files required somewhere with Babel
          exclude: excludeNodeModulesExcept(['swiper', 'dom7']),
          use: {
            loader: 'babel-loader',
            options: options.babelQuery,
          },
        },
        {
          test: /\.scss$/,
          use: [
            MiniCssExtractPlugin.loader,
            {
              loader: 'css-loader',
            },
            {
              loader: 'postcss-loader',
              options: {
                postcssOptions,
              },
            },
            {
              loader: 'sass-loader',
              options: {
                additionalData: "$isAmp: " + isAMP + ";",
              }
            },
          ],
          exclude: /\.module.scss$/,
        },
        {
          test: /\.module.scss$/,
          use: [
            {
              loader: 'style-loader',
            },
            {
              loader: 'css-loader',
            },
            {
              loader: 'postcss-loader',
              options: {
                postcssOptions,
              },
            },
            {
              loader: 'sass-loader',
            },
          ],
        },
        {
          test: /\.css$/,
          use: [
            MiniCssExtractPlugin.loader,
            {
              loader: 'css-loader',
            },
            {
              loader: 'postcss-loader',
              options: {
                postcssOptions,
              },
            },
          ],
          exclude: /\.module.css$/,
        },
        {
          test: /\.module.css$/,
          use: [
            {
              loader: 'style-loader',
            },
            {
              loader: 'css-loader',
            },
            {
              loader: 'postcss-loader',
              options: {
                postcssOptions,
              },
            },
          ],
        },
        {
          test: /\.component.svg$/,
          use: ['@svgr/webpack'],
        },
        {
          test: /\.(eot|svg|otf|ttf|woff|woff2)$/,
          // use: 'file-loader',
          type: 'asset/resource',
        },
        {
          test: /\.(jpg|png|gif)$/,
          // use: [
          //   {
          //     loader: 'url-loader',
          //     options: {
          //       // Inline files smaller than 10 kB
          //       limit: 10 * 1024,
          //     },
          //   },
          // ],
          type: 'asset/resource'
        },
      ],
    },
    plugins: options.plugins.concat([
      ...helpers.getHTMLPlugins(chunks),
      new WebpackBar({
        name: ['Loan Builder 2022', name, process.env.NODE_ENV].join(' - '),
      }),
      new webpack.DefinePlugin({
        'process.env': {
          NODE_ENV: JSON.stringify(process.env.NODE_ENV),
          ...helpers.objectToEnv(appSettings),
        },
      }),
    ]),
    resolve: {
      modules: ['node_modules'],
      extensions: ['.js', '.jsx'],
      mainFields: ['browser', 'main', 'jsnext:main'],
      alias: {
        '~': assetsFolderPath,
        '@front-end': path.resolve(process.cwd(), 'Loan2022.FrontEndCore'),
        '@admin': path.resolve(process.cwd(), 'Loan2022.Admin/ClientApp'),
        '@client': path.resolve(process.cwd(), 'Loan2022.Client/ClientApp'),
        ...((options.resolve && options.resolve.alias) || []),
      },
    },
    devtool: options.devtool,
    target: ['web', 'es5'],
    performance: options.performance || {},
    devServer: options.devServer || {},
  };

  return webpackOptions;
};
