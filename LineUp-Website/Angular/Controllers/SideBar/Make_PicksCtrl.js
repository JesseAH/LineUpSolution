

app.controller("make_PicksCtrl", function ($scope, $rootScope, $http) {

    //#region Variables

    $scope.teams = [];
    //#endregion

    //#region Get Data

    $http.get('../Pick/Teams').then(function (results) {
        $scope.pickableList = results.data;
    });

    //#endregion
});

