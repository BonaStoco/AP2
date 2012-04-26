var grandTotalIDR = 0;
var grandTotalUSD = 0;
var tenanId;
function CreateDivSummary(tenanId) {
    tenanId = tenanId;
    $("#DivSummary").remove();
    $(".tabscontent").append("<div id='DivSummary'>" +
                             "<label class='headerContent'>Laporan Void Summary</label><br /><br />" +
                             "<table id='PencarianSummary'>" +
                             "<tr><td>Dari</td>" +
                            "<td><input type='text' name='from' id='from' readonly='readonly'/></td></tr>" +
                            "<tr><td>Sampai</td>" +
                            "<td><input type='text' name='until' id='until' readonly='readonly' /></td></tr>" +
                             "<tr><td>Kasir</td>" +
                             "<td><select id='SessionPerkasir'></select></td></tr>" +
                             "<tr><td colspan='3'>" +
                             "<button id='TampilSummary' class='positive button'><img src='../../Content/images/button/search.png'/> Tampilkan Laporan</button>&nbsp;" +
                             "</td></tr></table>" +
                             "</div>");
    var dates = $("#from, #until").datepicker({ dateFormat: 'yy-mm-dd',
        defaultDate: "+1w",
        gotoCurrent: true,
        changeMonth: true,
        numberOfMonths: 1,
        onSelect: function (selectedDate) {
            var option = this.id == "from" ? "minDate" : "maxDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
            var dari = $("#from").val();
            var sampai = $("#until").val();
            SearchSession( dari, sampai);
        }
    });
    SetDefaultDateSummary();
    $("#TampilSummary").click(FindDetailSummary);
}

function FindDetailSummary() {
    var sessionId = $("select#SessionPerkasir").val();
    var dari = $("#from").val();
    var sampai = $("#until").val();
    if (sessionId == "" || dari == "" || sampai == "")
        return alert("Semua Field Harus Di Isi");
    $("#tabContainer").hide();
    CreateDivListSummary();
    $.ajax({
        type: "GET",
        url: "/LaporanVoidAP/FindSummaryVoidPerKasirByDate",
        data: { "tenanId":tenanId, "dari": dari, "sampai": sampai, "sessionId": sessionId },
        dataType: "json",
        beforeSend: AjaxStart,
        complete: AjaxEnd,
        success: InsertSummaryToTable
    });
}

function CreateDivListSummary() {
    $("div#DivListSummary").remove();
    $("section#main").append("<div id='DivListSummary'>" +
                                     "<label id='BackSummary' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<table width='100%' id='TableSummary'><thead><tr>" +
                                     "<th class='Kasir Left'>Kasir</th>" +
                                     "<th class='Tanggal Left'>Tanggal</th>" +
                                     "<th class='No left'>No Transaksi</th>" +
                                     "<th class='Jumlah Right'>Total IDR</th>" +
                                     "<th class='Jumlah Right'>Total USD</th>" +
                                     "</thead><tbody></tbody></table></div>");
    $("#BackSummary").click(DestroyTableSummaryPenjualan);
}

function InsertSummaryToTable(data) {
    if (data == null || data.length == 0) {
        DestroyTableSummaryPenjualan();
        return alert("Data Tidak Di Temukan");
    }
    $("#TableSummary tbody").empty();
    var ccy;
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#F0F0F6";
        }
        else {
            color = "#FFF";
        }

        $("table#TableSummary tbody").append("<tr style='background-color:" + color + "'>" +
                                     "<td class='Kasir Left'>" + data[item].Kasir + "</td>" +
                                     "<td class='Tanggal Left'>" + ConvertDate(data[item].TransactionDate) + "</td>" +
                                     "<td class='No Left'>" + data[item].TransactionNo + "</td>" +
                                     "<td class='Jumlah Right'><span class='CCY'>Rp.</span>" + FormatIDR(Math.abs(data[item].NetAmountIDR))+ "</td>" +
                                     "<td class='Jumlah Right'><span class='CCY'>$</span>" + String.format("{0:c}", Math.abs(data[item].NetAmountUSD))+ "</td></tr>");
        
        grandTotalIDR += Math.abs(data[item].NetAmountIDR);
        grandTotalUSD += Math.abs(data[item].NetAmountUSD);
    
    });
   
    $("table#TableSummary tbody").append("<tr>" +
                                         "<td class='Jumlah Right Bold'colspan='3'>GRAND TOTAL</td>" +
                                         "<td class='Jumlah Right Bold'><span class='CCY'>RP.</span>" + FormatIDR(grandTotalIDR) + "</td>" +
                                         "<td class='Jumlah Right Bold'><span class='CCY'>$</span>" + String.format("{0:c}", grandTotalUSD) + "</td>" +
                                         "</tr>");
    grandTotalIDR = 0;
    grandTotalUSD = 0;
}

function DestroyTableSummaryPenjualan() {
    ClearTanggalPencarian();
    SetDefaultDateSummary();
    $("#DivListSummary").remove();
    $("#tabContainer").show();
}

function SetDefaultDateSummary() {
    var now = new Date();
    $("#from").val(now.format("yyyy-MM-dd"));
    $("#until").val(now.format("yyyy-MM-dd"));
}

function SearchSession(dari, sampai) {
    if (dari == "" || sampai == "") {
        ClearSessionSummaryField();
        return alert("Field Tanggal Harus Diisi");
    }
    $.ajax({
        type: "GET",
        url: "/LaporanVoidAP/FindSessionByTenantAndDate",
        data: { "tenantId": tenanId, "dari": dari, "sampai": sampai },
        dataType: "json",
        beforeSend: LoadingPerkasirStart,
        complete: LoadingPerkasirEnd,
        success: InsertSessionToList
    });
}
function InsertSessionToList(data) {
    if (data == null || data.length == 0) {
        ClearSessionSummaryField();
        return alert("Tidak Ada Penjualan");
    }
    $("select#SessionPerkasir").empty();
    $.each(data, function (item) {
        $("select#SessionPerkasir").append("<option value='" + data[item].SessionId + "'>" + data[item].Kasir + "</option>");
    });
}

function ClearTanggalPencarian() {
    $("#from").val('');
    $("#until").val('');
}

function ClearSessionSummaryField() {
    $("select#SessionPerkasir").empty();
}
function LoadingPerkasirStart() {
    $("select#SessionPerkasir").attr("disabled", true);
}
function LoadingPerkasirEnd() {
    $("select#SessionPerkasir").attr("disabled", false);
}

function AjaxStart() {
    $("div.DialogOverlay").show();
}
function AjaxEnd() {
    $("div.DialogOverlay").hide();
}

function ConvertDate(JsonDate) {
    var tempDate = parseInt(JsonDate.replace(/\/Date\((.*?)\)\//gi, "$1"), 10);
    var LastTransactionDate = new Date(tempDate);
    date = new Date(LastTransactionDate.getFullYear(), LastTransactionDate.getMonth(), LastTransactionDate.getDate());
    var FullDate = String.format("{0:dd MMMM yyyy}", date);
    return FullDate;
}

function FormatIDR(value) {
    value += '';
    x = value.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}