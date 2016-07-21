
(function () {

    'use strict';

    //Contact Service
    var DefaultFactory = angular.module('DefaultFactory', ['ngResource']);

    DefaultFactory.factory('DefaultFactory', ['$http', '$q', function ($http, $q) {
        var DefaultFactory = {};

        //Get Details
        DefaultFactory.Details = function (id, dataSource) {
            var deferred = $q.defer();
            $http.get('../' + dataSource + '/Details/' + id)
               .then(function (dto) {
                   deferred.resolve(dto.data);
               })
               .catch(function (error) {
                   //if (error == null)
                   //    notie.alert(3, "Error deleting utility account.", 3);
                   //else
                   //    notie.alert(3, error, 3);
               });

            return deferred.promise;
        };

        //Get List
        DefaultFactory.List = function (dataSource) {
            var deferred = $q.defer();

            $http.get('../' + dataSource)
               .then(function (dtos) {
                   deferred.resolve(dtos.data);
               })
               .catch(function (error) {
                   //if (error == null)
                   //    notie.alert(3, "Error getting list.", 3);
                   //else
                   //    notie.alert(3, error, 3);
               });

            return deferred.promise;
        };

        //Save
        DefaultFactory.Save = function (object, dataSource) {
            var deferred = $q.defer();
            return $http.post('../../api/' + dataSource, object)
               .success(function (id) {
                   if (dataSource != 'User_Link_MRU_Record')
                       notie.alert(1, "Save Successful!", 3);
                   deferred.resolve(id);
               })
               .error(function (error) {
                   //if (error == null)
                   //    notie.alert(3, "Error saving.", 3);
                   //else
                   //    notie.alert(3, error, 3);
               });

            return deferred.promise;
        };

        //Update
        DefaultFactory.Update = function (object, dataSource) {
            var deferred = $q.defer();
            return $http.post('../../api/' + dataSource, object)
               .success(function (id) {
                   if (dataSource != 'User_Link_MRU_Record')
                       notie.alert(1, "Save Successful!", 3);
                   deferred.resolve(id);
               })
               .error(function (error) {
                   //if (error == null)
                   //    notie.alert(3, "Error saving.", 3);
                   //else
                   //    notie.alert(3, error, 3);
               });

            return deferred.promise;
        };

        return DefaultFactory;
    }]);
})();