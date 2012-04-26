var currentYear;
var previousYear;
var nextYear;
var year;
var listNextYear;
var listBilling;

$(document).ready(function () {
    ClearLabel();
    curdate = new Date();
    listNextYear = new Array();
    currentYear = curdate.getFullYear();
    year = currentYear;
    previousYear = year - 1;
    nextYear = year + 1;
    LoadDataByYear(year);
    LoadDataByNextYear(nextYear);

})

function LoadDataByYear(year) {
    $.ajax({
        type: "GET",
        url: "/DaftarPembayaranTenan/FindDataBillingByYear",
        dataType: "json",
        data: { "year": year },
        success: SetDataListToLocal,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
}

function LoadDataByNextYear(nextyear) {
    $.ajax({
        type: "GET",
        url: "/DaftarPembayaranTenan/FindDataBillingByYear",
        dataType: "json",
        data: { "year": nextyear },
        success: SetDataListNextYearToLocal,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
}

function CreateDataBillingTenant(nextyear) {
    $.ajax({
        type: "POST",
        url: "/DaftarPembayaranTenan/AddAllActiveTenanForNextYear",
        dataType: "json",
        data: { "year": nextyear },
        success: ViewListDataTenan,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
}

function AddDataUnregisteredTenant() {
    $.ajax({
        type: "POST",
        url: "/DaftarPembayaranTenan/AddUnregisterTenanPayment",
        dataType: "json",
        data: { "year": year },
        success: ViewListDataTenan,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
}


function UpdateTenanPayment(tenanId, tahun, bulan, blnCeklist) {
    $.ajax({
        type: "POST",
        url: "/DaftarPembayaranTenan/UpdateTenanPayment",
        dataType: "json",
        data: { "tenanId": tenanId, "tahun": tahun, "bulan": bulan, "blnCeklist": blnCeklist }
    });
}

function UpdateChecklist(checkId, tenanId, tdId) {
    var blnCeklist = $("#" + checkId).val();
    var tenanid = $("#" + tenanId).text();
    var bulan = checkId.substring(0, 3);
    var no = checkId.substring(3);
    var tahun = year;
    if (blnCeklist == "true") {
        blnCeklist = "false";
    }
    else if (blnCeklist == "false") {
        blnCeklist = "true";
    }
//    $("#tr" + no + " #" + tdId).css("display", "table-cell");
//    $("#tr" + no + " #" + tdId).html("<img src='./Content/images/loader.gif'/>");
    UpdateTenanPayment(tenanid, tahun, bulan, blnCeklist);
}

function ViewListDataTenan(data, status) {
    LoadDataByYear(year);
}

function SetDataListToLocal(data, status) {
    listBilling = null;
    listBilling = data;
    SetLabel();
    SetLabelNext();
    ViewListBilling();
}

function SetDataListNextYearToLocal(data, status) {
    listNextYear = null;
    listNextYear = data;
    isDataExist();
    SetLabel();
    SetLabelNext();
}

function ViewListBilling() {
    $("#billingTenant tbody").empty();
    var no = 1;
    var warna;
    $.each(listBilling, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }
        $("#billingTenant tbody").append("<tr id=tr" + no + " bgcolor=" + warna + ">" +
        "<td>" + no + "</td>" +
        "<td id=tenanid" + no + ">" + listBilling[item].TenanId + "</td>" +
        "<td>" + listBilling[item].TenanName + "</td>" +
        "<td>" + listBilling[item].BulanBergabung + "</td>" +
        "<td>" + listBilling[item].BulanKeluar + "</td>" +
        "<td align=center id='loader1'><input type='checkbox' value=" + listBilling[item].Januari + "  onclick='UpdateChecklist(\"jan" + no + "\" ,\"tenanid" + no + "\",\"loader1\")' name='jan'  id=jan" + no + " " + isMonthChecked(listBilling[item].Januari) + "/></td>" +
        "<td align=center id='loader2'><input type='checkbox' value=" + listBilling[item].Februari + " onclick='UpdateChecklist(\"feb" + no + "\" ,\"tenanid" + no + "\",\"loader2\")' name='feb' id=feb" + no + " " + isMonthChecked(listBilling[item].Februari) + "/></td>" +
        "<td align=center id='loader3'><input type='checkbox' value=" + listBilling[item].Maret + " onclick='UpdateChecklist(\"mar" + no + "\" ,\"tenanid" + no + "\",\"loader3\")' name='mar' id=mar" + no + " " + isMonthChecked(listBilling[item].Maret) + "/></td>" +
        "<td align=center id='loader4'><input type='checkbox' value=" + listBilling[item].April + " onclick='UpdateChecklist(\"apr" + no + "\" ,\"tenanid" + no + "\",\"loader4\")' name='apr' id=apr" + no + " " + isMonthChecked(listBilling[item].April) + "/></td>" +
        "<td align=center id='loader5'><input type='checkbox' value=" + listBilling[item].Mei + " onclick='UpdateChecklist(\"mei" + no + "\" ,\"tenanid" + no + "\",\"loader5\")' name='mei' id=mei" + no + " " + isMonthChecked(listBilling[item].Mei) + "/></td>" +
        "<td align=center id='loader6'><input type='checkbox' value=" + listBilling[item].Juni + " onclick='UpdateChecklist(\"jun" + no + "\" ,\"tenanid" + no + "\",\"loader6\")' name='jun' id=jun" + no + " " + isMonthChecked(listBilling[item].Juni) + "/></td>" +
        "<td align=center id='loader7'><input type='checkbox' value=" + listBilling[item].Juli + " onclick='UpdateChecklist(\"jul" + no + "\" ,\"tenanid" + no + "\",\"loader7\")' name='jul' id=jul" + no + " " + isMonthChecked(listBilling[item].Juli) + "/></td>" +
        "<td align=center id='loader8'><input type='checkbox' value=" + listBilling[item].Agustus + " onclick='UpdateChecklist(\"agu" + no + "\" ,\"tenanid" + no + "\",\"loader8\")' name='agu' id=agu" + no + " " + isMonthChecked(listBilling[item].Agustus) + "/></td>" +
        "<td align=center id='loader9'><input type='checkbox' value=" + listBilling[item].September + " onclick='UpdateChecklist(\"sep" + no + "\" ,\"tenanid" + no + "\",\"loader9\")' name='sep' id=sep" + no + " " + isMonthChecked(listBilling[item].September) + "/></td>" +
        "<td align=center id='loader10'><input type='checkbox' value=" + listBilling[item].Oktober + " onclick='UpdateChecklist(\"okt" + no + "\" ,\"tenanid" + no + "\",\"loader10\")' name='okt' id=okt" + no + " " + isMonthChecked(listBilling[item].Oktober) + "/></td>" +
        "<td align=center id='loader11'><input type='checkbox' value=" + listBilling[item].November + " onclick='UpdateChecklist(\"nov" + no + "\" ,\"tenanid" + no + "\",\"loader11\")' name='nov' id=nov" + no + " " + isMonthChecked(listBilling[item].November) + "/></td>" +
        "<td align=center id='loader12'><input type='checkbox' value=" + listBilling[item].Desember + " onclick='UpdateChecklist(\"des" + no + "\" ,\"tenanid" + no + "\",\"loader12\")' name='des' id=des" + no + " " + isMonthChecked(listBilling[item].Desember) + "/></td>" +
    "</tr>");
        no++;
    });
}

function NextYear() {
    year++
    nextYear = year + 1
    previousYear = year - 1;
    LoadDataByYear(year);
    LoadDataByNextYear(nextYear);
    SetLabel();
}

function PreviousYear() {
    year--;
    nextYear = year + 1
    previousYear = year - 1;
    LoadDataByYear(year);
    LoadDataByNextYear(nextYear);
    SetLabel();
}

function SetYearValue() {
    $('#year').text(year);
}

function SetLabel() {
    SetYearValue();
    SetLabelSingkron();
    SetLabelPrevious();
}

function SetLabelPrevious() {
    $('#lbl-previous-year').text('Balik ke tahun ');
    $('#previous-year').text(previousYear);
}

function SetLabelSingkron() {
    if (year == currentYear) {
        $('#lbl-singkron').text('Singkronkan data dengan tenan tahun ');
        $('#singkron').text(currentYear);
    }
    else {
        $('#lbl-singkron').text('');
        $('#singkron').text('');
    }
}

function SetLabelNext() {
    if (isDataYearNotExist()) {
        $('#lbl-next-year').text('Buat daftar untuk tahun ');
        $('#next-year').text(nextYear);
    }
    else {
        $('#lbl-next-year').text('Menuju tahun ');
        $('#next-year').text(nextYear);
    }
}

function isMonthChecked(value) {

    if (value == true) {
        return "Checked ";
    }

    return " ";
}

function isDataYearNotExist() {
    var found = false;
    if (listNextYear.length == 0) {
        found = true;
    }

    return found;
}

function isDataExist() {
    if (isDataYearNotExist()) {
        CreateDataBillingTenant(nextYear);
    }
    else {
        LoadDataByYear(year);
    }
}

function ClearLabel() {
    $('#lbl-singkron').text('');
    $('#singkron').text('');
    $('#lbl-singkron').text('');
    $('#singkron').text('');
    $('#lbl-next-year').text('');
    $('#next-year').text('');
    $('#lbl-previous-year').text('');
    $('#previous-year').text('');
    currentYear = 0;
    previousYear = 0;
    nextYear = 0;
    year = 0;
    listNextYear = null;
    listBilling = null;
    billingTenant = null;
}

function LoadingStart() {

    $("#dialog-overlay").show();
}

function LoadingStop() {

    $("#dialog-overlay").hide();
}
