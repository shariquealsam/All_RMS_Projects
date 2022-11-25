/// <reference path="angular.js" />
var app = angular.module("myApp", []);

app.service("myService", function ($filter) {

    this.ConvertDateToYYYY_MM_DD = function (date) {
        var dd = ("0" + date.getDate()).slice(-2);
        var mm = ("0" + (date.getMonth() + 1)).slice(-2);
        var yyyy = date.getFullYear();
        var rdate = [yyyy, mm, dd].join('-');
        return rdate;
    }

    this.StartDate = function (date) {
        //date.setDate(date.getDate() + (-30));
        var dd = ("0" + date.getDate()).slice(-2);
        var mm = ("0" + (date.getMonth())).slice(-2);
        var yyyy = date.getFullYear();
        var rdate = new Date(yyyy, mm, dd);
        return rdate;
    }

    this.ConvertJsonDateToDate = function (jdate) {
        var date = null;
        if (jdate != undefined) {
            date = new Date(jdate.split('-').reverse().join('-'));  
        }
        return date;
    }
});