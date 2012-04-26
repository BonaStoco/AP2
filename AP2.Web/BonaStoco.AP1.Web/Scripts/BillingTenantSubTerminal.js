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
    LoadDataByYear(year)
    LoadDataByNextYear(nextyear);
})

$("#dialog-overlay").ajaxStart(function () {
    $(this).show();
}).ajaxStop(function () {
    $(this).hide();
});

function LoadDataByYear(year) { 
    $.ajax({
        type: "GET",
        url: "/DaftarPembayaranTenanAP/FindDataBillingTenantByYearAndBandaraAndTerminalAndSubTerminal",
        dataType: "json",
        data : {'year' : year},
        success: SetDataListToLocal
    });
}

function LoadDataByNextYear(nextyear) {
    $.ajax({
        type: "GET",
        url: "/DaftarPembayaranTenanAP/FindDataBillingTenantByYearAndBandaraAndTerminalAndSubTerminal",
        dataType: "json",
        data: { 'year': year},
        success: SetDataListNextYearToLocal
    });
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
        $("#billingTenant tbody").append("<tr id=tr"+no+" bgcolor="+warna+">" +
        "<td>" + no + "</td>" +
        "<td>" + listBilling[item].TenanId + "</td>" +
        "<td>" + listBilling[item].TenanName + "</td>" +
        "<td>" + listBilling[item].BulanBergabung + "</td>" +
        "<td>" + listBilling[item].BulanKeluar + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Januari) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Februari) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Maret) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].April) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Mei) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Juni) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Juli) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Agustus) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].September) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Oktober) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].November) + "</td>" +
        "<td align=center>" + isMonthChecked(listBilling[item].Desember) + "</td>" +
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
    SetLabelPrevious();  
}

function SetLabelPrevious() {
    $('#lbl-previous-year').text('Balik ke tahun ');
    $('#previous-year').text(previousYear);
}

function SetLabelNext() {   
        $('#lbl-next-year').text('Menuju tahun ');
        $('#next-year').text(nextYear);
}

function isMonthChecked(value) {

    if (value == true) {
        return "<img src='../Content/images/checked.png'/>";
    }
        
    return"";
}

function ClearLabel() {
  $('#lbl-next-year').text('');
  $('#next-year').text('');
  $('#lbl-previous-year').text('');
  $('#previous-year').text('');
  currentYear=0;
  previousYear=0;
  nextYear=0;
  year=0;
  listNextYear = null;
  listBilling=null;
  billingTenant = null;
}
