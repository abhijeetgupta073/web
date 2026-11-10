// Cache versioning
var CACHE_VERSION = 'v9';  // Update this version when you change cache contents
var CACHE_NAME = 'my-OfflineRegister-cache-' + CACHE_VERSION;

var urlsToCache = [
    '/Content/fonts/css/font-awesome.min.css',
    '/Content/Css/bootstrap.min.css',
    '/Content/Css/jquery.dataTables.min.css',
    '/Content/Css/custom.css',
    '/Content/Css/CustomAlert.css',
    '/Content/Css/w3.css',
    '/Content/Css/SecStyle.css',
    '/Content/js/jquery.min.js',
    '/Content/js/jquery-ui.min.js',
    '/Content/js/bootstrap.min.js',
    '/Content/js/bootbox.min.js',
    '/Content/js/CustomAlert.js',
    '/Content/js/jquery.dataTables.min.js',
    '/Content/js/OfflineRegister.js',
    '/images/fonts/fa_delete.png',
    '/images/fonts/fa_list.png',
    '/images/fonts/fa_save.png',
    '/images/fonts/fa_add.png',
    '/images/fonts/fa_book.png',
    '/images/fonts/fa_user.png',
    '/images/fonts/fa_edit.png',
    '/Images/loading.gif',
    '/images/online.png',
    '/images/offlinelogo.png',
    '/images/TopLogo.png',
    '/images/LockedIn.png',
    '/images/forspinicon.png',
    '/images/backspinicon.png',
    '/images/noimg.jpg',
    '/images/fonts/fa_offlinedelete.png',
    '/images/onlineStatus.png',
    '/images/offlineStatus.png',
    '/images/Green.jpg',
    '/images/Red.jpg',
    '/images/Yellow.jpg',
    '/img/logo1.png',
    '/OfflineOffSystemInspection.html',
    '/Offline.html',
    '/OfflineRegister.html',
    '/Content/js/OfflineOffSystemInspection.js',
    '/Content/js/OfflineCustomeAlert.js',
    '/Content/Css/jquery-ui.css',
    '/Content/Css/jquery.datetimepicker.min.css',
    '/Content/js/bootstrap-datetimepicker.min.js',
    '/Content/js/jszip.min.js',
    '/Content/js/dataTables.buttons.min.js',
    '/Content/js/html5.min.js',
    '/Content/js/Layout.js'
];

// Install Event - Caching essential resources
self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(CACHE_NAME).then(function (cache) {
            console.log('Opened cache:', CACHE_NAME);
            return cache.addAll(urlsToCache);
        })
    );
    self.skipWaiting();  // Activate the new service worker immediately
});

// Activate Event - Clean up old caches
self.addEventListener('activate', function (event) {
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            console.log('Existing caches:', cacheNames);  // Log all existing caches for debugging
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);  // Delete old caches
                    }
                })
            );
        })
    );
    self.clients.claim();  // Take control of uncontrolled clients immediately
});

// Fetch Event - Handling requests with query parameters
self.addEventListener('fetch', function (event) {
    var requestUrl = new URL(event.request.url);

    // Check if the request is for the OfflineOffSystemInspection page
    if (requestUrl.pathname.endsWith('/OfflineOffSystemInspection.html')) {
        event.respondWith(
            caches.match(event.request).then(function (response) {
                if (response) {
                    console.log('Serving from cache:', event.request.url);
                    return response;  // Return the cached page
                }
                return fetch(event.request).then(function (networkResponse) {
                    // Cache the dynamic page with the full URL, including query parameters
                    return caches.open(CACHE_NAME).then(function (cache) {
                        cache.put(event.request, networkResponse.clone());
                        return networkResponse;
                    });
                }).catch(function () {
                    // Optional: You can return an error message if both cache and network fail
                    return new Response('You are offline, and the page is not cached.');
                });
            })
        );
    } else {
        // For other requests, fallback to cache or fetch from network
        event.respondWith(
            fetch(event.request).catch(function () {
                return caches.match(event.request);
            })
        );
    }
});
