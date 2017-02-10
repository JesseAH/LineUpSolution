


app.controller("teamDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables

    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        teamid: $routeParams.teamid,
        gameid: $routeParams.gameid,
        detailsObject: {},
        header: "Loading...",
        loading: true,
        submitted: false
    };


    //#endregion

    //#region Get Data

    DefaultFactory.Details($scope.detailsScope.teamid, "Team")
        .then(function (dto) {
            $scope.detailsScope.detailsObject = dto;

            if ($routeParams.teamid == 0)
                $scope.detailsScope.header = "Create Side";
            else
                $scope.detailsScope.header = "Edit Side";
            $scope.detailsScope.loading = false;
        });

    //#endregion


    //#region Button Clicks

    $scope.save = function (isValid) {
        if (!isValid) {
            toastr.warning('One or more of the fields is not properly filled in.')
            return;
        }

        $scope.detailsScope.submitted = true;

        if ($scope.detailsScope.detailsObject.game_type_id == undefined || $scope.detailsScope.detailsObject.game_type_id == 0)
            $scope.detailsScope.detailsObject.game_type_id = $scope.detailsScope.gameid;

        //call save then navigate to game dashboard
        DefaultFactory.Save($scope.detailsScope.detailsObject, "Team")
        .then(function (data) {
            window.location.href = '../user/game/dashboard/' + $scope.detailsScope.gameid;
        });
    }


    //#endregion
});