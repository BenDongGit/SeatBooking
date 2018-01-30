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

var meetup = {
    seats: []
};
(function () {
    var row = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"],
          col = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    for (var r = 0; r < row.length; r++) {
        var names = [];
        for (var c = 1; c < col.length; c++) {
            names.push(row[r] + c);
        }
        meetup.seats.push(names);
    }
})()
