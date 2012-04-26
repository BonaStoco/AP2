function CreateDivSummary() {
    $("#DivSummary").remove();
    $(".tabscontent").append("<div id='DivSummary'>" +
                             "<label class='headerContent'>Ringkasan Penjualan Per Barang</label><br /><br />" +
                             "<table id='PencarianSummaryPenjualan'><tr><td>Tanggal</td>" +
                             "<td><input type='text' id='tanggal' readonly='readonly'/></td>" +
                             "</tr><tr><td>Tenant</td>" +
                             "<td><input type='text' id='tenantSummary' onchange='FindTenantNameByTenanId(this.value)'/></td>" +
                             "<td><label id='AdvSearchSummary' class='positive button'><img src='../Content/images/button/search.png'/> Pencarian Tenan</label></td>" +
                             "<td><label id='tenantSummaryName'></label></td></tr>" +
                             "<tr><td colspan='3'>" +
                             "<button id='TampilSummary' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Ringkasan Per Barang</button>&nbsp;" +
                             "</td></tr></table>" +
                             "</div>");
    var dates = $("#tanggal").datepicker({ dateFormat: 'yy-mm-dd',
        defaultDate: "+1w",
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
        }
    });
    $("#AdvSearchSummary").click(OpenTenanSearchDialog);
    $("#TampilSummary").click(FindDetailSummary);
    $("input#tenantSummary").blur(FindTenantNameByTenanId);
    SetDefaultDateSummaryPenjualan();
}

function FindDetailSummary() {
    var tenant = $("#tenantSummary").val();
    var tanggal = $("#tanggal").val();
    if (tenant == "" || tanggal == "")
        return alert("Semua Field Harus Di Isi");
    $("#tabContainer").hide();
    CreateDivListSummary();
    $.ajax({
        type: "POST",
        url: "/ReportSale/FindSummaryPenjualanPerTenan",
        data: { "tenanId": tenant, "tanggal": tanggal },
        dataType: "json",
        beforeSend: AjaxSummaryStart,
        complete: AjaxSummaryEnd,
        success: InsertSummaryPenjualanToTable
    });
}
function CreateDivListSummary() {
    $("div#DivListSummaryPenjualan").remove();
    $("section#main").append("<div id='DivListSummaryPenjualan'>" +
                                     "<label id='BackSummary' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke Pencarian Summary</label>" +
                                     "<label id='PrintSummary' class='positive button'><img src='../Content/images/button/search.png'/>Print</label>" +
                                     "<label class='TenantName'>Tenan : " + TenanName + "</label>" +
                                     "<label class='Tanggal'>Tanggal : " + $("#tanggal").val() + "</label>" +
                                     "<div class='DialogOverlay'></div>" +
                                     "<table width='100%' id='TableSummaryPenjualan' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                                     "<th class='No Center'>No</th>" +
                                     "<th class='Kode Left'>Kode Produk</th>" +
                                     "<th class='left'>Nama Produk</th>" +
                                     "<th class='Qty Right'>Qty</th>" +
                                     "<th class='Jumlah Right'>Jumlah</th>" +
                                     "</thead><tbody></tbody></table></div>");
    $("#BackSummary").click(DestroyTableSummaryPenjualan);
    $("#PrintSummary").click(PrintSummary);
}
function InsertSummaryPenjualanToTable(data) {
    if (data == null || data.length == 0) {
        DestroyTableSummaryPenjualan();
        return alert("Data Tidak Di Temukan");
    }
    var ccy;
    var total;
    var No = 0;
    $.each(data, function (item) {
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        total = data[item].HargaJual * data[item].Qty + data[item].ServiceCharge;
        No++;
        $("#TableSummaryPenjualan tbody").append("<tr>" +
                                     "<td class='No Center'>" + No + "</td>" +
                                     "<td class='Kode Left'>" + data[item].KodeProduk + "</td>" +
                                     "<td class='Nama Left'>" + data[item].NamaProduk + "</td>" +
                                     "<td class='Qty Right'>" + data[item].Qty + " PCS</td>" +
                                     "<td class='Jumlah Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", total) + "</td></tr>");
    });
    $("#TableSummaryPenjualan").tablesorter({ widgets: ['zebra'] });
}

function DestroyTableSummaryPenjualan() {
    $("#tenantSummary").val('');
    $("#tanggal").val('');
    $("#tenantSummaryName").text('');
    SetDefaultDateSummaryPenjualan();
    $("#DivListSummaryPenjualan").remove();
    $("#tabContainer").show();
}
function AjaxSummaryStart() {
}
function AjaxSummaryEnd() {
}
function PrintSummary() {
    var print = $("#DivListSummaryPenjualan");
    print.jqprint();
}
function SetDefaultDateSummaryPenjualan() {
    var now = new Date();
    $("#tanggal").val(now.format("yyyy-MM-dd"));
}