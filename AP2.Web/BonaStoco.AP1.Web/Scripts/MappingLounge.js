var tenanId;
var mappingId;
$("#loadingMapping").ajaxStart(function () { $(this).show() }).ajaxStop(function () { $(this).hide() });

$("#cariBtn").click(function () {
    tenanId = $("#tenanid").val();
    $.ajax({
        type:'GET',
        url: '/MappingPriceAp/GetListProduct',
        data : {'tenanId' : tenanId},
        dataType: 'json',
        success: InsertProductToList
    });
});

function GetMappingRepository(tenanId) {
    $.ajax({
        type: 'GET',
        url: '/MappingPriceAp/GetListMappingPrice',
        data: { 'tenanId': tenanId },
        dataType: 'json',
        success: InsertMappingItemList
    });
}

function InsertProductToList(data) {
    if (data.length == 0) {
        $("#errorMessage").show();
        $("#error").text("Kartu Kredit Telah Di Mapping Semua");
        return;
    }
    $("#DialogOverlayProduct").show();
    $(".divListproduct").show();
    $("table#tableListProduct tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#FFF";
        }
        else {
            color = "#e3ecff";
        }
        $("table#tableListProduct tbody").append("<tr style='background-color:" + color + "' id=tr" + data[item].ProductId + " onclick=showItem('" + data[item].ProductId + "')>" +
                                                           "<td>" + data[item].Nama + "</td>" +
                                                           "<td>" + data[item].Kode + "</td>" +
                                                           "<td style='text-align:right'>" + String.format("{0:c}", data[item].HargaJual) + "</td>" +
                                                           "</tr>");
    });
}

function InsertMappingItemList(data) {
    if (data.length == 0) {
        $("#divListItemMappingPirce").hide();
        return;
    }
    HideView();
    $("#divListItemMappingPirce").show();
    $("table#tableListProductTenanLounge tbody").empty();
    $.each(data, function (item) {
        var color;
        if (item % 2 == 0) {
            color = "White";
        }
        else {
            color = "Blue";
        }
        $("table#tableListProductTenanLounge tbody").append("<tr class="+ color +" id=tr" + data[item].Id + " onmouseover=showEditMappingPrice('" + data[item].Id + "')  onmouseout=hideEditItem('" + data[item].Id + "')>" +
                                                           "<td>" + data[item].Nama + "</td>" +
                                                           "<td>" + data[item].Kode + "</td>" +
                                                           "<td style='text-align:right;cursor:pointer; width:20%;' onmouseover=showEditMappingPrice('" + data[item].Id + "')  onmouseout=hideEditItem('" + data[item].Id + "')>"+ String.format("{0:c}",data[item].Price) +"</td>"+
                                                           "<td id='tdButton'><div id='DivButton" + data[item].Id + "' style='display:none'><div class='EditPrice'  onclick=showEditItem('" + data[item].Id + "')></div>" +
                                                           "<div class='delete' onclick=showDeleteItem('" + data[item].Id + "','" + data[item].TenanId + "')>X</div></div></td>" +
                                                           "</tr>");
    });
}

function hideEditItem(id) {
    $("#DivButton" + id).hide();
}

function showEditMappingPrice(id) {
    $("#DivButton" + id).show();
}
function LoadingStart() {
    $("#loading").show();
}

function LoadingEnd() {
    $("#loading").hide();
}
function showItem(id) {
    $(".divListproduct").hide();
    tenanId = $("#tenanid").val();
    $.ajax({
        type: "GET",
        url: "/MappingPriceAp/GetProductById",
        data: {"tenanId": tenanId, "id":id},
        datatype : "json",
        success: InsertToViewAdd
    });
    $("#DivDialogMappingPrice").show();
}

function showEditItem(id) {
    $.ajax({
        type: "GET",
        url: "/MappingPriceAp/GetMappingPriceById",
        data: { "id": id },
        datatype: "json",
        success: InsertToViewEdit
    });
    $("#DivDialogEditMappingPrice").show();
}

function showDeleteItem(id, tenanid) {
    mappingId = id;
    tenanId = tenanid;
    $("#DivDialogMessage").show();
}
function Cancel() {
    $("#DivDialogMessage").hide();
}
function Delete() {
    $.ajax({
        type: "POST",
        url: "/MappingPriceAp/DeleteMappingPrice",
        data: { "id": mappingId },
        datatype: "json",
        success: function (data) {
            if (data == "Ok") {
                GetMappingRepository(tenanId);
                $("#DivDialogMessage").hide();
                $("#errorMessage").show();
                $("#error").text("Kartu Kredit Telah Berhasil Di Hapus");
            }
            else {
                setTimeout(Delete(), 2000);
            }
        }
    });

}
function error() {
    $("#errorMessage").hide();
}
function InsertToViewAdd(data) {
    $("#tableAddDialog").html("<tr>" +
                                    "<td>Nama Barang</td>" +
                                    "<td>" +
                                        "<input type='text' id='tenanId' name='tenanId' hidden='hidden' value='" + tenanId + "' />" +
                                         "<input type='text' id='productguid' name='productid' hidden='hidden' value='" + data.ModelGuid + "' />" +
                                        "<input type='text' id='productid' name='productid' hidden='hidden' value='" + data.ProductId + "' />" +
                                        ": <input type='text' name='productName' value='" + data.Nama + "' readonly='readonly' />" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>Harga</td>" +
                                    "<td>" +
                                        ": <input type='text' id='harga' name='harga' value='" + String.format("{0:c}",data.HargaJual) + "'/>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td colspan='2'>" +
                                        "<div class='simpan' id='simpanMapping' onclick='SaveData()'>Simpan</div>" +
                                        "<div class='batal' id='batalMapping'onclick='HideView()'>Batal</div>" +
                                    "</td>" +
                                "</tr>");
}

function InsertToViewEdit(data) {
    $("#DialogOverlayProduct").show();
    $("#tableEditDialog").html("<tr>" +
                                    "<td>Nama Barang</td>" +
                                    "<td>" +
                                        "<input type='text' id='idUpdate' name='tenanId' hidden='hidden' value='" + data.Id + "' />" +
                                        "<input type='text' id='tenanIdUpdate' name='tenanId' hidden='hidden' value='" + data.TenanId + "' />" +
                                        "<input type='text' id='productidUpdate' name='productid' hidden='hidden' value='" + data.ProductId + "' />" +
                                        "<input type='text' id='productguidUpdate' name='productid' hidden='hidden' value='" + data.ProductGuid + "' />" +
                                        ": <input type='text' name='productName' value='" + data.Nama + "' readonly='readonly' />" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>Harga</td>" +
                                    "<td>" +
                                        ": <input type='text' id='hargaUpdate' name='harga' value='" + String.format("{0:c}",data.Price) + "'/>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td colspan='2'>" +
                                        "<div class='simpan' id='updateMapping' onclick='SaveEditData()'>Simpan</div>" +
                                        "<div class='batal' id='batalMapping'onclick='HideViewEdit()'>Batal</div>" +
                                    "</td>" +
                                "</tr>");
}


function SaveData() {
    var tenanId = $("#tenanId").val();
    var productId = $("#productid").val();
    var productguid = $("#productguid").val();
    var harga = $("#harga").val();

    $.ajax({
        type: "POST",
        url: "/MappingPriceAp/AddToMappingPrice",
        data: {"tenanId":tenanId, "productId":productId, "harga":harga, "productguid":productguid},
        datatype:"json",
        success: InsertMappingItemList
    });
}

function SaveEditData() {
    var tenanId = $("#tenanIdUpdate").val();
    var productId = $("#productidUpdate").val();
    var productguidUpdate = $("#productguidUpdate").val();
    var harga = $("#hargaUpdate").val();
    var id = $("#idUpdate").val();

    $.ajax({
        type: "POST",
        url: "/MappingPriceAp/UpdateToMappingPrice",
        data: { "id":id, "tenanId": tenanId, "productId": productId, "harga": harga, "productguidUpdate":productguidUpdate },
        datatype: "json",
        success: InsertMappingItemListReplace
    });
}
function HideView() {
    $("#DivDialogMappingPrice").hide();
    $("#DialogOverlayProduct").hide();
}
function HideViewEdit() {
    $("#DivDialogEditMappingPrice").hide();
    $("#DialogOverlayProduct").hide();
}
function hideDialog() {
    $(".divListproduct").hide();
    $("#DialogOverlayProduct").hide();
}
function InsertMappingItemListReplace(data) {
    HideViewEdit();
    var color = $("table#tableListProductTenanLounge tbody tr[id='tr" + data.Id + "']").attr("class");
    $("table#tableListProductTenanLounge tbody tr[id='tr" + data.Id + "']").replaceWith("<tr class="+ color +" id=tr" + data.Id + " onmouseover=showEditMappingPrice('" + data.Id + "')  onmouseout=hideEditItem('" + data.Id + "')>" +
                                                           "<td>" + data.Nama + "</td>" +
                                                           "<td>" + data.Kode + "</td>" +
                                                         "<td style='text-align:right;cursor:pointer; width:20%;' onmouseover=showEditMappingPrice('" + data.Id + "')  onmouseout=hideEditItem('" + data.Id + "')>" + String.format("{0:c}",data.Price) + "</td>" +
                                                           "<td id='tdButton'><div id='DivButton" + data.Id + "' style='display:none'><div class='EditPrice'  onclick=showEditItem('" + data.Id + "')></div>" +
                                                           "<div class='delete' onclick=showDeleteItem('" + data.Id + "','" + data.TenanId + "')>X</div></div></td>" +
                                                           "</tr>");
}