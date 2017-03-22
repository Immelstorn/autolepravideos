(function() {

    var app = angular.module("autoLepraTop");

    var ListController = function ($scope, $location,$route, $routeParams, $window, api) {

        var list = function() {

            var from = $routeParams.from;
            var to = $routeParams.to;
            var sort = $routeParams.sort;
            var page = $routeParams.page;

            api.getList(page, sort, from, to).then(function(response) {
                $scope.comments = response.data.Comments;
                $scope.totalItems = response.data.TotalItems;
                $scope.itemsPerPage = response.data.ItemsPerPage;
                $scope.currentPage = response.data.Page;

                $scope.$parent.lastUpdated = moment(response.data.LastUpdated).format("DD-MM-YYYY hh:mm A");
                $scope.$parent.datepicker.minDate = moment(response.data.MinDate);
                $scope.$parent.datepicker.maxDate = moment(response.data.MaxDate);
                $scope.$parent.datepicker.momentFrom = moment(response.data.MinDate);
                $scope.$parent.datepicker.momentTo = moment(response.data.MaxDate);
            });
        };

        $scope.setPage = function () {
            $route.updateParams({ page: $scope.currentPage });
            list();
            $window.scrollTo(0, 0);
        };

        $scope.$parent.filter = function() {
            var datePicker = $scope.$parent.datepicker;
            $route.updateParams({
                from: datePicker.stringFrom,
                to: datePicker.stringTo
            });
            list();
        }

        $scope.maxSize = 5;
        $scope.totalItems = 200;
        $scope.itemsPerPage = 25;
        $scope.currentPage = 1;

        $scope.$parent.showError();
        list();
    };

    app.controller("ListController", ListController);
}());