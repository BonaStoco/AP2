
function CreateDivRingkasanPerHari() {
    $('#DivRingkasanPerHari').remove();
    $(".tabscontent").append("<div id='DivRingkasanPerHari'>" +
                            "<label class='headerContent'>Ringkasan Penjualan Tenan Per Hari</label><br /><br />" +
                            "<table id='PencarianRingkasanPerHari'>" +
                            "<tr><td>Dari</td>" +
                            "<td><input type='text' name='from' id='from' readonly='readonly'/></td></tr>" +
                            "<tr><td>Sampai</td>" +
                            "<td><input type='text' name='to' id='to' readonly='readonly' /></td></tr>" +
                            "<tr><td>Tenant</td>" +
                            "<td><input type='text' name='Tenant' id='tenantIdPerhari' onchange='FindTenantNameByTenanId(this.value)'/></td>" +
                            "<td><label id='AdvSearchPerHari' class='positive button'>" +
                            "<img src='../Content/images/button/search.png' alt='Loader'/> Pencarian Tenan</label></td>" +
                            "<td><label id='tenantNamePerhari'></label></td></tr>" +
                            "<tr><td colspan='3'>" +
                            "<button id='TampilLaporanPenjualanPerHari' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Laporan</button>&nbsp;" +
                            "<img id='progress' src='../Content/images/loader.gif' alt='Loader'/></td></tr></table></div>");

    var dates = $("#from, #to").datepicker({ dateFormat: 'yy-mm-dd',
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
        }
    });
    SetDefaultRingkasanPerHari();
    $("#TampilLaporanPenjualanPerHari").click(FindRingkasanDetailPenjualanPerhari);
    $("#AdvSearchPerHari").click(OpenTenanSearchDialog);
}

function SetDefaultRingkasanPerHari() {
    var now = new Date();
    $("#from").val(now.format("yyyy-MM-dd"));
    $("#to").val(now.format("yyyy-MM-dd"));
}

function ClearDataPencarianRingkasanPerHari() {
    $("#from").val('');
    $("#to").val('');
    $("#tenantIdPerhari").val('');
    $("#tenantNamePerhari").text('');
}

function FindRingkasanDetailPenjualanPerhari() {
    var tenanName = $("#tenantIdPerhari").val();
    var from = $("#from").val();
    var to = $("#to").val();
    if (tenanName == ""||from == ""||to == "")
        return alert("Semua Field Harus Di Isi")
    $("#tabContainer").hide();
    CreatTableRingkasanPenjualanTenantPerHari();

    $.ajax({
        type: "POST",
        url: "/ReportSale/ListDetailRingkasanPenjualanTenantPerHari",
        data: { "tenanName": tenanName, "to": to, "from": from },
        dataType: "json",
        beforeSend: AjaxStart,
        complete: AjaxEnd,
        success: InsertDetailPenjualanToTableRingkasanPerHari
    });
}

function CreatTableRingkasanPenjualanTenantPerHari() { 
    $("div#DivTableDetailRingkasanPenjualanPerhari").remove();
    $("section#main").append("<div id='DivTableDetailRingkasanPenjualanPerhari'>" +
                                     "<label id='BackTo' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<label class='TenantName'>Tenan : " + TenanName + "</label>" +
                                     "<div class='DialogOverlay'></div>" +
                                     "<table width='100%' id='TableDetailRingkasaPenjualanPerHari' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                                     "<th class='tanggalTransaksi'>Tanggal</th>" +
                                     "<th class='jumlahIDR' >Total Penjualan (IDR)</th>" +
                                     "<th class='jumlahUSD' >Total Penjualan (USD)</th></tr>"+
                                     "</thead><tbody></tbody><tfoot></tfoot></table></div>");
    $("#BackTo").click(DestroyTableDetailRingkasanPenjualanPerHari);
}

function InsertDetailPenjualanToTableRingkasanPerHari(data) {
    if (data == null || data.length == 0) {
        DestroyTableDetailRingkasanPenjualanPerHari();
        return alert("Data Tidak Di Temukan");
    }
    $("#TableDetailRingkasaPenjualanPerHari tbody").empty();
    var tanggal;
    $.each(data, function (item) {
        tanggal = data[item].TransactionDate.substring(0, 26 - 8);
        $("#TableDetailRingkasaPenjualanPerHari tbody").append("<tr>" +
                                                "<td class='tanggalTransaksi'>" + tanggal + "</td>" +
                                                "<td class='Right jumlahIDR'><span class='CCY'>Rp</span>" + String.format("{0:c}", data[item].TransactionIDR) + "</td>" +
                                                "<td class='Right jumlahUSD'><span class='CCY'>$</span>" + String.format("{0:c}", data[item].TransactionUSD) + "</td></tr>");
    });
    $("#TableDetailRingkasaPenjualanPerHari tfoot").append("<tr class='Bold'>" +
                                                   "<td class='tanggalTransaksi Right'>Total</td>" +
                                                   "<td class='jumlahIDR Right'><span class='CCY'>Rp</span>" + data[0].TotalIDR + "</td>" +
                                                   "<td class='jumlahUSD Right'><span class='CCY'>$</span>" + data[0].TotalUSD + "</td></tr>");
   $("#TableDetailRingkasaPenjualanPerHari").tablesorter({ widgets: ['zebra'] });

}

function DestroyTableDetailRingkasanPenjualanPerHari() {
    ClearDataPencarianRingkasanPerHari();
    SetDefaultRingkasanPerHari();
    $("div#DivTableDetailRingkasanPenjualanPerhari").remove();
    $("#tabContainer").show();
}