

app.controller("leagueJoinCtrl", function ($scope, $rootScope, $http, $routeParams, DefaultFactory) {

    toastr.options = {
        "closeButton": true,
        "showDuration": "300"
    };

    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: {},
        header: "Join",
        loaded: false
    };
    $scope.joinObject = { league_id: $routeParams.id };
    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id, "League")
    .then(function (dto) {

        $scope.detailsScope.header = "Join " + dto.name
        $scope.detailsScope.detailsObject = dto;
        $scope.detailsScope.loaded = true;
    });

    $scope.join = function (isValid) {

        $scope.detailsScope.submitted = true;

        //Join
        if (isValid) {
            $http.post('../league/Join', $scope.joinObject).success(function (response) {
                if (response == false)
                    toastr.error('Incorrect Password');
                else
                    window.location.href = '../user/team/details/' + response;
            });
        }

    }

    $scope.disableJoin = function ()
    {
        if (!$scope.joinObject.league_team_name)
            return true;

        if (!$scope.detailsScope.loaded)
            return true;

        if ($scope.detailsScope.detailsObject.max_players && $scope.detailsScope.detailsObject.league_teams.length >= $scope.detailsScope.detailsObject.max_players)
            return true;

        if ($scope.detailsScope.detailsObject.lock_date && $rootScope.formatJSONDateTimeToDateObject($scope.detailsScope.detailsObject.lock_date) < new Date())
            return true

        return false;

    }

});

