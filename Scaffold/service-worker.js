// Cache versioning
var CACHE_VERSION = 'v13';
var CACHE_NAME = 'my-OfflineRegister-cache-' + CACHE_VERSION;

var urlsToCache = [
    '/Content/fonts/css/font-awesome.min.css',
    '/Content/Css/bootstrap.min.css',
    '/Content/Css/jquery.dataTables.min.css',
    '/Content/Css/custom.css',
    '/Content/Css/CustomAlert.css',
    '/Content/Css/w3.css',
    '/Content/Css/SecStyle.css',
    '/Content/Css/jquery-ui.css',
    '/Content/Css/jquery.datetimepicker.min.css',
    '/Content/Css/select2.min.css',
    '/Content/js/jquery.min.js',
    '/Content/js/jquery-ui.min.js',
    '/Content/js/bootstrap.min.js',
    '/Content/js/bootbox.min.js',
    '/Content/js/CustomAlert.js',
    '/Content/js/jquery.dataTables.min.js',
    '/Content/js/OfflineRegister.js',
    '/Content/js/OfflineOffSystemInspection.js',
    '/Content/js/OfflineCustomeAlert.js',
    '/Content/js/bootstrap-datetimepicker.min.js',
    '/Content/js/jszip.min.js',
    '/Content/js/dataTables.buttons.min.js',
    '/Content/js/html5.min.js',
    '/Content/js/Layout.js',
    '/Content/js/select2.min.js',

    // FontAwesome webfonts
    '/Content/fonts/fonts/fontawesome-webfont.woff2?v=4.5.0',
    '/Content/fonts/fonts/fontawesome-webfont.woff?v=4.5.0',
    '/Content/fonts/fonts/fontawesome-webfont.ttf?v=4.5.0',
    '/Content/fonts/fonts/fontawesome-webfont.svg?v=4.5.0',
    '/Content/fonts/fonts/fontawesome-webfont.eot?v=4.5.0',
    '/Content/fonts/fonts/FontAwesome.otf',

    '/images/fonts/fa_delete.png',
    '/images/fonts/fa_list.png',
    '/images/fonts/fa_save.png',
    '/images/fonts/fa_add.png',
    '/images/fonts/fa_book.png',
    '/images/fonts/fa_user.png',
    '/images/fonts/fa_edit.png',
    '/images/fonts/fa_offlinedelete.png',
    '/Images/loading.gif',
    '/images/online.png',
    '/images/offlinelogo.png',
    '/images/TopLogo.png',
    '/images/LockedIn.png',
    '/images/forspinicon.png',
    '/images/backspinicon.png',
    '/images/noimg.jpg',
    '/images/onlineStatus.png',
    '/images/offlineStatus.png',
    '/images/Green.jpg',
    '/images/Red.jpg',
    '/images/Yellow.jpg',
    '/img/logo1.png',

    '/OfflineOffSystemInspection.html',
    '/Offline.html',
    '/OfflineRegister.html'
];

// Offline pages mapping
var OFFLINE_PAGES = {
    'OfflineRegister': '/OfflineRegister.html',
    'OfflineOffSystemInspection': '/OfflineOffSystemInspection.html',
    'Offline': '/Offline.html'
};

var DEFAULT_OFFLINE_PAGE = '/Offline.html';
function isCacheable(request) {
    return request.method === 'GET';
}

function safeCachePut(cache, request, response) {
    if (!isCacheable(request)) {
        console.warn('[SW] Skipping cache for non-GET request:', request.method, request.url);
        return;
    }
    if (response && response.status === 200) {
        console.log('[SW] Cached:', request.url);
        cache.put(request, response.clone());
    }
}

function getOfflinePageForRequest(requestUrl) {
    var pathname = requestUrl.pathname;
    for (var key in OFFLINE_PAGES) {
        if (pathname.indexOf(key) !== -1) {
            return OFFLINE_PAGES[key];
        }
    }
    return DEFAULT_OFFLINE_PAGE;
}

self.addEventListener('install', function (event) {
    console.log('[SW] Installing cache:', CACHE_NAME);
    event.waitUntil(
        caches.open(CACHE_NAME).then(function (cache) {
            var results = urlsToCache.map(function (url) {
                return cache.add(url)
                    .then(function () {
                        console.log('[SW] ✅ Cached successfully:', url);
                    })
                    .catch(function (err) {
                        console.warn('[SW] ❌ Failed to cache:', url, err.message);
                    });
            });
            return Promise.allSettled(results);
        }).then(function () {
            console.log('[SW] ✅ All resources processed for cache:', CACHE_NAME);
        })
    );
    self.skipWaiting();
});

self.addEventListener('activate', function (event) {
    console.log('[SW] Activating:', CACHE_NAME);
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (cacheName !== CACHE_NAME) {
                        console.log('[SW] 🗑️ Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
    self.clients.claim();
});

self.addEventListener('fetch', function (event) {
    var requestUrl = new URL(event.request.url);

    if (!isCacheable(event.request)) {
        console.log('[SW] POST/non-GET request, passing through:', event.request.url);
        event.respondWith(
            fetch(event.request).catch(function (err) {
                console.error('[SW] POST fetch failed (offline?):', event.request.url, err);
                return new Response(
                    JSON.stringify({ success: false, offline: true, message: 'You are offline. Request could not be sent.' }),
                    { status: 503, headers: { 'Content-Type': 'application/json' } }
                );
            })
        );
        return;
    }

    if (event.request.mode === 'navigate') {
        event.respondWith(
            caches.match(event.request).then(function (cachedResponse) {
                if (cachedResponse) {
                    console.log('[SW] 📄 Navigation from cache:', event.request.url);
                    return cachedResponse;
                }

                return fetch(event.request).then(function (networkResponse) {
                    return caches.open(CACHE_NAME).then(function (cache) {
                        safeCachePut(cache, event.request, networkResponse);
                        return networkResponse;
                    });
                }).catch(function () {
                    var offlinePage = getOfflinePageForRequest(requestUrl);
                    console.log('[SW] 🔌 Offline navigation fallback:', offlinePage);
                    return caches.match(offlinePage).then(function (fallback) {
                        if (fallback) return fallback;
                        return caches.match(DEFAULT_OFFLINE_PAGE);
                    });
                });
            })
        );
        return;
    }
    event.respondWith(
        caches.match(event.request).then(function (cachedResponse) {
            if (cachedResponse) {
                console.log('[SW] ⚡ Serving from cache:', event.request.url);
                return cachedResponse;
            }

            console.log('[SW] 🌐 Fetching from network:', event.request.url);
            return fetch(event.request).then(function (networkResponse) {
                return caches.open(CACHE_NAME).then(function (cache) {
                    if (requestUrl.origin === location.origin) {
                        safeCachePut(cache, event.request, networkResponse);
                    }
                    return networkResponse;
                });
            }).catch(function (err) {
                console.warn('[SW] ❌ Asset fetch failed (offline):', event.request.url, err.message);
                return new Response('', { status: 503, statusText: 'Service Unavailable' });
            });
        })
    );
});