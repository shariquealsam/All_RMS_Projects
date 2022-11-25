/// <reference path="myapp.js" />

app.service("myService", function ($http, $filter) {

    this.ConvertJsonDateToDate = function (jdate) {
        var date = new Date($filter('date')(jdate, 'yyyy-MM-dd'));
        return date;
    }
    this.ConvertJsonDateToTime = function (jtime) {
        var date = new Date($filter('date')(jtime, 'hh:mm a'));
        return date;
    }

    this.ConvertDateToYYYY_MM_DD = function (date) {
        var dd = ("0" + date.getDate()).slice(-2);
        var mm = ("0" + (date.getMonth() + 1)).slice(-2);
        var yyyy = date.getFullYear();
        var rdate = [yyyy, mm, dd].join('-');
        return rdate;
    }
});