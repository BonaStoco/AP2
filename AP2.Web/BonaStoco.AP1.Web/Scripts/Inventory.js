var company = null;
var tenanid = null;
var groupID = null;
var jumPage = null;

$(document).ready(function () {
    var tenanId = $("#tenan-id").text();
    $("#tblInventory tfoot").empty()
    if (tenanId!="")
        FindGroupNameByTenanId(tenanId);
});

function FindGroupNameByTenanId(id) {
    $.ajax({
        type: "GET",
        url: "/ReportInventory/FindGroupNameByTenanId/" + id,
        dataType: "json",
        success: ShowStateGroupName
    });
}

function ShowStateGroupName(data, status) {
    var i = 1;
    $('#CategoryInventoryList').empty();
    $("#tblInventory tbody").empty();
    $("#tblInventory thead").empty();
    $("#ErrorInvTenan").hide();
    $("#searchInventory").empty();

    if (data.length == 0)
        return;

    $.each(data, function (item) {
        var dat = data[item].PartGroupId;
        var nama = data[item].PartGroupName.toString();
        $("#CategoryInventoryList").append("<div id=menugroup >" +
        "<a href=\"#\" onclick=\"FindPartByGroupId(" + dat + ")\">" +
        "<img class='loading' src='../Content/images/JSPGetProperty.gif'/>" + nama + "</a></div>");
        i++;
    });
    tenanid = data[0].TenanId;
}

function FindPartByGroupId(partgroupId) {
    $("#ErrorInvTenan").hide()
    $("#tblInventory thead").empty();
    $("#tblInventory tbody").empty();
    $("#tblInventory tfoot").empty()

    groupID = partgroupId;
    GetInventoryPart(partgroupId);
}

function GetInventoryPart(partgroupId) {
    $.ajax({
        type: "GET",
        url: "/ReportInventory/FindPartNameByGroupId",
        dataType: "json",
        data: { 'tenantid': parseInt(tenanid), 'groupid': parseInt(partgroupId), 'starts': parseInt(0), 'limits': parseInt(20) },
        beforeSend: LoadingStart,
        complete: LoadingStop,
        success: function (data) {
            if (data.length > 0) {
                GetCountPagination(partgroupId);
                ShowPartByGroupName(data);
            }

            if (data.length == 0) {
                $("#ErrorInvTenan").text("Data tidak ada");
                $("#ErrorInvTenan").show();
            }
        }
    });
}

function LoadingStart() {
    $("#dialog-overlay-inventory").show();
}

function LoadingStop() {
    $("#dialog-overlay-inventory").hide();
}

function GetCountPagination(partgroupId) {
    $.ajax({
        type: "GET",
        url: "/ReportInventory/CountPagination",
        dataType: "json",
        async: false,
        data: { 'tenantid': parseInt(tenanid), 'groupid': parseInt(partgroupId) },
        success: function (data) {
            jumPage = data;
        }
    });
}

function ShowPartByGroupName(data, status) {
    var no = 1;
    var warna;
    var jumlahHal = jumPage;
    $("#tblInventory tbody").empty();
    $("#tblInventory thead").empty();
    $("#tblInventory tfoot").empty();
    $("#searchInventory").empty();
    $("#tblInventory thead").append("<tr><th>No</th><th>Kode</th><th>Barcode</th><th>Nama Barang</th><th>Stok</th></tr>");
    $.each(data, function (item) {
        if (data[item] != null) {
            if (no % 2 == 0) {
                warna = "#f8f8f8";
            }
            else {
                warna = "#fff";
            }
            $("#tblInventory tbody").append("<tr id='trInventory' bgcolor=" + warna + "><td width=20>" + no + "</td ><td>" + data[item].Kode + "</td ><td>" + data[item].Barcode + "</td><td>" + data[item].Nama + "</td><td>" + data[item].Balance + "</td></tr>");

            no++;
        }
    });

    var navPaging = "<div class ='Pagging'>" +
                  "<div class ='DivPrev'>" +
                    "<img class ='first pagingSize'></img>" +
                    "<img class ='prev pagingSize'></img>" +
                  "</div>" +
                  "<div class ='disablePrev' style='display:none;'>" +
                    "<img class ='firstDisable pagingSize'></img>" +
                    "<img class ='prevDisable pagingSize'></img>" +
                  "</div>" +
                  "<div class ='DivPage'>" +
                    "Page <input type='text' id='idInputPageInventory' value='1' style='width:20px; height:14px'/> of <span id='totalPageInvoice'>" + jumlahHal + "</span>" +
                  "</div>" +
                  "<div class='DivNext'>" +
                      "<img class ='next pagingSize'></img>" +
                      "<img class ='last pagingSize'></img>" +
                  "</div>" +
                  "<div class ='disableNext' style='display:none;'>" +
                    "<img class ='nextDisable pagingSize'></img>" +
                    "<img class ='lastDisable pagingSize'></img>" +
                  "</div>";

    $("#tblInventory tfoot").append("<tr><td colspan='5'></td></tr><tr><td colspan='5'>" + navPaging + "</td></tr>");
    navPageLink(1, jumlahHal);

}

$(".prev").live("click", function (event) {
    var prev = $("#idInputPageInventory").val();
    prev--;
    $("#idInputPageInventory").val(prev);
    GetInventoryPartChangePage(prev);
});

$(".first").live("click", function (event) {
    var first = 1;
    $("#idInputPageInventory").val(first);
    GetInventoryPartChangePage(first);
});

$(".next").live("click", function (event) {
    var next = $("#idInputPageInventory").val();
    next++;
    $("#idInputPageInventory").val(next);
    GetInventoryPartChangePage(next);
});

$(".last").live("click", function (event) {
    var last = jumPage;
    $("#idInputPageInventory").val(last);
    GetInventoryPartChangePage(last);
});

function navPageLink(halamanAktif, jmlHalaman) {
    if (halamanAktif > 1) {
        $(".DivPrev").show();
        $(".disablePrev").hide();
    }
    else {
        $(".DivPrev").hide();
        $(".disablePrev").show();
    }

    if (halamanAktif < jmlHalaman) {
        $(".DivNext").show();
        $(".disableNext").hide();
    } else {
        $(".DivNext").hide();
        $(".disableNext").show();
    }
}

$("#idInputPageInventory").live("keyup", function (event) {
    if (event.which == 13) {
        if (!isNaN($(this).val())) {
            if ($(this).val() > jumPage) {
                return false;
            }

            if ($(this).val() < 1) {
                return false;
            }
            GetInventoryPartChangePage($(this).val())
        }
    }
});

function GetInventoryPartChangePage(inputPage) {
    var start = (inputPage - 1) * 20;
    var locGroupId = groupID;
    $.ajax({
        type: "GET",
        url: "/ReportInventory/FindPartNameByGroupId",
        dataType: "json",
        data: { 'tenantid': parseInt(tenanid), 'groupid': parseInt(locGroupId), 'starts': parseInt(start), 'limits': parseInt(20) },
        beforeSend: LoadingStart,
        complete: LoadingStop,
        success: function (data) {
            if (data != null) {
                ShowPartByGroupNamePage(data, start, inputPage);
            }
        }
    });
}

function ShowPartByGroupNamePage(data, start, inputPage) {
    var no = start + 1;
    var warna;
    var jumlahHal = jumPage;
    $("#tblInventory tbody").empty();
    $("#tblInventory thead").empty();
    $("#tblInventory tfoot").empty();
    $("#searchInventory").empty();
    $("#tblInventory thead").append("<tr><th>No</th><th>Kode</th><th>Barcode</th><th>Nama Barang</th><th>Stok</th></tr>");
    $.each(data, function (item) {
        if (data[item] != null) {
            if (no % 2 == 0) {
                warna = "#f8f8f8";
            }
            else {
                warna = "#fff";
            }
            $("#tblInventory tbody").append("<tr id='trInventory' bgcolor=" + warna + "><td width=20>" + no + "</td ><td>" + data[item].Kode + "</td ><td>" + data[item].Barcode + "</td><td>" + data[item].Nama + "</td><td>" + data[item].Balance + "</td></tr>");

            no++;
        }
    });

    var navPaging = "<div class ='Pagging'>" +
                  "<div class ='DivPrev'>" +
                    "<img class ='first pagingSize'></img>" +
                    "<img class ='prev pagingSize'></img>" +
                  "</div>" +
                  "<div class ='disablePrev' style='display:none;'>" +
                    "<img class ='firstDisable pagingSize'></img>" +
                    "<img class ='prevDisable pagingSize'></img>" +
                  "</div>" +
                  "<div class ='DivPage'>" +
                    "Page <input type='text' id='idInputPageInventory' value=" + inputPage + " style='width:20px; height:14px'/> of <span id='totalPageInvoice'>" + jumlahHal + "</span>" +
                  "</div>" +
                  "<div class='DivNext'>" +
                      "<img class ='next pagingSize'></img>" +
                      "<img class ='last pagingSize'></img>" +
                  "</div>" +
                  "<div class ='disableNext' style='display:none;'>" +
                    "<img class ='nextDisable pagingSize'></img>" +
                    "<img class ='lastDisable pagingSize'></img>" +
                  "</div>";

    //$("#tblInventory tfoot").replaceWith("<tr><td colspan='5'></td></tr><tr><td colspan='5'>" + navPaging + "</td></tr>");
    $("#tblInventory tfoot").append("<tr><td colspan='5'></td></tr><tr><td colspan='5'>" + navPaging + "</td></tr>");
    navPageLink(inputPage, jumlahHal);
}

function FindNameByTenanid(id) {
    $("#tblInventory tfoot").empty();
    var len = $("#tenanId").val().trim().length;
    if (len > 1)
        LoadStockDataAjaxbyList(id);
}

function LoadStockDataAjaxbyList(id) {

    $.ajax({
        type: "GET",
        url: "/ReportInventory/FindCompanyByTenanId/" + id,
        dataType: "json",
        success: ShowTenanName
    });
}

function ShowTenanName(data, status) {
    company = data;
    document.getElementById('nameTenan').innerHTML = company[1];
    FindGroupNameByTenanId(company[0]);
}

