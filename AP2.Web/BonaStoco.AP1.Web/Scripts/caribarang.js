$(document).ready(function () {
    $("#dialog-print").hide();
    $(".validateTips").text(" ");
    $("#qty").val(" ");

});

function Search(id) {
    $.ajax({
        type: "GET",
        url: "/MasterData/CariBarangByID",
        data: { 'code': id },
        dataType: "json",
        success: InsertDataToTable
    });
    $("#AdvancedSearchProduct").dialog("close");
}

function InsertDataToTable(data) {
    var printable;
    $("#divListProduct").empty();
    $("#divListProduct").append("<br />");
    $("#divListProduct").append("<table width='100%' style='clear:both;'><thead><tr><th width='23%'>Nama</th><th width='5%'>Group</th><th width='14%'>Barcode</th><th width='10%'>Kode</th><th width='10%'>Unit</th><th width='10%' style='text-align:right;'>Harga Beli</th><th width='10%' style='text-align:right;'>Harga Jual</th><th width='10%'>Mata Uang</th><th width='10%'></th></tr></thead><tbody id='ListProduct'></tbody></table>");
    $.each(data, function (item) {
        if (data[item].CcyName == "US Dollar") {
            hargaBeli = String.format("{0:c}", data[item].HargaBeli);
            hargaJual = String.format("{0:c}", data[item].HargaJual);
        }
        else if (data[item].CcyName == "Rupiah") {
            hargaBeli = String.format("{0:n}", data[item].HargaBeli);
            hargaJual = String.format("{0:n}", data[item].HargaJual);
        }

        if (data[item].StatusPrint == false) {
            printable = "Print";
        }
        else {
            printable = "<a href='#' onclick='dialogPrintBarcode(\"" + data[item].Kode + "\",\"" + data[item].TenanId + "\" )'>Print</a>";
        }
        $("#ListProduct").append("<tr>" +
                                         "<td>" + data[item].Nama + "</td> " +
                                         "<td>" + data[item].GroupName + "</td>" +
                                         "<td>" + data[item].Barcode + "</td>" +
                                         "<td>" + data[item].Kode + "</td>" +
                                         "<td>" + data[item].UnitName + "</td>" +
                                         "<td align='right'>" + hargaBeli + "</td>" +
                                         "<td align='right'>" + hargaJual + "</td>" +
                                         "<td>" + data[item].CcyName + "</td>" +
                                         "<td> <a href='MasterData/Edit?tenanId=" + data[item].TenanId + "&productId=" + data[item].ProductId + "'>Edit</a>" +
                                           " || " + printable + "</td>" +
                                         "</tr>");
    });
}

function showEditDialog(kode, tenanId) {
   
    var options = {
        autoOpen: true,
        height: 200,
        width: 350,
        modal: true,
        buttons: {
            "Print": function () {
                FindAllItemsThenPrint(kode, tenanId);
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    };
    $("#dialog-print").dialog(options);
}

function dialogPrintBarcode(kode, tenanId) {
    $(".validateTips").text(" ");
    $("#qty").val(" ");
    showEditDialog(kode, tenanId);
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
function FindAllItemsThenPrint(kode, tenanId) {

    $.ajax({
        type: "GET",
        url: "/MasterData/FindAllProductByCode",
        dataType: "json",
        data:{"kode":kode, "tenanId": tenanId},
        success: PrintAllBarcode,
        async: false
    });
}
function PrintAllBarcode(data, status) {
    if ($("#qty").val() == " ") {
        $(".validateTips").css({
            'color': 'Red',
            'font-size': '11px'
        });
        $(".validateTips").text('Qty yang anda Isi kosong');

    }
    var code = "";
    var qty = $("#qty").val();
    for (var i = 0; i < data.length; i++) {
        var item = data[i];
        var code = item.Barcode;
        var hargaJual;
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