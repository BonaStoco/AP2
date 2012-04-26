function CreateDivSummaryPerHariTenan() {
    $("#DivSummaryPerHariTenan").remove();
    $(".tabscontent").append("<div id='DivSummaryPerHariTenan'>" +
                             "<label class='headerContent'>Ringkasan Penjualan Per Hari</label><br /><br />" +
                             "<table id='PencarianSummaryPerHariTenan'>" +
                             "<tr><td>Dari</td>" +
                             "<td><input type='text' name='dariTenan' id='dariTenan' readonly='readonly'/></td></tr>" +
                             "<tr><td>Sampai</td>" +
                             "<td><input type='text' name='sampaiTenan' id='sampaiTenan' readonly='readonly' /></td></tr>" +
                             "<tr><td colspan='3'>" +
                             "<button id='TampilPerHariTenan' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Ringkasan Per Hari</button>&nbsp;" +
                             "</td></tr></table>" +
                             "</div>");

    var dates = $("#dariTenan, #sampaiTenan").datepicker({ dateFormat: 'yy-mm-dd',
        defaultDate: "+1w",
        gotoCurrent: true,
        changeMonth: true,
        numberOfMonths: 1,
        onSelect: function (selectedDate) {
            var option = this.id == "dariTenan" ? "minDate" : "maxDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    SetDefaultDateDetailPenjualanPerHariTenan();
    $("#TampilPerHariTenan").click(FindDetailSummaryPerHariTenan);
}


function FindDetailSummaryPerHariTenan() {
    var dari = $("#dariTenan").val();
    var sampai = $("#sampaiTenan").val();
    if (dari == "" || sampai == "")
        return alert("Semua Field Harus Di Isi");

    $("#tabContainer").hide();
    CreateTableDetailPenjualanPerHariTenan();
    $.ajax({
        type: "POST",
        url: "/ReportSaleTenant/ListDetailPenjualanPerHariTenan",
        data: { "dari": dari, "sampai": sampai },
        dataType: "json",
        beforeSend: AjaxStart,
        complete: AjaxEnd,
        success: InsertDetailPenjualanPerHariTenanToTable
    });
}

function InsertDetailPenjualanPerHariTenanToTable(data) {
    var tanggal;    
    if (data == null || data.length == 0) {
        DestroyTableDetailPenjualanPerHariTenan();
        return alert("Data Tidak DiTemukan");
    }

    $("#DivTableDetailPenjualanPerHariTenan tbody").empty();
    var ccy;
    $.each(data, function (item) {
        
        tanggal = data[item].TransactionDate.substring(0, 26 - 8);
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        $("#TableDetailPenjualanPerHariTenan tbody").append("<tr>" +
                                                "<td class='tanggalTransaksiTenan'>" + tanggal + "</td>" +
                                                "<td class='Right jumlahIDRTenan'> <span Class='CCY'> Rp </span>" + String.format("{0:c}", data[item].TransactionIDR) + "</td>" +
                                                "<td class='Right jumlahUSDTenan'> <span Class='CCY'> $ </span>" + String.format("{0:c}", data[item].TransactionUSD) + "</td></tr>");
    });

    $("#TableDetailPenjualanPerHariTenan tfoot").append("<tr class='bold'>" +
                                                "<td class='Right tanggalTransaksiTenan'>Total</td>" +
                                                "<td class='Right jumlahIDRTenan'> <span Class='CCY'> Rp </span>" + String.format("{0:c}", data[0].TotalIDR) + "</td>" +
                                                "<td class='Right jumlahUSDTenan'> <span Class='CCY'> $ </span>" + String.format("{0:c}", data[0].TotalUSD) + "</td></tr>");

    $("#TableDetailPenjualanPerHariTenan").tablesorter({ widgets: ['zebra'] });

//    $('#kode_transaksi_search').quicksearch('table#DivTableDetailPenjualanPerHariTenan tbody tr',
//            {
//                stripeRows: ['odd', 'even'],
//                loader: 'img.loadingSearchTransaction'
//            });
//            $("#DivTableDetailPenjualanPerHariTenan").tablesorter({ widgets: ['zebra'] });
}

function CreateTableDetailPenjualanPerHariTenan() {
    $("div#DivTableDetailPenjualanPerHariTenan").remove();
    $("section#main").append("<div id='DivTableDetailPenjualanPerHariTenan'>" +
                                     "<label id='Back' class='positive button'><img src='../../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<div class='DialogOverlay'></div>" +
                                     "<table width='100%' id='TableDetailPenjualanPerHariTenan' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                                     "<th class='tanggalTransaksiTenan'>Tanggal</th>" +
                                     "<th class='jumlahIDRTenan' >Total Penjualan (IDR)</th>" +
                                     "<th class='jumlahUSDTenan' >Total Penjualan (USD)</th></tr>" +
                                     "</thead><tfoot></tfoot><tbody></tbody></table></div>");
    $("#Back").click(DestroyTableDetailPenjualanPerHariTenan);
}

function DestroyTableDetailPenjualanPerHariTenan() {
    ClearDataPencarianPerHariTenan();
    SetDefaultDateDetailPenjualanPerHariTenan();
    $("div#DivTableDetailPenjualanPerHariTenan").remove();
    $("#tabContainer").show();
}

function ClearDataPencarianPerHariTenan() {
    $("#dariTenan").val('');
    $("#sampaiTenan").val('');
}

function SetDefaultDateDetailPenjualanPerHariTenan() {
    var now = new Date();
    $("#dariTenan").val(now.format("yyyy-MM-dd"));
    $("#sampaiTenan").val(now.format("yyyy-MM-dd"));
}