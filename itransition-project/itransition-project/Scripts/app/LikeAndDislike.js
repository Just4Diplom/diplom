var app = angular.module('Like', []);
app.controller("LikeController", [
    "$scope", "$http", "$window", function ($scope, $http, $window) {
        $scope.Id = null;
        $scope.setId = function (id) {
            $scope.Id = id;
            return $scope.getRate(id);
        };


        $scope.rating = null;
        $scope.getRate = function (id) {
            var q, successCallback;
            q = {
                Id: id
            };
            $http({
                method: "POST",
                url: "/Comix/GetRate/",
                data: q
            }).then((successCallback = function (response) {
                return $scope.rating = response.data;
            }));
            return $http({
                method: "POST",
                url: "/Comix/GetUserRate/",
                data: q
            }).then((successCallback = function (response) {
                if (response.data === "") {
                    return $scope.isRateSet = false;
                } else {
                    $scope.isRateSet = true;
                    return $scope.isPositive = response.data;
                }
            }));
        };
        return $scope.vote = function (ispositive) {
            var q, successCallback;
            q = {
                Id: $scope.Id,
                isPositive: ispositive
            };
            return $http({
                method: "POST",
                url: "/Comix/AddRate/",
                data: q
            }).then((successCallback = function (response) {
                return $scope.getRate($scope.Id);
            }));
        };
    }
]);