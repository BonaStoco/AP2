var transactionDateTemp = "";
var subTotalIDR = 0;
var subTotalUSD = 0;
var grandTotalIDR = 0;
var grandTotalUSD = 0;
var tenanId;

function CreateDivDetail(tenanId) {
    tenanId = tenanId;
    $("#DivDetail").remove();
    $(".tabscontent").append("<div id='DivDetail'>" +
                            "<label class='headerContent'>Laporan Void Detail</label><br /><br />" +
                            "<table id='PencarianDetail'>" +
                            "<tr><td>Dari</td>" +
                            "<td><input type='text' name='dari' id='dari' readonly='readonly'/></td></tr>" +
                            "<tr><td>Sampai</td>" +
                            "<td><input type='text' name='sampai' id='sampai' readonly='readonly' /></td></tr>" +
                             "<tr><td>Kasir</td>" +
                             "<td><select id='SessionDetailPerkasir'></select></td></tr>" +
                             "<tr><td colspan='3'>" +
                             "<button id='TampilLaporan' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Laporan</button>&nbsp;" +
                            "<img id='progress' src='../../Content/images/loader.gif' alt='Loader'/></td></tr></table></div>");

    var dates = $("#dari, #sampai").datepicker({ dateFormat: 'yy-mm-dd',
        defaultDate: "+1w",
        gotoCurrent: true,
        changeMonth: true,
        numberOfMonths: 1,
        onSelect: function (selectedDate) {
            var option = this.id == "dari" ? "minDate" : "maxDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
            var dari = $("#dari").val();
            var sampai = $("#sampai").val();
            SearchSessionDetail(tenanId,dari, sampai);
        }
    });
    SetDefaultDateDetail();
    $("#TampilLaporan").click(FindDetail);
}

function FindDetail() {
    var sessionId = $("select#SessionDetailPerkasir").val();
    var dari = $("#dari").val();
    var sampai = $("#sampai").val();
    if (sessionId == "" || dari == "" || sampai == "")
        return alert("Semua Field Harus Di Isi");
    $("#tabContainer").hide();
    CreateTableDetail();
    $.ajax({
        type: "GET",
        url: "/LaporanVoidAP/FindDetailVoidPerKasirByDate",
        data: { "tenanId":tenanId, "dari": dari, "sampai": sampai, "sessionId": sessionId },
        dataType: "json",
        beforeSend: AjaxStart,
        complete: AjaxEnd,
        success: InsertDetailToTable
    });
}

function CreateTableDetail() {
    $("div#DivTableDetail").remove();
    $("section#main").append("<div id='DivTableDetail'>" +
                                     "<label id='Back' class='positive button'><img src='../../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<table width='100%' id='TableDetail'><thead><tr>" +
                                     "<th class='tanggal'>Tanggal</th>" +
                                     "<th class='noTransaksi'>No Transaksi</th>" +
                                     "<th class='part'>Part</th>" +
                                     "<th class='qty Right'>Qty</th>" +
                                     "<th class='harga Right'>Harga IDR</th>" +
                                     "<th class='harga Right'>Harga USD</th>" +
                                     "<th class='total Right'>Total IDR</th>" +
                                     "<th class='total Right'>Total USD</th></tr>" +
                                     "</thead><tbody></tbody></table></div>");
    $("#Back").click(DestroyTableDetail);
}

function InsertDetailToTable(data) {
    if (data == null || data.length == 0) {
        DestroyTableDetail();
        return alert("Data Tidak Di Temukan");
    }
    $("#TableDetail tbody").empty();
    var ccy;
    index = 0;
    $.each(data, function (item) {
        try {
            var idxnext = "undefined";
            idxnext = index + 1;
            var count = data.length;

            if (idxnext < count) {
                datanext = data[idxnext];
            }
            else {
                datanext = data[0];
            }

            if (index % 2 == 0) {
                color = "#F0F0F6";
            }
            else {
                color = "#FFF";
            }

            $("table#TableDetail tbody").append("<tr style='background-color:" + color + "'>" +
                                                "<td class='tanggal'>" + ConvertDate(data[index].TransactionDate) + "</td>" +
                                                "<td class='noTransaksi'>" + data[index].TransactionNo + "</td>" +
                                                "<td class='part'>" + data[index].NamaProduk + "</td>" +
                                                "<td class='qty Right'>" + Math.abs(data[index].Qty) + "</td>" +
                                                "<td class='harga Right'><span class='CCY'>Rp.</span> " +
                                                FormatIDR(data[index].HargaJualIDR) + "</td>" +
                                                "<td class='harga Right'><span class='CCY'>$</span> " +
                                                String.format("{0:c}", data[index].HargaJualUSD) + "</td>" +
                                                "<td class='total Right'><span class='CCY'>Rp.</span> " +
                                                FormatIDR(Math.abs(data[index].NetAmountIDR)) + "</td>" +
                                                "<td class='total Right'><span class='CCY'>$</span> " +
                                                String.format("{0:c}", Math.abs(data[index].NetAmountUSD)) + "</td>" +
                                                "</tr>");




            transactionDateTemp = ConvertDate(data[index].TransactionDate);
            subTotalIDR += Math.abs(data[index].HargaJualIDR);
            subTotalUSD += Math.abs(data[index].HargaJualUSD);
            grandTotalIDR += Math.abs(data[index].NetAmountIDR);
            grandTotalUSD += Math.abs(data[index].NetAmountUSD);
            
            if ((datanext != "undefined") && (transactionDateTemp != ConvertDate(datanext.TransactionDate)) && (ConvertDate(datanext.TransactionDate) != "undefined")) {

                $("table#TableDetail tbody").append("<tr class='SubTotal'>" +
                                         "<td class='total Right Bold'colspan='6'>SUB TOTAL</td>" +
                                         "<td class='total Right Bold'><span class='CCY'>Rp.</span>" + FormatIDR(subTotalIDR) + "</td>" +
                                         "<td class='total Right Bold'><span class='CCY'>$</span>" + String.format("{0:c}", subTotalUSD) + "</td>" +
                                         "</tr>");
                subTotalIDR = 0;
                subTotalUSD = 0;

            }


            index++;
        } catch (e) {
            alert(e);
        }
    });

    if (grandTotalIDR != 0 || grandTotalUSD != 0) {
        $("table#TableDetail tbody").append("<tr>" +
                                         "<td class='total Right Bold'colspan='6'>GRAND TOTAL</td>" +
                                         "<td class='total Right Bold'><span class='CCY'>Rp.</span>" + FormatIDR(grandTotalIDR) + "</td>" +
                                         "<td class='total Right Bold'><span class='CCY'>$</span>" + String.format("{0:c}", grandTotalUSD) + "</td>" +
                                         "</tr>");
        grandTotalIDR = 0;
        grandTotalUSD = 0;
    }
}

function DestroyTableDetail() {
    ClearDataPencarian();
    SetDefaultDateDetail();
    $("div#DivTableDetail").remove();
    $("#tabContainer").show();
}

function SearchSessionDetail(tenantId, dari, sampai) {
    if (dari == "" || sampai == "") {
        ClearSessionDetailField();
        return alert("Field Tanggal Harus Diisi");
    }
    $.ajax({
        type: "GET",
        url: "/LaporanVoidAP/FindSessionByTenantAndDate",
        data: { "tenantId":tenantId, "dari": dari, "sampai": sampai },
        dataType: "json",
        beforeSend: LoadingDetailPerkasirStart,
        complete: LoadingDetailPerkasirEnd,
        success: InsertSessionDetailToList
    });
}

function InsertSessionDetailToList(data) {
    if (data == null || data.length == 0) {
        ClearSessionDetailField();
        return alert("Tidak Ada Penjualan");
    }
    $("select#SessionDetailPerkasir").empty();
    $.each(data, function (item) {
        $("select#SessionDetailPerkasir").append("<option value='" + data[item].SessionId + "'>" + data[item].Kasir + "</option>");
    });
}

function ClearSessionDetailField() {
    $("select#SessionDetailPerkasir").empty();
}
function LoadingDetailPerkasirStart() {
    $("select#SessionDetailPerkasir").attr("disabled", true);
}
function LoadingDetailPerkasirEnd() {
    $("select#SessionDetailPerkasir").attr("disabled", false);
}

function ClearDataPencarian() {
    $("#dari").val('');
    $("#sampai").val('');
}

function SetDefaultDateDetail() {
    var now = new Date();
    $("#dari").val(now.format("yyyy-MM-dd"));
    $("#sampai").val(now.format("yyyy-MM-dd"));
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