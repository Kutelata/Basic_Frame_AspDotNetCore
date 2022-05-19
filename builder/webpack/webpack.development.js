/**
 * DEVELOPMENT WEBPACK CONFIGURATION
 */

const webpack = require('webpack');
const CircularDependencyPlugin = require('circular-dependency-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

const helpers = require('./helpers');
const { entry, outputFolderPath } = helpers.getProjectInfo();

module.exports = require('./webpack.base')({
  mode: 'development',

  entry: {
    'hot-reload': 'webpack-hot-middleware/client?reload=true',
    ...entry,
  },

  // Don't use hashes in dev mode for better performance
  output: {
    path: `${outputFolderPath}/dev`,
    publicPath: `http://localhost:${process.env.PORT || 3004}/`,
    filename: '[name].js',
    chunkFilename: '[name].chunk.js',
  },

  optimization: {
    minimize: false,
  },

  // Add development plugins
  plugins: [
    new webpack.HotModuleReplacementPlugin(), // Tell webpack we want hot reloading
    new CircularDependencyPlugin({
      exclude: /a\.js|node_modules/, // exclude node_modules
      failOnError: false, // show a warning when there is a circular dependency
    }),
    new MiniCssExtractPlugin({
      filename: '[name].css',
      chunkFilename: '[name].chunk.css',
    }),
  ],

  // Emit a source map for easier debugging
  // See https://webpack.js.org/configuration/devtool/#devtool
  devtool: 'source-map',

  performance: {
    hints: false,
  },

  devServer: {
    static: {
      directory: `${outputFolderPath}/dev`,
    },
    hot: true,
    port: process.env.PORT || 3004,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Headers': '*',
      'Access-Control-Allow-Methods': '*',
    },
  },
});
