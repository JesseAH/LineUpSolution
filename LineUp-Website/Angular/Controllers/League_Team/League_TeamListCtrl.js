

app.controller("league_teamListCtrl", function ($scope, $rootScope, DefaultFactory) {

    //#region Variables

    $scope.header = "My Leagues";
    $scope.loading = true;
        //#endregion

        //#region Get Data

        DefaultFactory.List("League_Team")
            .then(function (dtos) {
                $scope.loading = false;
                $scope.displayedCollection = dtos;

            });

        //#endregion
});

