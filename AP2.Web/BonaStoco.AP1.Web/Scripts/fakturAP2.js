var databilling;

$(document).ready(function () {
       
    var Tenan = $("#Tenant").text();
     var tahun = $("#Tahun").text();
      var bulan = $("#Bulan").text();

    LoadDataUsingAjax(Tenan,tahun,bulan);
    
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
                url: "/AP2Report/DetailFakturbyTenanAndPeriod",
                dataType: "json",
                data:{'Id':id,'tahun':tahun,'bulan':bulan},
                success: showStates,
           });
        }
        function showStates(data, status) {
           
           databilling = data;
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

     

function openwindow(innerHtml)
{
    
	var openWinPrint = window.open("","mywindow","menubar=0,resizable=0,width=0,height=0");
    openWinPrint.document.write("<!DOCTYPE html><html><head><meta charset='utf-8'/><link href='/Content/FakturAP/FakturAP2Print.css' rel='stylesheet' type='text/css' />");
    openWinPrint.document.write(innerHtml);
    openWinPrint.document.close();
    openWinPrint.print(); 
 
    
}