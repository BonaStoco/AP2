$(document).ready(function () {
    $("#AdvancedSearchProduct").append("<input type='text' id='id_search' placeholder='Search'/>" +
                     "<input type='text' id='id_search_grn' style='display:none;' placeholder='Search'/>" +
                     "<img id='loading' src='./Content/images/loader.gif'/>" +
                     "<table id='tableProduct' width='100%'><thead class='headerSearchTbl'><tr >" +
                        "<th width='15%'>Kode</th>" +
                        "<th width='15%'>Barcode</th>" +
                        "<th width='20%'>Group</th>" +
                        "<th width='35%'>Nama</th>" +
                        "<th width='15%'>Harga Jual</th></tr></thead>" +
                     "<tbody id='tbodyProduct'></tbody></table>");


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
            url: "/MasterData/InitialSearchProduct",
            dataType: "json",
            beforeSend: LoadingStart,
            complete: LoadingEnd,
            success: InsertProductToTable
        });
        $("#AdvancedSearchProduct").dialog("open");
    });

    $("#AdvSearchGrn").click(function () {
        $("#tbodyProduct").empty();
        $("#id_search").css("display", "none");
        $("#id_search_grn").css("display", "block");
        $.ajax({
            type: "GET",
            url: "/MasterData/InitialSearchGRNProduct",
            dataType: "json",
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
            url: "/MasterData/SearchProductByName",
            data: { 'name': value },
            dataType: "json",
            beforeSend: LoadingStart,
            complete: LoadingEnd,
            success: InsertProductToTable
        });
    });

    $("#id_search_grn").keyup(function () {
        var value = $("#id_search_grn").val();
        $.ajax({
            type: "GET",
            url: "/MasterData/SearchProductGrnByName",
            data: { 'name': value },
            dataType: "json",
            beforeSend: LoadingStart,
            complete: LoadingEnd,
            success: InsertProductToTable
        });
    });
});

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
