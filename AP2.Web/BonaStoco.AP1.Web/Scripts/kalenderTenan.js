var currentMonth = 0;
var currentYear = 0;
var datasales;
var curdate;
$(document).ready(function () {
    curdate = new Date();
    currentYear = curdate.getFullYear();
    currentMonth = curdate.getMonth();
    LoadDataUsingAjax(curdate);
})
function LoadDataUsingAjax(tanggal) {
    var YearMounth = GetFormatMonth(tanggal);
    $.ajax({
        type: "POST",
        url: "kalenderViewByTenant/GetSalesByDate/",
        data: { date: YearMounth },
        dataType: "json",
        success: showStates
    });
}
function showStates(data, status) {
    datasales = data;
    UpdateKalenderUntukBulan(curdate);
}
function GetFormatMonth(tanggal) {
    var bln = tanggal.getMonth();

    if (bln < 10)
        return tanggal.getFullYear().toString() + "0" + (tanggal.getMonth() + 1).toString();
    else
        return tanggal.getFullYear().toString() + (tanggal.getMonth() + 1).toString();
}
function GetSalesAmountOnDate(tanggal) {
    for (var idx in datasales) {
        if (datasales[idx].Tanggal == tanggal) {
            var inmillion = datasales[idx].Sales / 1000000;
            return addCommas(inmillion);
        }
    }
    return 0;
}
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}
function UpdateKalenderUntukBulan(sekarang) {

    var currentDate = sekarang;

    document.getElementById("headerTgl").innerHTML = getMonthName(currentDate.getMonth()) + ' ' + currentDate.getFullYear() + "(PENJUALAN SEMUA BANDARA AP1)";
    var date = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    var day = date.getDay();
    var totalDays = getTotalDaysInMonth(currentDate.getMonth(), currentDate.getFullYear());
    for (var i = 0; i < 42; i++) {
        var transDate = new Date();
        transDate.setDate(i);
        transDate.setMonth(currentMonth);
        transDate.setFullYear(currentYear);
        if (i >= day && i < totalDays + day)
            document.getElementById(i.toString()).innerHTML = (i - day + 1) + '<br /><br /> <div align="center" style="height:40px"><span style="color:Black; font-weight:bold; font-size:0.6cm">' + GetSalesAmountOnDate(i - day + 1) + '</span></div>';
        else
            document.getElementById(i.toString()).innerHTML = '<br /><br /> <div align="center" style="height:40px"></div>';
    }

}
function getTotalDaysInMonth(bulan, tahun) {
    days_in_month = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    if (tahun % 4 == 0 && tahun != 1900) {
        days_in_month[1] = 29;
    }
    return days_in_month[bulan];
}

function getMonthName(bulan) {
    monthName = new Array("JANUARY", "FEBRUARY", "MARET", "APRIL", "MEI", "JUNI", "JULI", "AGUSTUS", "SEPTEMBER", "OKTOBER", "NOVEMBER", "DESEMBER");
    return monthName[bulan];
}
function UpdateKalenderBulanBerikutnya() {
    if (currentMonth == 11) {
        currentMonth = 0;
        currentYear++;
    }
    else
        currentMonth++;

    curdate = new Date(currentYear, currentMonth, 1);
    LoadDataUsingAjax(curdate);
}
function UpdateKalenderBulanSebelumnya() {
    if (currentMonth == 0) {
        currentMonth = 11;
        currentYear--;
    }
    else
        currentMonth--;

    curdate = new Date(currentYear, currentMonth, 1);
    LoadDataUsingAjax(curdate);
}