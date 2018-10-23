var app = angular.module('ComixShow', [], function ($locationProvider) {
    $locationProvider.html5Mode(true);
});

app.controller("comixViewController", function ($scope, $location, $http) {
    var path = '/Comix/SendComix' + $location.path();
    var comix = $http.get(path);
    comix.success(function(data, status, headers, config) {
        alert(data);
    });
    comix.error(function (data, status, headers, config) {
        alert("failure");
    });
}
);