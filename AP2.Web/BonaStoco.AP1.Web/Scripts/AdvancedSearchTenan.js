function InsertTenanToTableTenanIDLama(data) {
    if (data == 0 || data == null) {
        DestroyModalDialog();
        return;
    }
    var color;
    $("table#TableTenan tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        $("table#TableTenan tbody").append("<tr style='background-color:" + color + "' onclick='Searched(" + data[item].TenanId + ")'>" +
                                           "<td class='TenanId_IdLama'>" + data[item].TenanId + "</td>" +
                                           "<td class='TenanIdLama_IdLama'>" + data[item].TenanIdLama + "</td>" +
                                           "<td class='NamaTenan_IdLama'>" + data[item].TenanName + "</td>" +
                                           "<td class='Gate_IdLama'>" + data[item].Gate + "</td></tr>");
    });
    $("label#TotalTenan").text('Total : ' + data.length + ' Tenan');
}

function InsertTenanToTable(data) {
    if (data == 0 || data == null) {
        DestroyModalDialog();
        return;
    }
    var color;
    $("table#TableTenan tbody").empty();
    $.each(data, function (item) {
        if (item % 2 == 0) {
            color = "#e3ecff";
        }
        else {
            color = "#FFF";
        }
        $("table#TableTenan tbody").append("<tr style='background-color:" + color + "' onclick='Searched(" + data[item].TenanId + ")'>" +
                                           "<td class='TenanId'>" + data[item].TenanId + "</td>" +                                           
                                           "<td class='NamaTenan'>" + data[item].TenanName + "</td>" +
                                           "<td class='Gate'>" + data[item].Gate + "</td></tr>");
    });
    $("label#TotalTenan").text('Total : '+ data.length + ' Tenan');
}
function CreateModalDialogTenanIDLama(title) {
    $("body").append("<div id='ModalDialog'><div id='DialogOverlay'></div><label class='TitleOverlay'>E-POS</label>" +
	                         "<div class='TenanDialog'><label class='Header'>" +
        	                 "<label class='LogoDialog'><img src='../Content/images/button/Icon-Bonastoco.png' alt='Logo'/></label>" +
                             title +
                             "<label id='Close'>X</label></label>" +
                             "<div id='Loading'></div>" +
                             "<table id='TableTenan' width='100%'><thead><tr>" +
                             "<th class='TenanId_IdLama'>Tenan ID</th>" +
                             "<th class='TenanIdLama_IdLama'>Tenan ID Lama</th>" +
                             "<th class='NamaTenan_IdLama'>Nama Tenan</th>" +
                             "<th class='Gate_IdLama'>Gate</th></tr></thead><tbody></tbody></table>" +
                             "<label id='TotalTenan_IdLama'></label>" +
                             "<input type='text' id='search_tenant' placeholder='Search By Name'/>" +
                             "</div>");
    $("#search_tenant").focus();
    $("#search_tenant").keyup(SearchByTenanName);
    $("#Close").click(DestroyModalDialog);
}
function CreateModalDialog(title) {
    $("body").append("<div id='ModalDialog'><div id='DialogOverlay'></div><label class='TitleOverlay'>E-POS</label>" +
	                         "<div class='TenanDialog'><label class='Header'>" +
        	                 "<label class='LogoDialog'><img src='../Content/images/button/Icon-Bonastoco.png' alt='Logo'/></label>" +
                             title +
                             "<label id='Close'>X</label></label>" +
                             "<div id='Loading'></div>" +
                             "<table id='TableTenan' width='100%'><thead><tr>" +
                             "<th class='TenanId'>Tenan ID</th>" +                             
                             "<th class='NamaTenan'>Nama Tenan</th>" +
                             "<th class='Gate'>Gate</th></tr></thead><tbody></tbody></table>" +
                             "<label id='TotalTenan'></label>" +
                             "<input type='text' id='search_tenant' placeholder='Search By Name'/>" +
                             "</div>");
    $("#search_tenant").focus();
    $("#search_tenant").keyup(SearchByTenanName);
    $("#Close").click(DestroyModalDialog);
}
function DestroyModalDialog() {
    $("div#ModalDialog").remove();
}
function LoadingStart() {
    $("#Loading").show();
}
function LoadingEnd() {
    $("#Loading").hide();
}
function SearchByTenanName() {
    $('#search_tenant').quicksearch('table#TableTenan tbody tr',
            {
                stripeRows: ['odd', 'even']
            });
}