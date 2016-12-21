

app.controller("leagueJoinCtrl", function ($scope, $rootScope, $http, $routeParams, DefaultFactory) {

    var clientToken = "eyJ2ZXJzaW9uIjoyLCJhdXRob3JpemF0aW9uRmluZ2VycHJpbnQiOiJhNDM2MThjOGFlM2MzMDJmMjllNDI5ZWRiOGVhYjcyYzU2ZjY4YWM2NmNhMTgwMDlmZWMwOWYwNTViNDgxM2U0fGNyZWF0ZWRfYXQ9MjAxNi0xMi0xOVQxOTo0NDoxMy4wODg0NjM0MjgrMDAwMFx1MDAyNm1lcmNoYW50X2lkPTM0OHBrOWNnZjNiZ3l3MmJcdTAwMjZwdWJsaWNfa2V5PTJuMjQ3ZHY4OWJxOXZtcHIiLCJjb25maWdVcmwiOiJodHRwczovL2FwaS5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tOjQ0My9tZXJjaGFudHMvMzQ4cGs5Y2dmM2JneXcyYi9jbGllbnRfYXBpL3YxL2NvbmZpZ3VyYXRpb24iLCJjaGFsbGVuZ2VzIjpbXSwiZW52aXJvbm1lbnQiOiJzYW5kYm94IiwiY2xpZW50QXBpVXJsIjoiaHR0cHM6Ly9hcGkuc2FuZGJveC5icmFpbnRyZWVnYXRld2F5LmNvbTo0NDMvbWVyY2hhbnRzLzM0OHBrOWNnZjNiZ3l3MmIvY2xpZW50X2FwaSIsImFzc2V0c1VybCI6Imh0dHBzOi8vYXNzZXRzLmJyYWludHJlZWdhdGV3YXkuY29tIiwiYXV0aFVybCI6Imh0dHBzOi8vYXV0aC52ZW5tby5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tIiwiYW5hbHl0aWNzIjp7InVybCI6Imh0dHBzOi8vY2xpZW50LWFuYWx5dGljcy5zYW5kYm94LmJyYWludHJlZWdhdGV3YXkuY29tLzM0OHBrOWNnZjNiZ3l3MmIifSwidGhyZWVEU2VjdXJlRW5hYmxlZCI6dHJ1ZSwicGF5cGFsRW5hYmxlZCI6dHJ1ZSwicGF5cGFsIjp7ImRpc3BsYXlOYW1lIjoiQWNtZSBXaWRnZXRzLCBMdGQuIChTYW5kYm94KSIsImNsaWVudElkIjpudWxsLCJwcml2YWN5VXJsIjoiaHR0cDovL2V4YW1wbGUuY29tL3BwIiwidXNlckFncmVlbWVudFVybCI6Imh0dHA6Ly9leGFtcGxlLmNvbS90b3MiLCJiYXNlVXJsIjoiaHR0cHM6Ly9hc3NldHMuYnJhaW50cmVlZ2F0ZXdheS5jb20iLCJhc3NldHNVcmwiOiJodHRwczovL2NoZWNrb3V0LnBheXBhbC5jb20iLCJkaXJlY3RCYXNlVXJsIjpudWxsLCJhbGxvd0h0dHAiOnRydWUsImVudmlyb25tZW50Tm9OZXR3b3JrIjp0cnVlLCJlbnZpcm9ubWVudCI6Im9mZmxpbmUiLCJ1bnZldHRlZE1lcmNoYW50IjpmYWxzZSwiYnJhaW50cmVlQ2xpZW50SWQiOiJtYXN0ZXJjbGllbnQzIiwiYmlsbGluZ0FncmVlbWVudHNFbmFibGVkIjp0cnVlLCJtZXJjaGFudEFjY291bnRJZCI6ImFjbWV3aWRnZXRzbHRkc2FuZGJveCIsImN1cnJlbmN5SXNvQ29kZSI6IlVTRCJ9LCJjb2luYmFzZUVuYWJsZWQiOmZhbHNlLCJtZXJjaGFudElkIjoiMzQ4cGs5Y2dmM2JneXcyYiIsInZlbm1vIjoib2ZmIn0=";

    braintree.setup(clientToken, "dropin", {
        container: "payment-form"
    });


    toastr.options = {
        "closeButton": true,
        "showDuration": "300"
    };

    //#region Variables
    //$scope.lookupOptions = [];
    $scope.detailsScope = {
        detailsObject: {},
        header: "Join",
        loaded: false
    };
    $scope.joinObject = { league_id: $routeParams.id };
    //#endregion

    //#region Get Data

    $http.get('../league/BasicDetails/' + $routeParams.id).success(function (response) {

        var dto = response;
        $scope.detailsScope.header = "Join " + dto.name
        $scope.detailsScope.detailsObject = dto;
        $scope.detailsScope.loaded = true;
    });


    $scope.join = function (isValid) {

        $scope.detailsScope.submitted = true;

        //Join
        if (isValid) {
            $http.post('../league/Join', $scope.joinObject).success(function (response) {
                if (response == false)
                    toastr.error('Incorrect Password');
                else
                    window.location.href = '../user/team/details/' + response;
            });
        }

    }

    $scope.disableJoin = function ()
    {
        if (!$scope.joinObject.league_team_name)
            return true;

        if (!$scope.detailsScope.loaded)
            return true;

        if ($scope.detailsScope.detailsObject.max_players && $scope.detailsScope.detailsObject.league_teams.length >= $scope.detailsScope.detailsObject.max_players)
            return true;

        if ($scope.detailsScope.detailsObject.lock_date && $rootScope.formatJSONDateTimeToDateObject($scope.detailsScope.detailsObject.lock_date) < new Date())
            return true

        return false;

    }

});

