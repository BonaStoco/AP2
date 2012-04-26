var no = 0;
var tanggal;
var locationId;

function nextMonth() {
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());
    no++;

    if (no - 1 == 0) {
        $("#next").css('display', 'none');
    }
    $("#no").text(no);

    $.ajax({
        type: "GET",
        url: "/APReportSale/NextMonthPenjualan",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResultMonth,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });

}

function previousMonth() {
    $("#next").css('display', 'inline');
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());
    no--;
    $("#no").text(no);

    $.ajax({
        type: "GET",
        url: "/APReportSale/PreviousMonthPenjualan",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResultMonth
    });
}

function dateResultMonth(data, status) {
    if (data != null) {
        var tgl = data.Transactiondate;
        tanggal = tgl;
        $("#today").text(data.Transactiondate);
        $("#twoDaysBefore").text(data.TwoMonthBefore);
        $("#previousDay").text(data.OneMonthBefore);
        $("#date1-idr").text("Rp." + data.TotalSaleIDRTwoMonthBefore);
        $("#date2-idr").text("Rp." + data.TotalSaleIDROneMonthBefore);
        $("#date3-idr").text("Rp." + data.TotalSaleIDRCurrentMonth);
        $("#date1-usd").text("$" + data.TotalSaleUSDTwoMonthBefore);
        $("#date2-usd").text("$" + data.TotalSaleUSDOneMonthBefore);
        $("#date3-usd").text("$" + data.TotalSaleUSDCurrentMonth);
    }
}

function next() {
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());
    no++;

    if (no - 1 == 0) {
        $("#next").css('display', 'none');
    }
    $("#no").text(no);

    $.ajax({
        type: "GET",
        url: "/APReportSale/NextPenjualanHarian",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResult,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
}

function dateResult(data, status) {
    if (data != null) {
        var tgl = data.Transactiondate;
        tanggal = tgl;
        $("#today").text(data.Transactiondate);
        $("#twoDaysBefore").text(data.TwoDaysBefore);
        $("#previousDay").text(data.PreviousDay);
        $("#date1-idr").text("Rp." + data.TotalSaleIDRTwoDaysBefore);
        $("#date2-idr").text("Rp." + data.TotalSaleIDRPreviousDay);
        $("#date3-idr").text("Rp." + data.TotalSaleIDR);
        $("#date1-usd").text("$" + data.TotalSaleUSDTwoDaysBefore);
        $("#date2-usd").text("$" + data.TotalSaleUSDPreviousDay);
        $("#date3-usd").text("$" + data.TotalSaleUSD);
    }
}

function previous() {
    $("#next").css('display', 'inline');
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());
    no--;
    $("#no").text(no);

    $.ajax({
        type: "GET",
        url: "/APReportSale/PreviousPenjualanHarian",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResult
    });
}

function LoadingStart() {

    $("#dialog-overlay").show();
}

function LoadingStop() {

    $("#dialog-overlay").hide();
}

function CetakLaporan() {
    var print = $("#detail");
    print.jqprint();
}