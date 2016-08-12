

app.controller("league_teamDetailsCtrl", function ($scope, $rootScope, $http, $routeParams, DefaultFactory) {


    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Team: ",
        loading: true
    };
    $scope.leagueTeamID = $routeParams.id;
    $scope.today = new Date();

    //#endregion

    //#region Get Data

    $http.get('../Round/Team/' + $routeParams.id).then(function (results) {
        $scope.displayedCollection = results.data;
    });

    DefaultFactory.Details($routeParams.id, "League_Team")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = dto.name;
            $scope.detailsScope.loading = false;
        });

    //#endregion

    $scope.isPickable = function (lockDate)
    {
        var date = new Date(parseInt(lockDate.substr(6)));
        return date > new Date();
    }




});

