var datas;
var headerproductdata;
var timeout;
var tenanIdLocal;
$("#overlay").ajaxStart(function () { $(this).show() }).ajaxStop(function () { $(this).hide() });
$(document).ready(function () {
    // LoadPartMaster();   
    FindAllProductPendingByGroupTenan();
    $("#group-btn-footer").hide();
});
function FindPartPendingByTenanId(tenanId) {
    var tenantId = tenanId;
    $.ajax({
        type: "GET",
        url: "/RequestProduct/FindPartPendingByTenanId",
        dataType: "json",
        data: { "tenantId": tenantId },         
        success: ViewPartMasterAprove       
    });
}

function FindDetailPartPendingByTenanId(tenanId) {
    var tenantId = tenanId
    tenanIdLocal = tenanId;
    $.ajax({
        type: "GET",
        url: "/RequestProduct/FindPartPendingByTenanId",
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        data: { "tenantId": tenantId },
        success: ViewPartMasterAprove
    });
}


function LoadPartMaster() {
    $.ajax({
        type: "GET",
        url: "/RequestProduct/LoadPartMaster",
        dataType: "json",       
        success: ViewPartMasterAprove
    });
}

function FindAllProductPendingByGroupTenan() {
    $.ajax({
        type: "GET",
        url: "/RequestProduct/FindAllProductPendingByGroupTenan",
        dataType: "json",
        success: ViewHeaderProduct
    });
}
function ViewPartMasterAprove(data, status) {
    var i = 1;
    datas = data;
    $("#group-btn").show();
    $("#aprovepart").show();
    $("#headerproduct").empty();
    $("#group-btn").empty();
    $("#group-btn").append("<thead><tr><td><div id='btn-approve-all' onclick='approveAllByTenanId(" + tenanIdLocal + ")'>Approve All</div><div id='btn-reject-all' onclick='rejectAllByTenanId(" + tenanIdLocal + ")'>Reject All</div></td><td></td><td></td><td></td><td></td><td></td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td></td><td><a href='#' onclick='Back()'> Back </a></td></tr></thead>");
    $("#aprovepart").empty();
    $("#aprovepart ").append("<thead><tr></th><th>Kode - Nama Tenant </th><th>Kode Barang</th><th>Barcode</th><th>Nama Barang </th><th>Harga Jual</th><th>Harga Beli</th><th>Unit</th><th>Status</th></tr></thead>")
    $.each(data, function (item) {
        var colors = rowColor(i);
        $("#aprovepart").append("<tbody><tr id=tr" + i + " bgcolor=" + colors + "><td>&nbsp;" + data[item].TenanId + "-" + data[item].TenanName + "</td><td>" + data[item].Kode + "</td><td>" + data[item].Barcode + "</td><td>&nbsp;" + data[item].Nama + "</td><td>" + nominalFormat(data[item].HargaJual) + "</td><td>&nbsp;" + nominalFormat(data[item].HargaBeli) + "</td><td>" + data[item].UnitName + "</td> <td><center><input type='hidden' id=productid" + i + " name=productid" + i + " value=" + data[item].ModelGuid + "><input type='hidden' id=status" + i + " name=status" + i + " value=" + data[item].Status + "><input type='button' id=btn-aprove" + i + " name='btn-aprove' value='Approve' onclick='CekStatusAprove(document.getElementById(\"productid" + i + "\").value,\"tr" + i + "\")'> | <input type='submit' id=btn-reject" + i + " name='btn-reject' value='Reject' onclick='CekStatusReject(document.getElementById(\"productid" + i + "\").value,\"tr" + i + "\")'></center></td></tr></tbody>");
        i++;
    });
    $("#group-btn-footer").show();
    $("#group-btn-footer").append("<thead><tr><td></td><td></td><td></td><td></td><td></td><td></td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td></td><td><a href='#' onclick='Back()'> Back </a></td></tr></thead>");

}

function ViewHeaderProduct(data, status) {
    var i = 1;
    headerproductdata = data;
    $("#headerproduct").empty();
    $("#headerproduct").append("<thead><tr><th width=10px><center>No</center></th><th>Kode Tenan </th><th>Nama Tenan </th><th width=20px>Total Pending</th><th></th></tr></thead>")
    $.each(data, function (item) {
        var colors = rowColor(i);
        $("#headerproduct").append("<tbody><tr id=tr" + i + " bgcolor=" + colors + "><td>"+i+"</td><td>&nbsp;" + data[item].TenanId + "</td><td>&nbsp;" + data[item].TenanName + "</td><td class='right'>" +data[item].TotalRequest + "</td><td><center><a href='#' onclick=FindDetailPartPendingByTenanId("+data[item].TenanId+")>Detail</a></center></td></tr></tbody>");
        i++;
    });
}


function rowColor(idRow) {
    var colors;
    if (idRow % 2 == 0) {colors = "#E1F0FF";}else {colors = "#fff";}
    return colors;
}

function CekStatusAprove(guidId, rowId) {   
    $.ajax({
        type: "GET",
        url: "/RequestProduct/StatusAprove",
        data: { 'guidId': guidId},
        dataType: "json"        
    });
    RemoveCurrentRow(rowId);
}

function CekStatusReject(guidId, rowId) {    
    $.ajax({
        type: "GET",
        url: "/RequestProduct/StatusReject",
        data: {'guidId': guidId},
        dataType: "json"

    });
    RemoveCurrentRow(rowId);
}

function RemoveCurrentRow(rowId) {

    $("#" + rowId).text("");
}

function nominalFormat(nStr) {
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

function rejectAll() {    
    if (Konfirmasi()) {
       // var idx = 0;
//        $.each(datas, function (item) {
//            idx = idx + 1;
//            timeout = idx;           
//            var requestProduct = new Object();
//            requestProduct.TenanId = datas[item].TenanId;
//            requestProduct.TenanName = datas[item].TenanName;
//            requestProduct.Nama = datas[item].Nama;
//            requestProduct.UnitName = datas[item].UnitName;
//            requestProduct.ModelGuid = datas[item].ModelGuid;
//            requestProduct.HargaJual = datas[item].HargaJual;
//            requestProduct.HargaBeli = datas[item].HargaBeli;
            $.ajax({
                type: "POST",
                url: "/RequestProduct/RejectAllProduct",
               // data: { 'RequestProduct': JSON.stringify(requestProduct) },
                dataType: "json",
                success: function () {
                    LoadPartMaster();
                }
            });
            $("#aprovepart tbody #tr" + idx).empty();      
//        });
       // idx++;
      
    }
}
var timer_is_on = 0;

function timedCount(idx) {

    $("#aprovepart").text(idx);
    t = setTimeout("timedCount()", 1000);
}

function doTimer() {
    if (!timer_is_on) {
        timer_is_on = 1;
        timedCount();
    }
}
function approveAll() {
    if (Konfirmasi()) {
            $.ajax({
                type: "POST",
                url: "/RequestProduct/ApproveAllProduct",
              //  data: { 'RequestProduct': JSON.stringify(requestProduct) },
                dataType: "json",
                success: function () {
                    LoadPartMaster();
                }       
            });
            $("#aprovepart tbody #tr" + idx).empty();

//        });
       // idx++;
    }
    }

    function approveAllByTenanId(tenanId) {
    if (Konfirmasi()) {
        var tenantId = tenanId;
        $.ajax({
            type: "POST",
            url: "/RequestProduct/ApproveAllProductbyTenanId",
            data: { "tenantId": tenantId },
            dataType: "json",
            success: function () {

                FindDetailPartPendingByTenanId(tenanId);
            }
        });
        $("#aprovepart tbody #tr").empty();

        //        });
        // idx++;
    }
}
function rejectAllByTenanId(tenanId) {
    if (Konfirmasi()) {
        var tenantId = tenanId;
        $.ajax({
            type: "POST",
            url: "/RequestProduct/RejectAllProductbyTenanId",
            data: { "tenantId": tenantId },
            dataType: "json",
            success: function (data) {              
             
                    FindDetailPartPendingByTenanId(tenanId);
              
            }
        });
        $("#aprovepart tbody #tr").empty();
        //        });
        // idx++;

    }
}
function Konfirmasi() {
    if (confirm("Apakah Anda yakin melanjutkan proses ini ?"))
        return true
    else
        return false;
}

function LoadingStart() {
    $("DIV#loading").show(); 
}
function LoadingStop() {
    $("DIV#loading").hide();
}

function Back() {
    $("#aprovepart").hide();
    $("#headerproduct").show();
    $("#group-btn").hide();
    $("#group-btn-footer").hide();
    this.FindAllProductPendingByGroupTenan();
}

