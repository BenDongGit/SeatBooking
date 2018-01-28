seatBooking.controller('seatController', function ($scope, $http, $q) {
    $scope.data = {
        result: null,
        seats: {
            others: [],
            selected: [],
            existings: [],
            releases: [],
            tips: "",
            getBookedSeats: function () {
                $scope.data.destory();
                var config = {
                    cache: false,
                    params: {}
                }
                var url = "/seat/getbookedseats";
                $http.get(url, config)
                    .then(function (response) {
                        var others = response.data.seatsBookedByOthers;
                        if (others.length) {
                            others.map(function (seat) {
                                $scope.data.seats.others.push(seat.id)
                            })
                        }
                        var selected = response.data.seatsBookedByCurrentUser;
                        if (selected.length) {
                            selected.map(function (seat) {
                                $scope.data.seats.selected.push(seat.id);
                                $scope.data.seats.existings.push(seat.id);
                            });
                        }
                    },
                    function (errorReponse) {
                        $scope.data.error = errorReponse.data;
                        alert($scope.data.error);
                    })
            },
            book: function () {
                $scope.data.seats.releases = [];
                var existingsSeats = $scope.data.seats.existings;
                if (existingsSeats.length) {
                    existingsSeats.map(function (seat, index) {
                        if ($scope.data.seats.selected.indexOf(seat) == -1) {
                            $scope.data.seats.releases.push(seat);
                        }
                    })
                }

                var url = "/seat/update",
                    params = {
                        bookingSeatIds: $scope.data.seats.selected,
                        releasingSeatIds: $scope.data.seats.releases
                    };

                $http.post(url, params).then(
                    function (response) {
                        $scope.data.seats.existings = [];
                        $scope.data.seats.selected.map(function (seat) {
                            $scope.data.seats.existings.push(seat);
                        })
                        alert("Congratulation! You've booked successfully!");
                    },
                    function (errorReponse) {
                        alert(errorReponse.data);
                    })
            }
        },
        error: "",
        destory: function () {
            $scope.data.seats.others = [];
            $scope.data.seats.selected = [];
            $scope.data.seats.error = "";
        },
        seatNames: [],

    }
    $scope.data.seats.getBookedSeats();
    $scope.$watch("data.seats.selected", function () {
        if ($scope.data.seats.selected.length) {
            $scope.data.seats.tips = "You have booked " + $scope.data.seats.selected.join(',');
        }
        else {
            $scope.data.seats.tips = "No seats are selected";
        }
    }, true)
    var rowNames = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"],
        colNames = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    for (var row = 0; row < rowNames.length; row++) {
        var names = [];
        for (var col = 1; col < colNames.length; col++) {
            names.push(rowNames[row] + col);
        }
        $scope.data.seatNames.push(names);
    }

    $scope.bookedByOthers = function (name) {
        return $scope.data.seats.others.indexOf(name) > -1;
    }

    $scope.bookedByCurrentUser = function (name) {
        return $scope.data.seats.selected.indexOf(name) > -1;
    }

    $scope.addSeats = function (name) {
        if ($scope.data.seats.others.indexOf(name) > -1) {
            return;
        }
        var index = $scope.data.seats.selected.indexOf(name);
        if (index > -1) {
            $scope.data.seats.selected.splice(index, 1);
            return;
        }
        if ($scope.data.seats.selected.length > 3) {
            alert("One people can't book more than four seats.");
            return;
        }
        $scope.data.seats.selected.push(name);
    }
})