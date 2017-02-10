


app.controller("accountDetailsCtrl", function ($scope, $rootScope, $routeParams, $http) {


    //#region Variables

    $scope.detailsScope = {
        detailsObject: {},
        header: "Loading...",
        loading: true
    };

    //#endregion

    //#region Get Data

    $http.get('../Transaction/Account').then(function (results) {

            $scope.detailsScope.detailsObject = results.data;

            $scope.detailsScope.header = "My Account";

            $scope.detailsScope.loading = false;
    });

    //#endregion


    //#region Button Clicks

    $scope.collectClick = function () {

        toastr["info"]("I was thinking we would ask them for there email here.  Then shoot out a email to TeamLineUp saying 'Person X wants to be PAID'");

    }


    //#endregion
});