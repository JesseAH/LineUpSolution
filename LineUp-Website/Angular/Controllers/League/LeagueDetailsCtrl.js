

app.controller("leagueDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "League: "
    };

    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id,"League")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = "League: " + dto.name;
            $scope.displayedCollection = dto.rounds;
        });

    //#endregion
});

