var date = new Array();
var dateString;
var Sales = new Array();
var SalesAmountIDR = new Array();
var SalesAmountUSD = new Array();
var IDR = new Array();
var USD = new Array();
var Tot_Transaction = new Array();
var SalesAmount;

function GetSalesMonitoring() {
    $.ajax({
        type: "GET",
        url: "/Home/GetDetailFakturbyPeriod",
        dataType: "json",
        success: ShowSalesMonitoring
    });
}

function ShowSalesMonitoring(data) {
    SetDataSales(data);
    CreateSalesMonitoringGraph();
}

function SetDataSales(data) {
    $.each(data, function (item) {
        date.push(data[item].DateStringParam);
        dateString = data[item].DateString;
        Sales.push([dateString, data[item].TotalTransaction]);
        IDR.push([dateString, data[item].TotalSalePerTenan]);
        USD.push([dateString, data[item].TotalSalesPerTenantInUSD]);
        SalesAmountIDR.push([String.format("Rp {0}", FormatCurrencyIDR(data[item].TotalSalePerTenan))]);
        SalesAmountUSD.push([String.format("$ {0:c}",data[item].TotalSalesPerTenantInUSD)]);       
        Tot_Transaction.push([data[item].TotalTransaction]);   
          
    });
    return data;
}

function CreateSalesMonitoringGraph() {
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'SalesMonitoring'
        },

        title: {
            text: ''
        },

        xAxis: {
            type: 'datetime',
            tickInterval: 7 * 24 * 3600 * 1000, // one week
            tickWidth: 0,
            gridLineWidth: 1,
            labels: {
                align: 'left',
                x: 3,
                y: 3,
                formatter: function () {
                    return Highcharts.numberFormat(this.value, 0);
                }
            },
            showFirstLabel: true
        },

        yAxis: [{ // left y axis
            title: {
                text: null
            },
            labels: {
                align: 'left',
                x: -10,
                y: 0,              
                formatter: function () {
                    return Highcharts.numberFormat(this.value, 0);
                }
            },
            showFirstLabel: true,
            
        }],

        legend: {
            align: 'left',
            verticalAlign: 'top',
            y: 0,
            x: 20,
            floating: true,
            borderWidth: 0
        },

        tooltip: {
            formatter: function () {
                if (this.series.name == "Total Penjualan IDR") {
                    SalesAmount = SalesAmountIDR[this.x];

                }
                else {
                    SalesAmount = SalesAmountUSD[this.x];
                }

                return '<b>' + this.series.data[this.x].name + '</b><br/>' +   
               this.series.name+ ' : ' + SalesAmount+ '<br/>' 
              
            }
        },

        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            ShowDetailSales(date[this.x]);
                        }
                    }
                },
                marker: {
                    lineWidth: 1
                }
            }
        },

        series: [{
            name: 'Total Penjualan IDR',
            lineWidth: 2,
            marker: {
                radius: 4
            },
            data: IDR
            },
            {
                name: "Total Penjualan USD",
                data:USD
                       
        }]
    });
}

function ShowDetailSales(date) {
    CreateModalDialog("Transaksi");
    $.ajax({
        type: "POST",
        url: "/ReportSaleTenant/ListDetailPenjualanTenan",
        data: { "dari": date, "sampai": date },
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        success: InsertDetailPenjualanToTable
    });
}

function CreateModalDialog(title) {
    $("body").append("<div id='ModalDialogSales'><div id='DialogOverlay'></div><label class='TitleOverlay'>E-POS</label>" +
	                         "<div class='SalesDialog'><label class='Header'>" +
        	                 "<label class='LogoDialog'><img src='../Content/images/button/Icon-Bonastoco.png' alt='Logo'/></label>" +
                             title +
                             "<label id='Close'>X</label></label>" +
                             "<div id='Loading'></div>" +
                             "<table id='TableTransaction' width='100%'><thead><tr>" +
                             "<th class='Tanggal'>Tanggal</th>" +
                             "<th class='NoTransaksi'>No Transaksi</th>" +
                             "<th class='Jumlah'>Jumlah</th></tr></thead><tbody></tbody></table>" +
                             "<label id='TotalTransaction'></label>" +
                             "<input type='text' id='search_transaction' placeholder='Search By Name'/>" +
                             "</div>");
    $("#search_transaction").focus();
    $("#search_transaction").keyup(SearchByTransactionNo);
    $("#Close").click(DestroyModalDialog);
}

function DestroyModalDialog() {
    $("div#ModalDialogSales").remove();
}
function InsertDetailPenjualanToTable(data) {
    if (data == 0 || data == null) {
        DestroyModalDialog();
        return;
    }
    var color;
    $("table#TableTransaction tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        $("table#TableTransaction tbody").append("<tr style='background-color:" + color + "' onclick='ShowDetailProduct(\"" + data[item].TransactionNo + "\")'>" +
                                           "<td class='Tanggal'>" + data[item].TransactionDate + "</td>" +
                                           "<td class='NoTransaksi'>" + data[item].TransactionNo + "</td>" +
                                           "<td class='Jumlah'><span class='CCY'>" + ccy + "</span> " +
                                                FormatCurrencyIDR(data[item].SellingPerTransaction) + "</td></tr>");
    });
    $("label#TotalTransaction").text('Total : ' + data.length + ' Transaksi');
}

function SearchByTransactionNo() {
    $('#search_transaction').quicksearch('table#TableTransaction tbody tr',
            {
                stripeRows: ['odd', 'even']
            });
}

function LoadingStart() {
    $("#Loading").show();
}
function LoadingEnd() {
    $("#Loading").hide();
}

function ShowDetailProduct(transactionNo) {
    var _tenanid = $("input#tenant").val();
    $.ajax({
        type: "POST",
        url: "/ReportSaleTenant/FindProductDetailByTransactionNumberTenan",
        data: { 'transactionNo': transactionNo },
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
    $("div#ModalDialogSales").hide();
    CreateDetailTransactionDialog("Product Detail No Transaction " + data[0].TransactionNo);
    RenderDataToTable(data);
}

function CreateDetailTransactionDialog(title) {
    $("body").append("<div id='ModalDialogDetailTransaction'><div id='DialogOverlay'></div><label class='TitleOverlay'>E-POS</label>" +
	                         "<div class='DetailTransactionDialog'><label class='Header'>" +
        	                 "<label class='LogoDialog'><img src='../Content/images/button/Icon-Bonastoco.png' alt='Logo'/></label>" +
                             title +
                             "<label id='Back'>Kembali</label></label>" +
                             "<div id='Loading'></div>" +
                             "<table id='TableDetailTransaction' width='100%' class='tablesorter'><thead style='cursor:pointer;'><tr>" +
                             "<th class='No Center'>No</th>" +
                             "<th class='KodeProduk Left'>Kode Barang</th>" +
                             "<th class='NamaProduk Left'>Nama Barang</th>" +
                             "<th class='Qty Right'>Qty</th>" +
                             "<th class='HargaJual Center'>Harga Jual</th>" +
                             "<th class='Diskon Center'>Diskon</th>" +
                             "<th class='Jumlah Right'>Jumlah</th>" +
                             "</thead><tbody></tbody><tfoot></tfoot></table>" +
                             "</div>");
    $("#Back").click(BackToTransactionSaleDialog);
}

function RenderDataToTable(data) {
    var total;
    var totalPenjualan = 0;
    var ccy;
    var no = 0;
    var hargaJual;
    var discountItem;
    var netAmount;
    var discountTotal;
    var tax;
    var charge;
    var serviceCharge;
    var totalAmount;
    $("table#TableDetailTransaction tbody").empty();
    $.each(data, function (item) {
        no++;
        ccy = (data[item].Ccy == "IDR") ? "Rp" : "$";
        total = data[item].HargaJual * data[item].Qty;
        totalPenjualan = totalPenjualan + total;

        if (data[item].Ccy == "IDR") {
            hargaJual = FormatCurrencyIDR(data[item].HargaJual);
            discountItem = FormatCurrencyIDR(data[item].DiscountItemAmount);
            netAmount = FormatCurrencyIDR(data[item].NetAmount);
            discountTotal = FormatCurrencyIDR(data[item].DiscountTotalAmount);
            tax = FormatCurrencyIDR(data[item].TaxAmount);
            charge = FormatCurrencyIDR(data[item].ChargeAmount);
            serviceCharge = FormatCurrencyIDR(data[item].ServiceCharge);
            totalAmount = FormatCurrencyIDR(data[item].TotalAmount);
        }
        else {
            hargaJual = String.format("{0:c}", data[item].HargaJual) ;
            discountItem = String.format("{0:c}", data[item].NetAmount) ;
            netAmount = String.format("{0:c}", data[item].DiscountItemAmount) ;
            discountTotal = String.format("{0:c}", data[0].DiscountTotalAmount) ;
            tax = String.format("{0:c}", data[0].TaxAmount) ;
            charge = String.format("{0:c}", data[0].ChargeAmount) ;
            serviceCharge = String.format("{0:c}", data[0].ServiceCharge) ;
            totalAmount = String.format("{0:c}", data[0].TotalAmount);
        }

        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        $("table#TableDetailTransaction tbody").append("<tr style='background-color:" + color + "'>" +
                            "<td class='Center'>" + no + "</td>" +
                            "<td class='Left'>" + data[item].KodeProduk + "</td>" +
                            "<td class='Left'>" + data[item].NamaProduk + "</td>" +
                            "<td class='Right Qty'>" + data[item].Qty + " PCS</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + hargaJual + "</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + discountItem + "</td>" +
                            "<td class='Right'><span class='CCY'>" + ccy + "</span>" + netAmount + "</td></tr>");
    });
    $("table#TableDetailTransaction tfoot").append("<tr class='Bold'><td colspan='5'></td>" +
                                                   "<td class='Bold Left'>Total Diskon </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + discountTotal + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5'></td>" +
                                                   "<td class='Bold Left'>Tax </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + tax + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelChargeAmount Right'></td>" +
                                                   "<td class='Bold Left'>Charge </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + charge + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelChargeAmount Right'></td>" +
                                                   "<td class='Bold Left'>Service Charge </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + serviceCharge + "</td></tr>" +
                                                   "<tr class='Bold'><td colspan='5' class='LabelTotalPenjualan Right'></td>" +
                                                   "<td class='Bold Left'>Total </td>" +
                                                   "<td class='TotalPenjualan Right'><span class='CCY'>" + ccy + "</span>" + totalAmount + "</td></tr>");

}

function BackToTransactionSaleDialog() {
    $("div#ModalDialogDetailTransaction").remove();
    $("div#ModalDialogSales").show();
}

function FormatCurrencyIDR(value) {
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