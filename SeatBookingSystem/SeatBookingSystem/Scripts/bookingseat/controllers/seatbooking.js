seatBooking.controller('seatBookingController', function ($scope, $http, $q) {
    $scope.seats = meetup.seats;
    $scope.data = {
        seats: {
            booked: [],
            selected: [],
            getBookedSeats: function () {
                $scope.data.seats.destory = [];
                var config = { cache: false, params: { meetupId: 0 } },
                    url = "/seat/getallbookedseats";
                $http.get(url, config)
                    .then(function (response) {
                        var booked = response.data;
                        if (booked.length) {
                            booked.map(function (seat) {
                                $scope.data.seats.booked.push(seat.name)
                            })
                        }
                    }, function (errorReponse) {
                        alert(errorReponse.data);
                    })
            },
            book: function () {
                if (!$scope.data.seats.selected.length) {
                    alert("You haven't selected one seat!");
                    return;
                }

                if (!confirm("Do you want to book these seats?")) {
                    return;
                }

                $http.post(
                    "/seat/bookseats",
                    {
                        models: $scope.data.seats.selected, meetupId: 0, accounter: ""
                    }).then(
                    function (response) {
                        $scope.data.seats.selected.map(function (seat) {
                            $scope.data.seats.booked.push(seat.name);
                        })
                        $scope.data.seats.selected = [];
                        alert("Successfully booked!");
                    }, function (error) {
                        alert(error.data);
                    })
            },
            isBooked: function (name) {
                return $scope.data.seats.booked.indexOf(name) > -1;
            },
            isSelected: function (name) {
                var seats = [];
                $scope.data.seats.selected.map(function (selected) {
                    seats.push(selected.name);
                })

                return seats.indexOf(name) > -1;
            },
            addSelected: function () {
                var duplicated = false;
                $scope.data.seats.selected.map(function (selected, index) {
                    if (selected.owner == $scope.data.seatInput.owner || selected.email == $scope.data.seatInput.email) {
                        alert("The owner or email has been used!");
                        duplicated = true;
                        return;
                    }
                })

                if (duplicated) {
                    $('.select-seat-modal').modal('hide');
                    return;
                }

                $scope.data.seats.selected.push($scope.data.seatInput);
                $('.select-seat-modal').modal('hide');
            },
            initInput: function (name) {
                if ($scope.data.seats.booked.indexOf(name) > -1) {
                    return;
                }

                var spliced = false;
                $scope.data.seats.selected.map(function (selected, index) {
                    if (selected.name == name) {
                        $scope.data.seats.selected.splice(index, 1);
                        spliced = true;
                    }
                })

                if (spliced) {
                    return;
                }

                $('.select-seat-modal').modal('show');
                $scope.data.seatInput = { name: name, owner: '', email: '' };
            },
            destory: function () {
                $scope.data.seats.booked = [];
                $scope.data.seats.error = "";
            }
        },
        seatInput: {
            name: '',
            owner: '',
            email: ''
        },
        error: ""
    }

    $scope.data.seats.getBookedSeats();
})