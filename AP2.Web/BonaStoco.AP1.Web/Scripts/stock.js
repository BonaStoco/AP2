var company;
var tenanid;


$(document).ready(function () {
    HideStockCardDetail();
    init();
});
function init() {
    $("#BackToList").click(function () {
        init();
        HideStockCardDetail();
        ClearStocklist();
        ShowListStock();
    });

    var tenanId = $("#tenan-id").text();
    if (tenanId != "")
        FindGroupNameByTenanId(tenanId);
}
function FindGroupNameByTenanId(id) {
    $.ajax({
        type: "GET",
        url: "/ReportStock/FindGroupNameByTenanId/" + id,
        dataType: "json",
        success: ShowStateGroupName
    });

}

function ShowStateGroupName(data, status) {
    var i = 1;
    $('#CategoryStockList').empty();
    $("#tblstock tbody").empty();
    $("#tblstock thead").empty();
    $("#searchStockCard").empty();
    $.each(data, function (item) {
        var dat = data[item].PartGroupId;
        var nama = data[item].PartGroupName.toString();
        $("#CategoryStockList").append("<div id=menugroup ><a href='#' onclick=\"FindStockCardByGroupName(" + dat + ")\"><img class='loading' src='../Content/images/JSPGetProperty.gif'/>" + nama + "</a></div>");
        i++;
    });

    tenanid = data[0].TenanId;
}

function FindStockCardByGroupName(partgroupId) {
    $("#tblstock tbody").empty();
    $("#tblstock thead").empty();
    $("#searchStockCard").empty();
    $.ajax({
        type: "GET",
        url: "/ReportStock/FindProductIdStockByTenanAndGroup",
        dataType: "json",
        data: { 'tenantid': tenanid, 'groupid': partgroupId },
        success: ShowStockCardByGroupName
    });
}
function ShowStockCardByGroupName(data, status) {
    var no = 1;
    var warna;
    $("#tblstock tbody").empty();
    $("#tblstock thead").empty();

    $("#searchStockCard").empty();
    $("#tblstock thead").append("<tr><th>No</th><th>Kode</th><th>Barcode</th><th>Nama Barang</th><th>Stok</th></tr>");
    $("#searchStockCard").append("<input type='text' id='searchStock' placeholder='Cari barang' onkeyup='SearchStock()'>");
    $.each(data, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";
        }
        $("#tblstock tbody").append("<tr id=trstock" + no + " bgcolor=" + warna + " onmouseover=mouseOver('" + no + "') onmouseout=mouseOut('" + no + "') onclick=trStockonClick('" + data[item].Kode + "')>" +
        "<td width=20>" + no + "</td >" +
        "<td>" + data[item].Kode + "</td >" +
        "<td>" + data[item].Barcode + "</td>" +
        "<td>" + data[item].Nama + "</td>" +
        "<td>" + data[item].Balance + "</td></tr>");

        no++;
    });


}

function LoadStockDataAjaxbyList(id) {

    $.ajax({
        type: "GET",
        url: "/ReportStock/FindCompanyByTenanId/" + id,
        dataType: "json",
        success: ShowTenanName
    });
}

function ShowTenanName(data, status) {
    company = data;
    document.getElementById('nameTenan').innerHTML = company[1];
    FindGroupNameByTenanId(company[0]);

}

function GetStockCardByCompanyid(e, id) {

    if (e.keyCode == 13) {
        FindStockCardById(id);

    }

}

function FindStockCardById(id) {

    $.ajax({
        type: "GET",
        url: "/ReportStock/FindStockCardByTenanId/" + id,
        success: ShowStockCardById
    });

}

function ShowStockCardById(data, status) {
    var no = 1;
    var warna;
    $.each(data, function (item) {
        if (no % 2 == 0) {
            warna = "#f8f8f8";
        }
        else {
            warna = "#fff";

        }
        $("#tblstock tbody").append("<tr bgcolor=" + warna + "><td width=20>" + no + "</td ><td>" + data[item].Kode + "</td ><td>" + data[item].Barcode + "</td><td>" + data[item].Nama + "</td><td>" + data[item].Balance + "</td></tr>");
        no++;
    });

}

function FindNameByTenanid(id) {
    if (id != null)
        LoadStockDataAjaxbyList(id);
}

function SearchStock() {
    $('#searchStock').quicksearch('table#tblstock tbody tr',
             {
                 stripeRows: ['odd', 'even']
             });
}

function mouseOut(index) {
    // $("#trstock" + index).css("background-color", "#3294F0");
}

function mouseOver(index) {
    //    if (index % 2 == 0) {
    //        warna = "#f8f8f8";
    //    }
    //    else {
    //        warna = "#fff";
    //    }
    //    $("#trstock" + index).css("background-color", "#3294F0");
}

function trStockonClick(code) {
    loadStockCardDetail(code);
    HideListStock();
}

function loadStockCardDetail(code) {
    $.ajax({
        type: "GET",
        url: "/ReportStock/FindStockCardDetailByCode",
        data: { "tenantId": tenanid, "code": code },
        dataType: "json",
        success: ViewListStockCard
    });
}

function ViewListStockCard(data) {
    ShowStockCardDetail();
    var incoming = 0;
    var outgoing = 0;
    $("#tableListStockCardDetail tbody").empty();
    $("#tableListStockCardDetail tfoot").empty();

    $.each(data, function (item) {
        incoming = incoming + data[item].Incoming;
        outgoing = outgoing + data[item].OutGoing;
        var Dates = $.datepicker.formatDate('dd M yy', new Date(parseInt(data[item].Date.replace(/\/Date\((-?\d+)\)\//, '$1'))))

        $("#tableListStockCardDetail tbody").append("<tr>" +
                    "<td>" + Dates + "</td>" +
                    "<td>" + data[item].TransactionNumber + "</td>" +
                    "<td>" + data[item].TransactionType + "</td>" +
                    "<td style='text-align: right;'>" + data[item].Incoming + "</td>" +
                    "<td style='text-align: right;'>" + data[item].OutGoing + "</td>" +
                    "<td style='text-align: right;'>" + data[item].Balance + "</td>" +

     "</tr>");
    });
    $("#tableListStockCardDetail tfoot").append("<tr>" +
                    "<td style='background-color:white;'></td>" +
                    "<td style='background-color:white;'></td>" +
                    "<td style='background-color:white;'></td>" +
                    "<td style='text-align: right;' class='border-top'>" + incoming + "</td>" +
                    "<td style='text-align: right;' class='border-top'>" + outgoing + "</td>" +
                    "<td style='text-align: right;'></td>" +
        "</tr>");
}

function ShowStockCardDetail() {
    $(".divStockCardDetail").show();
    $("#BackToList").show();
}

function HideStockCardDetail() {
    $(".divStockCardDetail").hide();
    $("#BackToList").hide();
}

function HideListStock() {
    $("#ReportStock").hide();
}

function ShowListStock() {
    $("#ReportStock").show();
}

function ClearStocklist() {
    $('#CategoryStockList').empty();
    $("#tblstock tbody").empty();
    $("#tblstock thead").empty();
    $("#searchStockCard").empty();
    $("input#tenanId").val("");
    $("#nameTenan").empty();
}

