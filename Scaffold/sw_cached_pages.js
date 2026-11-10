
////const cacheName = 'v5';

////const cacheAssets = [
////    //'Offline.html',
////    //'OfflineInspectionList.html',
////    '/Offline.html',
////    '/OfflineRegister.html',
////    '/OfflineInspectionForm.html',
////    '/OfflineInspectionList.html',
////    '/Content/fonts/css/font-awesome.min.css',
////    '/Content/Css/bootstrap.min.css',
////    '/Content/Css/jquery.dataTables.min.css',
////    '/Content/Css/CustomAlert.css',
////    '/Content/Css/w3.css',
////    '/Content/Css/SecStyle.css',
////    '/Content/Css/signature-pad.css',
////    '/Content/Css/bootstrap-datetimepicker.min.css',
////    '/Content/Css/Offline-bootstrap-glyphicons.css',
////    '/Content/Css/jquery.datetimepicker.min.css',
////    '/Content/js/jquery.min.js',
////    '/Content/js/bootstrap.min.js',
////    '/Content/js/bootbox.min.js',
////    '/Content/js/jquery.dataTables.min.js',
////    '/Content/js/jquery-ui.min.js',
////    '/Content/js/videoInsp.js',
////    '/Content/js/cameraInsp.js',
////    '/Content/js/signature_pad.js',
////    '/Content/js/html2canvas.js',
////    '/Content/js/sketch.min.js',
////    '/Content/js/signInsp.js',
////    '/Content/js/bootstrap-datetimepicker.min.js',
////    '/Content/js/jquery.datetimepicker.js',
////    '/Content/js/CustomAlert.js',
////    '/Content/js/OfflineInspection.js',
////    '/Content/js/OfflineInspectionForm.js',
////    '/Content/js/OfflineCustomeAlert.js',
////    '/Content/Css/jquery-ui.css',
////    '/Content/js/jszip.min.js',
////    '/Content/js/dataTables.buttons.min.js',
////    '/Content/js/html5.min.js',
////    '/Content/js/OfflineEncryptDecript.js',
////    '/Content/js/crypto-js.min.js',
////    '/images/noimg.jpg',
////    '/Images/loading.gif',
////    '/img/logo1.png',
////    '/images/lock.png',
////    '/images/fonts/fa_list.png',
////    '/images/TopLogo.png',
////    '/images/fonts/fa_edit.png',
////    '/images/onlineStatus.png',
////    '/images/offlineStatus.png',
////    '/images/forspinicon.png',
////    '/images/backspinicon.png',
////    '/images/fonts/fa_backward_arrow.png',
////    '/images/fonts/fa_offlinedelete.png',
////    '/images/fonts/fa_qrcode.png',
////    '/images/fonts/fa_ImageGallery.png',
////    '/images/fonts/fa_SearchImage.png'

////]

////// Call Install Event
////self.addEventListener('install', e => {
////    console.log('Service Worker: Installed');

////    e.waitUntil(
////        caches
////            .open(cacheName)
////            .then(cache => {
////                console.log('Service Worker: Caching Files');
////                cache.addAll(cacheAssets);
////            })
////            .then(() => self.skipWaiting())
////    );
////});

////// Call Activate Event
////self.addEventListener('activate', e => {
////    console.log('Service Worker: Activated');
////    //Remove unwanted caches
////    e.waitUntil(
////        caches.keys().then(cacheNames => {
////            return Promise.all(
////                cacheNames.map(cache => {
////                    if (cache !== cacheName) {
////                        console.log('Service Worker: Clearing Old Cache');
////                        return caches.delete(cache);
////                    }
////                })
////            );
////        })
////    );
////});


////// Call Fetch Event
////self.addEventListener('fetch', e => {
////    console.log('Service Worker: Fetching');
////    e.respondWith(fetch(e.request).catch(() => caches.match(e.request)));
////});


//const cacheName = 'offline-Register-v10';

//const cacheAssets = [

//        '/OfflineRegister.html',
//        '/OfflineInspectionForm.html',
//        '/OfflineInspectionList.html',
//        '/Content/fonts/css/font-awesome.min.css',
//        '/Content/Css/bootstrap.min.css',
//        '/Content/Css/jquery.dataTables.min.css',
//        '/Content/Css/CustomAlert.css',
//        '/Content/Css/w3.css',
//        '/Content/Css/SecStyle.css',
//        '/Content/Css/signature-pad.css',
//        '/Content/Css/bootstrap-datetimepicker.min.css',
//        '/Content/Css/Offline-bootstrap-glyphicons.css',
//        '/Content/Css/jquery.datetimepicker.min.css',
//        '/Content/js/jquery.min.js',
//        '/Content/js/bootstrap.min.js',
//        '/Content/js/bootbox.min.js',
//        '/Content/js/jquery.dataTables.min.js',
//        '/Content/js/jquery-ui.min.js',
//        '/Content/js/videoInsp.js',
//        '/Content/js/cameraInsp.js',
//        '/Content/js/signature_pad.js',
//        '/Content/js/html2canvas.js',
//        '/Content/js/sketch.min.js',
//        '/Content/js/signInsp.js',
//        '/Content/js/bootstrap-datetimepicker.min.js',
//        '/Content/js/jquery.datetimepicker.js',
//        '/Content/js/CustomAlert.js',
//        '/Content/js/OfflineInspection.js',
//        '/Content/js/OfflineInspectionForm.js',
//        '/Content/js/OfflineCustomeAlert.js',
//        '/Content/js/OfflineRegister.js',
//        '/Content/js/OfflineRegisterList.js',
//        '/Content/js/OfflineScaffoldMaterialList.js',
//        '/Content/js/OfflineSiteRequest.js',
//        '/Content/Css/jquery-ui.css',
//        '/Content/js/jszip.min.js',
//        '/Content/js/dataTables.buttons.min.js',
//        '/Content/js/html5.min.js',
//        '/Content/js/OfflineEncryptDecript.js',
//        '/Content/js/crypto-js.min.js',
//        '/images/noimg.jpg',
//        '/Images/loading.gif',
//        '/img/logo1.png',
//        '/images/lock.png',
//        '/images/fonts/fa_list.png',
//        '/images/TopLogo.png',
//        '/images/fonts/fa_edit.png',
//        '/images/onlineStatus.png',
//        '/images/offlineStatus.png',
//        '/images/forspinicon.png',
//        '/images/backspinicon.png',
//        '/images/fonts/fa_backward_arrow.png',
//        '/images/fonts/fa_offlinedelete.png',
//        '/images/fonts/fa_qrcode.png',
//        '/images/fonts/fa_ImageGallery.png',
//        '/images/fonts/fa_SearchImage.png'
//];

//// Install Event
//self.addEventListener('install', (event) => {
//    console.log('Service Worker: Installing and caching essential assets');
//    event.waitUntil(
//        caches.open(cacheName).then((cache) => {
//            console.log('Service Worker: Caching Files');
//            return cache.addAll(cacheAssets);
//        })
//    );
//});

//// Activate Event
//self.addEventListener('activate', (event) => {
//    console.log('Service Worker: Activated');
//    event.waitUntil(
//        caches.keys().then((cacheNames) => {
//            return Promise.all(
//                cacheNames.map((cache) => {
//                    if (cache !== cacheName) {
//                        console.log(`Service Worker: Removing old cache: ${cache}`);
//                        return caches.delete(cache);
//                    }
//                })
//            );
//        })
//    );
//});

//// Fetch Event
//self.addEventListener('fetch', (event) => {
//    console.log('Service Worker: Fetching', event.request.url);

//    event.respondWith(
//        caches.match(event.request).then((cachedResponse) => {
//            // Return cached response if found
//            if (cachedResponse) {
//                return cachedResponse;
//            }

//            // Otherwise, fetch from the network and cache the response
//            return fetch(event.request)
//                .then((response) => {
//                    // Check if valid response before caching
//                    if (!response || response.status !== 200 || response.type !== 'basic') {
//                        return response;
//                    }

//                    // Clone the response for caching
//                    const responseClone = response.clone();
//                    caches.open(cacheName).then((cache) => {
//                        cache.put(event.request, responseClone);
//                    });

//                    return response;
//                })
//                .catch(() => {
//                    // Offline fallback for HTML pages
//                    if (event.request.headers.get('accept')?.includes('text/html')) {
//                        return caches.match('/OfflineRegister.html');
//                    }
//                });
//        })
//    );
//});

// Cache versioning
var CACHE_VERSION = '54';  // Update this version when you change cache contents
var CACHE_NAME = 'my-OfflineJobRegister-cache-' + CACHE_VERSION;

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
    '/images/fonts/FaFaSave.png',
    '/images/online.png',
    '/images/offlinelogo.png',
    '/images/LockedIn.png',
    '/images/Green.jpg',
    '/images/Red.jpg',
    '/images/Yellow.jpg'
];
// Install Event - Cache essential resources
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
                    if (cacheName.startsWith('my-OfflineJobRegister-cache-') && cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);  // Delete old caches
                    }
                })
            );
        })
    );
    self.clients.claim();  // Take control of uncontrolled clients immediately
});

// Fetch Event - Handle requests
self.addEventListener('fetch', function (event) {
    const requestUrl = new URL(event.request.url);

    // Handle specific pages explicitly
    if (requestUrl.pathname === '/OfflineRegister.html') {
        event.respondWith(
            caches.match('OfflineRegister.html').then(function (response) {
                if (response) {
                    console.log('Serving cached OfflineRegister page:', requestUrl.href);
                    return response;
    }

                // If offline and not in the cache, return a fallback message
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

                // If offline and not in the cache, return a fallback message
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

                // If offline and not in the cache, return a fallback message
                return fetch(event.request).catch(function () {
                    return new Response('You are offline, and OfflineInspectionForm.html is not cached.');
                    });
                })
        );
                    }
    else {
        // Handle other requests as usual
        event.respondWith(
            caches.match(event.request).then(function (response) {
                return response || fetch(event.request).catch(function () {
                    return new Response('You are offline, and the requested resource is not cached.');
                });
        })
    );
    }
});

//// Install Event - Cache essential resources
//self.addEventListener('install', function (event) {
//    event.waitUntil(
//        caches.open(CACHE_NAME).then(function (cache) {
//            console.log('Opened cache:', CACHE_NAME);
//            return cache.addAll(urlsToCache);
//        })
//    );
//    self.skipWaiting();  // Activate the new service worker immediately
//});

//// Activate Event - Clean up old caches
//self.addEventListener('activate', function (event) {
//    event.waitUntil(
//        caches.keys().then(function (cacheNames) {
//            console.log('Existing caches:', cacheNames);  // Log all existing caches for debugging
//            return Promise.all(
//                cacheNames.map(function (cacheName) {
//                    if (cacheName.startsWith('my-OfflineJobRegister-cache-') && cacheName !== CACHE_NAME) {
//                        console.log('Deleting old cache:', cacheName);
//                        return caches.delete(cacheName);  // Delete old caches
//                    }
//                })
//            );
//        })
//    );
//    self.clients.claim();  // Take control of uncontrolled clients immediately
//});

//// Fetch Event - Handle requests
//self.addEventListener('fetch', function (event) {
//    const requestUrl = new URL(event.request.url);

//    // Normalize the URL by removing query parameters
//    if (requestUrl.pathname === '/OfflineRegister.html') {
//        event.respondWith(
//            caches.match('OfflineRegister.html').then(function (response) {
//                if (response) {
//                    console.log('Serving cached offline page (normalized):', requestUrl.href);
//                    return response;
//                }

//                // If offline and not in the cache, return a fallback message
//                return fetch(event.request).catch(function () {
//                    return new Response('You are offline, and the requested page is not cached.');
//                });
//            })
//        );
//    }
//    else if (requestUrl.pathname === '/OfflineSiteRequest.html') {
//        event.respondWith(
//            caches.match('OfflineSiteRequest.html').then(function (response) {
//                if (response) {
//                    console.log('Serving cached offline page (normalized):', requestUrl.href);
//                    return response;
//                }

//                // If offline and not in the cache, return a fallback message
//                return fetch(event.request).catch(function () {
//                    return new Response('You are offline, and the requested page is not cached.');
//                });
//            })
//        );
//    }
//    else{

//    }

//    //} else {
//    //    // Handle other requests as usual
//    //    event.respondWith(
//    //        caches.match(event.request).then(function (response) {
//    //            return response || fetch(event.request);
//    //        })
//    //    );
//    //}
//});

