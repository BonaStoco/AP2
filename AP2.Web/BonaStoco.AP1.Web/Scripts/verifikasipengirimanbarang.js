function Edit() {
    $("#actqty").val("");
    $("#FormEdit").dialog("open");
    return false;
};
$('#CloseUpdate').click(
        function () {
            $('#FormEdit').dialog("close");
            return true;
        }
    );
$(function () {
    $("#FormEdit").dialog({
        autoOpen: false,
        width: 450,
        height: 250,
        show: "Fade",
        hide: "explode"
    });

    $(".validateTips").text(" ");
    $("#actqty").val("");

});

function showEditDialog(barcode, ccyCode, hargaJual, nama, actualQty, statusPrint) {
    $("#dialog-print").html(template(actualQty));
    var options = {
        autoOpen: true,
        height: 230,
        width: 370,
        modal: true,
        buttons: {
            "Print": function () {
              // $("#dialog-print").html(template(actualQty));
                var actQty = $("#actqty").val();
                PrintAllBarcode(barcode, ccyCode, hargaJual, nama, actQty, statusPrint); 
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    };
    $("#dialog-print").dialog(options);
}


function dialogPrintBarcode(Barcode, CcyCode, HargaJual, Nama, ActualQty, StatusPrint) {
    $(".validateTips").text(" ");
    showEditDialog(Barcode, CcyCode, HargaJual, Nama, ActualQty, StatusPrint);
}

/* print barcode using jzebra */
function printStruk(barcode, ccykode, hargajual, nama, actqty) {

   
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
        var countdown = actqty;
        var i = 0;
        while (i < actqty) {
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

function PrintAllBarcode(barcode, ccyCode, hargaJual, nama, actualQty, statusPrint) {
    if ($("#actqty").val() == " ") {
        $(".validateTips").css({
            'color': 'Red',
            'font-size': '11px'
        });
        $(".validateTips").text('Qty yang anda Isi kosong');
         $("#actqty").focus();

    }
    var code = "";
    var actqty = $("#actqty").val();
    var hargajual;
    var CCYCODE;

    if (ccyCode.toUpperCase() == "USD" || ccyCode.toUpperCase() == "US DOLLAR") {
            CCYCODE = "USD";
            hargajual = String.format("{0:c}", hargaJual);
        }
        else if (ccyCode.toUpperCase() == "IDR" || ccyCode.toUpperCase() == "RUPIAH") {
            CCYCODE = "IDR";
            hargajual = FormatCurrency(hargaJual);
        }
        if (statusPrint)
            printStruk(barcode, CCYCODE, hargajual, nama, actqty);
  
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


var template= function(actqty){
$("#dialog-print").html(""+
"<p class='validateTips'></p>"+
    "<table width='100%' border='0'>"+
      "<tr id='headerTable'"+
        "<td width='37%' class='left'>Jumlah Barcode</td>"+
        "<td width='63%' class='left'>"+
            "<input type='text' id='actqty' value='" + actqty + "'/>" +
            "<input type='hidden' id='kode' />"+
        "</td>"+
      "</tr>"+
    "</table>"+
   "")
    };