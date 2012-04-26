var date = new Date();
var ccy;


$(document).ready(function () {
    GenerateDateTimePanel();
    SendRequestDataMonitoringByDate();
});

function GenerateDateTimePanel() {
    $("#dateTimePanel").append("<div id='datePanel'><label id='Back' class='direction left'><</label><label id='date'>" + String.format("{0:yyyy-MM-dd}", date) + "</label>" +
                                "<label id='Forward' class='direction right'>></label></div><label id='clock' class='right'>" + String.format("{0:HH:mm:ss}", date) + "</label>");
    $("#mainMOnitoringEPOS").append("<table id='tableMonitoring'><thead><th class='no'>No</th><th class='tenanId'>Tenant ID</th><th class='tenanName'>Tenant</th><th class='jam'>Transaksi Pertama</th>" +
                                "<th class='jam'>Transaksi Terakhir</th><th class='idle'>Idle</th><th class='totalItem'>Total Item</th>" +
                                "<th class='totalTransaksi'>Total Transaksi</th><th class='totalPenjualan'>Total Penjualan</th></thead><tbody></tbody><tfoot></tfoot></table>");
    $("#Back").click(GoToPreviousDay);
    $("#Forward").click(GoToNextDay);
}

function SendRequestDataMonitoringByDate() {
    var dateForRequest = String.format("{0}-{1}-{2}", date.getFullYear(), date.getMonth() + 1, date.getDate());
    $.ajax({
        type: "GET",
        url: "DisplayMonitoringEPOS/FindMonitoringByDate",
        data: { "date": dateForRequest },
        datatype: "json",
        success: InsertDataToTable
    });
}

function InsertDataToTable(data) {
    var index = 1;
    var SumTotalSalesPerTenan = 0;
    var SumTotalSalesPerTenanUSD = 0;
    var SumTotalTransaction = 0;
    var SumTotalItems = 0;
    $("#tableMonitoring tbody").empty();
    $("#tableMonitoring tfoot").empty();
    $.each(data, function (item) {

        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        SumTotalItems = SumTotalItems + data[item].TotalItem;
        SumTotalTransaction = SumTotalTransaction + data[item].TotalTransaction;
        if (data[item].Ccy == "IDR") {
            SumTotalSalesPerTenan = SumTotalSalesPerTenan + data[item].TotalSalesPerTenan;
        } else {
            SumTotalSalesPerTenanUSD = SumTotalSalesPerTenanUSD + data[item].TotalSalesPerTenan;
        }
        $("#tableMonitoring tbody").append("<tr style='background-color:" + color + "'>" +
                                "<td class='no'>" + index + "</td>" +
                                "<td class='tenanId'>" + data[item].TenanId + "</td>" +
                                "<td class='tenanName'>" + data[item].TenanName + "</td>" +
                                "<td class='jam'>" + data[item].StartTimeString + "</td>" +
                                "<td class='jam'>" + data[item].EndTimeString + "</td>" +
                                "<td class='idle'>" + CountIdleTime(data[item].EndTime) + "</td>" +
                                "<td class='totalItem'>" + data[item].TotalItem + "</td>" +
                                "<td class='totalTransaction'>" + data[item].TotalTransaction + "</td>" +
                                "<td class='totalSales'><span id='ccy'>" + ccy + "</span>" + String.format("{0:c}", data[item].TotalSalesPerTenan) + "</td></tr>");
        index++;
    });
    $("#tableMonitoring tfoot").append("<tr><td class='Total' colspan='6' rowspan='2'>Total</td><td class='totalItem' rowspan='2'>" + SumTotalItems + "</td>" +
                                "<td class='totalTransaction' rowspan='2'>" + SumTotalTransaction + "</td>" +
                                "<td class='totalSales'><span id='ccy'>Rp</span>" + String.format("{0:c}", SumTotalSalesPerTenan) + "</td></tr>" +
                                "<tr><td class='totalSales'><span id='ccy'>$</span>" + String.format("{0:c}", SumTotalSalesPerTenanUSD) + "</td></tr>");
}

function GoToPreviousDay() {
    var prevDate = date.getDate() - 1;
    date = new Date(date.getFullYear(), date.getMonth(), prevDate);
    $("#date").text(String.format("{0:yyyy-MM-dd}", date));
    SendRequestDataMonitoringByDate();
}

function GoToNextDay() {
    var nextDate = date.getDate() + 1;
    date = new Date(date.getFullYear(), date.getMonth(), nextDate);
    $("#date").text(String.format("{0:yyyy-MM-dd}", date));
    SendRequestDataMonitoringByDate();
}

function CountIdleTime(jsonTime) {
    var tempDate = parseInt(jsonTime.replace(/\/Date\((.*?)\)\//gi, "$1"), 10);
    var LastTransactionDate = new Date(tempDate);
    var Idle = GetTimeDifference(LastTransactionDate, new Date());
    var IdleTime = (Idle.hours+(Idle.days*24)) + ":" + Idle.minutes + ":" + Idle.seconds;
    return IdleTime;
}

function GetTimeDifference(earlierDate, laterDate) {
    var nTotalDiff = laterDate.getTime() - earlierDate.getTime();
    var oDiff = new Object();

    oDiff.days = Math.floor(nTotalDiff / 1000 / 60 / 60 / 24);
    nTotalDiff -= oDiff.days * 1000 * 60 * 60 * 24;

    oDiff.hours = Math.floor(nTotalDiff / 1000 / 60 / 60);
    nTotalDiff -= oDiff.hours * 1000 * 60 * 60;

    oDiff.minutes = Math.floor(nTotalDiff / 1000 / 60);
    nTotalDiff -= oDiff.minutes * 1000 * 60;

    oDiff.seconds = Math.floor(nTotalDiff / 1000);

    return oDiff;

}