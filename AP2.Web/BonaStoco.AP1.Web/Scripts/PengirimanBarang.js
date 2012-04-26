var valtxtcari;
$(document).ready(
function () {
    //    $("#kirimbaranglink").hide();
    //    $("#DivImport").hide();
    $("#resultForEdit").hide();
    $("#createNewProduct").hide();
    $("#dialogError").hide();
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
        url: "/PengirimanBarang/GetListPengirimanBarang",
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
        $("#createNewProduct").empty();
        $("#dialogError").empty();
        $("#DivImport").show();
    });
    $("#BtnImport").click(function () {
        document.getElementById("dialog-overlay").style.display = "inline";
    });
    $("#ErrorOK").click(function () {
        $("#error").remove();
    });
    $("#txtCari").focus();
});
function InsertDataToTable(data) {
    if (data == null || data.length == 0)
        return null;
    $("#divItem").empty();
    CreateTableItem();
    $("#DivImport").hide();
    $("#grnitem-" + data.Id).empty();
    $.each(data, function (item) {
        var statusBarang = (data[item].ProductId == 0) ? "style ='background:transparent url(../../Content/images/new.png) center right no-repeat'" : "";
        $("#ListProduct").append("<tr id='grnitem-" + data[item].Id + "'>" +
                        "<td class='center'>" + data[item].Barcode + "</td> " +
                        "<td class='center' " + statusBarang + ">" + data[item].NamaBarang + "</td>" +
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
    valtxtcari = id;
    if (id == "")
        return alert("Barcode Atau Code Harus Di isi");
    $.ajax({
        type: "GET",
        url: "/PengirimanBarang/CariBarangUntukGRNBerdasarkanKode",
        data: { 'code': id },
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertNameAndQtyForEdit
    });
    $("#txtCari").val("");
    $("#txtCari").blur();
    
}
function InsertDataToListProduct(data) {
    if (data == null)
        return alert("Data Tidak Ditemukan");
    if (!($("#divItem").html()))
        CreateTableItem();
    $("#resultForEdit").empty();
    $("#createNewProduct").empty();
    $("#dialogError").empty();
    $("#grnitem-" + data.Id).remove();
    var statusBarang = (data.ProductId == 0) ? "style ='background:transparent url(../../Content/images/new.png) center right no-repeat'" : "";
    $("#ListProduct").append("<tr id='grnitem-" + data.Id + "'>" +
                        "<td class='center'>" + data.Barcode + "</td> " +
                        "<td class='center' " + statusBarang + ">" + data.NamaBarang + "</td>" +
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
    if (data.length == 0) {
        DialogError();
    }
    else if(data[0].StatusProduct==false){
        DialogDiskontinyu(data.Nama);
    } 
    else {
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
}

function DialogError() {
    $("#dialogError").empty();
    $("#dialogError").show();
    $("#dialogError").append("<tr><td><img src='../../Content/images/button/warning.png' /></td><td><label><span style='color:red'>Kode Atau Barcode ("+ valtxtcari+") Tidak Ditemukan. Apakah Anda Ingin Mengangapnya Sebagai barang baru</span></label></td></tr>" +
                             "<tr><td></td>" +
                             "<td><button id = 'btnDialogError' class='positive button' onclick='OKError()'><img src='../../Content/images/button/search.png' />Enter</button><button class='positive button' onclick='CancelError()'><img src='../../Content/images/button/search.png' />Cancel</button></td></tr>");
    $("#btnDialogError").focus();
}

function DialogDiskontinyu(nama) {
    $("#dialogError").empty();
    $("#dialogError").show();
    $("#dialogError").append("<tr><td><img src='../../Content/images/button/warning.png' /></td><td><label><span style='color:red'>Barang (" + nama + ") sudah diskontinyu.</span></label></td></tr>" +
                             "<tr><td></td>" +
                             "<td><button class='positive button' onclick='CancelError()'><img src='../../Content/images/button/search.png' />Ok</button></td></tr>");
    $("#btnDialogError").focus();
}

function getPartgroup() {
    $.ajax({
        type: "GET",
        url: "/PengirimanBarang/GetPartGroup",
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertDataToComboBoxPartGroup
    });
}

function InsertDataToComboBoxPartGroup(data) {
    $("#CMBPartGroup").append("<option value=''>...</option>");
    $.each(data, function (item) {
        $("#CMBPartGroup").append("<option value='" + data[item].PartGroupId + "'>" + data[item].GroupName + "</option>");
    });
}

function getCcy() {
    $.ajax({
        type: "GET",
        url: "/PengirimanBarang/GetCcy",
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertDataToComboBoxCcy
    });
}

function InsertDataToComboBoxCcy(data) {
    $("#CMBCcy").append("<option value=''>...</option>");
    $.each(data, function (item) {
        $("#CMBCcy").append("<option value='" + data[item].Ccyid + "'>" + data[item].CcyName + "</option>");
    });
}

function getUnit() {
    $.ajax({
        type: "GET",
        url: "/PengirimanBarang/GetUnit",
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertDataToComboBoxUnit
    });
}

function InsertDataToComboBoxUnit(data) {
    $("#CMBUnit").append("<option value=''>...</option>");
    $.each(data, function (item) {
        $("#CMBUnit").append("<option value='" + data[item].UnitId + "'>" + data[item].UnitName + "</option>");
    });
}

function CreateNewProduct() {
    getPartgroup();
    getUnit();
    getCcy();
    $("#createNewProduct").empty();
    $("#createNewProduct").show();
    $("#createNewProduct").append("<tr><td>Part Group</td><td><select id = 'CMBPartGroup'></select></td></tr>" +
                                  "<tr><td>Barcode</td><td><input type='text' id='barcode' value='" + valtxtcari + "'/>" +
                                  "<input type='checkbox' id='statusPrint'/> Cetak Barcode</td></tr>" +
                                  "<tr><td>Kode</td><td><input type='text' id='kode' value='" + valtxtcari + "'/></td></tr>" +
                                  "<tr><td>Nama Barang</td><td><input type='text' id='nama' /></td></tr>" +
                                  "<tr><td>Satuan</td><td><select id = 'CMBUnit'></select></td></tr>" +
                                  "<tr><td>Mata Uang</td><td><select id = 'CMBCcy'></select></td></tr>" +
                                  "<tr><td>Harga Beli</td><td><input type='text' id='beli' onkeypress='validate(event)'/></td></tr>" +
                                  "<tr><td>Harga Jual</td><td><input type='text' id='jual' onkeypress='validate(event)'/></td></tr>" +
                                  "<tr><td>Qty</td><td><input type='text' id='qty' onkeypress='validate(event)'/></td></tr>" +
                                  "<tr><td><button class='positive button' onclick='AddNewProduct()'><img src='../../Content/images/button/search.png' />Enter</button></td>" +
                                  "<td><button class='positive button' onclick='CancelForm()'><img src='../../Content/images/button/search.png' />Cancel</button></td></tr>");
    //$("#CMBPartGroup").select();
    $("#CMBPartGroup").focus();
}
function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function AddNewProduct() {
    var statusPrint

    if ($.trim($("#CMBPartGroup").val()).length < 1) {
        alert("Part Group harus diisi");
        $("#CMBPartGroup").focus();
        return false;
    }

    if ($.trim($("#barcode").val()).length < 1) {
        alert("Barcode harus diisi");
        $("#barcode").focus();
        return false;
    }

    if ($.trim($("#kode").val()).length < 1) {
        alert("Kode harus diisi");
        $("#kode").focus();
        return false;
    }

    if ($.trim($("#nama").val()).length < 1) {
        alert("Nama harus diisi");
        $("#nama").focus();
        return false;
    }

    if ($.trim($("#CMBUnit").val()).length < 1) {
        alert("Satuan harus diisi");
        $("#CMBUnit").focus();
        return false;
    }     

    if ($.trim($("#CMBCcy").val()).length < 1) {
        alert("Mata Uang harus diisi");
        $("#CMBCcy").focus();
        return false;
    }

    if ($.trim($("#beli").val()).length < 1) {
        alert("Harga Beli harus diisi");
        $("#barcode").focus();
        return false;
    }

    if ($.trim($("#jual").val()).length < 1) {
        alert("Harga Jual harus diisi");
        $("#barcode").focus();
        return false;
    }

    if ($.trim($("#qty").val()).length < 1) {
        alert("Qty harus diisi");
        $("#qty").focus();
        return false;
    }
    
    if ($("#statusPrint").is(':checked')) {
        statusPrint = true;
    }
    else {
        statusPrint = false;
    }
    var NewProduct = new Object();
    NewProduct.PartGroup = $("#CMBPartGroup").val();
    NewProduct.PartGroupName = $("#CMBPartGroup option:selected").text();
    NewProduct.Barcode = $("#barcode").val();
    NewProduct.Kode = $("#kode").val();
    NewProduct.NamaBArang = $("#nama").val();
    NewProduct.CcyId = $("#CMBCcy").val();
    NewProduct.CcyName = $("#CMBCcy option:selected").text();
    NewProduct.UnitId = $("#CMBUnit").val();
    NewProduct.UnitName = $("#CMBUnit option:selected").text();
    NewProduct.HargaBeli = $("#beli").val();
    NewProduct.HargaJual = $("#jual").val();
    NewProduct.Qty = $("#qty").val();
    NewProduct.statusPrint = statusPrint;
   
    $.ajax({
        type: "POST",
        url: "/PengirimanBarang/CreateNewProduct",
        data:{'NewProduct' : JSON.stringify(NewProduct)},
        dataType: "json",
        beforeSend: PogressStart,
        complete: PogressEnd,
        success: InsertSuccess
    })
}

function Search(id) {
    $.ajax({
        type: "GET",
        url: "/PengirimanBarang/CariBarangUntukGRNBerdasarkanKode",
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
        url: "/PengirimanBarang/Edit",
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
        url: "/PengirimanBarang/AddItemToListPengiriman",
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
        url: "/PengirimanBarang/UpdateItem",
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
        url: "/PengirimanBarang/DeleteItem",
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
function InsertSuccess(data) {
    InsertDataToListProduct(data);
    $("#createNewProduct").hide();
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
        url: "/PengirimanBarang/DeleteAllItem",
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
                        "<th class='center'>Harga Jual</th>" +
                        "<th class='center'>Jumlah</th>" +
                        "<th class='center'><span onclick='DeleteAll()' id='deleteall'>Hapus Semua</span></th>" +
                        "</tr></thead><tbody id='ListProduct'></tbody></table>");
    $("#Import").hide();
}
function Cancel() {
    $("#resultForEdit").empty();
    $("#resultForEdit").hide();
}
function CancelForm() {
    $("#createNewProduct").empty();
    $("#createNewProduct").hide();
}

function CancelError() {
    $("#dialogError").empty();
    $("#dialogError").hide();
}

function OKError() {
    CreateNewProduct();
    $("#dialogError").empty();
    $("#dialogError").hide();
}