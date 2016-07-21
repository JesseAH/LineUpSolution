


var app = angular.module('myApp', ['ngRoute', 'DefaultFactory', 'smart-table'])

        // run is similar to a C# class constructor.  we will set our application/constant variables here
        .run(function ($rootScope, $http) {

            //Convert JSON date to a JavaScipt date
            $rootScope.formatJSONDateTimeToDateObject = function (myValue) {
                if (myValue == null)
                    return null;

                var jsonDate = myValue;
                var value = new Date(jsonDate);
                return value;
            }

            //Check if Date1 is before Date 2
            $rootScope.DateObjectIsBefore = function (date1, date2) {
                return date1 <= date2;
            }

            //Check if Date1 is after Date 2
            $rootScope.DateObjectIsAfter = function (date1, date2) {
                return date1 > date2;
            }

            //Check if Date is in this month
            $rootScope.DateInThisMonth = function (date1) {
                var date = new Date();

                if (date1.getMonth() == date.getMonth() && date1.getYear() == date.getYear())
                    return true;
                else
                    return false;
            }

            //Check if Date is in last month
            $rootScope.DateInLastMonth = function (date1) {
                var today = new Date();
                var compareDateStart = new Date();
                var compareDateEnd = new Date();

                compareDateEnd = new Date(today.getFullYear(), today.getMonth() + 1, 0);
                compareDateStart.setMonth(compareDateStart.getMonth() - 1);

                if (date1 > compareDateStart && date1 < compareDateEnd)
                    return true;
                else
                    return false;
            }

            //Check if Date is in last three months
            $rootScope.DateInLastThreeMonths = function (date1) {
                var today = new Date();
                var compareDateStart = new Date();
                var compareDateEnd = new Date();

                compareDateEnd = new Date(today.getFullYear(), today.getMonth() + 1, 0);
                compareDateStart.setMonth(compareDateStart.getMonth() - 3);

                if (date1 > compareDateStart && date1 < compareDateEnd)
                    return true;
                else
                    return false;
            }

            //Check if Date is in last six months
            $rootScope.DateInLastSixMonths = function (date1) {
                var today = new Date();
                var compareDateStart = new Date();
                var compareDateEnd = new Date();

                compareDateEnd = new Date(today.getFullYear(), today.getMonth() + 1, 0);
                compareDateStart.setMonth(compareDateStart.getMonth() - 6);

                if (date1 > compareDateStart && date1 < compareDateEnd)
                    return true;
                else
                    return false;
            }

            // used to parse date strings in JSON being passed into Kendo grids:
            // undefined is rendered as empty string to display nulls as blank
            $rootScope.parseDate = function (data) {
                return data === null ? undefined : new Date(data);
            };

            //pass in a list and them item you want to remove
            //this will find that items in the list and remove it
            $rootScope.removeItemFromListByID = function (list, item) {
                $.each(list, function (idx, obj) {
                    if (obj != undefined && item.ID == obj.ID) {
                        list.splice(idx, 1);
                        return true;
                    }
                });
            }

        })
        .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider

        //user dashboard routes
        .when('/user/dashboard', {
            templateUrl: 'Angular/Partials/dashboard.html'
        })

        //League routes
        .when('/user/league/details/:id', {
            templateUrl: 'Angular/Partials/League/leagueDetails.html'
        })
        .when('/user/league/join', {
            templateUrl: 'Angular/Partials/League/leagueJoin.html'
        })
        .when('/user/league/create', {
            templateUrl: 'Angular/Partials/League/leagueCreate.html'
        })

        //League_Team routes
        .when('/user/team/list', {
            templateUrl: 'Angular/Partials/League_Team/league_TeamList.html'
        })
        .when('/user/team/details/:id', {
            templateUrl: 'Angular/Partials/League_Team/league_TeamDetails.html'
        })

        //Pick routes
        .when('/user/pick/list', {
            templateUrl: 'Angular/Partials/pick/pickList.html'
        })
        .when('/user/pick/details/:id', {
            templateUrl: 'Angular/Partials/pick/pickDetails.html'
        })

        //Default route is dashboard
        .otherwise({ redirectTo: '/user/dashboard' });

        $locationProvider.html5Mode(true);
    }]);


    //app.controller("leagueListCtrl", function ($scope) {
    //    alert();
    //});

    //angular
    //    .module('myApp')
    //    .controller('leagueListCtrl', leagueListCtrl) // list controller

    ////League List - shows list
    //leagueListCtrl.$inject = ['$scope', '$rootScope', 'DefaultFactory'];
    //function contactListCtrl($scope, $rootScope, DefaultFactory) {

    //    //#region Variables

    //    $scope.header = "My Leagues";

    //    //#endregion

    //    //#region Get Data

    //    DefaultFactory.defaultList("League")
    //        .then(function (dtos) {

    //            $scope.originalDataSource = dtos;

    //        });

    //    //#endregion

    //}