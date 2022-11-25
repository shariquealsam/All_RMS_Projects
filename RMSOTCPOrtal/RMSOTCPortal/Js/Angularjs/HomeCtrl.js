/// <reference path="myapp.js" />
app.controller("HomeCtrl", function ($scope, $http, myService,$filter) {

    //$scope.apiUrl = "http://10.61.0.44:120/";
    $scope.apiUrl = "http://localhost:4686/";
    $scope.txtATMID = '';
    var date = new Date();
    $scope.txtFromDate = date;
    $scope.txtToDate = date;
    $scope.scolist = [];
    $scope.AuditDtls = [];
    $scope.lstFlms = [];
    $scope.lstLast3Details = [];
    $scope.flmSite = {};
    $scope.loader = false;
    $scope.rowIndex = 0;
    $scope.randQuestion = {};
    $scope.randQuestionAudit = {};
    $scope.flmQ = {};
    $scope.manual = {};
    $scope.manual.txtDate = date;
    $scope.manual.ddlTimeBlock = "4";
    $scope.manual.ddlLockStatus = "0";
    $scope.manual.ddlActivity = "";
    $scope.otc3Title = "Last 3 OTC On This Route";

    $scope.btnSearch_Click = function (atmid, fromdate, todate) {
        if (atmid != null && atmid != '' && fromdate != null && fromdate != undefined && todate != null && todate != undefined) {
            $scope.loader = true;
            var fdate = myService.ConvertDateToYYYY_MM_DD(fromdate);
            var tdate = myService.ConvertDateToYYYY_MM_DD(todate);

          $http.post($scope.apiUrl + 'Home/ScoDetails?FromDate=' + fdate + "&ToDate=" + tdate + "&ATMID=" + atmid).then(function (res) {
                console.log(res.data.AuditDtls)
                $scope.scolist = res.data.lstDetails;
                $scope.flmSite = res.data.flmSite;
                $scope.routeStatus = res.data.routeStatus;
                $scope.lstFlms = res.data.lstFlms;
                $scope.lstOtherRms = res.data.lstOtherRms;
                $scope.AuditDtls = res.data.AuditDtls;
                angular.forEach($scope.scolist, function (item) {
                    item.ddlScoTimeBlock = "4";
                    item.ddlScoLockStatus = "0";
                });
                angular.forEach($scope.lstFlms, function (item) {
                    item.ddlFlmTimeBlock = "4";
                    item.ddlFlmLockStatus = "0";
                });
                $scope.loader = false;
            });
        }
        else {
            alert('Please select Atm ID, From Date and To Date First!');
        }
    }

    // atm search on text
    $scope.SearchAtm_OnChange = function () {
        $("#txtATMID").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: $scope.apiUrl+ 'Home/AutoComplete',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.ATMId, value: item.ATMId, id: item.ATMId }
                        }));
                    },
                    error: function (response) {
                        console.log(response);
                        alert('No Record Found.');
                    },
                    failure: function (response) {
                        console.log(response.responseText);
                    }
                });
            },
            minLength: 3,
        });
    }

    // atm search on text
    $scope.SearchAtmManual_OnChange = function () {
        $("#txtManualATMID").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: $scope.apiUrl + 'Home/AutoComplete',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.ATMId, value: item.ATMId, id: item.ATMId }
                        }));
                    },
                    error: function (response) {
                        console.log(response);
                        alert('No Record Found.');
                    },
                    failure: function (response) {
                        console.log(response.responseText);
                    }
                });
            },
            minLength: 3,
        });
    }

    //search route key
    $scope.TakeRouteKeyName = function () {
        $(".routekeyname").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: $scope.apiUrl + 'Home/GetRouteKeyName',
                    data: "{ 'prefix': '" + request.term + "','AtmID': '" + $scope.txtATMID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.OTCRouteKeyId, value: item.OTCRouteKeyId, id: item.OTCRouteKeyId }
                        }));
                    },
                    error: function (response) {
                        console.log(response);
                        alert('No Record Found.');
                    },
                    failure: function (response) {
                        console.log(response.responseText);
                    }
                });
            },
            minLength: 3,
        });
    }

    //search flm route key
    $scope.TakeRouteKeyNameFLM = function () {
        $(".FLMRouteKeyName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: $scope.apiUrl + 'Home/GetRouteKeyName',
                    data: "{ 'prefix': '" + request.term + "','AtmID': '" + $scope.txtATMID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.OTCRouteKeyId, value: item.OTCRouteKeyId, id: item.OTCRouteKeyId }
                        }));
                    },
                    error: function (response) {
                        console.log(response);
                        alert('No Record Found.');
                    },
                    failure: function (response) {
                        console.log(response.responseText);
                    }
                });
            },
            minLength: 3,
        });
    }

    //take route key name manual
    $scope.TakeRouteKeyNameManual = function () {
        $(".ManualRouteKeyName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: $scope.apiUrl + 'Home/GetRouteKeyName',
                    data: "{ 'prefix': '" + request.term + "','AtmID': '" + $scope.manual.txtManualATMID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.OTCRouteKeyId, value: item.OTCRouteKeyId, id: item.OTCRouteKeyId }
                        }));
                    },
                    error: function (response) {
                        console.log(response);
                        alert('No Record Found.');
                    },
                    failure: function (response) {
                        console.log(response.responseText);
                    }
                });
            },
            minLength: 3,
        });
    }

    //get random question for sco detail
    $scope.GetSCORandomQuestion = function (selecteddata) {

        $http.post($scope.apiUrl + "Home/RandomQuestion?CustodianIdOne=" + selecteddata.Custodian1RegNo + "&CustodianIdTwo=" +
            selecteddata.Custodian2RegNo + "&Branch=" + selecteddata.BRANCH).then(function (res) {
                $scope.randQuestion = res.data[0];
                $scope.randQuestion.txtQuestionATM = selecteddata.ATMId;
                $scope.randQuestion.txtQuestionRouteNo = selecteddata.Route;
                $scope.randQuestion.txtQuestionDate = selecteddata.Date;
                $scope.randQuestion.txtQuestionCustiodianOneId = selecteddata.Custodian1RegNo;
                $scope.randQuestion.txtQuestionCustiodianOneName = selecteddata.Custodian1Name;
                $scope.randQuestion.txtQuestionCustiodianTwoId = selecteddata.Custodian2RegNo;
                $scope.randQuestion.txtQuestionCustiodianTwoName = selecteddata.Custodian2Name;
                $scope.randQuestion.item_route_sheet_id = selecteddata.item_route_sheet_id;
            });
    }

    //question mark submit button click
    $scope.btnQuestionSave_Click = function () {
        var QuestionOneCheck = "Yes";
        var QuestionTwoCheck = "Yes";
        var QuestionThreeCheck = "Yes";
        var QuestionFourCheck = "Yes";

        //if ($scope.randQuestion.QuestionOne == true) {
        //    QuestionOneCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question One');
        //    $('#QuestionOne').focus();
        //    return false;
        //}
        //if ($scope.randQuestion.QuestionTwo == true) {
        //    QuestionTwoCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question Two');
        //    $('#QuestionTwo').focus();
        //    return false;
        //}
        //if ($scope.randQuestion.QuestionThree == true) {
        //    QuestionThreeCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question One');
        //    $('#QuestionThree').focus();
        //    return false;
        //}
        //if ($scope.randQuestion.QuestionFour == true) {
        //    QuestionFourCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question Two');
        //    $('#QuestionFour').focus();
        //    return false;
        //}
        if (QuestionOneCheck == "Yes" && QuestionTwoCheck == "Yes" && QuestionThreeCheck == "Yes" && QuestionFourCheck == "Yes") {
            $http.post($scope.apiUrl + "Home/SaveRandomQuestions?QuestionOne=" + QuestionOneCheck +
                "&QuestionTwo=" + QuestionTwoCheck + "&QuestionThree=" + QuestionThreeCheck +
                "&QuestionFour=" + QuestionFourCheck + "&ATMId=" + $scope.randQuestion.txtQuestionATM +
                "&Date=" + $scope.randQuestion.txtQuestionDate + "&RouteNo=" + $scope.randQuestion.txtQuestionRouteNo +
                "&CustodianOneQuestionOne=" + $scope.randQuestion.CustodianOneQ1 +
                "&CustodianOneQuestionTwo=" + $scope.randQuestion.CustodianOneQ2 +
                "&CustodianTwoQuestionOne=" + $scope.randQuestion.CustodianTwoQ1 +
                "&CustodianTwoQuestionTwo=" + $scope.randQuestion.CustodianTwoQ2 +
                "&Itefolrot=" + $scope.randQuestion.item_route_sheet_id).then(function (res) {
                    if (res.data.Message == "Successfully Inserted") {
                        $('#btnQuestionClose').click();
                        //alert(res.data.Message);
                    }
                    else {
                        $scope.randQuestion.QuestionOne = false;
                        $scope.randQuestion.QuestionTwo = false;
                        $scope.randQuestion.QuestionThree = false;
                        $scope.randQuestion.QuestionFour = false;
                        QuestionOneCheck = "No";
                        QuestionTwoCheck = "No";
                        QuestionThreeCheck = "No";
                        QuestionFourCheck = "No";
                    }
                });
        }
        else {
            alert("Please check all the question first!");
        }
    }

    // get Audit random question
    $scope.GetAuditSCORandomQuestion = function (selecteddata) {
        
        $http.post($scope.apiUrl + "Home/RandomQuestionAudit?CustodianIdOne=" + selecteddata.Custodian1RegNo + "&CustodianIdTwo=" +
            selecteddata.Custodian2RegNo + "&AuditorId=" +
          selecteddata.AuditorId + "&Branch=" + selecteddata.BRANCH).then(function (res) {
            console.log(res)
                $scope.randQuestionAudit = res.data[0];
                $scope.randQuestionAudit.txtAuditQuestionATM = selecteddata.ATMId;
                $scope.randQuestionAudit.txtAuditQuestionRouteNo = selecteddata.Route;
                $scope.randQuestionAudit.txtAuditQuestionDate = selecteddata.Date;
                $scope.randQuestionAudit.txtAuditQuestionCustiodianOneId = selecteddata.Custodian1RegNo;
                $scope.randQuestionAudit.txtAuditQuestionCustiodianOneName = selecteddata.Custodian1Name;
                $scope.randQuestionAudit.txtAuditQuestionCustiodianTwoId = selecteddata.Custodian2RegNo;
                $scope.randQuestionAudit.txtAuditQuestionCustiodianTwoName = selecteddata.Custodian2Name;

                $scope.randQuestionAudit.txtAuditQuestionAuditorId = selecteddata.AuditorId;
                $scope.randQuestionAudit.txtAuditQuestionAuditor = selecteddata.Auditor;
            });
    }

    //get flm random question
    $scope.getFlmPopUp = function (selectflm) {
        $scope.flmQ = selectflm;
        $scope.flmQ.txtFLMQuestionATM = selectflm.FlMATMId;
        $scope.flmQ.txtFLMQuestionDocket = selectflm.DocketNo;
        $scope.flmQ.txtFLMQuestionDate = selectflm.OPENDATE;
    }

    $scope.GetFLMRandomQuestion = function () {

        $scope.flmQ.FLMQuestionOne = true;
        $scope.flmQ.FLMQuestionTwo = true;
        $scope.flmQ.FLMQuestionThree = true;
        $scope.flmQ.FLMQuestionFour = true;

        var phone1 = ($scope.flmQ.CustodianOnePhone == undefined || $scope.flmQ.CustodianOnePhone == null) ? "" : $scope.flmQ.CustodianOnePhone;
        var phone2 = ($scope.flmQ.CustodianTwoPhone == undefined || $scope.flmQ.CustodianTwoPhone == null) ? "" : $scope.flmQ.CustodianTwoPhone;
        $http.post($scope.apiUrl + "Home/FLMRandomQuestion?CustodianPhoneNumber1=" + phone1 +
            "&CustodianPhoneNumber2=" + phone2).then(function (res) {
                var result = res.data[0];
                $scope.flmQ.CustodianOneId = result.CustodianOneId;
                $scope.flmQ.CustodianOneName = result.CustodianOneName;
                $scope.flmQ.CustodianOneQ1 = result.CustodianOneQ1;
                $scope.flmQ.CustodianOneQ2 = result.CustodianOneQ2;
                $scope.flmQ.CustodianTwoId = result.CustodianTwoId;
                $scope.flmQ.CustodianTwoName = result.CustodianTwoName;
                $scope.flmQ.CustodianTwoQ1 = result.CustodianTwoQ1;
                $scope.flmQ.CustodianTwoQ2 = result.CustodianTwoQ2;
            });
    }

    //flm question mark submit button click
    $scope.btnFLMQuestionSave_Click = function () {
        var QuestionOneCheck = "Yes";
        var QuestionTwoCheck = "Yes";
        var QuestionThreeCheck = "Yes";
        var QuestionFourCheck = "Yes";

        //if ($scope.flmQ.FLMQuestionOne == true) {
        //    QuestionOneCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question One');
        //    $('#FLMQuestionOne').focus();
        //    return false;
        //}
        //if ($scope.flmQ.FLMQuestionTwo) {
        //    QuestionTwoCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question Two');
        //    $('#FLMQuestionTwo').focus();
        //    return false;
        //}

        //if ($scope.flmQ.FLMQuestionThree) {
        //    QuestionThreeCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question One');
        //    $('#FLMQuestionThree').focus();
        //    return false;
        //}

        //if ($scope.flmQ.FLMQuestionFour) {
        //    QuestionFourCheck = "Yes";
        //}
        //else {
        //    alert('Please Check Checkbox for Question Two');
        //    $('#FLMQuestionFour').focus();
        //    return false;
        //}
        if (QuestionOneCheck == "Yes" && QuestionTwoCheck == "Yes" && QuestionThreeCheck == "Yes" && QuestionFourCheck == "Yes") {
            $http.post($scope.apiUrl + "Home/SaveFLMRandomQuestions?QuestionOne=" + QuestionOneCheck +
                "&QuestionTwo=" + QuestionTwoCheck + "&QuestionThree=" + QuestionThreeCheck +
                "&QuestionFour=" + QuestionFourCheck + "&ATMId=" + $scope.flmQ.txtFLMQuestionATM +
                "&DocketNumber=" + $scope.flmQ.txtFLMQuestionDocket + "&FLMDate=" + $scope.flmQ.txtFLMQuestionDate +
                "&CustodianOneQuestionOne=" + $scope.flmQ.CustodianOneQ1 +
                "&CustodianOneQuestionTwo=" + $scope.flmQ.CustodianOneQ2 +
                "&CustodianOneId=" + $scope.flmQ.CustodianOneId +
                "&CustodianOneName=" + $scope.flmQ.CustodianOneName +
                "&CustodianOnePhone=" + $scope.flmQ.CustodianOnePhone +
                "&CustodianTwoQuestionOne=" + $scope.flmQ.CustodianTwoQ1 +
                "&CustodianTwoQuestionTwo=" + $scope.flmQ.CustodianTwoQ2 +
                "&CustodianTwoId=" + $scope.flmQ.CustodianTwoId +
                "&CustodianTwoName=" + $scope.flmQ.CustodianTwoName +
                "&CustodianTwoPhone=" + $scope.flmQ.CustodianTwoPhone
            ).then(function (res) {
                if (res.data.Message == "Successfully Inserted") {
                    $('#btnFLMQuestionClose').click();
                    //alert(res.data.Message);
                }
                else {
                    $scope.flmQ.FLMQuestionOne = false;
                    $scope.flmQ.FLMQuestionTwo = false;
                    $scope.flmQ.FLMQuestionThree = false;
                    $scope.flmQ.FLMQuestionFour = false;
                    QuestionOneCheck = "No";
                    QuestionTwoCheck = "No";
                    QuestionThreeCheck = "No";
                    QuestionFourCheck = "No";
                }
            });
        }
        else {
            alert("Please check all the question first!");
        }
    }

    //generate combination here
    $scope.btnSCOOTCGenerate_Click = function () {
        $scope.loader = true;
        var rowval = $scope.scolist[$scope.rowIndex];
        if (rowval == null) {
            $scope.loader = false;
            return false;
        }
        else {
            $http.post($scope.apiUrl + "Home/RetriveATMLock?ATMID=" + $scope.txtATMID + "&RouteKeyName=" + rowval.RouteKeyName +
                "&TimeBlock=" + rowval.ddlScoTimeBlock + "&LockStatus=" + rowval.ddlScoLockStatus).then(function (res) {
                    $scope.scolist[$scope.rowIndex].CombinationNo = (res.data.OTC).toString();
                    //$scope.scolist[$scope.rowIndex].CallerNumber = res.data.CallerNo;
                    $scope.loader = false;
                }, err => {
                        console.log(err.data);
                        $scope.loader = false;
                });
        }
    }

    //generate otc manual
    $scope.GenerateManualOTC = function (manual) {
        $scope.loader = true;
        $http.post($scope.apiUrl + "Home/RetriveATMLock?ATMID=" + manual.txtManualATMID + "&RouteKeyName=" + manual.txtRouteKeyName +
            "&TimeBlock=" + manual.ddlTimeBlock + "&LockStatus=" + manual.ddlLockStatus).then(function (res) {
                $scope.manual.txtCombinationNo = (res.data.OTC).toString();
                $scope.manual.txtCallerNumber = res.data.CallerNo;
                $scope.loader = false;
            }, err => {
                console.log(err.data);
                $scope.loader = false;
            });
    }

    //save sco details
    $scope.SaveScoDetails_Click = (sdata,index) => {
        $scope.loader = true;
        var fdate = myService.ConvertDateToYYYY_MM_DD($scope.txtFromDate);
        var tdate = myService.ConvertDateToYYYY_MM_DD($scope.txtToDate);
        if (sdata.CombinationNo != null && sdata.CombinationNo != undefined && sdata.CombinationNo != "") {
            $http.post($scope.apiUrl + 'Home/SaveScoDetails?ATMId=' + sdata.ATMId + '&Date=' + sdata.Date +
                '&REGION=' + sdata.REGION + '&BRANCH=' + sdata.BRANCH + '&CITY=' + sdata.CITY + '&MSP=' + sdata.MSP + '&BANK=' + sdata.BANK +
                '&Route=' + sdata.Route + '&Activity=' + sdata.Activity +
                '&Status=' + sdata.Status + '&Custodian1Name=' + sdata.Custodian1Name + '&Custodian1RegNo=' + sdata.Custodian1RegNo +
                '&Custodian2Name=' + sdata.Custodian2Name + '&Custodian2RegNo=' + sdata.Custodian2RegNo + '&CallerNumber=' + sdata.Custodian2Mobile +
                '&RouteKeyName=' + sdata.RouteKeyName + '&CombinationNo=' + sdata.CombinationNo +
                '&RemarksOne=' + ((sdata.RemarksOne == null || sdata.RemarksOne == undefined) ? "" : sdata.RemarksOne) +
                '&RemarksTwo=' + ((sdata.RemarksTwo == null || sdata.RemarksTwo == undefined) ? "" : sdata.RemarksTwo)  +
                '&ItemRouteSheetId=' + sdata.item_route_sheet_id +
                '&Custodian1Mobile=' + sdata.Custodian1Mobile).then(function (res) {
                    //alert(res.data.Message);
                    if (res.data.Message == "Successfully Inserted") {
                        $scope.scolist[index].TypeFlag = "RMS";
                    }
                    else {
                        console.log(res.data.Message);
                    }
                    $scope.loader = false;
                    //$http.post($scope.apiUrl + 'Home/ScoDetails?FromDate=' + fdate + "&ToDate=" + tdate + "&ATMID=" + sdata.ATMId).then(function (res) {
                    //    $scope.scolist = res.data.lstDetails;
                    //    $scope.loader = false;
                    //});
                }, err => {
                        console.log(err.data.Message);
                        $scope.loader = false;
                });
        }
        else {
            alert('Combination number not found.');
            $scope.loader = false;
        }
    }

    //save Audit entry
  $scope.SaveAuditScoDetails_Click = (sdata, index) => {
    console.log(sdata);
    
        $scope.loader = true;
        var fdate = myService.ConvertDateToYYYY_MM_DD($scope.txtFromDate);
        var tdate = myService.ConvertDateToYYYY_MM_DD($scope.txtToDate);
        if (sdata.CombinationNo != null && sdata.CombinationNo != undefined && sdata.CombinationNo != "") {
            $http.post($scope.apiUrl + 'Home/SaveAuditScoDetails?ATMId=' + sdata.ATMId + '&Date=' + sdata.Date +
                '&REGION=' + sdata.REGION + '&BRANCH=' + sdata.BRANCH + '&CITY=' + sdata.CITY + '&MSP=' + sdata.MSP + '&BANK=' + sdata.BANK +
                '&Route=' + sdata.Route + '&Activity=' + sdata.Activity +
                '&Status=' + sdata.Status + '&Custodian1Name=' + sdata.Custodian1Name + '&Custodian1RegNo=' + sdata.Custodian1RegNo +
                '&Custodian2Name=' + sdata.Custodian2Name + '&Custodian2RegNo=' + sdata.Custodian2RegNo + '&CallerNumber=' + sdata.Custodian2Mobile +
                '&RouteKeyName=' + sdata.RouteKeyName + '&CombinationNo=' + sdata.CombinationNo +
                '&RemarksOne=' + ((sdata.RemarksOne == null || sdata.RemarksOne == undefined) ? "" : sdata.RemarksOne) +
                '&RemarksTwo=' + ((sdata.RemarksTwo == null || sdata.RemarksTwo == undefined) ? "" : sdata.RemarksTwo) +
                '&ItemRouteSheetId=' + sdata.item_route_sheet_id +
                '&Custodian1Mobile=' + sdata.Custodian1Mobile +
              '&AuditorId=' + sdata.AuditorId).then(function (res) {
                console.log(res);
                    if (res.data.Message == "Successfully Inserted") {
                        $scope.AuditDtls[index].TypeFlag = "RMS";
                    }
                    else {
                        console.log(res.data.Message);
                    }
                    $scope.loader = false;
                    //$http.post($scope.apiUrl + 'Home/ScoDetails?FromDate=' + fdate + "&ToDate=" + tdate + "&ATMID=" + sdata.ATMId).then(function (res) {
                    //    $scope.scolist = res.data.lstDetails;
                    //    $scope.loader = false;
                    //});
                }, err => {
                    console.log(err.data.Message);
                    $scope.loader = false;
                });
        }
        else {
            alert('Combination number not found.');
            $scope.loader = false;
        }
    }

    //save flm entry
    $scope.FlmSaveClick = function (sdata,index) {
        $scope.loader = true;
        var fdate = myService.ConvertDateToYYYY_MM_DD($scope.txtFromDate);
        var tdate = myService.ConvertDateToYYYY_MM_DD($scope.txtToDate);
        if (sdata.FLMCombinationNo != null && sdata.FLMCombinationNo != '' && sdata.FLMCombinationNo != undefined) {
            $http.post($scope.apiUrl + 'Home/SaveFlmDetails?ATMId=' + sdata.FlMATMId + '&Date=' + sdata.OPENDATE +
                '&DocketNumber=' + sdata.DocketNo + '&CallType=' + sdata.Route_Id + '&Remarks=' + sdata.Remarks + '&OpenClose=' + sdata.OpenClose +
                '&CallerNumber=' + ((sdata.FLMCallerNumber == undefined || sdata.FLMCallerNumber == null) ? "" : sdata.FLMCallerNumber) +
                '&RouteKeyName=' + ((sdata.FLMRouteKeyName == undefined || sdata.FLMRouteKeyName == null) ? "" : sdata.FLMRouteKeyName) +
                '&CombinationNo=' + ((sdata.FLMCombinationNo == undefined || sdata.FLMCombinationNo == null) ? "" : sdata.FLMCombinationNo) +
                '&RemarksOne=' + ((sdata.FLMRemarksOne == undefined || sdata.FLMRemarksOne == null) ? "" : sdata.FLMRemarksOne) +
                '&RemarksTwo=' + ((sdata.FLMRemarksTwo == undefined || sdata.FLMRemarksTwo == null) ? "" : sdata.FLMRemarksTwo) +
                '&FlmRemarks=' + ((sdata.FlmRemarks == undefined || sdata.FlmRemarks == null) ? "" : sdata.FlmRemarks)
            ).then(function (res) {
                //alert(res.data.Message);
                if (res.data.Message == "Successfully Inserted") {
                    $scope.lstFlms[index].TypeFlag = "RMSFLM";
                }
                else {
                    console.log(res.data.Message);
                }
                $scope.loader = false
                //$http.post($scope.apiUrl + 'Home/ScoDetails?FromDate=' + fdate + "&ToDate=" + tdate + "&ATMID=" + sdata.FlMATMId).then(function (res) {
                //    $scope.scolist = res.data.lstDetails;
                //    $scope.lstFlms = res.data.lstFlms;
                //    $scope.loader = false;
                //});
            }, err => {
                    console.log(err.data);
                    $scope.loader = false;
            });
        }
        else {
            alert('Combination number not found.');
            $scope.loader = false;
        }
    }

    //add otc details
    $scope.AddOTCDetails = function () {
        $scope.manual = {};
        $scope.manual.ddlActivity = "";
        $scope.manual.txtDate = date;
        $scope.manual.ddlTimeBlock = "4";
        $scope.manual.ddlLockStatus = "0";
    }
    //save manual entry
    $scope.btnSaveManualDetails_click = function (mselect) {
        if (mselect.ddlActivity == '' || mselect.ddlActivity == null || mselect.ddlActivity == undefined) {
            alert('Please Select Activity!');
            $("#ddlActivity").focus();
            return;
        }
        else if (mselect.txtManualATMID == '' || mselect.txtManualATMID == null || mselect.txtManualATMID == undefined) {
            alert('Please Enter ATM ID Details!');
            $("#txtManualATMID").focus();
            return;
        }
        else if (mselect.txtCallerNumber == '' || mselect.txtCallerNumber == null || mselect.txtCallerNumber == undefined) {
            alert('Please Enter Caller Number!');
            $("#txtCallerNumber").focus();
            return;
        }
        else if (mselect.txtRouteKeyName == '' || mselect.txtRouteKeyName == null || mselect.txtRouteKeyName == undefined) {
            alert('Please Enter Route Key Name Details!');
            $("#txtRouteKeyName").focus();
            return;
        }
        else if (mselect.txtCombinationNo == '' || mselect.txtCombinationNo == null || mselect.txtCombinationNo == undefined) {
            alert('Please Enter Combination Number!');
            $("#txtCombinationNo").focus();
            return;
        }
        else if (mselect.txtRemarks1 == '' || mselect.txtRemarks1 == null || mselect.txtRemarks1 == undefined) {
            alert('Please Enter Remarks!');
            $("#txtRemarks1").focus();
            return;
        }
        else {
            $scope.loader = true;
            var fdate = myService.ConvertDateToYYYY_MM_DD(mselect.txtDate);
            if (mselect.txtCombinationNo != null && mselect.txtCombinationNo != undefined && mselect.txtCombinationNo != "") {
                $http.post($scope.apiUrl + 'Home/ManualDetailsSave?Date=' + fdate + '&Activity=' + mselect.ddlActivity +
                    '&ATMID=' + mselect.txtManualATMID + '&CallerNumber=' + mselect.txtCallerNumber + '&RouteKeyName=' + mselect.txtRouteKeyName +
                    '&CombinationNo=' + mselect.txtCombinationNo + '&RemarksOne=' + mselect.txtRemarks1 +
                    '&RemarksTwo=' + ((mselect.txtRemarks2 == null || mselect.txtRemarks2 == undefined) ? "" : mselect.txtRemarks2) +
                    '&IsManual=1').then(function (res) {
                        $scope.manual = {};
                        $scope.loader = false;
                        $('#btnclose').click();
                        alert(res.data.Message);
                    });
            }
            else {
                alert('Combination number not found.');
                $scope.loader = false;
            }
        }
    }

    //get rowindex number
    $scope.getIndexNo = function (index) {
        $scope.rowIndex = index;
    }

    //get last three 
    $scope.GetLastThreeOtcDetails = function (Date, Branch, RouteNo) {
        $scope.otc3Title = "Last 3 OTC On This Route";
        $scope.lstLast3Details = [];
        $http.post($scope.apiUrl + 'Home/SearchLast3OTCDetails?Date=' + Date + '&Route=' + RouteNo + '&Branch=' + Branch).then(function (res) {
            $scope.lstLast3Details = res.data;
        });
    }

    //get last 3 audit details
    $scope.GetLastThreeAuditOtcDetails = function (Date, Branch, RouteNo) {
        $scope.otc3Title = "Last 3 Audit On This Route";
        $scope.lstLast3Details = [];
        $http.post($scope.apiUrl + 'Home/SearchLast3AuditOTCDetails?Date=' + Date + '&Route=' + RouteNo + '&Branch=' + Branch).then(function (res) {
            $scope.lstLast3Details = res.data;
        });
    }

    //get last 3 otc for flm
    $scope.GetLastThreeFLMOtcDetails = function (Date, Branch, RouteNo) {
        $scope.otc3Title = "Last 3 FLM OTC On This Route";
        $scope.lstLast3Details = [];
        var date = $filter('date')($scope.txtFromDate, 'yyyy-MM-dd');
        console.log(date);
        $http.post($scope.apiUrl + 'Home/SearchLast3FLMOTCDetails?Date=' + date + '&Route=' + RouteNo + '&Branch=' + Branch).then(function (res) {
            $scope.lstLast3Details = res.data;
        });
    } 
    //function GetLastThreeOtcDetails(Date, Branch, RouteNo) {
    //    $.ajax({
    //        url: '@Url.Action("SearchLast3OTCDetails")',
    //        data: {
    //            Date: Date,
    //            Route: RouteNo,
    //            Branch: Branch,
    //        },
    //        dataType: "json",
    //        type: "POST",
    //        traditional: true,
    //        success: function (data) {
    //            console.log(data);

    //            $("#myDatatableLastOtc").find("tr:not(:first)").remove();
    //            var trHTML = '';

    //            $.each(data, function (i, item) {
    //                if (data[i].Slno == '1') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:red ; font-weight: bold;">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //                if (data[i].Slno == '2') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:orange ;font-weight: bold; ">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //                if (data[i].Slno == '3') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:green ; font-weight: bold;">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //            });

    //            $('#myDatatableLastOtc').append(trHTML);
    //            //$("#btnSearch").click();
    //            //alert(data.Message);
    //        },
    //        error: function (response) {
    //            alert(response.responseText);
    //        },
    //        failure: function (response) {
    //            alert(response.responseText);
    //        }
    //    });
    //}

    //function GetLastThreeFLMOtcDetails(Date, Branch, RouteNo) {
    //    $.ajax({
    //        url: '@Url.Action("SearchLast3FLMOTCDetails")',
    //        data: {
    //            Date: Date,
    //            Route: RouteNo,
    //            Branch: Branch,
    //        },
    //        dataType: "json",
    //        type: "POST",
    //        traditional: true,
    //        success: function (data) {
    //            console.log(data);

    //            $("#myDatatableLastOtc").find("tr:not(:first)").remove();
    //            var trHTML = '';

    //            $.each(data, function (i, item) {
    //                if (data[i].Slno == '1') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:red ; font-weight: bold;">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //                if (data[i].Slno == '2') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:orange ;font-weight: bold; ">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //                if (data[i].Slno == '3') {
    //                    trHTML += '<tr style="font-size: 11px;"><td>' + data[i].ATMID + '</td>' +
    //                        '<td>' + data[i].Branch + '</td>' +
    //                        '<td>' + data[i].RouteNo + '</td>' +
    //                        '<td>' + data[i].Activity + '</td>' +
    //                        '<td>' + data[i].Crew1Name + '</td>' +
    //                        '<td>' + data[i].Crew1Regno + '</td>' +
    //                        '<td>' + data[i].Crew2Name + '</td>' +
    //                        '<td>' + data[i].Crew2Regno + '</td>' +
    //                        '<td style="color:green ; font-weight: bold;">' + data[i].LastOTCTaken + '</td>' +
    //                        '<td>' + data[i].CallerNumber + '</td>' +
    //                        '<td>' + data[i].RouteKeyName + '</td>' +
    //                        '<td>' + data[i].CombinationNo + '</td>' +
    //                        '<td>' + data[i].RemarksOne + '</td>' +
    //                        '<td>' + data[i].RemarksTwo + '</td>' +

    //                        '</tr>';
    //                }
    //            });

    //            $('#myDatatableLastOtc').append(trHTML);
    //            //$("#btnSearch").click();
    //            //alert(data.Message);
    //        },
    //        error: function (response) {
    //            alert(response.responseText);
    //        },
    //        failure: function (response) {
    //            alert(response.responseText);
    //        }
    //    });
    //}

    //$(document).ready(function () {
    //    var UserID = '@Session["UserId"]';
    //    $("#btnSaveManualDetails").click(function () {

    //        if ($("#ddlActivity").val().trim() == '' || $("#ddlActivity").val() == 'Select') {
    //            alert('Please Select Activity!');
    //            $("#ddlActivity").focus();
    //            return;
    //        }
    //        if ($("#txtManualATMID").val().trim() == '' || $("#txtManualATMID").val() == null) {
    //            alert('Please Enter ATM ID Details!');
    //            $("#txtManualATMID").focus();
    //            return;
    //        }
    //        if ($("#txtCallerNumber").val().trim() == '' || $("#txtCallerNumber").val() == null) {
    //            alert('Please Enter Caller Number!');
    //            $("#txtCallerNumber").focus();
    //            return;
    //        }
    //        if ($("#txtRouteKeyName").val() == '' || $("#txtRouteKeyName").val() == null) {
    //            alert('Please Enter Route Key Name Details!');
    //            $("#txtRouteKeyName").focus();
    //            return;
    //        }
    //        if ($("#txtCombinationNo").val() == '' || $("#txtCombinationNo").val() == null) {
    //            alert('Please Enter Combination Number!');
    //            $("#txtCombinationNo").focus();
    //            return;
    //        }
    //        if ($("#txtRemarks1").val() == '' || $("#txtRemarks1").val() == null) {
    //            alert('Please Enter Remarks!');
    //            $("#txtRemarks1").focus();
    //            return;
    //        }

    //        $.ajax({
    //            url: '@Url.Action("ManualDetailsSave")',
    //            data: {
    //                Date: $("#txtDate").val(),
    //                Activity: $("#ddlActivity").val(),
    //                ATMID: $("#txtManualATMID").val(),
    //                CallerNumber: $("#txtCallerNumber").val(),
    //                RouteKeyName: $("#txtRouteKeyName").val(),
    //                CombinationNo: $("#txtCombinationNo").val(),
    //                RemarksOne: $("#txtRemarks1").val(),
    //                RemarksTwo: $("#txtRemarks2").val(),
    //                UserID: UserID,
    //                IsManual: 1,
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            traditional: true,
    //            success: function (data) {
    //                $("#btnclose").click();
    //                $("#salimshivaraju").show();
    //            },
    //            complete: function () {
    //                $('#loading-image').hide();
    //            },
    //            error: function (response) {
    //                alert(response.responseText);
    //                $('#loading-image').hide();
    //            },
    //            failure: function (response) {
    //                alert(response.responseText);
    //                $('#loading-image').hide();
    //            }
    //        });
    //        $("#btnclose").click();
    //    });
    //});

    //$(document).ready(function () {
    //    var UserID = '@Session["UserId"]';
    //    $("#btnGenerate").click(function () {
    //        if ($("#txtManualATMID").val() == "" || $("#txtManualATMID").val() == null) {
    //            alert("Please Enter ATM ID First");
    //            $("#txtManualATMID").focus();
    //            return;
    //        }

    //        if ($("#txtRouteKeyName").val() == "" || $("#txtRouteKeyName").val() == null) {
    //            alert("Please Enter Route Key Name");
    //            $("#txtRouteKeyName").focus();
    //            return;
    //        }
    //        $.ajax({
    //            url: '@Url.Action("GetCompany")',
    //            data: {
    //                ATMID: $("#txtManualATMID").val(),
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            traditional: true,
    //            success: function (data) {
    //                company = data[0].Company;
    //                if (company.length == 0) {
    //                    alert("Please Enter Correct ATM ID");
    //                    $('#loading-image').hide();
    //                    $('#cover').hide();
    //                    return;
    //                }
    //                $.ajax({
    //                    url: '@Url.Action("RetriveATMLock")',
    //                    data: {
    //                        ATMID: $("#txtManualATMID").val(),
    //                        RouteKeyName: $("#txtRouteKeyName").val(),
    //                        Company: company,
    //                        UserId: UserID,
    //                        TimeBlock: $("#ddlOtherTimeBlock").val(),
    //                        LockStatus: $("#ddlOtherLockStatus").val(),
    //                    },
    //                    dataType: "json",
    //                    type: "POST",
    //                    traditional: true,
    //                    success: function (data) {
    //                        if (data.Msg == "Error") {
    //                            alert(data.OTC);
    //                            return;
    //                        }
    //                        else {
    //                            $("#txtCombinationNo").prop("disabled", false);
    //                            $("#txtCallerNumber").prop("disabled", false);
    //                        }
    //                        if (data.Msg == "OK") {
    //                            $("#txtCombinationNo").val(data.OTC);
    //                            $("#txtCombinationNo").prop("disabled", true);
    //                            $("#txtCallerNumber").val(data.CallerNo);
    //                            $("#txtCallerNumber").prop("disabled", true);
    //                        }
    //                        else {
    //                            $("#txtCombinationNo").prop("disabled", false);
    //                            $("#txtCallerNumber").prop("disabled", false);
    //                        }
    //                    },
    //                    complete: function () {
    //                    },
    //                    error: function (response) {
    //                        alert(response.responseText);
    //                    },
    //                    failure: function (response) {
    //                        alert(response.responseText);
    //                    }
    //                });
    //            },
    //            complete: function () {
    //            },
    //            error: function (response) {
    //                $('#loading-image').hide();
    //                $('#cover').hide();
    //                alert(response.responseText);
    //            },
    //            failure: function (response) {
    //                $('#loading-image').hide();
    //                $('#cover').hide();
    //                alert(response.responseText);
    //            }
    //        });
    //    });
    //});

    //$(document).ready(function () {
    //    $("#btnFlmSearch").click(function () {
    //        $.ajax({
    //            url: '@Url.Action("FLMRandomQuestion")',
    //            data: {
    //                CustodianPhoneNumber1: $("#txtFLMQuestionCustiodianOnePhone").val(),
    //                CustodianPhoneNumber2: $("#txtFLMQuestionCustiodianTwoPhone").val(),
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            traditional: true,
    //            success: function (data) {
    //                $('#txtFLMQuestionCustiodianOneQ1').text(data[0].CustodianOneQ1);
    //                $('#txtFLMQuestionCustiodianOneQ2').text(data[0].CustodianOneQ2);
    //                $('#txtFLMQuestionCustiodianOneId').text(data[0].CustodianOneId);
    //                $('#txtFLMQuestionCustiodianOneName').text(data[0].CustodianOneName);

    //                $('#txtFLMQuestionCustiodianOneQ3').text(data[0].CustodianTwoQ1);
    //                $('#txtFLMQuestionCustiodianOneQ4').text(data[0].CustodianTwoQ2);
    //                $('#txtFLMQuestionCustiodianTwoId').text(data[0].CustodianTwoId);
    //                $('#txtFLMQuestionCustiodianTwoName').text(data[0].CustodianTwoName);
    //            },
    //            complete: function () {
    //            },
    //            error: function (response) {
    //                alert(response.responseText);
    //            },
    //            failure: function (response) {
    //                alert(response.responseText);
    //            }
    //        });
    //    });
    //});

    //$(document).ready(function () {
    //    $("#btnFLMQuestionSave").click(function () {
    //        var QuestionOneCheck = "No";
    //        var QuestionTwoCheck = "No";
    //        var QuestionThreeCheck = "No";
    //        var QuestionFourCheck = "No";

    //        if ($('#FLMQuestionOne').is(':checked')) {
    //            QuestionOneCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question One');
    //            $('#FLMQuestionOne').focus();
    //            return;
    //        }
    //        if ($('#FLMQuestionTwo').is(':checked')) {
    //            QuestionTwoCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question Two');
    //            $('#FLMQuestionTwo').focus();
    //            return;
    //        }

    //        if ($('#FLMQuestionThree').is(':checked')) {
    //            QuestionThreeCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question One');
    //            $('#FLMQuestionThree').focus();
    //            return;
    //        }

    //        if ($('#FLMQuestionFour').is(':checked')) {
    //            QuestionFourCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question Two');
    //            $('#FLMQuestionFour').focus();
    //            return;
    //        }
    //        var UserID = '@Session["UserId"]';

    //        $.ajax({
    //            url: '@Url.Action("SaveFLMRandomQuestions")',
    //            data: {

    //                QuestionOne: QuestionOneCheck,
    //                QuestionTwo: QuestionTwoCheck,
    //                QuestionThree: QuestionThreeCheck,
    //                QuestionFour: QuestionFourCheck,
    //                ATMId: $('#txtFLMQuestionATM').text(),
    //                DocketNumber: $('#txtFLMQuestionDocket').text(),
    //                FLMDate: $('#txtFLMQuestionDate').text(),

    //                CustodianOneQuestionOne: $('#txtFLMQuestionCustiodianOneQ1').text(),
    //                CustodianOneQuestionTwo: $('#txtFLMQuestionCustiodianOneQ2').text(),
    //                CustodianOneId: $('#txtFLMQuestionCustiodianOneId').text(),
    //                CustodianOneName: $('#txtFLMQuestionCustiodianOneName').text(),
    //                CustodianOnePhone: $('#txtFLMQuestionCustiodianOnePhone').val(),

    //                CustodianTwoQuestionOne: $('#txtFLMQuestionCustiodianOneQ3').text(),
    //                CustodianTwoQuestionTwo: $('#txtFLMQuestionCustiodianOneQ4').text(),
    //                CustodianTwoId: $('#txtFLMQuestionCustiodianTwoId').text(),
    //                CustodianTwoName: $('#txtFLMQuestionCustiodianTwoName').text(),
    //                CustodianTwoPhone: $('#txtFLMQuestionCustiodianTwoPhone').val(),

    //                UserID: UserID,
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            traditional: true,
    //            success: function (data) {
    //                console.log(data.Message);
    //                if (data.Message == "Successfully Inserted") {
    //                    $('#btnQuestionClose').click();
    //                    $("#QuestionOne").prop("checked", false);
    //                    $("#QuestionTwo").prop("checked", false);
    //                    $("#QuestionThree").prop("checked", false);
    //                    $("#QuestionFour").prop("checked", false);
    //                }
    //                else {
    //                    $("#QuestionOne").prop("checked", false);
    //                    $("#QuestionTwo").prop("checked", false);
    //                    $("#QuestionThree").prop("checked", false);
    //                    $("#QuestionFour").prop("checked", false);
    //                }
    //            },
    //            complete: function () {
    //            },
    //            error: function (response) {
    //                alert(response.responseText);
    //            },
    //            failure: function (response) {
    //                alert(response.responseText);
    //            }
    //        });
    //    });
    //});

    //$(document).ready(function () {
    //    $("#btnQuestionSave").click(function () {
    //        var QuestionOneCheck = "No";
    //        var QuestionTwoCheck = "No";
    //        var QuestionThreeCheck = "No";
    //        var QuestionFourCheck = "No";

    //        if ($('#QuestionOne').is(':checked')) {
    //            QuestionOneCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question One');
    //            $('#QuestionOne').focus();
    //            return;
    //        }
    //        if ($('#QuestionTwo').is(':checked')) {
    //            QuestionTwoCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question Two');
    //            $('#QuestionTwo').focus();
    //            return;
    //        }

    //        if ($('#QuestionThree').is(':checked')) {
    //            QuestionThreeCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question One');
    //            $('#QuestionThree').focus();
    //            return;
    //        }

    //        if ($('#QuestionFour').is(':checked')) {
    //            QuestionFourCheck = "Yes";
    //        }
    //        else {
    //            alert('Please Check Checkbox for Question Two');
    //            $('#QuestionFour').focus();
    //            return;
    //        }
    //        var UserID = '@Session["UserId"]';

    //        $.ajax({
    //            url: '@Url.Action("SaveRandomQuestions")',
    //            data: {
    //                QuestionOne: QuestionOneCheck,
    //                QuestionTwo: QuestionTwoCheck,
    //                QuestionThree: QuestionThreeCheck,
    //                QuestionFour: QuestionFourCheck,
    //                ATMId: $('#txtQuestionATM').text(),
    //                Date: $('#txtQuestionDate').text(),
    //                RouteNo: $('#txtQuestionRouteNo').text(),
    //                CustodianOneQuestionOne: $('#txtQuestionCustiodianOneQ1').text(),
    //                CustodianOneQuestionTwo: $('#txtQuestionCustiodianOneQ2').text(),
    //                CustodianTwoQuestionOne: $('#txtQuestionCustiodianOneQ3').text(),
    //                CustodianTwoQuestionTwo: $('#txtQuestionCustiodianOneQ4').text(),
    //                Itefolrot: $('#txtItefolrot').val(),
    //                UserID: UserID,
    //            },
    //            dataType: "json",
    //            type: "POST",
    //            traditional: true,
    //            success: function (data) {
    //                console.log(data.Message);
    //                if (data.Message == "Successfully Inserted") {
    //                    $('#btnQuestionClose').click();
    //                    $("#QuestionOne").prop("checked", false);
    //                    $("#QuestionTwo").prop("checked", false);
    //                    $("#QuestionThree").prop("checked", false);
    //                    $("#QuestionFour").prop("checked", false);
    //                }
    //                else {
    //                    $("#QuestionOne").prop("checked", false);
    //                    $("#QuestionTwo").prop("checked", false);
    //                    $("#QuestionThree").prop("checked", false);
    //                    $("#QuestionFour").prop("checked", false);
    //                }
    //            },
    //            complete: function () {
    //            },
    //            error: function (response) {
    //                alert(response.responseText);
    //            },
    //            failure: function (response) {
    //                alert(response.responseText);
    //            }
    //        });
    //    });
    //});

    //$(document).ready(function () {
    //    $("#Question").click(function () {
    //        var FromDate = $("#txtFromDate").val();
    //        var ToDate = $("#txtToDate").val();

    //        $('#txtQuestionCustiodianOneQ1').text("");
    //        $('#txtQuestionCustiodianOneQ2').text("");
    //        $('#txtQuestionCustiodianOneQ3').text("");
    //        $('#txtQuestionCustiodianOneQ4').text("");


    //        if ($("#txtATMID").val() == "" || $("#txtATMID").val() == null) {
    //            alert("Please Enter ATM Id ");
    //            $("#ForQuestion").removeClass("ForQuestion");
    //            return;
    //        }

    //        if (FromDate != ToDate) {
    //            alert("Please Select Current Date Only");
    //            $("#ForQuestion").removeClass("ForQuestion");
    //            return;
    //        }

    //        if ($('#txtCustodianOneId').val() == "" || $('#txtCustodianOneId').val() == null) {
    //            alert("Please Search Query first");
    //            $("#ForQuestion").removeClass("ForQuestion");
    //            return;
    //        }
    //        if (FromDate == ToDate) {

    //            var Count = $("#txtClickCount").val();

    //            {
    //                Count = parseInt(Count) + 1;
    //                $("#txtClickCount").val(Count);
    //            }

    //            $("#ForQuestion").addClass("ForQuestion");
    //            var CustodianOne = $('#txtCustodianOneId').val();
    //            var CustodianTwo = $('#txtCustodianTwoId').val();
    //            var CustodianATMID = $('#txtCustodianATMID').val();
    //            var CustodianRouteNo = $('#txtCustodianRouteNo').val();
    //            var CustodianDate = $('#txtCustodianDate').val();
    //            var CustodianOneName = $('#txtCustodianOneName').val();
    //            var CustodianTwoName = $('#txtCustodianTwoName').val();
    //            var Branch = $('#txtCustodianBranch').val();

    //            $('#txtQuestionATM').text(CustodianATMID);
    //            $('#txtQuestionRouteNo').text(CustodianRouteNo);
    //            $('#txtQuestionDate').text(CustodianDate);
    //            $('#txtQuestionCustiodianOneId').text(CustodianOne);
    //            $('#txtQuestionCustiodianTwoId').text(CustodianTwo);
    //            $('#txtQuestionCustiodianOneName').text(CustodianOneName);
    //            $('#txtQuestionCustiodianTwoName').text(CustodianTwoName);


    //            $.ajax({
    //                url: '@Url.Action("RandomQuestion")',
    //                data: {
    //                    CustodianIdOne: CustodianOne,
    //                    CustodianIdTwo: CustodianTwo,
    //                    Branch: Branch,
    //                },
    //                dataType: "json",
    //                type: "POST",
    //                traditional: true,
    //                success: function (data) {
    //                    console.log(data);
    //                    $('#txtQuestionCustiodianOneQ1').text(data[0].CustodianOneQ1);
    //                    $('#txtQuestionCustiodianOneQ2').text(data[0].CustodianOneQ2);
    //                    $('#txtQuestionCustiodianOneQ3').text(data[0].CustodianTwoQ1);
    //                    $('#txtQuestionCustiodianOneQ4').text(data[0].CustodianTwoQ2);
    //                },
    //                complete: function () {
    //                },
    //                error: function (response) {
    //                    alert(response.responseText);
    //                },
    //                failure: function (response) {
    //                    alert(response.responseText);
    //                }
    //            });
    //        }
    //    })
    //});

        //$(document).ready(function () {
        //    var UserID = '@Session["UserId"]';
        //    $("#btnSCOOTCGenerate").click(function () {
        //        if ($("#txtATMID").val() == "" || $("#txtATMID").val() == null) {
        //            alert("Please Enter ATM ID First");
        //            return;
        //        }

        //        if (RouteNameStr == "" || RouteNameStr == null) {
        //            alert("Please Enter Route Key Name");
        //            return;
        //        }

        //        if (TimeStampStr == "" || TimeStampStr == null) {
        //            TimeStampStr = 4;
        //        }

        //        if (LockStatusStr == "" || LockStatusStr == null) {
        //            LockStatusStr = 0;
        //            //return;
        //        }
        //        $('#loading-image').show();
        //        $('#cover').show();

        //        var r = "#myDatatable #" + RowIndex;
        //        var s = "#myDatatableFLM #" + RowIndex;

        //        $.ajax({
        //            url: '@Url.Action("GetCompany")',
        //            data: {
        //                ATMID: $("#txtATMID").val(),
        //            },
        //            dataType: "json",
        //            type: "POST",
        //            traditional: true,
        //            success: function (data) {
        //                company = data[0].Company;
        //                if (company.length == 0) {
        //                    alert("Please Enter Correct ATM ID");
        //                    $('#loading-image').hide();
        //                    $('#cover').hide();
        //                    return;
        //                }

        //                $.ajax({
        //                    url: '@Url.Action("RetriveATMLock")',
        //                    data: {
        //                        ATMID: $("#txtATMID").val(),
        //                        RouteKeyName: RouteNameStr,
        //                        Company: data[0].Company,
        //                        UserId: UserID,
        //                        TimeBlock: TimeStampStr,
        //                        LockStatus: LockStatusStr,
        //                    },
        //                    dataType: "json",
        //                    type: "POST",
        //                    traditional: true,
        //                    success: function (data) {
        //                        if (data.Msg == "Error") {
        //                            $('#loading-image').hide();
        //                            $('#cover').hide();
        //                            //if (strClassName == "RouteKeyName") {
        //                            //    $(r).find('.CombinationNo').val(data.OTC);
        //                            //    $(r).find('.CallerNumber').val(data.CallerNo);
        //                            //    $('#loading-image').hide();
        //                            //    $('#cover').hide();
        //                            //}

        //                            //if (strClassName == "FLMRouteKeyName") {
        //                            //    $(s).find('.FLMCombinationNo').val(data.OTC);
        //                            //    $(s).find('.FlmCallerNumber').val(data.CallerNo);
        //                            //    $('#loading-image').hide();
        //                            //    $('#cover').hide();
        //                            //}
        //                            alert(data.OTC);
        //                            return;
        //                        }
        //                        if (data.Msg == "OK") {
        //                            if (strClassName == "RouteKeyName") {
        //                                $(r).find('.CombinationNo').val(data.OTC);
        //                                $(r).find('.CallerNumber').val(data.CallerNo);
        //                                $('#loading-image').hide();
        //                                $('#cover').hide();
        //                            }

        //                            if (strClassName == "FLMRouteKeyName") {
        //                                $(s).find('.FLMCombinationNo').val(data.OTC);
        //                                $(s).find('.FlmCallerNumber').val(data.CallerNo);
        //                                $('#loading-image').hide();
        //                                $('#cover').hide();
        //                            }

        //                        }

        //                    },
        //                    complete: function () {
        //                    },
        //                    error: function (response) {
        //                        $('#loading-image').hide();
        //                        $('#cover').hide();
        //                        alert(response.responseText);
        //                    },
        //                    failure: function (response) {
        //                        $('#loading-image').hide();
        //                        $('#cover').hide();
        //                        alert(response.responseText);
        //                    }
        //                });
        //            },
        //            complete: function () {
        //            },
        //            error: function (response) {
        //                $('#loading-image').hide();
        //                $('#cover').hide();
        //                alert(response.responseText);
        //            },
        //            failure: function (response) {
        //                $('#loading-image').hide();
        //                $('#cover').hide();
        //                alert(response.responseText);
        //            }
        //        });

        //    })
        //})


        //$(document).ready(function () {
        //    $("#btnSearch").click(function () {

        //        $("#txtClickCount").val("0");
        //        $("#salimshivaraju").hide();
        //        var fullDate = new Date()
        //        var twoDigitMonth;

        //        var mm = (fullDate.getMonth()) + 1;

        //        if ((mm.toString().length) == 1) {
        //            twoDigitMonth = '0' + mm;
        //        }
        //        else {
        //            twoDigitMonth = mm;
        //        }

        //        var twoDigitDate = ((fullDate.getDate().toString().length) == 1) ? '0' + (fullDate.getDate() + 0) : (fullDate.getDate().toString());

        //        var currentDate = fullDate.getDate() + "/" + twoDigitMonth + "/" + fullDate.getFullYear();
        //        var currentDateConvert = fullDate.getFullYear() + "-" + twoDigitMonth + "-" + twoDigitDate;

        //        var FLMZeroIndex = "0";

        //        $('#txtCustodianOneId').val("");
        //        $('#txtCustodianTwoId').val("");
        //        $('#txtCustodianATMID').val("");
        //        $('#txtCustodianRouteNo').val("");
        //        $('#txtCustodianDate').val("");
        //        $('#txtCustodianOneName').val("");
        //        $('#txtCustodianTwoName').val("");
        //        $('#txtCustodianBranch').val("");

        //        $('#txtQuestionCustiodianOneQ1').text("");
        //        $('#txtQuestionCustiodianOneQ2').text("");
        //        $('#txtQuestionCustiodianOneQ3').text("");
        //        $('#txtQuestionCustiodianOneQ4').text("");

        //        $('#txtCustodianOneMobile').val("");
        //        $('#txtCustodianTwoMobile').val("");

        //        $("#spnAtmId").html("");
        //        $("#spnRouteNo").html("");
        //        $("#spnStatus").html("");
        //        $("#spnLastOtc").html("");

        //        $('#loading-image').show();
        //        $('#cover').show();
        //        $.ajax({
        //            url: '@Url.Action("ScoDetails")',
        //            data: {
        //                FromDate: $("#txtFromDate").val(),
        //                ToDate: $("#txtToDate").val(),
        //                ATMID: $("#txtATMID").val()
        //            },
        //            dataType: "json",
        //            type: "POST",
        //            traditional: true,
        //            success: function (data) {
        //                console.log(data);
        //                $("#myDatatable").find("tr:not(:first)").remove();
        //                $("#myDatatableFLM").find("tr:not(:first)").remove();
        //                $("#myDatatableOTC").find("tr:not(:first)").remove();

        //                var trHTML = '';
        //                var trHTMLFLM = '';
        //                var trHTMLOther = '';
        //                $.each(data, function (i, item) {
        //                    if (data[0].TypeFlag == "SCO") {
        //                        $('#txtCustodianOneId').val(data[0].Custodian1RegNo);
        //                        $('#txtCustodianTwoId').val(data[0].Custodian2RegNo);
        //                        $('#txtCustodianATMID').val(data[0].ATMId);
        //                        $('#txtCustodianRouteNo').val(data[0].Route);
        //                        $('#txtCustodianDate').val(data[0].Date);
        //                        $('#txtCustodianOneName').val(data[0].Custodian1Name);
        //                        $('#txtCustodianTwoName').val(data[0].Custodian2Name);
        //                        $('#txtItefolrot').val(data[0].item_route_sheet_id);
        //                        $('#txtCustodianBranch').val(data[0].BRANCH);
        //                        $('#txtCustodianOneMobile').val(data[0].Custodian1Mobile);
        //                        $('#txtCustodianTwoMobile').val(data[0].Custodian2Mobile);
        //                    }
        //                    if (data[i].TypeFlag == "FLM" && data[i].OpenClose != "Close") {
        //                        if (FLMZeroIndex == "0") {
        //                            $('#txtFLMATMId').val(data[i].FlMATMId);
        //                            $('#txtFLMDocketNumber').val(data[i].DocketNo);
        //                            $('#txtFLMDate').val(data[i].OPENDATE);
        //                            FLMZeroIndex = "1";
        //                        }
        //                    }
        //                    var date = (data[i].Date);

        //                    $('#Duration').val(data[i].SiteAccessTime);
        //                    $('#Operation').val(data[i].OperationAccessTime);

        //                    if (data[i].TypeFlag == "SCO" && (currentDateConvert >= data[i].Date && currentDateConvert <= data[i].CurrentDatePlusOneDay)) {
        //                        trHTML += '<tr style="font-weight: bold;" id=' + 'SCO' + i + '><td>' + data[i].Date + '</td>' +
        //                            '<td>' + data[i].REGION + '</td>' +
        //                            '<td>' + data[i].BRANCH + '</td>' +
        //                            '<td>' + data[i].CITY + '</td>' +
        //                            '<td>' + data[i].MSP + '</td>' +
        //                            '<td>' + data[i].BANK + '</td>' +
        //                            '<td>' + data[i].ATMId + '</td>' +
        //                            '<td  data-toggle="modal" data-target="#myModalLast" style="cursor:pointer;text-decoration: underline;color: blue;">' + data[i].Route + '</td>' +
        //                            '<td>' + data[i].Activity + '</td>' +
        //                            '<td>' + data[i].Status + '</td>' +
        //                            '<td>' + data[i].Custodian1Name + '</td>' +
        //                            '<td>' + data[i].Custodian1RegNo + '</td>' +
        //                            '<td>' + data[i].Custodian1Mobile + '</td>' +
        //                            '<td>' + data[i].Custodian2Name + '</td>' +
        //                            '<td>' + data[i].Custodian2RegNo + '</td>' +
        //                            '<td>' + data[i].Custodian2Mobile + '</td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeSco" class="form-control" value="SCO"/></td>' +
        //                            '<td style="display:none"> <input type="text" id="txtCallerNumberSco" value="' + data[i].Custodian2Mobile + '" class="form-control CallerNumber" onkeypress="return IsNumeric(event);" placeholder="Caller Number" maxlength="10" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text"  onchange="TakeRouteKeyName(\'' + 'SCO' + i + '\',event,\'RouteKeyName\')" class="form-control RouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            //'<td> <input type="text" onchange="TakeRouteKeyName(\'' + 'SCO' + i + '\',event,\'RouteKeyName\')" id="txtRouteKeyNameSco" class="form-control RouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtCombinationNoSco" class="form-control CombinationNo" placeholder="Combination No " style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksOneSco" class="form-control RemarksOne" placeholder="Remarks One" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksTwoSco" class="form-control RemarksTwo" placeholder="Remarks Two" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <select class="form-control ddlScoTimeBlock" onchange="TakeTimeBlock(\'' + 'SCO' + i + '\',event,\'ddlScoTimeBlock\')" id="ddlScoTimeBlock" style="background-color:#ecb7f4;width:80px;"> ' +
        //                            ' <option>4</option> ' +
        //                            ' <option>8</option> ' +
        //                            ' <option>12</option> ' +
        //                            ' <option>24</option> ' +
        //                            ' </select> </td>' +
        //                            '<td> <select class="form-control ddlScoLockStatus" onchange="TakeLockStatus(\'' + 'SCO' + i + '\',event,\'ddlScoLockStatus\')"  id="ddlScoLockStatus" style="background-color:#ecb7f4;width:80px;"> ' +
        //                            ' <option>0</option> ' +
        //                            ' <option>1</option> ' +
        //                            ' <option>2</option> ' +
        //                            ' <option>3</option> ' +
        //                            ' </select> </td>' +
        //                            '<td> <button type="button" id="" class="btn btn-primary Click">Save</button></td>' +
        //                            '<td style="display: none">' + data[i].item_route_sheet_id + '</td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }

        //                    if (data[i].TypeFlag == "BothRMSSCO" && (currentDateConvert >= data[i].Date && currentDateConvert <= data[i].CurrentDatePlusOneDay)) {
        //                        trHTML += '<tr style="font-weight: bold;" id=' + 'RMS' + i + '><td>' + data[i].Date + '</td>' +
        //                            '<td>' + data[i].REGION + '</td>' +
        //                            '<td>' + data[i].BRANCH + '</td>' +
        //                            '<td>' + data[i].CITY + '</td>' +
        //                            '<td>' + data[i].MSP + '</td>' +
        //                            '<td>' + data[i].BANK + '</td>' +
        //                            '<td>' + data[i].ATMId + '</td>' +
        //                            '<td>' + data[i].Route + '</td>' +
        //                            '<td>' + data[i].Activity + '</td>' +
        //                            '<td>' + data[i].Status + '</td>' +
        //                            '<td>' + data[i].Custodian1Name + '</td>' +
        //                            '<td>' + data[i].Custodian1RegNo + '</td>' +
        //                            '<td>' + data[i].Custodian1Mobile + '</td>' +
        //                            '<td>' + data[i].Custodian2Name + '</td>' +
        //                            '<td>' + data[i].Custodian2RegNo + '</td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeSco" class="form-control" value="SCO"/></td>' +
        //                            '<td> <input type="text" id="txtCallerNumberSco" class="form-control CallerNumber" onkeypress="return IsNumeric(event);" placeholder="Caller Number" maxlength="10" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" onchange="TakeRouteKeyName(\'' + 'RMS' + i + '\',event,\'RouteKeyName\')" id="txtRouteKeyNameSco" class="form-control RouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtCombinationNoSco" class="form-control CombinationNo" placeholder="Combination No " style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksOneSco" class="form-control RemarksOne" placeholder="Remarks One" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksTwoSco" class="form-control RemarksTwo" placeholder="Remarks Two" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <button type="button" id="" class="btn btn-primary Click">Save</button></td>' +
        //                            '<td style="display: none">' + data[i].item_route_sheet_id + '</td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }

        //                    if (data[i].TypeFlag == "SCO" && (currentDateConvert < data[i].Date || currentDateConvert > data[i].CurrentDatePlusOneDay)) {
        //                        trHTML += '<tr Style="background:#e8c1ba;font-weight: bold;"><td>' + data[i].Date + '</td>' +
        //                            '<td>' + data[i].REGION + '</td>' +
        //                            '<td>' + data[i].BRANCH + '</td>' +
        //                            '<td>' + data[i].CITY + '</td>' +
        //                            '<td>' + data[i].MSP + '</td>' +
        //                            '<td>' + data[i].BANK + '</td>' +
        //                            '<td>' + data[i].ATMId + '</td>' +
        //                            '<td>' + data[i].Route + '</td>' +
        //                            '<td>' + data[i].Activity + '</td>' +
        //                            '<td>' + data[i].Status + '</td>' +
        //                            '<td>' + data[i].Custodian1Name + '</td>' +
        //                            '<td>' + data[i].Custodian1RegNo + '</td>' +
        //                            '<td>' + data[i].Custodian2Name + '</td>' +
        //                            '<td>' + data[i].Custodian2RegNo + '</td>' +
        //                            '<td style="display:none"> <input type="text" id="txtType" class="form-control" value="SCO" /></td>' +
        //                            '<td> <input type="text" id="txtCaller" class="form-control" placeholder="Caller Number" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRouteKey" class="form-control" placeholder="Route Key Name" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtCombination" class="form-control" placeholder="Combination No" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRemarks1" class="form-control" placeholder="Remarks One" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRemarks2" class="form-control" placeholder="Remarks Two" readonly="true"/></td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsScosco1" class="btn btn-primary" disabled>Save</button></td>' +
        //                            '<td style="display: none">' + data[i].item_route_sheet_id + '</td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }
        //                    if (data[i].TypeFlag == "RMS") {
        //                        trHTML += '<tr Style="background:#90ee90;font-weight: bold;"><td>' + data[i].Date + '</td>' +
        //                            '<td>' + data[i].REGION + '</td>' +
        //                            '<td>' + data[i].BRANCH + '</td>' +
        //                            '<td>' + data[i].CITY + '</td>' +
        //                            '<td>' + data[i].MSP + '</td>' +
        //                            '<td>' + data[i].BANK + '</td>' +
        //                            '<td>' + data[i].ATMId + '</td>' +
        //                            '<td>' + data[i].Route + '</td>' +
        //                            '<td>' + data[i].Activity + '</td>' +
        //                            '<td>' + data[i].Status + '</td>' +
        //                            '<td>' + data[i].Custodian1Name + '</td>' +
        //                            '<td>' + data[i].Custodian1RegNo + '</td>' +
        //                            '<td>' + data[i].Custodian2Name + '</td>' +
        //                            '<td>' + data[i].Custodian2RegNo + '</td>' +
        //                            '<td style="display:none"> <input type="text" id="txtType" class="form-control" value="SCO" /></td>' +
        //                            '<td>' + data[i].CallerNumber + '</td>' +
        //                            '<td>' + data[i].RouteKeyName + '</td>' +
        //                            '<td>' + data[i].CombinationNo + '</td>' +
        //                            '<td>' + data[i].RemarksOne + '</td>' +
        //                            '<td>' + data[i].RemarksTwo + '</td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsSco" class="btn btn-primary FlmClick" disabled>Save</button></td>' +
        //                            '<td  style="display:none">' + data[i].item_route_sheet_id + '</td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }

        //                    //This is code which need to change for flm otc integration
        //                    if (data[i].TypeFlag == "FLM" && data[i].OpenClose != "CLOSE" && data[i].OpenClose != "SLM") {
        //                        trHTMLFLM += '<tr style="font-weight: bold;" id=' + 'FLM' + i + '><td>' + data[i].FlMATMId + '</td>' +
        //                            '<td>' + data[i].OPENDATE + '</td>' +
        //                            '<td>' + data[i].DocketNo + '</td>' +
        //                            '<td  data-toggle="modal" data-target="#myModalLast" style="cursor:pointer;text-decoration: underline;color: blue;">' + data[i].Route_Id + '</td>' +
        //                            '<td style="display:none">' + data[i].Branch + '</td>' +
        //                            '<td>' + data[i].Remarks + '</td>' +
        //                            '<td>' + data[i].OpenClose + '</td>' +
        //                            '<td><input type="text" id="txtType1" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true" style="width:150px;"/></td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeFlm" class="form-control" value="FLM" /></td>' +
        //                            '<td> <input type="text" class="form-control FlmCallerNumber" maxlength="10" onkeypress="return IsNumeric(event);" placeholder="Caller Number" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" onchange="TakeRouteKeyName(\'' + 'FLM' + i + '\',event,\'FLMRouteKeyName\')" id="txtRouteKeyNameFlm2" class="form-control FLMRouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtCombinationNoFlm2" class="form-control FLMCombinationNo" placeholder="Combination No" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksOneFlm2" class="form-control FlmRemarksOne" placeholder="Remarks One" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksTwoFlm2" class="form-control FlmRemarksTwo" placeholder="Remarks Two" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsFlm" class="btn btn-primary FlmClick">Save</button></td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td> <select class="form-control ddlFlmTimeBlock" onchange="TakeTimeBlock(\'' + 'FLM' + i + '\',event,\'ddlFlmTimeBlock\')" id="ddlFlmTimeBlock" style="background-color:#ecb7f4;width:80px;"> ' +
        //                            ' <option>4</option> ' +
        //                            ' <option>8</option> ' +
        //                            ' <option>12</option> ' +
        //                            ' <option>24</option> ' +
        //                            ' </select> </td>' +
        //                            '<td> <select class="form-control ddlFlmLockStatus" onchange="TakeLockStatus(\'' + 'FLM' + i + '\',event,\'ddlFlmLockStatus\')"  id="ddlFlmLockStatus" style="background-color:#ecb7f4;width:80px;"> ' +
        //                            ' <option>0</option> ' +
        //                            ' <option>1</option> ' +
        //                            ' <option>2</option> ' +
        //                            ' <option>3</option> ' +
        //                            ' </select> </td>' +
        //                            '</tr>';
        //                    }
        //                    if (data[i].TypeFlag == "FLM" && (data[i].OpenClose == "CLOSE")) {
        //                        trHTMLFLM += '<tr Style="background:#e8c1ba;font-weight: bold;"><td>' + data[i].FlMATMId + '</td>' +
        //                            '<td>' + data[i].OPENDATE + '</td>' +
        //                            '<td>' + data[i].DocketNo + '</td>' +
        //                            '<td>' + data[i].Route_Id + '</td>' +
        //                            '<td style="display:none">' + data[i].Branch + '</td>' +
        //                            '<td>' + data[i].Remarks + '</td>' +
        //                            '<td>' + data[i].OpenClose + '</td>' +
        //                            '<td><input type="text" id="txtType" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true"/></td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeFlm1" class="form-control" value="FLM" /></td>' +
        //                            '<td> <input type="text" id="txtCallerNumberFLM1" class="form-control" placeholder="CallerNumber" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRouteKeyNameFlm1" class="form-control" placeholder="Route Key Name" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtCombinationNoFlm1" class="form-control" placeholder="Combination No" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksOneFlm1" class="form-control" placeholder="Remarks One" readonly="true"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksTwoFlm1" class="form-control" placeholder="Remarks Two" readonly="true"/></td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsFlm3" class="btn btn-primary"  disabled>Save</button></td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }
        //                    //NEW FLM SLM
        //                    if (data[i].TypeFlag == "FLM" && data[i].OpenClose == "SLM") {
        //                        trHTMLFLM += '<tr style="font-weight: bold;"><td>' + data[i].FlMATMId + '</td>' +
        //                            '<td>' + data[i].OPENDATE + '</td>' +
        //                            '<td>' + data[i].DocketNo + '</td>' +
        //                            '<td  data-toggle="modal" data-target="#myModalLast" style="cursor:pointer;text-decoration: underline;color: blue;">' + data[i].Route_Id + '</td>' +
        //                            '<td style="display:none">' + data[i].Branch + '</td>' +
        //                            '<td>' + data[i].Remarks + '</td>' +
        //                            '<td>' + data[i].OpenClose + '</td>' +
        //                            '<td><input type="text" id="txtType1" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true" style="width:150px;"/></td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeFlm" class="form-control" value="FLM" /></td>' +
        //                            '<td> <input type="text" class="form-control FlmCallerNumber" maxlength="10" onkeypress="return IsNumeric(event);" placeholder="Caller Number" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRouteKeyNameFlm2" class="form-control FLMRouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtCombinationNoFlm2" class="form-control FLMCombinationNo" placeholder="Combination No" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksOneFlm2" class="form-control FlmRemarksOne" placeholder="Remarks One" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <input type="text" id="txtRemarksTwoFlm2" class="form-control FlmRemarksTwo" placeholder="Remarks Two" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsFlm" class="btn btn-primary FlmClick">Save</button></td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }
        //                    if (data[i].TypeFlag == "RMSFLM") {
        //                        trHTMLFLM += '<tr Style="background:#90ee90;font-weight: bold;"><td>' + data[i].FlMATMId + '</td>' +
        //                            '<td>' + data[i].OPENDATE + '</td>' +
        //                            '<td>' + data[i].DocketNo + '</td>' +
        //                            '<td>' + data[i].Route_Id + '</td>' +
        //                            '<td style="display:none">' + data[i].Branch + '</td>' +
        //                            '<td>' + data[i].Remarks + '</td>' +
        //                            '<td>' + data[i].OpenClose + '</td>' +
        //                            '<td><input type="text" id="txtType2" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true"/></td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeFlm1" class="form-control" value="FLM" /></td>' +
        //                            '<td>' + data[i].FLMCallerNumber + '</td>' +
        //                            '<td>' + data[i].FLMRouteKeyName + '</td>' +
        //                            '<td>' + data[i].FLMCombinationNo + '</td>' +
        //                            '<td>' + data[i].FLMRemarksOne + '</td>' +
        //                            '<td>' + data[i].FLMRemarksTwo + '</td>' +
        //                            '<td> <button type="button" id="btnSaveUserDetailsFlm1" class="btn btn-primary"  disabled>Save</button></td>' +
        //                            '<td>' + data[i].ActualTime + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '<td>' + data[i].UserName + '</td>' +
        //                            '</tr>';
        //                    }

        //                    if (data[i].TypeFlag == "BothRMSFLMData") {
        //                        if (data[i].OpenClose != "CLOSE") {
        //                            trHTMLFLM += '<tr style="font-weight: bold;" id=' + 'BothRMSFLMData' + i + '><td>' + data[i].FlMATMId + '</td>' +
        //                                '<td>' + data[i].OPENDATE + '</td>' +
        //                                '<td>' + data[i].DocketNo + '</td>' +
        //                                '<td  data-toggle="modal" data-target="#myModalLast" style="cursor:pointer;text-decoration: underline;color: blue;">' + data[i].Route_Id + '</td>' +
        //                                '<td style="display:none">' + data[i].Branch + '</td>' +
        //                                '<td>' + data[i].Remarks + '</td>' +
        //                                '<td>' + data[i].OpenClose + '</td>' +
        //                                '<td><input type="text" id="txtType3" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true"/></td>' +
        //                                '<td style="display:none"> <input type="text" id="txtTypeFlm" class="form-control" value="FLM" /></td>' +
        //                                '<td> <input type="text" id="txtCallerNumberFlm2" class="form-control FlmCallerNumber" maxlength="10" onkeypress="return IsNumeric(event);" placeholder="Caller Number" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                                '<td> <input type="text" onchange="TakeRouteKeyName(\'' + 'BothRMSFLMData' + i + '\',event,\'FLMRouteKeyName\')" id="txtRouteKeyNameFlm2" class="form-control FLMRouteKeyName" placeholder="Route Key Name" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                                '<td> <input type="text" id="txtCombinationNoFlm2" class="form-control FLMCombinationNo" placeholder="Combination No" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                                '<td> <input type="text" id="txtRemarksOneFlm2" class="form-control FlmRemarksOne" placeholder="Remarks One" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                                '<td> <input type="text" id="txtRemarksTwoFlm2" class="form-control FlmRemarksTwo" placeholder="Remarks Two" style="background-color:#ecb7f4;width:100px;"/></td>' +
        //                                '<td> <button type="button" id="btnSaveUserDetailsFlm" class="btn btn-primary FlmClick">Save</button></td>' +
        //                                '<td>' + data[i].ActualTime + '</td>' +
        //                                '<td>' + data[i].UserName + '</td>' +
        //                                '<td> <select class="form-control ddlFlmTimeBlock" onchange="TakeTimeBlock(\'' + 'BothRMSFLMData' + i + '\',event,\'ddlFlmTimeBlock\')" id="ddlFlmTimeBlock" style="background-color:#ecb7f4;width:80px;"> ' +
        //                                ' <option>4</option> ' +
        //                                ' <option>8</option> ' +
        //                                ' <option>12</option> ' +
        //                                ' <option>24</option> ' +
        //                                ' </select> </td>' +
        //                                '<td> <select class="form-control ddlFlmLockStatus" onchange="TakeLockStatus(\'' + 'BothRMSFLMData' + i + '\',event,\'ddlFlmLockStatus\')"  id="ddlFlmLockStatus" style="background-color:#ecb7f4;width:80px;"> ' +
        //                                ' <option>0</option> ' +
        //                                ' <option>1</option> ' +
        //                                ' <option>2</option> ' +
        //                                ' <option>3</option> ' +
        //                                ' </select> </td>' +
        //                                '</tr>';
        //                        }

        //                        if (data[i].OpenClose == "CLOSE") {
        //                            trHTMLFLM += '<tr Style="background:#e8c1ba;font-weight: bold;"><td>' + data[i].FlMATMId + '</td>' +
        //                                '<td>' + data[i].OPENDATE + '</td>' +
        //                                '<td>' + data[i].DocketNo + ' CLOSE</td>' +
        //                                '<td>' + data[i].Route_Id + '</td>' +
        //                                '<td style="display:none">' + data[i].Branch + '</td>' +
        //                                '<td>' + data[i].Remarks + '</td>' +
        //                                '<td>' + data[i].OpenClose + '</td>' +
        //                                '<td><input type="text" id="txtType" class="form-control FlmRemarks" value="' + data[i].FlmRemarks + '" title="' + data[i].FlmRemarks + '" readonly="true"/></td>' +
        //                                '<td style="display:none"> <input type="text" id="txtTypeFlm1" class="form-control" value="FLM" /></td>' +
        //                                '<td> <input type="text" id="txtCallerNumberFLM1" class="form-control" placeholder="CallerNumber" readonly="true"/></td>' +
        //                                '<td> <input type="text" id="txtRouteKeyNameFlm1" class="form-control" placeholder="Route Key Name" readonly="true"/></td>' +
        //                                '<td> <input type="text" id="txtCombinationNoFlm1" class="form-control" placeholder="Combination No" readonly="true"/></td>' +
        //                                '<td> <input type="text" id="txtRemarksOneFlm1" class="form-control" placeholder="Remarks One" readonly="true"/></td>' +
        //                                '<td> <input type="text" id="txtRemarksTwoFlm1" class="form-control" placeholder="Remarks Two" readonly="true"/></td>' +
        //                                '<td> <button type="button" id="btnSaveUserDetailsFlm3" class="btn btn-primary"  disabled>Save</button></td>' +
        //                                '<td>' + data[i].ActualTime + '</td>' +
        //                                '<td>' + data[i].UserName + '</td>' +
        //                                '<td>' + data[i].UserName + '</td>' +
        //                                '<td>' + data[i].UserName + '</td>' +
        //                                '</tr>';
        //                        }
        //                    }
        //                    //End of FLM Segment
        //                    if (data[i].TypeFlag == "OtherRMS") {
        //                        trHTMLOther += '<tr Style="background:#90ee90;font-weight: bold;"><td>' + data[i].OtherActivity + '</td>' +
        //                            '<td>' + data[i].OtherCallerNumber + '</td>' +
        //                            '<td>' + data[i].OtherDate + '</td>' +
        //                            '<td>' + data[i].OtherRouteKeyName + '</td>' +
        //                            '<td>' + data[i].OtherCombinationNo + '</td>' +
        //                            '<td>' + data[i].OtherRemarksOne + '</td>' +
        //                            '<td>' + data[i].OtherRemarksTwo + '</td>' +
        //                            '<td style="display:none"> <input type="text" id="txtTypeOTC" class="form-control" value="OTC" /></td>' +
        //                            '<td>' + data[i].OtherCombinationIssuedBy + '</td>' +
        //                            '<td>' + data[i].OtherCombinationIssuedTime + '</td>' +
        //                            '<td>' + data[i].OtherATMID + '</td>' +
        //                            '<td>' + data[i].OtherATMID + '</td>' +
        //                            '<td>' + data[i].OtherATMID + '</td>' +
        //                            '</tr>';
        //                    }
        //                });

        //                $('#myDatatable').append(trHTML);//To add the table row to table
        //                $('#myDatatableFLM').append(trHTMLFLM);//To add the table row to table
        //                $('#myDatatableOTC').append(trHTMLOther);//To add the table row to table


        //            },
        //            complete: function () {
        //                $('#loading-image').hide();
        //                $('#cover').hide();
        //            },
        //            error: function (response) {
        //                alert(response.responseText);
        //                $('#loading-image').hide();
        //            },
        //            failure: function (response) {
        //                alert(response.responseText);
        //                $('#loading-image').hide();
        //            }
        //        });
        //        $.ajax({
        //            url: '@Url.Action("SearchMessageDetails")',
        //            data: {
        //            },
        //            dataType: "json",
        //            type: "POST",
        //            traditional: true,
        //            success: function (data) {
        //                $(".messages li").remove();
        //                $.each(data, function (i, item) {
        //                    var date = (data[i].Date);

        //                    messagesContainer = $('.messages');
        //                    messagesContainer.append([
        //                        '<li class="self" onclick="GetId(' + data[i].RecId + ',\'' + data[i].MessageBox + '\')" value="' + data[i].RecId + '">',
        //                        data[i].MessageBox,
        //                        '</li>'
        //                    ].join(''));

        //                });
        //            },
        //            complete: function () {
        //            },
        //            error: function (response) {
        //                alert(response.responseText);
        //            },
        //            failure: function (response) {
        //                alert(response.responseText);
        //            }
        //        });
        //        $.ajax({
        //            url: '@Url.Action("GetOTCRouteStatus")',
        //            data: {
        //                FromDate: $("#txtFromDate").val(),
        //                ToDate: $("#txtToDate").val(),
        //                ATMID: $("#txtATMID").val()
        //            },
        //            dataType: "json",
        //            type: "POST",
        //            traditional: true,
        //            success: function (data) {
        //                $("#spnAtmId").html(data[0].AtmId);
        //                $("#spnRouteNo").html(data[0].RouteNo);
        //                $("#spnStatus").html(data[0].Status);
        //                $("#spnLastOtc").html(data[0].LastOtcTaken);
        //            },
        //            complete: function () {
        //            },
        //            error: function (response) {
        //                alert(response.responseText);
        //            },
        //            failure: function (response) {
        //                alert(response.responseText);
        //            }
        //        });
        //    });
        //});
});
