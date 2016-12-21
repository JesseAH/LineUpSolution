

app.controller("leagueSelectCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: null,
        header: "Select a League to Join",
        loading: true
    };


    //#endregion

    //#region Get Data

    DefaultFactory.ObjectLookupList("League")
    .then(function (dtos) {

        $scope.detailsScope.loading = false;
        $scope.lookupOptions = dtos;

    });


    $scope.select = function (league) {
        $scope.detailsScope.detailsObject = league;
    }

});

