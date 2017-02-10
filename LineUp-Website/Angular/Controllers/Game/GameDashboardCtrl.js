


app.controller("gameDashboardCtrl", function ($scope, $rootScope, $routeParams, $http, DefaultFactory) {


    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: {},
        header: "Loading...",
        loading: true,
        submitted: false,
        id: $routeParams.id
    };

    $scope.openToEdit = false;

    //#endregion

    //#region Get Data

    DefaultFactory.ObjectLookupList("Game")
    .then(function (dtos) {
        $scope.lookupOptions = dtos;
    });


    DefaultFactory.Details($scope.detailsScope.id, "Game")
        .then(function (dto) {

            if (dto.completed == null)
                dto.completed = false;

            $scope.openToEdit = !dto.completed;
            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = dto.name + ' Dashboard';
            $scope.detailsScope.loading = false;
        });

    //#endregion


    $scope.complete = function ()
    {
        $scope.comepleteClicked = true;
        var confirmation = confirm("Are you sure you want to complete this game?  Once a game is completed no changes can be made and all final winnings are calculated.");
        if (confirmation == true) {
            var headerHolder = $scope.detailsScope.detailsObject.name + ' Dashboard';
           $scope.detailsScope.header = 'Marking Game As Complete...';
            $scope.detailsScope.loading = true;
            $http.get('../Game/Complete/' + $scope.detailsScope.id)
            .then(function (dtos) {
                toastr["success"]("Your game has been marked as completed and all winnings have been calculated. <br> <b>No further changes will be allowed to this game </b>.");
                $scope.openToEdit = false;
                $scope.detailsScope.loading = false;
                $scope.detailsScope.header = headerHolder;
            })
            .catch(function (error) {
                $scope.detailsScope.loading = false;
                $scope.detailsScope.header = headerHolder;
                toastr["error"]("There was an error marking this game as completed.");
            });
        }
        $scope.comepleteClicked = false;
    }
});