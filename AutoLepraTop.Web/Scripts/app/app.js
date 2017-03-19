(function() {

    var app = angular.module("autoLepraTop", ["ngRoute", /*"ngAnimate", */"ngSanitize", "ui.bootstrap"]);

    app.config([
        "$locationProvider", function($locationProvider) {
            $locationProvider.hashPrefix("");
        }
    ]);

    app.config(function($routeProvider) {
        $routeProvider
            .when("/list", {
                templateUrl: "Views/list.html",
                controller: "ListController"
            })
            .otherwise({ redirectTo: "/list" });
    });


    var httpInterceptor = function() {
        var requests = 0;
        var showLoader = function(config) {
            if (config.url.indexOf('/api/') >= 0) {
                if (requests === 0) {
                    $('#loader').modal('show');
                }
                requests++;
            }
            return config;
        };

        var hideLoader = function(res) {
            var config = res.config == undefined ? res : res.config; //making this method usable from requestError and from response/responseError endpoints (one of them has config  in argumern, another two have res, which has config inside it)
            if (config.url.indexOf('/api/') >= 0) {
                requests--;
                if (requests === 0) {
                    $('#loader').modal('hide');
                }
            }

            return res;
        }

        return {
            request: showLoader,
            requestError: hideLoader,
            response: hideLoader,
            responseError: hideLoader
        }
    };

    app.factory('httpInterceptor', httpInterceptor);
    app.config(function($httpProvider) {
        $httpProvider.interceptors.push('httpInterceptor');
    });
}());