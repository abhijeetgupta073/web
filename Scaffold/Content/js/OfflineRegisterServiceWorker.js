var CACHE_VERSION = '1';  
var CACHE_NAME = 'OfflineRegisterCache' + CACHE_VERSION;

var urlsToCache = [
    '/OfflineRegister.html',
    '/OfflineRegisterList.html',
    '/OfflineInspectionForm.html',
    '/OfflineSiteRequest.html',
    '/Content/fonts/css/font-awesome.min.css',
    '/Content/Css/bootstrap.min.css',
    '/Content/Css/jquery.dataTables.min.css',
    '/Content/Css/CustomAlert.css',
    '/Content/Css/w3.css',
    '/Content/Css/SecStyle.css',
    '/Content/Css/signature-pad.css',
    '/Content/Css/bootstrap-datetimepicker.min.css',
    '/Content/Css/jquery.datetimepicker.min.css',
    '/Content/Css/jquery-ui.css',
    '/Content/Css/custom.css',
    '/Content/js/jquery.min.js',
    '/Content/js/bootstrap.min.js',
    '/Content/js/bootbox.min.js',
    '/Content/js/jquery.dataTables.min.js',
    '/Content/js/jquery-ui.min.js',
    '/Content/js/videoInsp.js',
    '/Content/js/cameraInsp.js',
    '/Content/js/locationInsp.js',
    '/Content/js/signature_pad.js',
    '/Content/js/html2canvas.js',
    '/Content/js/sketch.min.js',
    '/Content/js/signInsp.js',
    '/Content/js/bootstrap-datetimepicker.min.js',
    '/Content/js/jquery.datetimepicker.js',
    '/Content/js/CustomAlert.js',
    '/Content/js/moment-with-locales.js',
    '/Content/js/OfflineInspection.js',
    '/Content/js/OfflineInspectionForm.js',
    '/Content/js/OfflineCustomeAlert.js',
    '/Content/js/OfflineRegister.js',
    '/Content/js/OfflineRegisterList.js',
    '/Content/js/OfflineSiteRequest.js',
    '/Content/js/jszip.min.js',
    '/Content/js/dataTables.buttons.min.js',
    '/Content/js/html5.min.js',
    '/Content/js/OfflineEncryptDecript.js',
    '/Content/js/crypto-js.min.js',
    '/Content/js/Layout.js',
    '/images/noimg.jpg',
    '/Images/loading.gif',
    '/img/logo1.png',
    '/images/lock.png',
    '/images/fonts/fa_list.png',
    '/images/TopLogo.png',
    '/images/fonts/fa_edit.png',
    '/images/onlineStatus.png',
    '/images/offlineStatus.png',
    '/images/forspinicon.png',
    '/images/backspinicon.png',
    '/images/fonts/fa_backward_arrow.png',
    '/images/fonts/fa_offlinedelete.png',
    '/images/fonts/fa_qrcode.png',
    '/images/fonts/fa_ImageGallery.png',
    '/images/fonts/fa_SearchImage.png',
    '/images/fonts/fa_delete.png',
    '/images/fonts/fa_save.png',
    '/images/fonts/fa_add.png',
    '/images/fonts/fa_book.png',
    '/images/fonts/fa_user.png',
    '/images/online.png',
    '/images/offlinelogo.png',
    '/images/LockedIn.png',
    '/images/Green.jpg',
    '/images/Red.jpg',
    '/images/Yellow.jpg',
];

// Install Event - Cache resources
self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(CACHE_NAME).then(function (cache) {
            console.log('Opened cache:', CACHE_NAME);
            return cache.addAll(urlsToCache).catch((err) => {
                console.error('Failed to cache resources during install:', err);
            });
        })
    );
    self.skipWaiting();
});

// Activate Event - Clean up old caches
self.addEventListener('activate', function (event) {
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (cacheName.startsWith('OfflineRegisterCache') && cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
    self.clients.claim();
});

// Fetch Event - Handle requests with page-specific logic
self.addEventListener('fetch', function (event) {
    const requestUrl = new URL(event.request.url);

    if (requestUrl.pathname === '/OfflineRegister.html') {
        event.respondWith(
            caches.match('OfflineRegister.html').then(function (response) {
                if (response) {
                    console.log('Serving cached OfflineRegister page:', requestUrl.href);
                    return response;
                }
                return fetch(event.request).catch(function () {
                    return new Response('You are offline, and OfflineRegister.html is not cached.');
                });
            })
        );
    } else if (requestUrl.pathname === '/OfflineSiteRequest.html') {
        event.respondWith(
            caches.match('OfflineSiteRequest.html').then(function (response) {
                if (response) {
                    console.log('Serving cached OfflineSiteRequest page:', requestUrl.href);
                    return response;
                }
                return fetch(event.request).catch(function () {
                    return new Response('You are offline, and OfflineSiteRequest.html is not cached.');
                });
            })
        );
    } else if (requestUrl.pathname === '/OfflineInspectionForm.html') {
        event.respondWith(
            caches.match('OfflineInspectionForm.html').then(function (response) {
                if (response) {
                    console.log('Serving cached OfflineInspectionForm page:', requestUrl.href);
                    return response;
                }
                return fetch(event.request).catch(function () {
                    return new Response('You are offline, and OfflineInspectionForm.html is not cached.');
                });
            })
        );
    } else {
        event.respondWith(
            caches.match(event.request).then(function (response) {
                return response || fetch(event.request).catch(function () {
                    return new Response('You are offline, and the requested resource is not cached.');
                });
            })
        );
    }
});
