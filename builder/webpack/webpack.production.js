const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { InjectManifest } = require('workbox-webpack-plugin');
const { RetryChunkLoadPlugin } = require('webpack-retry-chunk-load-plugin');
const helpers = require('./helpers');
const { entry } = helpers.getProjectInfo();

const chunkSuffix = (process.env.npm_package_version || '1.0.0').replace(/\./g, '-');

module.exports = require('./webpack.base')({
  mode: 'production',

  entry: {
    ...entry,
  },

  // Utilize long-term caching by adding content hashes (not compilation hashes) to compiled assets
  output: {
    filename: `[name]-${chunkSuffix}.[chunkhash].js`,
    chunkFilename: `[name]-${chunkSuffix}.[chunkhash].chunk.js`,
  },

  optimization: {
    minimize: true,
    nodeEnv: 'production',
    runtimeChunk: true,
  },

  plugins: [
    new InjectManifest({
      swSrc: `./builder/service-worker/service-worker.${process.env.NODE_ENV}.js`,
      swDest: `../sw.js`,
      // swDest: `../../Views/Shared/Layouts/sw.js`,
      include: [],
    }),
    new MiniCssExtractPlugin({
      filename: `[name]-${chunkSuffix}.[chunkhash].css`,
      chunkFilename: `[name]-${chunkSuffix}.[chunkhash].chunk.css`,
    }),
    new RetryChunkLoadPlugin({
      // optional stringified function to get the cache busting query string appended to the script src
      // if not set will default to appending the string `?cache-bust=true`
      // cacheBust: `function() {
      //   return Date.now();
      // }`,
      // optional value to set the amount of time in milliseconds before trying to load the chunk again. Default is 0
      retryDelay: 1000,
      // optional value to set the maximum number of retries to load the chunk. Default is 1
      maxRetries: 3,
      // optional list of chunks to which retry script should be injected
      // if not set will add retry script to all chunks that have webpack script loading
      // chunks: ['chunkName'],
      // optional code to be executed in the browser context if after all retries chunk is not loaded.
      // if not set - nothing will happen and error will be returned to the chunk loader.
      lastResortScript:
        "window.Sentry && window.Sentry.captureMessage && window.Sentry.captureMessage('WEBPACK RETRY')",
    }),
  ],
});
