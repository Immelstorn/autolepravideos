(function() {

    var app = angular.module("autoLepraTop");

    var BaseController = function ($scope, $rootScope) {

        $scope.datepicker = {};
        $scope.lastUpdated = "";
        $rootScope.hideMain = false;

        $scope.showError = function(errorMsg) {
            if (errorMsg) {
                $scope.error = errorMsg;
                $('#error').show();
            } else {
                $scope.error = "";
                $('#error').hide();
            }
        };

    };
   
    app.controller("BaseController", BaseController);
}());