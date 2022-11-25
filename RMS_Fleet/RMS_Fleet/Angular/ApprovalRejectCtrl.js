/// <reference path="myapp.js" />
app.controller("ApprRejController", function ($scope, $http, $window, $location,myService) {

    $scope.baseUrl = new $window.URL($location.absUrl()).origin;
    var strURL = $scope.baseUrl;
    //var strURL = $scope.baseUrl + "/RMSFleet";
    var date = new Date();
    var dminus = new Date();
    dminus.setDate(dminus.getDate() + (-30));
    $scope.fromdate = myService.StartDate(dminus);
    $scope.todate = date;
    $scope.status = 'Pending';
    $scope.userType = '';
    $scope.loader = false;

    $scope.getVehiclesByParam = function (status) {
        $scope.status = (status == undefined) ? 'Pending' : status;
        var regId = $scope.regionId > 0 ? $scope.regionId : 0;
        var bchId = $scope.branchId > 0 ? $scope.branchId : 0;
        $scope.VehicleNumber = $scope.VehicleNumber == undefined ? '' : $scope.VehicleNumber;
        var sdate = angular.copy($scope.fromdate);
        var tdate = angular.copy($scope.todate);

        if (sdate != null && sdate != undefined) {
            sdate = myService.ConvertDateToYYYY_MM_DD(sdate);
        }

        if (tdate != null && tdate != undefined) {
            tdate = myService.ConvertDateToYYYY_MM_DD(tdate);
        }
        $scope.loader = true;
        $http.post(strURL + '/ApprovalRejection/SearchVehicle?FromDate=' + sdate + '&ToDate=' + tdate +
            '&RegionId=' + regId + '&BranchId=' + bchId + '&VehicleNumber='
            + $scope.VehicleNumber + '&status=' + $scope.status).then(res => {
                $scope.lstVehiclePending = res.data.lstVehiclePending;
                $scope.lstVehicleRecommend = res.data.lstVehicleRecommend;
                $scope.lstVehicleReject = res.data.lstVehicleReject;
                $scope.lstVehicleApproved = res.data.lstVehicleApproved;
                $scope.lstVehicleRejectSA = res.data.lstVehicleRejectSA;
                $scope.lstVehicleRejectUser = res.data.lstVehicleRejectUser;
                $scope.loader = false;
            }, err => {
                    console.log(err.data);
                    $scope.loader = false;
            });
        //var sdate = myService.ConvertDateToYYYY_MM_DD($sc)
    }

    $scope.GetRegionList = function () {
        $http.get(strURL + '/ApprovalRejection/GetRegionDetails').then(res => {
            $scope.regions = res.data.lstRegion;
            $scope.userType = res.data.usertype;
        });
        $scope.getVehiclesByParam('Pending');
    }
    $scope.GetRegionList();

    $scope.getBranchByRegion = function (regionId) {
        regionId = regionId > 0 ? regionId : 0;
        $http.post(strURL + '/ApprovalRejection/GetBranchdetails?RegionId=' + regionId).then(res => {
            $scope.branches = res.data.lstBranch;
        });
    }

    $scope.getVehicleByBranchId = function (branchId) {
        branchId = branchId > 0 ? branchId : 0;
        $http.post(strURL + '/ApprovalRejection/GetVechileList?BranchId=' + branchId).then(res => {
            $scope.vehicles = res.data.lstVechileMaster;
        });
    }

    

    $scope.getVehicleDetails = function (selected) {
        $scope.loader = true;
        var det = angular.copy(selected);
        $http.post(strURL + '/ApprovalRejection/getVehicleDetailsByRecId?recId=' + det.RecId).then(res => {
            
            
            $scope.salesdet = res.data.salesdet;
            $scope.salesdet.RoutineInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.RoutineInvoiceDate);
            $scope.salesdet.BatteryInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.BatteryInvoiceDate);
            $scope.salesdet.DentingInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.DentingInvoiceDate);
            $scope.salesdet.MinorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.MinorInvoiceDate);
            $scope.salesdet.SeatInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SeatInvoiceDate);
            $scope.salesdet.SelfInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SelfInvoiceDate);
            $scope.salesdet.ElectricalInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.ElectricalInvoiceDate);
            $scope.salesdet.ClutchInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.ClutchInvoiceDate);
            $scope.salesdet.AlternatorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AlternatorInvoiceDate);
            $scope.salesdet.SuspensionInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SuspensionInvoiceDate);
            $scope.salesdet.GearBoxInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.GearBoxInvoiceDate);
            $scope.salesdet.BreakWorkInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.BreakWorkInvoiceDate);
            $scope.salesdet.EngineWorkInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.EngineWorkInvoiceDate);
            $scope.salesdet.FuelInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.FuelInvoiceDate);
            $scope.salesdet.PuncherInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.PuncherInvoiceDate);
            $scope.salesdet.OilInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.OilInvoiceDate);
            $scope.salesdet.RadiatorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.RadiatorInvoiceDate);
            $scope.salesdet.AxleInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AxleInvoiceDate);
            $scope.salesdet.DifferentialInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.DifferentialInvoiceDate);
            $scope.salesdet.TurboInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.TurboInvoiceDate);
            $scope.salesdet.EcmInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.EcmInvoiceDate);
            $scope.salesdet.AccidentalInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AccidentalInvoiceDate);
            $scope.salesdet.LeafInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.LeafInvoiceDate);
            $scope.apprdet = res.data.apprdet;
            $scope.userType = res.data.userType;
            $scope.loader = false;
        }, err => {
            $scope.loader = false;
        });
    }

    $scope.getVehicleDetailsForSA = function (selected) {
        $scope.loader = true;
        var det = angular.copy(selected);
        $http.post(strURL + '/ApprovalRejection/getVehicleDetailsByRecId?recId=' + det.RecId).then(res => {
            
            
            $scope.salesdet = res.data.salesdet;
            $scope.salesdet.RoutineInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.RoutineInvoiceDate);
            $scope.salesdet.BatteryInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.BatteryInvoiceDate);
            $scope.salesdet.DentingInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.DentingInvoiceDate);
            $scope.salesdet.MinorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.MinorInvoiceDate);
            $scope.salesdet.SeatInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SeatInvoiceDate);
            $scope.salesdet.SelfInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SelfInvoiceDate);
            $scope.salesdet.ElectricalInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.ElectricalInvoiceDate);
            $scope.salesdet.ClutchInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.ClutchInvoiceDate);
            $scope.salesdet.AlternatorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AlternatorInvoiceDate);
            $scope.salesdet.SuspensionInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.SuspensionInvoiceDate);
            $scope.salesdet.GearBoxInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.GearBoxInvoiceDate);
            $scope.salesdet.BreakWorkInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.BreakWorkInvoiceDate);
            $scope.salesdet.EngineWorkInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.EngineWorkInvoiceDate);
            $scope.salesdet.FuelInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.FuelInvoiceDate);
            $scope.salesdet.PuncherInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.PuncherInvoiceDate);
            $scope.salesdet.OilInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.OilInvoiceDate);
            $scope.salesdet.RadiatorInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.RadiatorInvoiceDate);
            $scope.salesdet.AxleInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AxleInvoiceDate);
            $scope.salesdet.DifferentialInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.DifferentialInvoiceDate);
            $scope.salesdet.TurboInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.TurboInvoiceDate);
            $scope.salesdet.EcmInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.EcmInvoiceDate);
            $scope.salesdet.AccidentalInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.AccidentalInvoiceDate);
            $scope.salesdet.LeafInvoiceDate = myService.ConvertJsonDateToDate($scope.salesdet.LeafInvoiceDate);
            $scope.apprdet = res.data.apprdet;
            $scope.userType = res.data.userType;
            
            $scope.loader = false;
        }, err => {
            $scope.loader = false;
        });
    }

    $scope.SelectSuperAdmiReason = function (Status) {
        if ($scope.userType == "SuperAdmin") {
            $scope.apprdet.reason_sadmin = Status;
        }
        
    }

    $scope.ApproveEntryById = function () {
        $scope.loader = true;
        var aprdata = angular.copy($scope.apprdet);
        $http.post(strURL + '/ApprovalRejection/UpdateApproveRejectEntry', aprdata).then(res => {
            //console.log(res.data);
            if (res.data.msg == "s") {
                alert("Entry Saved Successfully.");
                $('#myModalRecommendSA').modal('hide');
                $('#myModal').modal('hide');
                $scope.getVehiclesByParam($scope.status);
                $scope.loader = false;
            }
            else {
                alert("Failed to save...");
                $scope.loader = false;
            }
        }, err => {
                $scope.loader = false;
        });
    }

    $scope.SelectAll = function (isChkAll) {
        if (isChkAll == true) {
            angular.forEach($scope.lstVehicleRecommend, function (item) {
                item.isCheck = true;
            });
        }
        else {
            angular.forEach($scope.lstVehicleRecommend, function (item) {
                item.isCheck = false;
            });
        }
    }

    $scope.ApproveChecked = function () {
        $scope.loader = true;
        var checkarray = $.grep($scope.lstVehicleRecommend, function (item) {
            return item.isCheck == true;
        });
        $http.post(strURL + '/ApprovalRejection/UpdateApproveRejectEntryInBulk', checkarray).then(res => {
            if (res.data.msg == "s") {
                alert("Entry Saved Successfully.");
                $scope.getVehiclesByParam($scope.status);
                //$scope.loader = false;
            }
            else {
                alert("Failed to save...");
                $scope.loader = false;
            }
        }, err => {
                $scope.loader = false;
        });
    }

    $scope.ResendForApproval = function (selected) {
        $scope.loader = true;
        var seldata = selected
        $http.post(strURL + '/ApprovalRejection/UpdateRejectedToPending', seldata).then(res => {
            if (res.data.msg == "s") {
                alert("Entry Saved Successfully.");
                $scope.getVehiclesByParam($scope.status);
                //$scope.loader = false;
            }
            else {
                alert("Failed to save...");
                $scope.loader = false;
            }
        }, err => {
            $scope.loader = false;
        });
    }

    $scope.SetSessionValue = function (type, selected) {
        $scope.loader = true;
        $http.post(strURL + '/ApprovalRejection/SetSessionValue?RecId=' + selected.RecId + "&Type=" + type +
            "&VehicleNo=" + selected.VehicleNumber).then(res => {
                if (res.data.base64ImageString != "") {
                    var filename = type + "_" + myService.ConvertDateToYYYY_MM_DD(date) + ".pdf";
                    var a = document.createElement("a"); //Create <a>
                    a.href = "data:application/pdf;base64," + res.data.base64ImageString; //Image Base64 Goes here
                    a.download = filename //"donwoadpdf.pdf"; //File name Here
                    a.click(); //Downloaded file
                    $scope.loader = false;
                }
                else {
                    alert('No File Found..');
                    $scope.loader = false;
                }
                
        }, err => {
            $scope.loader = false;
        });
    }
});
