function CreateModalDialogGRN(title) {
    $("body").append("<div id='ModalDialogGRN'><div id='DialogOverlayGRN'></div><label class='TitleOverlay'>E-POS</label>" +
	                         "<div class='GRNDialog'><label class='Header'>" +
        	                 "<label class='LogoDialog'><img src='../Content/images/button/Icon-Bonastoco.png' alt='Logo'/></label>" +
                             title +
                             "<label id='Close'>X</label></label>" +
                             "<div id='LoadingGRN'></div>" +
                             "<table id='TableGRN' width='100%'><thead><tr>" +
                             "<th class='NomorTransaksi'>No Transaksi</th>" +
                             "<th class='TenanId'>Tenan Id</th>" +
                             "<th class='Tanggal'>Tanggal</th>" +
                             "<th class='Pengirim'>Nama Pengirim</th>" +
                             "<th class='Referensi'>Referensi</th>" +
                             "<th class='Keterangan'>Keterangan</th>" +
                             "</tr></thead><tbody></tbody></table>" +
                             "<input type='text' id='search_transaksi' placeholder='Search By No Transaksi'/>" +
                             "</div>");
    $("#search_transaksi").focus();
    $("#search_transaksi").keyup(SearchByTransaksiNo);
    $("#Close").click(DestroyModalDialogGRN);
}
function InsertDataGRNToTable(data) {
    var color;
    $("table#TableGRN tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        $("table#TableGRN tbody").append("<tr style='background-color:" + color + "' onclick='searchGRN(\"" + data[item].Guid + "\"," + data[item].TenanId + ")'>" +
                                           "<td class='NomorTransaksi'>" + data[item].KodeTransaksi + "</td>" +
                                           "<td class='TenanId'>" + data[item].TenanId + "</td>" +
                                           "<td class='Tanggal'>" + myformater(data[item].TanggalTransaksi) + "</td>" +
                                           "<td class='Pengirim'>" + data[item].NamaPengirim + "</td>" +
                                           "<td class='Referensi'>" + data[item].Referensi + "</td>" +
                                           "<td class='Keterangan'>" + data[item].Keterangan + "</td></tr>");
    });
}
function searchGRN(grnId, tenanId) {
    DestroyModalDialogGRN();
    document.location = "../VerifikasiPengirimanBarang/DetailVerifikasiPengirimanBarang?grnId=" + grnId + "&&tenanId=" + tenanId;
}

function InsertDataRETToTable(data) {
    var color;
    $("table#TableGRN tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        $("table#TableGRN tbody").append("<tr style='background-color:" + color + "' onclick='searchRET(\"" + data[item].Guid + "\"," + data[item].TenanId + ")'>" +
                                           "<td class='NomorTransaksi'>" + data[item].KodeTransaksi + "</td>" +
                                           "<td class='TenanId'>" + data[item].TenanId + "</td>" +
                                           "<td class='Tanggal'>" + myformater(data[item].TanggalTransaksi) + "</td>" +
                                           "<td class='Pengirim'>" + data[item].NamaPengirim + "</td>" +
                                           "<td class='Referensi'>" + data[item].Referensi + "</td>" +
                                           "<td class='Keterangan'>" + data[item].Keterangan + "</td></tr>");
    });
}

function searchRET(grnId, tenanId) {
    DestroyModalDialogGRN();
    document.location = "../VerifikasiReturBarang/DetailVerifikasiReturnBarang?grnId=" + grnId + "&&tenanId=" + tenanId;
}

function DestroyModalDialogGRN() {
    $("div#ModalDialogGRN").remove();
}
function LoadingStart() {
    $("#LoadingGRN").show();
}
function LoadingEnd() {
    $("#LoadingGRN").hide();
}
function myformater(jsonDt) {
    //incoming json date string is of the form "/Date(946702800000)/"
    var jdate = new Date(+jsonDt.replace(/\D/g, ''));
    var months = new Array('January', 'February', 'March', 'April', 'May','June', 'July', 'August', 'September', 'October', 'November', 'December');
    var year = jdate.getFullYear();
    var month = jdate.getMonth() + 1 < 10 ? '0' + (jdate.getMonth() + 1) : jdate.getMonth();
    var day = jdate.getDay() < 10 ? '0' + jdate.getDay() : jdate.getDay();

    var dateString = (day + ' ' + months[month] + ' ' + year);
    return dateString;
}
function SearchByTransaksiNo() {
    LoadingStart();
    $('#search_transaksi').quicksearch('table#TableGRN tbody tr',
            {
                stripeRows: ['odd', 'even']
            });
    LoadingEnd();
}