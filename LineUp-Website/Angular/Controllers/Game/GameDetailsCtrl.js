


app.controller("gameDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        id: $routeParams.id,
        detailsObject: {},
        header: "Loading...",
        loading: true,
        submitted: false
    };


    //#endregion

    //#region Get Data

    DefaultFactory.Details($scope.detailsScope.id, "Game")
        .then(function (dto) {

            if (dto.lock_date != null)
            {
                var myDate = new Date(parseInt(dto.lock_date.substr(6)))

                var month = myDate.getMonth() + 1; //months from 1-12
                var day = myDate.getDate();
                var year = myDate.getFullYear();

                newdate = month + "/" + day + "/" + year;
                dto.lock_date = newdate;

            }


            $scope.detailsScope.detailsObject = dto;

            if ($routeParams.id == 0)
                $scope.detailsScope.header = "Create New Game";
            else
                $scope.detailsScope.header = dto.name;
            $scope.detailsScope.loading = false;
        });

    //#endregion


    //#region Button Clicks

    $scope.save = function (isValid) {
        if (!isValid) {
            toastr.warning('One or more of the fields is not properly filled in.')
            return;
        }

        $scope.detailsScope.submitted = true;

        //call save then navigate to game dashboard
        DefaultFactory.Save($scope.detailsScope.detailsObject, "Game")
        .then(function (data) {
            window.location.href = '../user/game/dashboard/' + data.data;
        });
    }


    //#endregion
});