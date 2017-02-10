


app.controller("roundManagerCtrl", function ($scope, $rootScope, $routeParams, $http, DefaultFactory) {

    //#region Variables

    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        id: $routeParams.roundId,
        gameId: $routeParams.gameId,
        header: "Loading...",
        loading: true,
        submitted: false
    };


    //#endregion

    //#region Get Data


    $http.get('../round/lookups/' + $scope.detailsScope.gameId).success(function (dtos) {
        $scope.lookupOptions = dtos;
        getDetails();
        //toastr.warning('Error loading round information.');
    });


    function getDetails() {

        DefaultFactory.Details($scope.detailsScope.id, "Round")
            .then(function (dto) {

                //start date
                if (dto.start_date != null)
                {
                    var sdate = dto.start_date;
                    sdate = sdate.substr(6);
                    sdate = new Date(parseInt(sdate));
                }

                //lock date
                if (dto.lock_date != null) {
                    var ldate = dto.lock_date;
                    ldate = ldate.substr(6);
                    ldate = new Date(parseInt(ldate));
                }

                //end date
                if (dto.end_date != null) {
                    var edate = dto.end_date;
                    edate = edate.substr(6);
                    edate = new Date(parseInt(edate));
                }


                $scope.detailsScope.detailsObject = dto;

                if ($scope.detailsScope.id == 0)
                {
                    $scope.detailsScope.header = "Create New Round";
                    $scope.detailsScope.detailsObject.game_type_id = $scope.detailsScope.gameId;
                }
                else
                {
                    $scope.detailsScope.header = "Edit Round: " + $scope.detailsScope.detailsObject.name;
                }

                $scope.detailsScope.loading = false;

                $scope.detailsScope.detailsObject.start_date = sdate;
                $scope.detailsScope.detailsObject.lock_date = ldate;
                $scope.detailsScope.detailsObject.end_date = edate;
            });

    }

    //#endregion


    //#region Button Clicks

    $scope.save = function (isValid) {
        if (!isValid || $scope.detailsScope.detailsObject.start_date == "" || $scope.detailsScope.detailsObject.start_date == "") {
            toastr.warning('One or more of the fields is not properly filled in.');
            return;
        }

        $scope.detailsScope.submitted = true;

        if ($scope.detailsScope.detailsObject.game_type_id == undefined || $scope.detailsScope.detailsObject.game_type_id == 0)
            $scope.detailsScope.detailsObject.game_type_id = $scope.detailsScope.gameid;

        //Convert Date Times to Server Timezone (EST)
        $scope.detailsScope.detailsObject.lock_date = $rootScope.convertToServerTimeZone($scope.detailsScope.detailsObject.lock_date);
        $scope.detailsScope.detailsObject.start_date = $rootScope.convertToServerTimeZone($scope.detailsScope.detailsObject.start_date);
        $scope.detailsScope.detailsObject.end_date = $rootScope.convertToServerTimeZone($scope.detailsScope.detailsObject.end_date);

        //call save then navigate to game dashboard
        DefaultFactory.Save($scope.detailsScope.detailsObject, "Round")
        .then(function (data) {
            window.location.href = '../user/game/dashboard/' + $scope.detailsScope.detailsObject.game_type_id;
        });
    }

    $scope.addMatchClick = function ()
    {
        if ($scope.newMatch.team1_id == null) {
            toastr.warning('You must fill in Side 1.');
            return;
        }

        if ($scope.newMatch.team2_id == null) {
            toastr.warning('You must fill in Side 2.');
            return;
        }

        var obj = {
            team1_id: $scope.newMatch.team1_id,
            team2_id: $scope.newMatch.team2_id,
            description: $scope.newMatch.description
        };

        $scope.newMatch.team1_id = null;
        $scope.newMatch.team2_id = null;
        $scope.newMatch.description = null;

        $scope.detailsScope.detailsObject.matches.push(obj);
    }

    $scope.removeMatchClick = function (match)
    {
        $scope.detailsScope.detailsObject.matches = $scope.detailsScope.detailsObject.matches.filter(function (m) {
            return m !== match;
        });
    }

    //#endregion

    $scope.winnerFilter = function (team1, team2) {
        return function (team) {
            return (team.id == team1 || team.id == team2);
        }
    };
});