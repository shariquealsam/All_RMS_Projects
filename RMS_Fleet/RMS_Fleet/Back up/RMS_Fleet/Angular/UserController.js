/// <reference path="Angular.js" />

var myApp = angular.module("myApp", []);

myApp.controller("myController", function ($scope, $http, $window, $location) {

    $scope.baseUrl = new $window.URL($location.absUrl()).origin;
    var strURL = $scope.baseUrl;

    if (location.hostname != "localhost") {
        strURL = strURL + "/RMSFleet";
    }

    $scope.UserDetails = function () {

        $http({

            method: 'GET',

            url: strURL + '/User/GetUserDetails'


        }).then(function (response) {

            $scope.UserDetails = response.data;
        },
        function (response) {

            alert(response);

        });
    }

    $scope.SaveUserDetails = function () {

        $http({

            method: 'POST',

            url: strURL + '/User/SaveUserDetails',

            data: $scope.User

        }).then(function (response) {
            alert(response.data.Message);
        },
        function (response) {

            alert(response);

        });
    }

    $scope.UserDetails();

});

