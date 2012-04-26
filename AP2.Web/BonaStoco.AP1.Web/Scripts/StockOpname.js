var index = 0;
var stockOpnameId;
var opnameId;
var opnameItemId;
var tenanId;
var statusopname;

$("#loadingStockOpname").ajaxStart(function () { $(this).show() }).ajaxStop(function () { $(this).hide() });
ListStockOpnameDetail = new Array();

$(document).ready(function () {
    init();
});
function init() {
    $("#tblListInvoice tbody").empty();
    tenanId = $("#tenanId").text().trim();
    loadDataStockOpname(tenanId);
    getOpnameByStatusOpen(tenanId);
    $("#resultForEdit").hide();
    $("#editQtyDialog").hide();
    $("#AdvanceSearchTenanDialog").hide();
    AdvanceSearchProduct(tenanId);
    $('#addStockOpname').click(function () {
        OpenStockOpname(tenanId);
        // SetDateDefault();
        $("#listitem > tbody").empty();
        $("#listitem > tfoot#summary").show();
        $("#SubMenuDetailInvoiceClose").show();
    });  

    $("#SubMenuDetailInvoiceFormCetakPdf").click(function () {
        $("#stockopnamepriview").submit();
    });
    $('#CariBarcode').click(function () {
        ShowDialogBarcode();
        ClearandHideDialogProductQty();
    });
}

$('#tmblBack').click(function () {
    hideDetailStockOpname();
    init();
   
    
});

function AddStockOpnameItem() {
    var qty = $("#Qty").val();
    var partId = $("#PartId").val();
    var opnameId = $("#OpnameId").val();
    var opnameNumber = $("#OpnameNumber").val();
    var partGroupId = $("#PartGroupId").val();
    var partGuid = $("#PartGuid").val();
    $.ajax({
        type: "POST",
        url: "/StockOpname/AddStockOpnameItem",
        dataType: "json",
        data: { "tenanId": this.tenanId, "qty": qty, "partId": partId, "partGuid": partGuid, "partGroupId": partGroupId, "opnameId": opnameId },
        success: function (data) {
            if (data.ok === true)
                GetStockOpnameItemById(data.itemid);
            else
                alert(data.errormessage);
        },
        error: function (xhr, err) {
            alert(err);
        }
    });

}

function GetStockOpnameItemById(id) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/GetStockOpnameItemById",
        dataType: "json",
        data: { "id": id },
        success: function (data) {
            if (data != null) {
                ViewAddItem(data);
            }
            else {
                setTimeout(GetStockOpnameItemById(id), 2000);
            }
        }
    });
}

function getOpnameByStatusOpen(tenanId) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/GetOpnameByStatusOpen",
        dataType: "json",
        data: { "tenanId": tenanId },
        success: function (data) {
            if (data != null) {
                if (data.Status == "Open") {
                    $("#addStockOpname").hide();
                }               
            } else {

                $("#addStockOpname").show();
            }
        }
    });
}

function GetStockOpnameItemByIdAfterEdit(id) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/GetStockOpnameItemById",
        dataType: "json",
        data: { "id": id },
        success: function (data) {
            if (data != null) {
                ViewEditItem(data);
            }
            else {
                setTimeout(GetStockOpnameItemByIdAfterEdit(id), 2000);
            }

        }
    });
}

function EditStockOpnameItem(id) {
    var qty = $("#vQty").val();
    var partId = $("#PartId").val();
    var headerId = $("#OpnameId").val();
    $.ajax({
        type: "GET",
        url: "/StockOpname/UpdateItemQty",
        dataType: "json",
        data: { "qty": qty, "id": id, "headerId": headerId },
        success: function (id) {
            GetStockOpnameItemByIdAfterEdit(id);
        }
    });
}

function OpenStockOpname(tenanId) {
    CreateStockOpname(tenanId);
}

function CreateStockOpname(tenanId) {
    $.ajax({
        type: "POST",
        url: "/StockOpname/OpenStockOpname",
        dataType: "json",
        data: { "tenanId": this.tenanId },
        success: function (data) {
            if (data.ok === true)
                GetStockOpnameId(data.opnameid);
            else
                alert(data.errormessage);
        },
        error: function (xhr, err) {
            alert(err);
        }          
       
    });
}

function GetStockOpnameId(id) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/GetStockOpenOpnameId",
        dataType: "json",
        data: { "id": id },
        success: function (data) {
            if (data != null) {
                hideListStockOpname();
                updateHeaderChanged(data);
                showDetailStockOpname();
            }
            else {
                setTimeout(GetStockOpnameId(id), 3000);
            }
        }
    });
}

function updateHeaderChanged(data) {
    var Dates = $.datepicker.formatDate('dd M yy', new Date(parseInt(data.OpenDate.replace(/\/Date\((-?\d+)\)\//, '$1'))));
    var closeDates = $.datepicker.formatDate('dd M yy', new Date(parseInt(data.CloseDate.replace(/\/Date\((-?\d+)\)\//, '$1'))));
    $("#OpnameId").val(data.Id);
    $("#OpnameNumber").val(data.OpnameNumber);
    $("#_id").val(data.Id);
    $("#OpenDate").val(Dates);
    $("#statusOpname").text(data.Status);
    if (data.Status == "Open") {
        $("#CloseDate").val("");
    }
    else {
        $("#CloseDate").val(closeDates);
    }
    $("#Storeman").val(data.Username);

}
function loadDataStockOpname(tenanId) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/LoadDataStockOpnameByCurrenDate",
        data: { "tenanId": tenanId },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //getOpnameByStatusOpen(data[0].TenanId);
                showListStockOpname();
                listStockOpnameView(data);
            }
        }
    });
}

function listStockOpnameView(data) {
    
    $.each(data, function (item) {
        var status = data[item].Status
        var id = data[item].Id;
        var Dates = $.datepicker.formatDate('dd M yy', new Date(parseInt(data[item].OpenDate.replace(/\/Date\((-?\d+)\)\//, '$1'))))
        $("#tblListInvoice tbody").append("<tr id=tr_" + data[item].Id + " onmouseover=ShowDelete('" + data[item].Id + "') onmouseout=HideDelete('" + data[item].Id + "') >" +
                    "<td style='text-align:left'><span class='linkStockOpnameList' onclick=ShowDetaiInvoice('" + data[item].Id + "')>" + data[item].OpnameNumber + "</span></td>" +
                    "<td>" + Dates + "</td>" +
                    "<td>" + data[item].Username + "</td>" +
                    "<td id='status' style='text-align:right'>" + data[item].Status + "</td>" +
                    "<td></td>" +
                    //"<td width=10px; style='color:red;cursor:pointer;text-align:center' onmouseover=ShowDelete('" + data.Id + "')  onmouseout=HideDelete('" + data.Id + "') ><span id=delete_" + data.Id + " onclick=onDelete('" + data.Id + "') style='color:red;cursor:pointer;text-align:center;display:none'>X</span></td>" +
                "</tr>");
        CekStatus(status, id);
    });
}

function ShowDelete(id) {
    if ($("#tr_" + id + " td[id='status']").text() == "Approved") {
        $("#delete_" + id).hide();
        $("#delete_" + id).css("text-align", "center");
    }
    else {
        $("#delete_" + id).show();
        $("#delete_" + id).css("text-align", "center");
    }
}

function HideDelete(id) {
    $("#delete_" + id).hide();
    $("#delete_" + id).css("text-align", "center");
}

function onDelete(id) {
    $("#dialogSuccesDelete").show();
    opnameId = id;
}

$("#bottonCancel").click(function () {
    $("#dialogSuccesDelete").hide();
});

$("#bottonOk").click(function () {
    var id = opnameId;
    $.ajax({
        type: "GET",
        url: "/StockOpname/Delete",
        data: { "id": id },
        dataType: "json",
        success: DeleteSuccess
    });
});

function DeleteSuccess(id) {
    $("#dialogSuccesDelete").hide();
    $("#tr_" + id).remove();
}

function viewDetailStockOpname(code) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/LoadDataStockOpnameItem",
        dataType: "json",
        data: { "code": code },
        success: viewDetailStockOpnameHeaderandDetail
    });
}

function showEditDialog(id) {
    $("#validateTips").hide();
    $("#validateTips").text("");
    var options = {
        autoOpen: true,
        height: 300,
        width: 410,
        modal: true,
        buttons: {
            "Update": function () {
                if ($("#vQty").val() == "" || isNaN($("#vQty").val())) {
                    $("#validateTips").show();
                    $("#validateTips").text("Qty tidak valid !!");

                } else {
                    EditStockOpnameItem(id);
                    $(this).dialog("close");
                }

            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    };
    $("#editQtyDialog").dialog(options);
}

function showAdjustQtyDialog(id) {
    $("#validateTips").hide();
    $("#validateTips").text("");
    var options = {
        autoOpen: true,
        height: 300,
        width: 410,
        modal: true,
        buttons: {
            "Update": function () {
                if ($("#adjQty").val() == "" || isNaN($("#adjQty").val())) {
                    $("#validateTips").show();
                    $("#validateTips").text("adjQty tidak valid !!");

                } else {
                    EditStockOpnameItem(id);
                    $(this).dialog("close");
                }

            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    };
    $("#adjQtyDialog").dialog(options);
}

var SetCloseDateOpname = function () {
    var now = new Date();
    var date = now.format("dd MMM yyyy");
    $("#CloseDate").val(date);
}

var dates = $("#OpenDate").datepicker({ dateFormat: 'yy-mm-dd',
    defaultDate: "+1w",
    gotoCurrent: true,
    changeMonth: true,
    numberOfMonths: 1,
    onSelect: function (selectedDate) {
        var option = this.id == "OpenDate" ? "minDate" : "maxDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
        dates.not(this).datepicker("option", option, date);
        var date = $("#OpenDate").val();

    }
});

var hideListStockOpname = function () { $("#tbllistinvoice").hide(); }
var hideDetailStockOpname = function () { $("#containerDetailInvoice").hide(); }
var showListStockOpname = function () { $("#tbllistinvoice").show(); }
var showDetailStockOpname = function () { $("#containerDetailInvoice").show(); }

function AdvanceSearchProduct(tenanId) {

    ClearandHideDialogBarcode();
    $("#AdvancedSearchProduct").dialog({
        autoOpen: false,
        width: 1000,
        height: 450,
        modal: true,
        close:
                function () {
                    $("#id_search").val("");
                }
    });

    $("#AdvSearch").click(function () {
        $("#tbodyProduct").empty();
        $.ajax({
            type: "GET",
            url: "/StockOpname/InitialSearchProduct",
            dataType: "json",
            data: { 'tenanId': tenanId },
            beforeSend: LoadingStart,
            complete: LoadingEnd,
            success: InsertProductToTable
        });
        $("#AdvancedSearchProduct").dialog("open");
    });

    $("#id_search").keyup(function () {
        var value = $("#id_search").val();
        $.ajax({
            type: "GET",
            url: "/StockOpname/SearchProductByName",
            data: { "tenanId": tenanId, 'name': value },
            dataType: "json",
            beforeSend: LoadingStart,
            complete: LoadingEnd,
            success: InsertProductToTable
        });
    });
}

function InsertProductToTable(data) {
    var hargaJual;
    var i = 0;
    $("#tbodyProduct").empty();
    $.each(data, function (item) {
        if (data[item].CcyName == "US Dollar") {
            hargaJual = "$ " + String.format("{0:c}", data[item].HargaJual);
        }
        else {
            hargaJual = "Rp. " + FormatCurrency(data[item].HargaJual);
        }
        $("#tbodyProduct").append("<tr class='Product' onclick='Search(\"" + data[item].Kode + "\")'>" +
                                          "<td>" + data[item].Kode + "</td>" +
                                          "<td>" + data[item].Barcode + "</td>" +
                                          "<td>" + data[item].GroupName + "</td>" +
                                          "<td>" + data[item].Nama + "</td>" +
                                          "<td align='right'>" + hargaJual + "</td></tr>");
    });
}

function LoadingStart() {
    $("#loading").show();
}

function LoadingEnd() {
    $("#loading").hide();
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

function Search(id) {
    $.ajax({
        type: "GET",
        url: "/StockOpname/FindProductByCodeOrBarcode",
        data: { 'tenanId': this.tenanId, 'code': id },
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        success: InsertNameAndQtyForEdit
    });
    $("#AdvancedSearchProduct").dialog("close");
}

function SearchBarcode() {
    var id = $("#Barcode").val();
    $.ajax({
        type: "GET",
        url: "/StockOpname/FindProductByCodeOrBarcode",
        data: { 'tenanId': this.tenanId, 'code': id },
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        success: InsertNameAndQtyForEdit
    });
    $("#AdvancedSearchProduct").dialog("close");
}

function InsertNameAndQtyForEdit(data) {
    ClearandHideDialogBarcode();
    ClearandHideDialogProductQty();
    if (data.length == 0) {
        // DialogError();
    } else {
        $("#error").remove();
        ShowDialogProductQty();
        $("#PartName").text(data[0].Nama);
        $("#PartId").val(data[0].ProductId);
        $("#PartGroupId").val(data[0].GroupId);
        $("#PartGuid").val(data[0].ModelGuid);
        $("#Code").val(data[0].Kode);
    }
}

function Cancel() {
    ClearandHideDialogProductQty();
    ClearandHideDialogBarcode();
}

function ClearandHideDialogProductQty() {
    $("#productQtyDialog").hide();
    $("#PartName").empty();
    $("#Code").val("");
    $("#Qty").val("");
    $("#Qty").select();
}

function ShowDialogProductQty() {
    $("#productQtyDialog").show();
    $("#PartName").empty();
    $("#Code").val("");
    $("#Qty").val("");
    $("#Qty").select();
}

function ClearandHideDialogBarcode() {
    $("#BarcodeDialog").hide();
    $("#Barcode").val("");
    $("#Barcode").select();
}

function ShowDialogBarcode() {
    $("#BarcodeDialog").show();
    $("#Barcode").val("");
    $("#Barcode").select();
}

$("#btnBarcode").keyup(function (event) {
    if (event.keyCode == 13) {
        SearchBarcode();
    }
});

$("#Barcode").keyup(function (event) {
    if (event.keyCode == 13) {
        SearchBarcode();
    }
});

$("#btnAddNew").keyup(function (event) {
    if (event.keyCode == 13) {
        AddStockOpnameItem();
    }
});

$("#Qty").keyup(function (event) {
    if (event.keyCode == 13) {
        AddStockOpnameItem();
    }
});

function ViewEditItem(data) {
    ClearandHideDialogProductQty();
    var datatr = $("#listitem > tbody");

    $("#listitem tbody tr[id='" + data.Id + "']").replaceWith("<tr id=" + data.Id + " onmouseover=showDeleteItem('" + data.Id + "')  onmouseout=hideDeleteItem('" + data.Id + "')  >" +
                        "<td>" + data.Barcode + "</td>" +
                    "<td>" + data.PartCode + "</td>" +
                    "<td id=PartName_" + data.Id + ">" + data.PartName + "</td>" +
                    "<td id=tdQty_" + data.Id + " style='text-align:center;cursor:pointer'  onmouseover=showEditQty('" + data.Id + "') onmouseout=hideEditQty('" + data.Id + "')><span id='Qty_" + data.Id + "' class='value'>" + data.Qty + "<span id=editQty_" + data.Id + " onclick=editQtyDialog('" + data.Id + "') class='editlogo' ></span></span></td>" +
                    "<td id=tdAdjust_" + data.Id + " style='text-align:center'><span id='SysQty_" + data.Id + "' class='value'>" + data.SysQty + "</span></td>" +
                    "<td id=tdAdjust_" + data.Id + " style='text-align:center'><span id='DiffQty_" + data.Id + "' class='value'>" + data.DiffQty + "</span></td>" +
    //"<td id=tdAdjust_" + data.Id + " style='text-align:center;cursor:pointer'  onmouseover=showAdjQty('" + data.Id + "') onmouseout=hideAdjQty('" + data.Id + "')><span id='AdjQty_" + data.Id + "' class='value'>" + data.AdjQty + "<span id=editAdjQty_" + data.Id + " onclick=editAdjQtyDialog('" + data.Id + "') class='editlogo' ></span></span></td>" +
                    "<td style='text-align:center;cursor:pointer;color:red'  onmouseover=showDeleteItem('" + data.Id + "')  onmouseout=hideDeleteItem('" + data.Id + "')><span id='deleteItem_" + data.Id + "' onclick=deleteItem('" + data.Id + "') style='display:none' class='deletelogo'>X</span></td>" +
       "</tr>");

}

function ViewAddItem(data) {
    ClearandHideDialogProductQty();
    var datatr = $("#listitem > tbody");
    $("#listitem tbody tr[id='" + data.Id + "']").remove();
    $("#listitem tbody").append("<tr id=" + data.Id + " onmouseover=showDeleteItem('" + data.Id + "')  onmouseout=hideDeleteItem('" + data.Id + "')  >" +
                    "<td>" + data.Barcode + "</td>" +
                    "<td>" + data.PartCode + "</td>" +
                    "<td id=PartName_" + data.Id + ">" + data.PartName + "</td>" +
                    "<td id=tdQty_" + data.Id + " style='text-align:center;cursor:pointer'  onmouseover=showEditQty('" + data.Id + "') onmouseout=hideEditQty('" + data.Id + "')><span id='Qty_" + data.Id + "' class='value'>" + data.Qty + "<span id=editQty_" + data.Id + " onclick=editQtyDialog('" + data.Id + "') class='editlogo' ></span></span></td>" +
                    "<td id=tdAdjust_" + data.Id + " style='text-align:center'><span id='SysQty_" + data.Id + "' class='value'>" + data.SysQty + "</span></td>" +
                    "<td id=tdAdjust_" + data.Id + " style='text-align:center'><span id='DiffQty_" + data.Id + "' class='value'>" + data.DiffQty + "</span></td>" +
    //"<td id=tdAdjust_" + data.Id + " style='text-align:center;cursor:pointer'  onmouseover=showAdjQty('" + data.Id + "') onmouseout=hideAdjQty('" + data.Id + "')><span id='AdjQty_" + data.Id + "' class='value'>" + data.AdjQty + "<span id=editAdjQty_" + data.Id + " onclick=editAdjQtyDialog('" + data.Id + "') class='editlogo' ></span></span></td>" +
                    "<td style='text-align:center;cursor:pointer;color:red'  onmouseover=showDeleteItem('" + data.Id + "')  onmouseout=hideDeleteItem('" + data.Id + "')><span id='deleteItem_" + data.Id + "' onclick=deleteItem('" + data.Id + "') style='display:none' class='deletelogo'>X</span></td>" +
   "</tr>");
}

function renderStockOpnameItemToList(data) {
    $("#listitem > tbody").empty();
    if (data.length != 0) {
        $.each(data, function (index) {          
            $("#listitem tbody").append("<tr id=" + data[index].Id + " onmouseover=showDeleteItem('" + data[index].Id + "')  onmouseout=hideDeleteItem('" + data[index].Id + "')  >" +
                    "<td>" + data[index].Barcode + "</td>" +
                    "<td>" + data[index].PartCode + "</td>" +
                    "<td id=PartName_" + data[index].Id + ">" + data[index].PartName + "</td>" +
                    "<td id=tdQty_" + data[index].Id + " style='text-align:center;cursor:pointer'  onmouseover=showEditQty('" + data[index].Id + "') onmouseout=hideEditQty('" + data[index].Id + "')><span id='Qty_" + data[index].Id + "' class='value'>" + data[index].Qty + "<span id=editQty_" + data[index].Id + " onclick=editQtyDialog('" + data[index].Id + "') class='editlogo' ></span></span></td>" +
                    "<td style='text-align:center;'>" + data[index].SysQty + "</td>" +
                    "<td style='text-align:center;'>" + data[index].DiffQty + "</td>" +
                    "<td style='text-align:center;cursor:pointer;color:red'  onmouseover=showDeleteItem('" + data[index].Id + "')  onmouseout=hideDeleteItem('" + data[index].Id + "')><span id='deleteItem_" + data[index].Id + "' onclick=deleteItem('" + data[index].Id + "') style='display:none' class='deletelogo' >X</span></td>" +
                    "</tr>");
        });
    }
}

function CekStatus(status,id) {
    if (status == "Open") {
        $("#tr_"+id+">#status").css("color", "red");
    }
}
function showEditQty(id) {
    if (statusopname != "Closed")
        $("#editQty_" + id).show();
}

function editQtyDialog(id) {
    var qty = $("span#Qty_" + id).text().trim();
    var PartName = $("#PartName_" + id).text().trim();
    $("#vQty").val(qty);
    $("#ProductName").val(PartName);
    showEditDialog(id);
}

function hideEditQty(id) {
    $("#editQty_" + id).hide();
    //    $("#editQty" + index).css("text-align", "right");
}

function hideDeleteItem(id) {
    $("#deleteItem_" + id).hide();
    //$("#editQty_" + id).hide();
}

function showDeleteItem(id) {
    //$("#editQty_" + id).show();    
    if (statusopname != "Closed")
        $("#deleteItem_" + id).show();
}

function deleteItem(id) {
    $("#dialogConfirmDelete").show();
    opnameItemId = id;
}

$("#btnItemOk").click(function () {
    var id = opnameItemId;
    var opnameId = $("#OpnameId").val();
    $.ajax({
        type: "GET",
        url: "/StockOpname/DeleteItem",
        data: { 'id': id, 'opnameId': opnameId },
        dataType: "json",
        beforeSend: LoadingStart,
        complete: LoadingEnd,
        success: function (Id) {
            removeDeletetr(Id)
        }
    });
});

$("#btnItemCancel").click(function () {
    $("#dialogConfirmDelete").hide();
});

function removeDeletetr(Id) {
    $("#dialogConfirmDelete").hide();
    var tr = $("#deleteItem_" + Id);
    $("#deleteItem_" + Id).parent().parent().remove();
}

//Format Date

String.prototype.toFormatDate = function (format) {
    var dateFormated = new Date(parseInt(this.replace(/\/Date\((-?\d+)\)\//, '$1')));
    return dateFormated.format(format);
}

String.prototype.toDefaultFormatDate = function () {
    var dateFormated = new Date(parseInt(this.replace(/\/Date\((-?\d+)\)\//, '$1')));
    var day = dateFormated.getDate();
    var month = dateFormated.getMonth();
    var year = dateFormated.getFullYear();
    return day + " " + ConvertMonthToIndonesian(month) + " " + year;
}

var ConvertMonthToIndonesian = function (month) {
    var bulan = new Array("Januari",
    "Februari", "Maret", "April",
    "Mei", "Juni", "Juli",
    "Agustus", "September",
    "Oktober", "November",
    "Desember");

    return bulan[month];
}

function ShowDetaiInvoice(id) {
    stockOpnameId = id;
    $("#OpnameId").val(id);
    $("#OpnameNumber").val(id);
    $.ajax({
        type: "GET",
        url: "/StockOpname/FindStockOpnameById",
        dataType: "json",
        data: { "id": id },
        async: false,
        success: function (data) {
            if (data != null) {                
                statusopname = data.Status;
                updateHeaderChanged(data);
                hideListStockOpname();
                showDetailStockOpname();
                renderStockOpnameItemToList(data.Items);
                ShowHideAction(data.Status);             
            }
        }
    });
}

function ShowHideAction(status) {
    if (status == "Closed") {
        $("#summary").hide();
        $(".editlogo").remove();
        $(".deletelogo").remove();
        $("#statusOpname").text(status);
        $("#SubMenuDetailInvoiceClose").hide();
    } else {
        $("#summary").show();       
        $("#statusOpname").text(status);
        $("#SubMenuDetailInvoiceClose").show();

    }
}

function PrintOpname() {
    $.ajax({
        type: "GET",
        url: "/StockOpname/CreateReportOpname",
        dataType: "json",
        async: false,
        success: function (data) {
        }
    });
}

$("#CloseOpname").click(function () {
    var opnameid = $("#OpnameId").val().trim();
    var tenanid = $("#tenanId").text().trim()
    var opnamenumber = $("#OpnameNumber").val().trim()
    var approval1 = $("#approval1").val().trim();
    var approval2 = $("#approval2").val().trim();
    var approval3 = $("#approval3").val().trim();
    var opnamenote = $("#opnamenote").val().trim();
    $.ajax({
        type: "POST",
        url: "/StockOpname/CloseStockOpname",
        data: { 'opnameid': opnameid, 'tenanid': tenanid, 'opnamenumber': opnamenumber, 'approval1': approval1, 'approval2': approval2, 'approval3': approval3, 'opnamenote': opnamenote },
        dataType: "json",
        success: CloseOpnameSuccess
    });
});

function ClearStockOpnameClose() {
    $("#approval1").val("");
    $("#approval2").val("");
    $("#approval3").val("");
    $("#opnamenote").val("");
}

function CloseOpnameSuccess(data) {
    $("#SubMenuDetailInvoiceClose").hide();
    SetCloseDateOpname();
    ShowHideAction("Closed");
    ClearStockOpnameClose();
    $(".ModalDialogStockOpnameClose").hide();
}

function GetOpnameItem(id) {
    var result;
    $.ajax({
        type: "GET",
        url: "/StockOpname/CoutOpnameItemById",
        data: { 'id': id },
        async: false,
        dataType: "json",
        success: function (data) {
            result = data;
        }
    });
    return result;
}

$("#CancelOpname").click(function () {
    ClearStockOpnameClose();
    $(".ModalDialogStockOpnameClose").hide();
});

$("#SubMenuDetailInvoiceClose").click(function () {
    var countopnameitem = GetOpnameItem($("#OpnameId").val().trim());
    if (countopnameitem.length > 0) {
        ClearStockOpnameClose();
        $(".ModalDialogStockOpnameClose").show();
    }
});
