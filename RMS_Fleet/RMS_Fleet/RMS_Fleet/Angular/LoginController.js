/// <reference path="Angular.js" />

var myApp = angular.module("myApp", []);

myApp.controller("myController", function ($scope, $http, $window, $location) {

    $scope.baseUrl = new $window.URL($location.absUrl()).origin;
    var strURL = $scope.baseUrl;

    if (location.hostname != "localhost") {
        strURL = strURL + "/RMSFleet";
    }

    $scope.LoginData = function () {

        $http({

            method: 'POST',
            url:  strURL + '/Login/UserLogin',

            data: $scope.Login

        }).then(function (response) {
            var array = response.data.split('_');
            var UserType = array[0];
            var RegionId = array[1];
            var Status = array[2];
        
            if (Status == "Active") {
                location.href = strURL + "/Dashboard";
            }

            if (response.data == "User is InActive") {
                alert("User is InActive");
            }

            if (response.data == "Invalid Credential") {
                alert("Invalid Credential");
            }
            // $scope.LoginData = response.data;
        },
        function (response) {

            alert(response);

        });
        $scope.required = true;
    }

});