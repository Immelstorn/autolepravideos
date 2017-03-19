(function() {

    var app = angular.module("autoLepraTop");

    var ListController = function ($scope, $location, $routeParams, api) {

        var list = function (page, sort) {
          
            api.getList(page, sort).then(function (response) {
                $scope.Comments = response.data.Comments;
                $scope.totalItems = response.data.TotalItems;
                $scope.itemsPerPage =  response.data.ItemsPerPage;
                $scope.currentPage = response.data.Page;
            });
        };

        $scope.setPage = function () {
            search($scope.searchQuery);
        };

        $scope.maxSize = 5;
        $scope.totalItems = 200;
        $scope.itemsPerPage = 25;
        $scope.currentPage = 1;

        $scope.$parent.showError();
        list($routeParams.page, $routeParams.sort);
    };

    app.controller("ListController", ListController);
}());