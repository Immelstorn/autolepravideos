(function() {

    var api = function($http) {
        var baseUrl = "http://autolepratop.azurewebsites.net/api/auto";

        var getList = function(page, sort) {
            var url = baseUrl + "?page=" + (page == null ? 1 : page) + "&sort=" + (sort == null ? "byrating" : sort);
            return $http.get(url)
                .then(function(response) {
                    return response;
                });
        };

        return {
            getList: getList
        };

    };

    var module = angular.module("autoLepraTop");
    module.factory("api", api);

}());