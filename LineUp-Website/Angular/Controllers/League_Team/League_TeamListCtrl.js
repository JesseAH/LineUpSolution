

app.controller("league_teamListCtrl", function ($scope, $rootScope, DefaultFactory) {

    //#region Variables

    $scope.header = "My Leagues";
        //#endregion

        //#region Get Data

        DefaultFactory.List("League_Team")
            .then(function (dtos) {

                $scope.displayedCollection = dtos;

            });

        //#endregion
});

