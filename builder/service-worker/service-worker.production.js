import { CacheableResponsePlugin } from 'workbox-cacheable-response/CacheableResponsePlugin';
import { StaleWhileRevalidate } from 'workbox-strategies/StaleWhileRevalidate';
import { ExpirationPlugin } from 'workbox-expiration/ExpirationPlugin';
import { registerRoute } from 'workbox-routing/registerRoute';

import './service-worker.base';

// Cache client
registerRoute(
  new RegExp('^https://money24.vn/dist'),
  new StaleWhileRevalidate({
    cacheName: 'money24-client',
    plugins: [
      new CacheableResponsePlugin({
        statuses: [200],
      }),
      new ExpirationPlugin({
        maxAgeSeconds: 60 * 60 * 24 * 1.5, //1.5 days
        maxEntries: 30,
      }),
    ],
  })
);

// Cache cms
registerRoute(
  new RegExp('^https://cms.money24.vn/dist'),
  new StaleWhileRevalidate({
    cacheName: 'money24-admin',
    plugins: [
      new CacheableResponsePlugin({
        statuses: [200],
      }),
      new ExpirationPlugin({
        maxAgeSeconds: 60 * 60 * 24 * 1.5, //1.5 days
        maxEntries: 30,
      }),
    ],
  })
);