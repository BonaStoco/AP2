$(document).ready(
function () {
//    $("#kirimbaranglink").hide();
//    $("#DivImport").hide();
    $("#resultForEdit").hide();
    $("#DialogEdit").dialog({
        autoOpen: false,
        width: 450,
        height: 250,
        modal: true,
        buttons:
                {
                    "Update": function () {
                        UpdateItem();
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
    });
    $.ajax({
        type: "GET",
        url: "/ReturBarang/GetListReturBarang",
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertDataToTable
    });
    $("#txtCari").keyup(function (event) {
        if (event.keyCode == 13) {
            Cari();
        }
    });
    $("#resultForEdit").keyup(function (event) {
        if (event.keyCode == 13) {
            AddNewItem();
        }
    });
    $("#Import").click(function () {
        $("#divItem").empty();
        $("#resultForEdit").empty();
        $("#DivImport").show();
    });
    $("#BtnImport").click(function () {
        document.getElementById("dialog-overlay").style.display = "inline";
    });
    $("#ErrorOK").click(function () {
        $("#error").remove();
    });
});
function InsertDataToTable(data) {
    if (data == null || data.length == 0)
        return null;
    $("#divItem").empty();
    CreateTableItem();
    $("#DivImport").hide();
    $("#grnitem-" + data.Id).empty();
    $.each(data, function (item) {
        $("#ListProduct").append("<tr id='grnitem-" + data[item].Id + "'>" +
                        "<td class='center'>" + data[item].Barcode + "</td> " +
                        "<td class='center'>" + data[item].NamaBarang + "</td>" +
                        "<td class='right'>" + data[item].Qty + "</td>" +
                        "<td class='left'>" + data[item].Unit + "</td>" +
                        "<td class='right'>" + data[item].Harga + "</td>" +
                        "<td class='right'>" + data[item].Jumlah + "</td>" +
                        "<td class='center'>&nbsp&nbsp&nbsp&nbsp<span onclick='Edit(" + data[item].Id + ")' class='pointer'><img src='../../Content/images/edit.png' /></span>&nbsp &nbsp" +
                        "<span onclick='Delete(" + data[item].Id + ")' class='pointer'><img src='../../Content/images/delete-icon.png' /></span></td>" +
                        "</tr>");
    });
    $("#kirimbaranglink").show();
    $("#txtCari").focus();
}
function Cari() {
    var id = document.getElementById("txtCari").value;
    $.ajax({
        type: "GET",
        url: "/ReturBarang/CariBarangUntukGRNBerdasarkanKode",
        data: { 'code': id },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertNameAndQtyForEdit
    });
    $("#txtCari").val("");
}
function InsertDataToListProduct(data) {
    if (data == null)
        return alert("Data Tidak Ditemukan");
    if (!($("#divItem").html()))
        CreateTableItem();
    $("#resultForEdit").empty();
    $("#grnitem-" + data.Id).remove();
    $("#ListProduct").append("<tr id='grnitem-" + data.Id + "'>" +
                        "<td class='center'>" + data.Barcode + "</td> " +
                        "<td class='center'>" + data.NamaBarang + "</td>" +
                        "<td class='right'>" + data.Qty + "</td>" +
                        "<td class='left'>" + data.Unit + "</td>" +
                        "<td class='right'>" + data.Harga + "</td>" +
                        "<td class='right'>" + data.Jumlah + "</td>" +
                        "<td class='center'>&nbsp&nbsp&nbsp&nbsp<span onclick='Edit(" + data.Id + ")' class='pointer'><img src='../../Content/images/edit.png' /></span>&nbsp &nbsp" +
                        "<span onclick='Delete(" + data.Id + ")' class='pointer'><img src='../../Content/images/delete-icon.png' /></span></td>" +
                        "</tr>");
    $("#kirimbaranglink").show();
    $("#txtCari").focus();
}

function InsertNameAndQtyForEdit(data) {
    if (data == null)
        return alert("Data Tidak Ditemukan");
    $("#error").remove();
    $("#DivImport").hide();
    $("#resultForEdit").empty();
    $("#resultForEdit").show();
    $("#resultForEdit").append("<tr><td><label>Nama Barang</label></td><td colspan='3'><label id='lblname'>" + data[0].Nama + "</label></td></tr>" +
                        "<tr><td><label>Qty</label></td><td><input type='text' id='Qty'/></td>" +
                        "<td><input type='hidden' id='id-edit' value='" + data[0].Kode + "'/></td>" +
                        "<td><button class='positive button' onclick='AddNewItem()'><img src='../../Content/images/button/search.png' />Enter</button></td>" +
                        "<td><button class='positive button' onclick='Cancel()'><img src='../../Content/images/button/search.png' />Cancel</button></td></tr>");
    $("#Qty").select();
}

function Search(id) {
    $.ajax({
        type: "GET",
        url: "/ReturBarang/CariBarangUntukGRNBerdasarkanKode",
        data: { 'code': id },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertNameAndQtyForEdit
    });
    $("#AdvancedSearchProduct").dialog("close");
}

function Edit(Id) {
    $.ajax({
        type: "GET",
        url: "/ReturBarang/Edit",
        data: { 'id': Id },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: ShowDialogEdit
    });
}
function ShowDialogEdit(data) {
    $("#DialogEdit").empty();
    $("#DialogEdit").append("<table><tbody><tr>" +
                    "<td>Nama Barang</td><td>" + data.NamaBarang +
                    "<input type='hidden' id='id-edit' value='" + data.Id + "'/></td>" +
                    "</tr><tr><td>Qty</td><td>" +
                    "<input type='text' id='Qty' value='" + data.Qty + "'/>" + data.Unit +
                    "</td></tr></tbody></table>");
    $("#DialogEdit").dialog("open");
}
function AddNewItem() {
    var id = $("#id-edit").val();
    var qty = $("#Qty").val();
    $.ajax({
        type: "POST",
        url: "/ReturBarang/AddItemToListPengiriman",
        data: { 'code': id, 'qty': qty },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: UpdateSuccess
    });
    $("#id-edit").val("");
    $("#Qty").val("");
}

function UpdateItem() {
    var id = $("#id-edit").val();
    var qty = $("#Qty").val();
    $.ajax({
        type: "POST",
        url: "/ReturBarang/UpdateItem",
        data: { 'id': id, 'Qty': qty },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: UpdateSuccess
    });
    $("#id-edit").val("");
    $("#Qty").val("");
}
function Delete(Id) {
    $.ajax({
        type: "POST",
        url: "/ReturBarang/DeleteItem",
        data: { 'id': Id },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: DeleteSuccess
    });
}
function DeleteSuccess(data) {
    $("#grnitem-" + data.Id).remove();
    var countListProduct = document.getElementById("ListProduct").rows.length;
    if (countListProduct == 0) {
        $("#kirimbaranglink").hide();
        $("#divItem").empty();
        $("#Import").show();
        return;
    }
    alert("Barang berhasil di Hapus");
}
function UpdateSuccess(data) {
    InsertDataToListProduct(data);
    $("#DialogEdit").dialog("close");
}
function PogressStart() {
    $("#Pogress").show();
}
function PogressEnd() {
    $("#Pogress").hide();
}

function DeleteAll() {
    $.ajax({
        type: "POST",
        url: "/ReturBarang/DeleteAllItem",
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: DeleteAllItemSuccess
    });
}

function DeleteAllItemSuccess() {
    $("#divItem").empty();
    $("#kirimbaranglink").hide();
    $("#Import").show();
}
function CreateTableItem() {
    $("#divItem").append("<table  style='clear:both'><thead><tr>" +
                        "<th class='center'>Barcode</th>" +
                        "<th class='center'>Nama Barang</th>" +
                        "<th class='center'>Qty</th>" +
                        "<th class='center'>Unit</th>" +
                        "<th class='center'>Harga</th>" +
                        "<th class='center'>Jumlah</th>" +
                        "<th class='center'><span onclick='DeleteAll()' id='deleteall'>Hapus Semua</span></th>" +
                        "</tr></thead><tbody id='ListProduct'></tbody></table>");
    $("#Import").hide();
}
function Cancel() {
    $("#resultForEdit").empty();
    $("#resultForEdit").hide();
}