

app.controller("leagueDetailsCtrl", function ($scope, $rootScope, $routeParams, DefaultFactory) {


    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "League: ",
        loading: true
    };

    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id,"League")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = "League: " + dto.name;
            $scope.displayedCollection = dto.rounds;
            $scope.detailsScope.loading = false;
        });

    //#endregion
});





app.controller("leagueManageCtrl", function ($scope, $rootScope, $routeParams, $http, DefaultFactory) {

    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Manage: ",
        loading: true
    };

    $scope.emailAddresses;
    $scope.sending = false;

    //#endregion

    //#region Get Data

    DefaultFactory.Details($routeParams.id, "League")
        .then(function (dto) {

            $scope.detailsScope.detailsObject = dto;
            $scope.detailsScope.header = "Manage: " + dto.name;
            $scope.displayedCollection = dto.rounds;
            $scope.detailsScope.loading = false;
        });

    //#endregion


    //#region Click Events

    $scope.invite = function ()
    {

        if ($scope.emailAddresses != null && $scope.emailAddresses != undefined) {

            $scope.sending = true;
            var einviteReq = { league_id: $routeParams.id, emails: $scope.emailAddresses };

            $http.post('../league/Invite', einviteReq).success(function (response) {

                $scope.sending = false;

                if (response == true)
                    toastr.success('Emails Sent');
                else
                    toastr.error('Emails Failed to Send');
            });
        }
    }

    $scope.send = function () {

        if ($scope.body != null && $scope.body != undefined) {

            $scope.sending = true;
            var emailReq = { league_id: $routeParams.id, body: $scope.body, subject: $scope.subject };

            $http.post('../league/Email', emailReq).success(function (response) {

                $scope.sending = false;

                if (response == true)
                    toastr.success('Emails Sent');
                else
                    toastr.error('Emails Failed to Send');
            });
        }
    }

    //#endregion


});

