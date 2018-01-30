seatBooking.controller('seatReleasingController', function ($scope, $http, $q) {
    $scope.seats = meetup.seats;
    $scope.data = {
        seats: {
            tip:"",
            booked: [],
            selected: [],
            getUserBookedSeats: function () {
                $scope.data.seats.destory();
                var config = { cache: false, params: { meetupId: 0 } },
                    url = "/seat/getuserbookedseats";
                $http.get(url, config)
                    .then(function (response) {
                        var booked = response.data;
                        if (booked.length) {
                            booked.map(function (seat) {
                                $scope.data.seats.booked.push(seat.name)
                            })
                        }
                        if (!$scope.data.seats.booked.length) {
                            $scope.data.seats.tip = "You have no seats to release!"
                        }
                    }, function (errorReponse) {
                        alert(errorReponse.data);
                    })
            },
            release: function () {
                if (!$scope.data.seats.selected.length) {
                    alert("You haven't selected one seat!");
                    return;
                }

                if (!confirm("Do you want to release these seats?")) {
                    return;
                }

                $http.post(
                    "/seat/releaseseats",
                    {
                        seatNames: $scope.data.seats.selected,
                        meetupId: 0
                    }).then(
                    function (response) {
                        $scope.data.seats.destory();
                        alert("Successfully released!");
                    }, function (error) {
                        alert(error.data);
                    })
            },
            isBooked: function (name) {
                return $scope.data.seats.booked.indexOf(name) > -1;
            },
            isSelected: function (name) {
                return $scope.data.seats.selected.indexOf(name) > -1;
            },
            select: function (name) {
                if ($scope.data.seats.booked.indexOf(name) > -1) {
                    if ($scope.data.seats.selected.indexOf(name) < 0) {
                        $scope.data.seats.selected.push(name);
                    }
                }
            },
            destory: function () {
                $scope.data.seats.booked = [];
                $scope.data.seats.selected = [];
                $scope.data.seats.error = "";
            }
        },
        error: ""
    }

    $scope.data.seats.getUserBookedSeats();
})