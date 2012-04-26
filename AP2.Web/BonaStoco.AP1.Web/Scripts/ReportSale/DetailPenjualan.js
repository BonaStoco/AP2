
function CreateDivDetailPenjualan() {
    $("#DivDetailPenjualan").remove();
    $(".tabscontent").append("<div id='DivDetailPenjualan'>" +
                            "<label class='headerContent'>Laporan Detail Penjualan Per Tenan</label><br /><br />" +
                            "<table id='PencarianDetailPenjualan'>" +
                            "<tr><td>Dari</td>" +
                            "<td><input type='text' name='dari' id='dari' readonly='readonly'/></td></tr>" +
                            "<tr><td>Sampai</td>" +
                            "<td><input type='text' name='sampai' id='sampai' readonly='readonly' /></td></tr>" +
                            "<tr><td>Tenant</td>" +
                            "<td><input type='text' name='Tenant' id='tenant' onchange='FindTenantNameByTenanId(this.value)'/></td>" +
                            "<td><label id='AdvSearch' class='positive button'>" +
                            "<img src='../Content/images/button/search.png' alt='Loader'/> Pencarian Tenan</label></td>" +
                            "<td><label id='tenantName'></label></td></tr>" +
                            "<tr><td colspan='3'>" +
                            "<button id='TampilLaporan' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Laporan</button>&nbsp;" +
                            "<img id='progress' src='../Content/images/loader.gif' alt='Loader'/></td></tr></table></div>");

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
        }
    });
    SetDefaultDateDetailPenjualan();
    $("#TampilLaporan").click(FindDetailPenjualan);
    $("#AdvSearch").click(OpenTenanSearchDialog);
}

function FindDetailPenjualan() {
    var tenant = $("#tenant").val();
    var dari = $("#dari").val();
    var sampai = $("#sampai").val();
    if (tenant == "" || dari == "" || sampai == "")
        return alert("Semua Field Harus Di Isi");
    $("#tabContainer").hide();
    CreateTableDetailPenjualan();
   

    $.ajax({
        type: "POST",
        url: "/ReportSale/ListDetailPenjualanPerTenan",
        data: { "tenant": tenant, "dari": dari, "sampai": sampai },
        dataType: "json",
        beforeSend: AjaxStart,
        complete: AjaxEnd,
        success: InsertDetailPenjualanToTable
    });
}

function CreateTableDetailPenjualan() {
    $("div#DivTableDetailPenjualan").remove();
    $("section#main").append("<div id='DivTableDetailPenjualan'>" +
                                     "<label id='Back' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<input type='text' id='kode_transaksi_search' placeholder='Search'/>" +
                                     "<img class='loadingSearchTransaction' src='../Content/images/loader.gif'/>" +
                                     "<label class='TenantName'>Tenan : " + TenanName + "</label>" +
                                     "<div class='DialogOverlay'></div>" +
                                     "<table width='100%' id='TableDetailPenjualan' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                                     "<th class='tanggal'>Tanggal</th>" +
                                     "<th class='noTransaksi'>No Transaksi</th>" +
                                     "<th class='IDR' >Jumlah</th></tr>" +
                                     "</thead><tbody></tbody><tfoot></tfoot></table></div>");
    $("#Back").click(DestroyTableDetailPenjualan);
}

function InsertDetailPenjualanToTable(data) {
    if (data == null || data.length == 0) {
        DestroyTableDetailPenjualan();
        return alert("Data Penjualan Tidak Di Temukan");
    }
    $("#TableDetailPenjualan tbody").empty();
    var color;
    var ccy;
    $.each(data, function (item) {
        color = (item % 2 == 0) ? "#e3ecff" : "#fff";
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        $("#TableDetailPenjualan tbody").append("<tr onclick='ShowDetailProduct(\"" + data[item].TransactionNo + "\")'>" +
                                                "<td class='tanggal'>" + data[item].TransactionDate + "</td>" +
                                                "<td class='noTransaksi'>" + data[item].TransactionNo + "</td>" +
                                                "<td class='Right IDR'><span class='CCY'>" + ccy + "</span> " +
                                                String.format("{0:c}", data[item].SellingPerTransaction) + "</td></tr>");
    });
    $("#TableDetailPenjualan tfoot").append("<tr>" +
                                            "<td id='tdFootNameTotal'> Total </td>" +
                                            "<td id='tdFootValueTotal'>" + data[0].Total + "</td>" +
                                            "</tr>");
    $('#kode_transaksi_search').quicksearch('table#TableDetailPenjualan tbody tr',
            {
                stripeRows: ['odd', 'even'],
                loader: 'img.loadingSearchTransaction'
            });
            $("#TableDetailPenjualan").tablesorter({ widgets: ['zebra'] });
        }
        

function ShowDetailProduct(transactionNo) {
    var _tenanid = $("input#tenant").val();
    $.ajax({
        type: "POST",
        url: "/ReportSale/FindSalesProductDetailByTransactionNumber",
        data: { 'transactionNo': transactionNo, 'tenanId': _tenanid },
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        success: ShowDetailProductToTable
    });
}
function ShowDetailProductToTable(data) {
    if (data == null || data.length == 0) {
        return;
    }
    $("div#DivTableDetailPenjualan").hide();
    CreateDivDetailTransaction("Product Detail No Transaction " + data[0].TransactionNo);
    RenderDataToTable(data)
}
function CreateDivDetailTransaction(title) {
    $("section#main").append("<div id='DivDetailProduct'>" +
                             "<label class='titleDetailTrans'>" + title + "</label>" +
                             "<label id='BackToListPenjualan' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke List Detail Penjualan</label>" +
                             "<table id='TableDetailTransaction' width='100%' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                             "<th class='No Center'>No</th>"+
                             "<th class='KodeProduk Left'>Kode Barang</th>" +
                             "<th class='NamaProduk Left'>Nama Barang</th>" +
                             "<th class='Qty Right'>Qty</th>" +
                             "<th class='HargaJual Center'>Harga Jual</th>" +
                             "<th class='Diskon Center'>Diskon</th>" +
                             "<th class='Jumlah Right'>Jumlah</th>" +
                             "</thead><tbody></tbody><tfoot></tfoot></table>" +
                             "</div>");


    $("label#BackToListPenjualan").click(function () {
        $("div#DivDetailProduct").remove();
        $("div#DivTableDetailPenjualan").show();
    });
}
function RenderDataToTable(data) {
    var total;
    var totalPenjualan = 0;
    var ccy;
    var no = 0;
    $.each(data, function (item) {
        no++;
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        $("table#TableDetailTransaction tbody").append("<tr>" +
                            "<td class='Center'>" + no + "</td>" +
                            "<td class='Left'>" + data[item].KodeProduk + "</td>" +
                            "<td class='Left'>" + data[item].NamaProduk + "</td>" +
                            "<td class='Right Qty'>" + data[item].Qty + " PCS</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[item].HargaJual) + "</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[item].DiscountItemAmount) + "</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[item].NetAmount) + "</td></tr>");
    });
    $("table#TableDetailTransaction tfoot").append("<tr class='Bold'><td colspan='5'></td>" +
                                                   "<td class='Bold Left'>Total Diskon </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[0].DiscountTotalAmount) + "</td></tr>" +
                                                   "<tr class='Bold'><td class='Italic Center' colspan='5'>Total tidak termasuk pajak</td>" +
                                                   "<td class='Bold Left'>Tax </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[0].TaxAmount) + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelChargeAmount Right'></td>" +
                                                   "<td class='Bold Left'>Charge </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[0].ChargeAmount) + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelChargeAmount Right'></td>" +
                                                   "<td class='Bold Left'>Service Charge </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[0].ServiceCharge) + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelTotalPenjualan Right'></td>"+
                                                   "<td class='Bold Left'>Total </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", data[0].TotalAmount) + "</td></tr>");

    $("#TableDetailTransaction").tablesorter({ widgets: ['zebra'] });
}
function DestroyTableDetailPenjualan() {
    ClearDataPencarian();
    SetDefaultDateDetailPenjualan();
    $("div#DivTableDetailPenjualan").remove();
    $("#tabContainer").show();
}
function AjaxStart() {
    $("div.DialogOverlay").show();
}
function AjaxEnd() {
    $("div.DialogOverlay").hide();
}
function ClearDataPencarian() {
    $("#dari").val('');
    $("#sampai").val('');
    $("#tenant").val('');
    $("#tenantName").text('');
}
function SetDefaultDateDetailPenjualan() {
    var now = new Date();
    $("#dari").val(now.format("yyyy-MM-dd"));
    $("#sampai").val(now.format("yyyy-MM-dd"));
}