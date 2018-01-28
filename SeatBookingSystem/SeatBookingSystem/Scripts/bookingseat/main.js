"use strict";

var seatBooking = angular.module("seatBooking", ['ui.bootstrap', 'ngSanitize']);
seatBooking.factory("httpProviderInterceptor", ["$q", function ($q) {
    return {
        responseError: function (rejection) {
            return $q.reject(rejection);
        }
    };
}]);
seatBooking.config(["$httpProvider", function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";

    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }
    if (!$httpProvider.defaults.headers.put) {
        $httpProvider.defaults.headers.put = {};
    }
    if (!$httpProvider.defaults.headers.post) {
        $httpProvider.defaults.headers.post = {};
    }

    $httpProvider.defaults.headers.get["If-Modified-Since"] = "Mon, 26 Jul 1997 05:00:00 GMT";
    $httpProvider.defaults.headers.get["Cache-Control"] = "no-cache";
    $httpProvider.defaults.headers.get["Pragma"] = "no-cache";

    $httpProvider.interceptors.push("httpProviderInterceptor");
}]);