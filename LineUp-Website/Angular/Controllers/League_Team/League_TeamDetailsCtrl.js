

app.controller("league_teamDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Team: "
    };

    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id, "League_Team")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = dto.name;
            $scope.displayedCollection = dto.rounds;
        });

    //#endregion
});

