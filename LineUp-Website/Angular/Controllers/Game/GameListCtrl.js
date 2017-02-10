

app.controller("gameListCtrl", function ($scope, $rootScope, DefaultFactory) {

    //#region Variables

    $scope.header = "My Games";
    $scope.loading = true;
    //#endregion

    //#region Get Data

    DefaultFactory.List("Game")
        .then(function (dtos) {
            $scope.loading = false;
            $scope.displayedCollection = dtos;

        });

    //#endregion
});

