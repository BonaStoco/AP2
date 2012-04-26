var no = 0;
var tanggal;
var locationId;

function nextMonth() {
    no = 0;
    locationId = $("#location-id").text();

    no = parseInt($("#no").text());

    no++;
    if (no - 1 == 0) {
        $("#next").css('display', 'none');
    }
    $("#no").text(no);

    $.ajax({
        type: "GET",
        url: "/APReportSale/NextMonth",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResultMonth,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });

}

function previousMonth() {
    $("#next").css('display', 'inline');
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());
    no--;
    $("#no").text(no);


    $.ajax({
        type: "GET",
        url: "/APReportSale/PreviousMonth",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResultMonth
    });
}


function dateResultMonth(data, status) {
    if (data != null) {
        var tgl = data.Transactiondate;
        tanggal = tgl;
        $("#today").text(data.Transactiondate);
        $("#twoDaysBefore").text(data.TwoMonthBefore);
        $("#previousDay").text(data.OneMonthBefore);
        $("#date1-idr").text("Rp." + data.TotalSaleIDRTwoMonthBefore);
        $("#date2-idr").text("Rp." + data.TotalSaleIDROneMonthBefore);
        $("#date3-idr").text("Rp." + data.TotalSaleIDRCurrentMonth);
        $("#date1-usd").text("$" + data.TotalSaleUSDTwoMonthBefore);
        $("#date2-usd").text("$" + data.TotalSaleUSDOneMonthBefore);
        $("#date3-usd").text("$" + data.TotalSaleUSDCurrentMonth);
        DetailTenantMonth(no, locationId);
    }


    //    $(".IDR").css({'background-color' : 'red', 'top' :'0'});
    //    $(".USD").css("background-color", "red");  
}

function DetailTenantMonth(no, locationId) {

    $.ajax({
        type: "GET",
        url: "/APReportSale/RekapTenanBulananData",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: DetailResultMonth,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });

}
function DetailResultMonth(data, status) {
    if (data != null) {
        $("#detail-penjualan tbody").empty();
        $("#detail-penjualan tfoot").empty();

        $("#waktu").text(tanggal);
    var no = 1;
    $.each(data, function (item) {

        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }

        $("#detail-penjualan tbody").append(" " +
   "<tr bgcolor=" + warna + ">" +
								        "<td class='left'>" + data[item].TenanName + "&nbsp;</td>" +
								        "<td class='right'> Rp " + Currency(data[item].MonthlyTotalSalePerTenan) + "</td>" +
                                        "<td class='right'> $ " + String.format("{0:c}", data[item].TotalSalePerTenantInUSD) + "</td>" +
							            "</tr>" +

   " ");
        no++;
    });

        $("#detail-penjualan tfoot").append(" " +
   "<tr>" +
								        "<th class='left'>Total</th>" +
								        "<th class='right'>" + $("#date3-idr").text() + "</th>" +
                                        "<th class='right'> $" + $("#date3-usd").text() + "</th>" +
							            "</tr>" +

   " ");
    }
}



function next() {
    no = 0;
    locationId = $("#location-id").text();

    no = parseInt($("#no").text());
   
    no++;
    if (no-1 == 0) {
        $("#next").css('display', 'none');
    }
    $("#no").text(no);   
      
    
 $.ajax({
        type: "GET",
        url: "/APReportSale/Next",
        dataType: "json",
        data:{'no':no, 'locationId':locationId},
        success: dateResult,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });
   
}

function DetailTenant(no, locationId) {

    $.ajax({
        type: "GET",
        url: "/APReportSale/RekapTenanHarianData",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },      
        success: DetailResult,
        beforeSend: LoadingStart,
        complete: LoadingStop
    });

}

function DetailResult(data, status) {
    if (data != null) {
        $("#detail-penjualan tbody").empty();
        $("#detail-penjualan tfoot").empty();


        $("#waktu").text(tanggal);
    var no = 1;
    $.each(data, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }
        $("#detail-penjualan tbody").append(" " +
   "<tr bgcolor=" + warna + ">" +
								        "<td class='left'>" + data[item].TenanName + "&nbsp;</td>" +
								        "<td class='right'> Rp " + Currency(data[item].TotalSalePerTenan) + "</td>" +
                                        "<td class='right'> $ " + String.format("{0:c}", data[item].TotalSalesPerTenantInUSD) + " </td>" +
							            "</tr>" +

   " ");
        no++;
    });
        $("#detail-penjualan tfoot").append(" " +
   "<tr>" +
								        "<th class='left'>Total</th>" +
								        "<th class='right'>" + $("#date3-idr").text() + "</th>" +
                                        "<th class='right'>" + $("#date3-usd").text() + "</th>" +
							            "</tr>" +

   " ");
    }
}

function Currency(value) {
value +='';
x = value.split(',');
x1 = x[0];
x2= x.length > 1 ? ',' + x[1]:'';
var rgx = /(\d+)(\d{3})/;
while (rgx.test(x1)) {
    x1 = x1.replace(rgx, '$1' + ',' + '$2');
}
return x1 + x2 ;
}




function dateResult(data, status) {
    if (data != null) {
        var tgl = data.Transactiondate;
        tanggal = tgl;
        $("#today").text(data.Transactiondate);
        $("#twoDaysBefore").text(data.TwoDaysBefore);
        $("#previousDay").text(data.PreviousDay);
        $("#date1-idr").text("Rp." + data.TotalSaleIDRTwoDaysBefore);
        $("#date2-idr").text("Rp." + data.TotalSaleIDRPreviousDay);
        $("#date3-idr").text("Rp." + data.TotalSaleIDR);
        $("#date1-usd").text("$" + data.TotalSaleUSDTwoDaysBefore);
        $("#date2-usd").text("$" + data.TotalSaleUSDPreviousDay);
        $("#date3-usd").text("$" + data.TotalSaleUSD);
        DetailTenant(no, locationId);
    }
    

//    $(".IDR").css({'background-color' : 'red', 'top' :'0'});
//    $(".USD").css("background-color", "red");  
}



function previous() {
    $("#next").css('display', 'inline');
    no = 0;
    locationId = $("#location-id").text();
    no = parseInt($("#no").text());   
     no--;
     $("#no").text(no);
   

    $.ajax({
        type: "GET",
        url: "/APReportSale/Previous",
        dataType: "json",
        data: { 'no': no, 'locationId': locationId },
        success: dateResult
    });
    
}

function LoadingStart() {

    $("#dialog-overlay").show();
}

function LoadingStop() {

    $("#dialog-overlay").hide();
    $("table").trigger("update");
}

function CetakLaporan() {
    var print = $("#detail");
    print.jqprint();
}