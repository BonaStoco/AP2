function CreateDivSummaryPerKasir() {
    $("#DivSummaryPerkasir").remove();
    $(".tabscontent").append("<div id='DivSummaryPerkasir'>" +
                             "<label class='headerContent'>Ringkasan Penjualan Per Kasir</label><br /><br />" +
                             "<table id='PencarianSummaryPerKasir'><tr><td>Tanggal</td>" +
                             "<td><input type='text' id='tanggalPerkasir' readonly='readonly'/></td>" +
                             "<tr><td>Kasir</td>" +
                             "<td><select id='SessionPerkasir'></select></td></tr>" +
                             "<tr><td colspan='3'>" +
                             "<button id='TampilPerkasir' class='positive button'><img src='../Content/images/button/search.png'/> Tampilkan Ringkasan Per Kasir</button>&nbsp;" +
                             "</td></tr></table>" +
                             "</div>");
    var dates = $("#tanggalPerkasir").datepicker({ dateFormat: 'yy-mm-dd',
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
            SearchSession(selectedDate);
        }
    });
    LoadingPerkasirEnd();
    $("#TampilPerkasir").click(FindDetailSummaryPerKasir);
    SetDefaultDateForPerKasir();
}

function FindDetailSummaryPerKasir() {
    var tanggal = $("input#tanggalPerkasir").val();
    var sessionId = $("select#SessionPerkasir").val();
    if (tanggal == "" || sessionId == "")
        return alert("Semua Field Harus Diisi");
    $.ajax({
        type: "POST",
        url: "/ReportSaleTenant/FindSummaryPerkasirByDate",
        data: { "tanggal": tanggal, "sessionId": sessionId },
        dataType: "json",
        success: InsertSummaryPerkasirToTable
    });
}

function CreateTableSummaryPerkasir() {
    $("div#DivListSummaryPerkasir").remove();
    $("section#main").append("<div id='DivListSummaryPerkasir'>" +
                                     "<label id='BackSummaryPerkasir' class='positive button'><img src='../Content/images/button/search.png'/>Kembali Ke Pencarian</label>" +
                                     "<label id='PrintSummaryPerkasir' class='positive button'><img src='../Content/images/button/search.png'/>Print</label>" +
                                     "<label class='Session'>Kasir : " + $("#SessionPerkasir option:selected").text() + "</label>" +
                                     "<label class='Tanggal'>Tanggal : " + $("#tanggalPerkasir").val() + "</label>" +
                                     "<div class='DialogOverlay'></div>" +
                                     "<table width='100%' id='TableSummaryPerkasir' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                                     "<th class='No Center'>No</th>" +
                                     "<th class='Kode Left'>Kode Produk</th>" +
                                     "<th class='left'>Nama Produk</th>" +
                                     "<th class='Qty Right'>Qty</th>" +
                                     "<th class='Jumlah Right'>Jumlah</th>" +
                                     "</thead><tbody></tbody></table></div>");
    $("#BackSummaryPerkasir").click(BackToDivSummary);
    $("#PrintSummaryPerkasir").click(PrintSummaryPerkasir);
}

function InsertSummaryPerkasirToTable(data) {
    if (data == null || data.length == 0)
        return alert("Data Tidak DiTemukan");

    $("#tabContainer").hide();
    CreateTableSummaryPerkasir();
    var No = 0;
    var ccy;
    var jumlah;
    $.each(data, function (item) {
        No++;
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        jumlah = (data[item].HargaJual * data[item].Qty) + data[item].ServiceCharge;
        $("#TableSummaryPerkasir tbody").append("<tr>" +
                            "<td class='Center'>" + No + "</td>" +
                            "<td class='Left'>" + data[item].KodeProduk + "</td>" +
                            "<td class='Left'>" + data[item].NamaProduk + "</td>" +
                            "<td class='Right'>" + data[item].Qty + " PCS</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + String.format("{0:c}", jumlah) + "</td>" +
                            "</tr>");
    });
    $("#TableSummaryPerkasir").tablesorter({ widgets: ['zebra'] });
}

function SearchSession(tanggal) {
    if (tanggal == "") {
        ClearAllField();
        return alert("Field Tanggal Harus Diisi");
    }
    $.ajax({
        type: "POST",
        url: "/ReportSaleTenant/FindSessionIdByDate",
        data: { "tanggal": tanggal },
        dataType: "json",
        beforeSend: LoadingPerkasirStart,
        complete: LoadingPerkasirEnd,
        success: InsertSessionToList
    });
}

function InsertSessionToList(data) {
    if (data == null || data.length == 0) {
        ClearAllField();
        alert("Tidak Ada Penjualan");
        SetDefaultDateForPerKasir();
    }
    $("select#SessionPerKasir").empty();
    $.each(data, function (item) {
        $("select#SessionPerkasir").append("<option value='" + data[item].SessionId + "'>" + data[item].Kasir + "</option>");
    });
}

function ClearAllField() {
    $("input#tanggalPerkasir").val('');
    $("select#SessionPerkasir").empty();
}

function LoadingPerkasirStart() {
    $("select#SessionPerkasir").attr("disabled", true);
}

function LoadingPerkasirEnd() {
    $("select#SessionPerkasir").attr("disabled", false);
}

function BackToDivSummary() {
    ClearAllField();
    $("#DivListSummaryPerkasir").remove();
    SetDefaultDateForPerKasir();
    $("#tabContainer").show();
}

function SetDefaultDateForPerKasir() {
    var now = new Date();
    $("#tanggalPerkasir").val(now.format("yyyy-MM-dd"));
}

function PrintSummaryPerkasir() {
    var print = $("#DivListSummaryPerkasir");
    print.jqprint();
}