var app = angular.module('tagCloud', []);
    app.controller("tagCloudCtrl", [
        "$scope", "$http", function ($scope, $http) {
            $scope.words = [];
            $scope.loadtags = function () {
                var successCallback;
                return $http({
                    method: "GET",
                    url: "/Home/GetTagsForCloud"
                }).then((successCallback = function (response) {
                    var fontMax, fontMin, i, lol, max, min, size, sizes, tag, _results;
                    sizes = [];
                    response.data.forEach(function (item) {
                        return sizes.push(item.Uses);
                    });
                    max = Math.max.apply(Math, sizes);
                    min = Math.min.apply(Math, sizes);
                    fontMin = 1;
                    fontMax = 6;
                    _results = [];
                    for (i in response.data) {
                        tag = response.data[i];
                        size = (tag.Uses === min ? fontMin : (tag.Uses / max) * (fontMax - fontMin) + fontMin);
                        lol = {
                            TagName: tag.TagName,
                            Weight: Math.round(size)
                        };
                        console.log(lol);
                        _results.push($scope.words.push(lol));
                    }
                    return _results;
                }));
            };
            return $scope.loadtags();
        }
    ]);