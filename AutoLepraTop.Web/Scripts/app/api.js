(function() {

    var api = function($http) {
        var baseUrl = "http://lepraTop.api/api/auto";

        var getList = function(page, sort) {
            var url = baseUrl + "?page=" + (page == null ? 1 : page) + "&sort=" + (sort == null ? "byrating" : sort);
            return $http.get(url)
                .then(function(response) {
                    return response;
                });
        };

//        var getDetails = function(appId) {
//            return $http.get(baseUrl + "/" + appId)
//                .then(function(response) {
//                    return response;
//                });
//        };
//
//        var getManager = function() {
//            return $http.get(baseUrl + "/manager")
//                .then(function(response) {
//                    return response;
//                });
//        };
//
//        var loadMoreApps = function(amountForLoading) {
//            return $http.post(baseUrl + "/loadMore?amount=" + amountForLoading)
//                .then(function (response) {
//                    return response;
//                });
//        };

        return {
            getList: getList,
//            getDetails: getDetails,
//            getManager: getManager,
//            loadMoreApps: loadMoreApps
        };

    };

    var module = angular.module("autoLepraTop");
    module.factory("api", api);

}());