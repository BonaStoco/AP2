$(document).ready(function () {
    $("#AdvancedSearch").dialog({
        autoOpen: false,
        width: 900,
        height: 450,
        show: "Fade",
        hide: "explode"
    });

    $("#AdvSearch").click(function () {
        document.getElementById("AdvancedSearch").style.display = "block";
        $("#AdvancedSearch").dialog("open");
        return false;
    });

    $('#id_search').quicksearch('table tbody tr',
            {
                noResults: '#noresults',
                stripeRows: ['odd', 'even'],
                loader: 'img.loading'
            });

    $("form#searchTenantForm").submit(function () {
        var tenantId = $("#txtSearch").val();
        if (!tenantId)
            return false;
    });
});

function submitted(tenanid) {
    if (!tenanid)
        return false;
    $("#txtSearch").val(tenanid);
    $("form#searchTenantForm").submit();
}

function CloseDialog() {
    $("#AdvancedSearch").dialog("close");
}