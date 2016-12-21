

app.controller("leagueCreateCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: {},
        header: "Create League",
        loading: true,
        submitted: false
    };

    //#endregion

    //#region Get Data

    DefaultFactory.ObjectLookupList("League")
    .then(function (dtos) {
        $scope.detailsScope.loading = false;
        $scope.lookupOptions = dtos;
    });


    DefaultFactory.Details(0, "League")
        .then(function (dto) {
            $scope.detailsScope.detailsObject = dto;
        });

    //#endregion

    $scope.gameChange = function (selectedID)
    {
        var result = $.grep($scope.lookupOptions.GameTypes, function (e) { return e.Lookup_ID == selectedID; });
        $scope.gameDescription = result[0].Description;
    }

    $scope.create = function (isValid)
    {
        $scope.detailsScope.submitted = true;

        //Validate combo box
        if (!$scope.detailsScope.detailsObject.game_type_id > 0)
            $scope.detailsForm.gameTypeID.$invalid = true;
        else
            $scope.detailsForm.gameTypeID.$invalid = false;

        if (isValid && !$scope.detailsForm.gameTypeID.$invalid) {


            DefaultFactory.Save($scope.detailsScope.detailsObject, "League")
            .then(function (data) {
                window.location.href = '../user/league/join/' + data.data.id;
            });

        }
        else
        {
            //toastr warning
        }
    }
});





app.controller("gameDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: {},
        header: "Loading...",
        loading: true,
        submitted: false
    };

    //#endregion

    //#region Get Data

    DefaultFactory.ObjectLookupList("Game")
    .then(function (dtos) {
        $scope.detailsScope.loading = false;
        $scope.lookupOptions = dtos;
    });


    DefaultFactory.Details(13, "Game")
        .then(function (dto) {
            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = dto.name;
        });

    //#endregion


    //$scope.create = function (isValid) {
    //    $scope.detailsScope.submitted = true;

    //    //Validate combo box
    //    if (!$scope.detailsScope.detailsObject.game_type_id > 0)
    //        $scope.detailsForm.gameTypeID.$invalid = true;
    //    else
    //        $scope.detailsForm.gameTypeID.$invalid = false;

    //    if (isValid && !$scope.detailsForm.gameTypeID.$invalid) {


    //        DefaultFactory.Save($scope.detailsScope.detailsObject, "Game")
    //        .then(function (data) {
    //            window.location.href = '../user/league/join/' + data.data.id;
    //        });

    //    }
    //    else {
    //        //toastr warning
    //    }
    //}
});

