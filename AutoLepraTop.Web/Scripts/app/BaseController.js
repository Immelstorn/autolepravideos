(function() {

    var app = angular.module("autoLepraTop");

    var BaseController = function ($scope) {

        $scope.datepicker = {};

        $scope.showError = function(errorMsg) {
            if (errorMsg) {
                $scope.error = errorMsg;
                $('#error').show();
            } else {
                $scope.error = "";
                $('#error').hide();
            }
        };

        $('#loader').modal({
            keyboard: false,
            backdrop: false,
            show: false
        });
    };
   
    app.controller("BaseController", BaseController);
}());