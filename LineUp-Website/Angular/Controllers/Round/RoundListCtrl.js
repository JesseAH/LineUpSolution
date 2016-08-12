

app.controller("roundListCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {

    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Getting Round Information... "
    };


    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id, "Round")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = dto.name;
            $scope.displayedCollection = dto.matches;
        });

    //#endregion


});

