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
            url: strURL + '/Report/GetRegion'

        }).then(function (response) {
            $scope.data = response.data;
        },
        function (response) {
            alert(response);
        });
    }
    $scope.Region();

    $scope.Branch = function () {
        $http({

            method: 'POST',
            url: strURL + '/Report/GetBranch',
            data: { RegionId: $scope.Region.RegionId }

        }).then(function (response) {
            $scope.Branchdata = response.data;
        },
        function (response) {
            alert(response);
        });
    }

    $scope.GetReportDetails = function () {
        $scope.loadingSpinner = true;
        $http({
            method: 'POST',
            url: strURL + '/Report/ReportMaster',
        }).then(function (response) {
            $scope.ReportMasterData = response.data;
            $scope.loadingSpinner = false;
        },
        function (response) {
            alert(response);
            $scope.loadingSpinner = false;
        });
    }

    $scope.GetReportDetails();

    $scope.GetDetails = function (Report) {
        var OCKey = Report.OCKey;
        var VechileNumber = Report.VechileNumber;

        //var OCKey = $scope.ReportMasterData[index].OCKey;
        //var VechileNumber = $scope.ReportMasterData[index].VechileNumber;

        $scope.GetOCKey = OCKey;
        $scope.VechileNumber = VechileNumber;

        $http({

            method: 'POST',
            url: strURL + '/Report/GetOpnClosDetails',
            data: { OCKey: $scope.GetOCKey, VechileNumber: $scope.VechileNumber }

        }).then(function (response) {
            window.open(strURL + '/Report/IGetOpnClosDetails', '_blank');

        },
        function (response) {

            alert(response);
            $scope.loadingSpinner = false;
        });
    };

    $scope.GetReportDetailsOCKey = function () {
        $scope.loadingSpinner = true;
        $http({
            method: 'POST',
            url: strURL + '/Report/ReportMasterOCKey',

        }).then(function (response) {
            $scope.MasterDataKeyValue = response.data;
            $scope.loadingSpinner = false;
        },
        function (response) {
            alert(response);
            $scope.loadingSpinner = false;
        });
    }

    $scope.GetReportDetailsOCKey();

    $scope.RevisedEntries = function (Value, Value2) {
        for (var i = 0; i < $scope.MasterDataKeyValue.length ; i++) {
            var OCKey = $scope.MasterDataKeyValue[i].OCKey;
        }
        $http({
            method: 'POST',
            url: strURL + '/Report/SaveRevisedEntries',
            data: { RevisedOpeningKM: Value, RevisedClosingKM: Value2, OCKey: OCKey }
        }).then(function (response) {
            alert(response.data);
            $scope.GetReportDetailsOCKey();
        },
       function (response) {
           alert(response);
       });
    }

    $scope.Export = function () {
        $scope.loadingSpinner = true;
        $(angular.element(document.getElementById('DailyReport'))).table2excel({
            filename: "Opening Closing Details.xls"
        });
        $scope.loadingSpinner = false;
    }

    $scope.SearchReport = function (Value, Value2, Value3, Value4) {
        if ((Value == null && Value2 == null) && Value3 == null) {
            alert("Please Select FromDate and ToDate or Region");
            return;
        }

        if (Value != null && Value2 == null) {
            alert("Please Select ToDate");
            return;
        }
        $scope.loadingSpinner = true;
        $http({
            method: 'POST',
            url: strURL + '/Report/ReportMaster',
            data: { FromDate: Value, ToDate: Value2, RegId: Value3, Branchid: Value4,IsFilter: 1 }
        }).then(function (response) {
            $scope.ReportMasterData = response.data;
            $scope.loadingSpinner = false;
        },
              function (response) {
                  $scope.loadingSpinner = false;
                  alert(response);
              });
    }

    $scope.ExportReportInExcel = function (Value, Value2, Value3, Value4) {
        if ((Value == null && Value2 == null) && Value3 == null) {
            alert("Please Select FromDate and ToDate or Region");
            return;
        }

        if (Value != null && Value2 == null) {
            alert("Please Select ToDate");
            return;
        }
        $scope.loadingSpinner = true;

        window.location.href = "Report/ReportMasterInExcel?FromDate='"+Value+"',ToDate='"+Value2+"',RegId='"+Value3+"',Branchid='"+Value4+"',IsFilter='"+1+"'";
        //$http({
        //    method: 'POST',
        //    url: strURL + '/Report/ReportMasterInExcel',
        //    data: { FromDate: Value, ToDate: Value2, RegId: Value3, Branchid: Value4, IsFilter: 1 }
        //}).then(function (response) {
        //    $scope.ReportMasterData = response.data;
        //    $scope.loadingSpinner = false;
        //},
        //      function (response) {
        //          $scope.loadingSpinner = false;
        //          alert(response);
        //      });
    }

    $scope.myvalue = true;

    $scope.toggleCustom = function () {
        $scope.myvalue = $scope.myvalue === false ? true : false;
    };
    $scope.ViewHistory = function (Value) {
        
        if ((Value == null) || (Value == "")) {
            alert("No Data Found");
            return;
        }

        $http({
            method: 'POST',
            url: strURL + '/Report/ViewHistoryVehicle',
            data: { VehicleNumber: Value }
        }).then(function (response) {
            $scope.ViewVehicleHistory = response.data;
            
        },
              function (response) {
                  alert(response);
              });
    }
});