import { setCacheNameDetails, clientsClaim, setConfig } from 'workbox-core';
import { CacheableResponsePlugin } from 'workbox-cacheable-response/CacheableResponsePlugin';
import { StaleWhileRevalidate } from 'workbox-strategies/StaleWhileRevalidate';
import { ExpirationPlugin } from 'workbox-expiration/ExpirationPlugin';
import { precacheAndRoute } from 'workbox-precaching/precacheAndRoute';
import { registerRoute } from 'workbox-routing/registerRoute';

clientsClaim();
self.skipWaiting();
self.__WB_DISABLE_DEV_LOGS = true;
setCacheNameDetails({ precache: 'money24' });

const ignored = self.__WB_MANIFEST;

precacheAndRoute([]);

// Cache the Google Fonts stylesheets with a stale-while-revalidate strategy.
registerRoute(
  ({ url }) => url.origin === 'https://fonts.googleapis.com',
  new StaleWhileRevalidate({
    cacheName: 'google-fonts-stylesheets',
    plugins: [
      new CacheableResponsePlugin({
        statuses: [200],
      }),
      new ExpirationPlugin({
        maxAgeSeconds: 60 * 60 * 24 * 365, //1 year
        maxEntries: 30,
      }),
    ],
  })
);

// Cache the underlying font files with a cache-first strategy for 1 year.
registerRoute(
  ({ url }) => url.origin === 'https://fonts.gstatic.com',
  new StaleWhileRevalidate({
    cacheName: 'google-fonts-webfonts',
    plugins: [
      new CacheableResponsePlugin({
        statuses: [200],
      }),
      new ExpirationPlugin({
        maxAgeSeconds: 60 * 60 * 24 * 365, //1 year
        maxEntries: 20,
      }),
    ],
  })
);