var databilling;
var detailBilling;

$(document).ready(function () {

    var Tenan = $("#Tenant").text();
    var tahun = $("#Tahun").text();
    var bulan = $("#Bulan").text();
    var ccyCode = $("#ccy").text();

    LoadDataUsingAjax(Tenan, tahun, bulan);

});

$("#dialog-overlay").ajaxStart(function () {
    $(this).show();
}).ajaxStop(function () {
    $(this).hide();
});
 

function LoadDataUsingAjax(Tenant, tahun, bulan) {
            var id = Tenant;
            $.ajax({
                type: "GET",
                url: "/APReport/DetailFakturbyTenanAndPeriod",
                dataType: "json",
                data:{'Id':id,'tahun':tahun,'bulan':bulan},
                success: showStates
           });
        }
        function showStates(data, status) {

            detailBilling = data;

            updateDetailBilling(data);
        
        }

        function updateDetailBilling(data)
       {        
       var i= 1;
       var temp=new Array();
       $.each(data, function (item) {
           i = 1;
           temp = data[item].TransactionDate.split('-');
           while (i <= 31) {
               if (temp[0] == i) {
                   $('#tgl' + i).text(data[item].TransactionDate)
                   if (data[item].CcyCode == 'USD') {
                       $('#jml' + i).text("USD " + String.format("{0:c}", data[item].TotalPenjualanHarian))
                   }
                   else {
                       $('#jml' + i).text(Currency(addCommas(data[item].TotalPenjualanHarian.toString())))
                   }
                   break;
               }
               i++;
           }
       });
        }


        function Currency(value) {
            value += '';
            x = value.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + '.' + '$2');
            }
            return x1 + x2 + "";
        }



       function addCommas(nStr)
        {
	        nStr += '';
	        x = nStr.split('.');
	        x1 = x[0];
	        x2 = x.length > 1 ? '.' + x[1] : '';
	        var rgx = /(\d+)(\d{3})/;
	        while (rgx.test(x1)) {
		        x1 = x1.replace(rgx, '$1' + ',' + '$2');
	        }
	        return x1 + x2;
        }

        $("#btnPrint").click(function () {
            var print = $("#PrintArea");
            print.jqprint();
        });

