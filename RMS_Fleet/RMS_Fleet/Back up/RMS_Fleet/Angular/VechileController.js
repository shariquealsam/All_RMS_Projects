/// <reference path="Angular.js" />
/// <reference path="../Js/table2excel.js" />

var myApp = angular.module("myApp", []);

myApp.controller("myController", function ($scope, $http, $window, $location) {

    $scope.baseUrl = new $window.URL($location.absUrl()).origin;
    var strURL = $scope.baseUrl;

    if (location.hostname != "localhost") {
        strURL = strURL + "/RMSFleet";
    }

    $scope.Region = function () {

        $http({

            method: 'GET',

            url: strURL + '/Vechile/GetRegion'


        }).then(function (response) {
            $scope.data = response.data;
        },
        function (response) {

            alert(response);

        });
    }
    $scope.Region();

    $scope.Branch = function () {
        $scope.RegionId = $scope.Vehicle.Region;
        $http({

            method: 'POST',

            url: strURL + '/Vechile/GetBranch',

            data: $scope.Vehicle.Region

        }).then(function (response) {
            $scope.RegionIdBindValue = ($scope.Vehicle.Region.RegionId);

            $scope.Branchdata = response.data;
        },
        function (response) {

            alert(response);

        });
    }

    $scope.SaveVehicleMaster = function () {
        $http({

            method: 'POST',

            url: strURL + '/Vechile/SaveVehicleDetails',

        }).then(function (response) {
            alert(response.data);
            $scope.IsVisible = true;
        },
        function (response) {

            alert(response);

        });
    }

    $scope.SaveVehicle = function () {
        $http({

            method: 'POST',

            url: strURL + '/Vechile/SaveVehicle',

            data: $scope.Vehicle

        }).then(function (response) {
            alert(response.data);
        },
        function (response) {
            alert(response);

        });
    }

    $scope.VehicleMaster = function () {
        $scope.loadingSpinner = true;
        $http({

            method: 'POST',

            url: strURL + '/Vechile/VehicleMaster',
        })
          .then(function (response) {
              $scope.VehicleMasterData = response.data;
              $scope.loadingSpinner = false;
          },
        function (response) {
            $scope.loadingSpinner = false;
            alert(response);

        });
    }

    $scope.GetDetails = function (Vechile) {
        var RecID = Vechile.Recid;

        $scope.GetRecid = RecID;
        $http({

            method: 'POST',

            url: strURL + '/Vechile/GetDeleted',

            data: { Recid: $scope.GetRecid }

        }).then(function (response) {
            $scope.VehicleMaster();
            alert(response.data.Message);

        },
        function (response) {

            alert(response);
            $scope.loadingSpinner = false;
        });
    };

    $scope.GetEditDetails = function (Vechile) {

        //var RecID = $scope.VehicleMasterData[index].Recid;
        var RecID = Vechile.Recid;
        $scope.GetEditRecid = RecID;
        $http({
            method: 'POST',
            url: strURL + '/Vechile/GetEditedValue',
            data: { Recid: $scope.GetEditRecid }
        }).then(function (response) {
            $scope.lstVechilevalue = response.data;
            window.open(strURL + '/Vechile/GetVechileMasterValue?Recid='+ RecID, '_blank');
        },
        function (response) {
            alert(response);
            $scope.loadingSpinner = false;
        });
    };

    $scope.Export = function () {
        $scope.loadingSpinner = true;
        $(angular.element(document.getElementById('Hide'))).table2excel({
            filename: "Vechile Master Details.xls"
        });
        $scope.loadingSpinner = false;
    }

    $scope.UpdateMasterVehicle = function (Recid, VehicleNumber, Make, ChassisNumber, ManufacturingYear, Password, Region, BranchName,value1,value2) {

        $http({
            method: 'POST',
            url: strURL + '/Vechile/GetUpdateMasterVehicle',
            data: {
                Recid: Recid, VehicleNumber: VehicleNumber, Make: Make, ChassisNumber: ChassisNumber, ManufacturingYear: ManufacturingYear, Password: Password, Region: Region, BranchName: BranchName
            }
        }).then(function (response) {
            //$scope.lstVechilevalue = response.data;
        },
        function (response) {
            alert(response);
           // $scope.loadingSpinner = false;
        });
    };
});



