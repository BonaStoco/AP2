var Comoditys;
var updateComodityList;
var ComodityAfterUpdate;
var tambahComodity;
var bValid1;
var bValid2;
var kode;
var nama;

$(document).ready(function () {
    LoadDataUsingAjax();
});

$("#dialog-overlay").ajaxStart(function () {
    $(this).show();
}).ajaxStop(function () {
    $(this).hide();
});
 
    function LoadDataUsingAjax() {
        $.ajax({
            type: "GET",
            url: "/Cargo/ListComodity",
            dataType: "json",
            success: onLoadingAllComoditySuccess,
            error : onUpdateComodityError
        });
    }

    function onLoadingAllComoditySuccess(data, status) {
        Comoditys = data;
        RenderViewListComodity(); 
    }

    function RenderViewListComodity() {
        $("tbody#bodyList").empty();
        $.each(Comoditys, function (item) {
            $("tbody#bodyList").append("<tr class='bodyTable'>" +
                    "<td>" + Comoditys[item].Kode + "</td>" +
                    "<td class='right'>" + Comoditys[item].Nama + "</td>" +
                    "<td class='center' id='Edit' onclick='dialogEdit(\"" + Comoditys[item].Guid + "\")'></td>" +
                "</tr>");
        });
    }

    function ClearListView() {
        var table = document.getElementById("ListMenu");
        while (table.rows.length > 1) {
            table.deleteRow(1);
        }
    }

    $("#tambah").click(function () {
        $("#add-dialog").dialog("open");
    });

    $("#add-dialog").dialog({
        autoOpen: false,
        height: 270,
        width: 500,
        modal: true,
        buttons:
            {
                "Add":  function () {
                        kode = $("#add_kode");
                        nama = $("#add_nama");
                        SaveNewComodity();
                    },
                Cancel: function () {
                        ClearAddForm();
                        $(this).dialog("close");
                    }
            }
    });

    function sendAddComodityToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/AddComodity",
            dataType: "json",
            data: { 'data': JSON.stringify(tambahComodity) },
            success: onAddComoditySuccess,
            error: onUpdateComodityError
        });
    }

    function onAddComoditySuccess(data, status) {
        addLocalModel();
        RenderViewListComodity();
        ClearAddForm();
        $("#add-dialog").dialog("close");
    }

    function addLocalModel() {
        Comoditys.push(tambahComodity);
    }

    function dialogEdit(id) {
        kode = $("#kode");
        nama = $("#nama");
        getComodityForEdit(id);
        showEditDialog();
    }

    function showEditDialog() {
        var options = {
            autoOpen: true,
            height: 270,
            width: 500,
            modal: true,
            buttons: {
                "Update": function () {
                    UpdateExistingComodity();
                },
                Cancel: function () {
                    ClearEditForm();
                    $(this).dialog("close");
                }
            }

        };
        $("#edit-dialog").dialog(options);
    }

    function sendUpdateComodityToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/UpdateComodity",
            dataType: "json",
            data: { 'data': JSON.stringify(updateComodityList) },
            success: onUpdateComoditySuccess,
            error : onUpdateComodityError
        });
    }

    function onUpdateComodityError(request, status, error) {
       var message = $(request.responseText)[1].text;
       alert(message);

    }
    function onUpdateComoditySuccess(data, status) {

        updateLocalModel();
        RenderViewListComodity();
        ClearEditForm();
        $("#edit-dialog").dialog("close");
    }

    function updateLocalModel() {

        $.each(Comoditys, function (item) {

            if (Comoditys[item].Guid == updateComodityList.Guid) {
                Comoditys[item] = updateComodityList;
                return;
            }
        });
    }

    function getComodityFromListComodity(id) {
        for (var e in Comoditys) {
            var value = Comoditys[e];
            if (value.Guid == id) {
                return value;
            }
        }
        return null;
    }

    function getComodityForEdit(id) {
        updateComodityList = getComodityFromListComodity(id);
        setUpdateModelToEditComodityDialog()
    }

    function setUpdateModelToEditComodityDialog() {
        $("#kode").val(updateComodityList.Kode);
        $("#nama").val(updateComodityList.Nama);
    }

    function SaveNewComodity() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        if (IsExistData()) {
            updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
        }
        else {
            setNewComodityModel();
            sendAddComodityToServer();
        }
    }

    function setNewComodityModel() {
        tambahComodity = new Object();
        tambahComodity.Kode = $("#add_kode").val();
        tambahComodity.Nama = $("#add_nama").val();
    }

    function updateTips(t) {
        $("p").addClass("validateTips");
        tips = $(".validateTips");
        tips
		.text(t)
		.addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkNullValidate(o, nilai, n) {
        if (nilai < 1) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        }
        else
            return true;
    }

    function IsExistData() {
        var found = false;
        for (var e in Comoditys) {
            var value = Comoditys[e];
            if (value.Kode == kode.val() && value.Nama == nama.val()) {
                found = true;
            }
        }
        return found;
    }

    function UpdateExistingComodity() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        updateComodityList.Kode = $("#kode").val();
        updateComodityList.Nama = $("#nama").val();
        sendUpdateComodityToServer();
    }

    function ClearAddForm() {
        $("p").text("");
        $("p").removeClass("validateTips");
        kode.removeClass("ui-state-error");
        nama.removeClass("ui-state-error");
        $(".ErrorMessage").text("");
        $("#add_kode").val("");
        $("#add_nama").val("");
        $("#kode").val("");
        $("#nama").val("");
    }

    function ClearEditForm() {
        $("p").text("");
        $("p").removeClass("validateTips");
        kode.removeClass("ui-state-error");
        nama.removeClass("ui-state-error");
        $(".ErrorMessage").text("");
        $("#kode").val("");
        $("#nama").val("");
    }




