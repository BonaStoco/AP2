var _currPage = 1;
var _totPage=0;
var _totPerPage = 0;
var _totRows = 0;
var offset = 0;
var tenanId = 0;
var number = 0;
$(document).ready(function () {
    $("#edit-dialog").hide();
    FindTenanByDate();
    $("div.pager").show();
    Navigation();
    $("#tblreport").hide();
});

function FindTenanByDate() {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindTenanByDate/",
        dataType: "json",
        success: SetTenanToList
    });
}

function FindTenanByWeek() {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindTenanByWeek/",
        dataType: "json",
        success: SetTenanToList
    });
}

function FindTenanByMonth() {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindTenanByMonth/",
        dataType: "json",
        success: SetTenanToList
    });
}

function FindAllTenan() {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindAllTenan/",
        dataType: "json",
        success: SetTenanToList
    });
}

function SetTenanToList(data) {
    $("#menugroup").remove();
    $("div.pager").hide();
    $("#TenanList").append("<table id='menugroup' ></table>");
    $.each(data, function (item) {
        $("#menugroup").append("<tr> <td>" +
                               "<a href='#' onclick='FindProductChange("+ data[item].TenanId +","+ $("#selectdate").val() +")'>" +
                               "" + data[item].TenanId + " " + data[item].TenanName + "</a></td></tr>");
    });
    $("#menugroup").pagingTenan();
    $("#tblreport").hide();
}

function SelectPeriod() {
    var pilih = $("#selectdate").val();

    if (pilih == 1) {
        FindTenanByDate();
    }
    else if (pilih == 2) {
        FindTenanByWeek();
    }
    else if (pilih == 3) {
        FindTenanByMonth();
    }
    else if (pilih == 4) {
        FindAllTenan();
    }
}

function FindProductChange(tenanid, periode) {
    tenanId = tenanid;
    _totPerPage = 50;
    if (periode == 1) {
        FindProductChangeByDate(tenanid);
    }
    else
    if (periode == 2) {
        FindProductChangeByWeek(tenanid);
    }
    else
    if (periode == 3) {
        FindProductChangeByMonth(tenanid);
    }
    else
    if (periode == 4) {
        clearDataNavigation();
        _totRows = CountAllProductChange(tenanid);
    }
}

function FindAllProductLimit() {
    _totPage = Math.ceil(_totRows / _totPerPage);
    $(".totalPages").text(_totPage);
    $(".currentPage").text(_currPage);
    $("div.pager").show();
    ShowNavigation();
    offset = Math.ceil((_currPage - 1) * _totPerPage);
    number = offset + 1;
    FindAllProductChange(tenanId, _totPerPage, offset);
}

function clearDataNavigation() {  
    _currPage = 1;
}

function ShowNavigation() {

    if (_currPage == _totPage) {

        $(".prevPage").show();
        $(".nextPage").hide();
        $(".lastPage").hide();
        $(".firstPage").show();
    } 

    if (_currPage == 1 && _currPage > 0) {

        $(".prevPage").hide();
        $(".nextPage").show();
        $(".lastPage").show();
        $(".firstPage").hide();
    }

   if (_currPage > 1 && _currPage < _totPage) {

        $(".prevPage").show();
        $(".nextPage").show();
        $(".lastPage").show();
        $(".firstPage").show();
    }

    if (_currPage == 1 && _totPage == 1) {

        $("div.pager").hide();
    }
    
}

function CountAllProductChange(tenanid) {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/CountAllProductChangeByTenan",
        dataType: "json",
        data: { "tenanid": tenanid },
        success: function (data) {
            if (data != null) {
                _totRows = data.length
                FindAllProductLimit();
            }
        }
    });
    return _totRows;
}

function Navigation() {
    $(".nextPage").click(function () {

        NextPage();
        FindAllProductLimit();
    });

    $(".prevPage").click(function () {

        PrevPage();
        FindAllProductLimit();

    });


    $(".lastPage").click(function () {

        lastPage();
        FindAllProductLimit();
    });


    $(".firstPage").click(function () {

        firstPage();
        FindAllProductLimit();

    });

}

function firstPage() {
    _currPage = 1;
}

function lastPage() {
    _currPage = _totPage;

}


function PrevPage() {

    _currPage--;

}

function NextPage() {

    _currPage++;
}



function FindProductChangeByDate(tenanid) {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindProductChangeByTenanAndDate",
        dataType: "json",
        data: { "tenanid": tenanid },
        success: SetProductChangeToTable
    });
}

function FindProductChangeByWeek(tenanid) {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindProductChangeByTenanAndWeek",
        dataType: "json",
        data: { "tenanid": tenanid },
        success: SetProductChangeToTable
    });
}

function FindProductChangeByMonth(tenanid) {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindProductChangeByTenanAndMonth",
        dataType: "json",
        data: { "tenanid": tenanid },
        success: SetProductChangeToTable
    });
}

function FindAllProductChange(tenanid, totalrow, currpage) {
    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindAllProductChange",
        dataType: "json",
        data: { "tenanid": tenanid, "totalRow": totalrow, "currPage": currpage },
        success: SetAllProductChange
    });
}


function SetProductChangeToTable(data, status) {
    if (data != null)
        $("#tblreport").show();
    var no = 1;
    $("#tblreport tbody").empty();
    $.each(data, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }
        $("#tblreport tbody").append("<tr bgcolor=" + warna + "><td width=20>" + no + "</td ><td>" + data[item].Tanggal + "</td ><td>" + data[item].Kode + "</td><td>" + data[item].Perubahan + "</td><td align='center'><button onclick='dialogEdit(\"" + data[item].Kode + "\",\"" + data[item].TenanId + "\")' class='btnPrint'>Print</button></td></tr>");
        no++;
    });
}

function SetAllProductChange(data, status) {
    if (data != null)
        $("#tblreport").show();
    var no = number;
    $("#tblreport tbody").empty();
    $.each(data, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }
        $("#tblreport tbody").append("<tr bgcolor=" + warna + "><td width=20>" + no + "</td ><td>" + data[item].Tanggal + "</td ><td>" + data[item].Kode + "</td><td>" + data[item].Perubahan + "</td><td align='center'><button onclick='dialogEdit(\"" + data[item].Kode + "\",\"" + data[item].TenanId + "\")' class='btnPrint'>Print</button></td></tr>");
        no++;
    });
}

function showEditDialog(id, tenanid) {
    var options = {
        autoOpen: true,
        height: 200,
        width: 350,
        modal: true,
        buttons: {
            "Print": function () {
                FindAllItemsThenPrint(id, tenanid);
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    };
    $("#edit-dialog").dialog(options);
}

function dialogEdit(id, tenanid) {
    showEditDialog(id, tenanid);
}

/* print barcode using jzebra */
function printStruk(barcode, ccykode, hargajual, nama, qty) {
    var applet = document.jZebra;
    if (applet != null) {
        str = trim(barcode);
        applet.findPrinter("Zebra  TLP2844");
        applet.append("N\n");
        applet.append("q847\n");
        applet.append("Q120,21\n");
        var x = 50;
        var y1 = 0;
        var y2 = 18;
        var y3 = 65;
        var y4 = 81;
        var countdown = qty;
        var i = 0;
        while (i < qty) {
            for (var j = 0; j < 3; j++) {
                if (countdown != 0) {
                    applet.append("A" + (x - 15) + "," + y1 + ",0,2,1,1,N,\"" + nama.slice(0, 20) + "\"\n");
                    applet.append("B" + x + "," + y2 + ",0,1,1,1,40,N,\"" + barcode + "\"\n");
                    applet.append("A" + x + "," + y3 + ",0,1,1,1,N,\"" + barcode + "\"\n");
                    applet.append("A" + x + "," + y4 + ",0,2,1,1,N,\"" + ccykode + " " + hargajual + "\"\n");
                    x += 280;
                    i++;
                    countdown--;
                }
            }
            if (i % 3 == 0) x = 70;
            applet.append("P1,1\n");
            applet.print();
        }

        while (!applet.isDonePrinting()) {
            // Wait
        }
        var e = applet.getException();
        if (e == null) var info = "Printed Successfully";
        else {
            var info = "Error: " + e.getLocalizedMessage();
            if (info != null)
                alert(info);
        }
    }
    else {
        var info = "Printer belum siap";
        alert(info);
    }
}

function trim(dataStr) {
    return dataStr.replace(/(\r\n|\r|\n)/g, "");
}
function FindAllItemsThenPrint(id, tenanid) {

    $.ajax({
        type: "GET",
        url: "/PerubahanBarang/FindAllProductByCode",
        data: { "id":id , "tenanid":tenanid },
        dataType: "json",
        success: PrintAllBarcode,
        async: false
    });
}

function FormatCurrency(value) {
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

function PrintAllBarcode(data, status) {
    var code = "";
    var qty = $("#qty").val();
    for (var i = 0; i < data.length; i++) {
        var item = data[i];
        var code = item.Barcode;
        if (item.CcyKode.toUpperCase() == "USD") {

            hargaJual = String.format("{0:c}", item.HargaJual);
        }
        else if (item.CcyKode.toUpperCase() == "IDR") {
            hargaJual = FormatCurrency(item.HargaJual);
        }
        if (item.StatusPrint)
            printStruk(item.Barcode, item.CcyKode, hargaJual, item.Nama, qty);
    }
}
