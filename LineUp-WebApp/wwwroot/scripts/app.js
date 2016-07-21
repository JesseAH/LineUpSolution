
(function () {
    'use strict';

    angular.module('myApp', ['ngRoute'])
        .run(function ($rootScope, $http) {
            // .run - run is similar to a C# class constructor.  we will set our application/constant variables here

            var myUser = {};

            ////To Do call razas new GetClientUSer method
            //$http.get('../../api/User/GetClientUser')
            //   .then(function (dto) {
            //       myUser = dto;
            //   })
            //   .catch(function (error) {
            //   });

            //Get User Details
            $rootScope.GetUserDetails = function () {
                return myUser;
            }

            //Convert a JSON Date into a JavaScript Date Obect
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

        });


    angular.module('myApp').config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider

        //home routes
        .when('/team/dashboard', {
            templateUrl: 'partials/home/dashboard.html'
        })
        .when('/team/settings', {
            templateUrl: 'partials/home/settings.html'
        })
        .when('/team/search', {
            templateUrl: 'partials/home/search.html'
        })
        .when('/team/diagnostics', {
            templateUrl: 'partials/diagnostics/diagnosticsDetails.cshtml'
        })

        //partner routes
        .when('/team/partner/list', {
            templateUrl: 'partials/partner/partnerList.html'
        })
        .when('/team/partner/details', {
            templateUrl: 'partials/partner/partnerDetails.html'
        })
            .when('/team/partner/:id', {
                templateUrl: 'partials/partner/partnerDetails.html'
            })

        //customer routes
        .when('/team/customer/list', {
            templateUrl: 'partials/customer/customerList.html'
        })
        .when('/team/customer/details', {
            templateUrl: 'partials/customer/customerDetails.html'
        })
        .when('/team/customer/details/:id', {
            templateUrl: 'partials/customer/customerDetails.html'
        })

        //Default route is dashboard
        .otherwise({ redirectTo: '/team/dashboard' });

        $locationProvider.html5Mode(true);
    }]);

})();

