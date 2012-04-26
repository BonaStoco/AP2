var units;
var updateUnitList;
var unitAfterUpdate;
var tambahUnit;
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
            url: "/Unit/ListUnit",
            dataType: "json",
            success: onLoadingAllUnitSuccess,
            error : onUpdateUnitError
        });
    }

    function onLoadingAllUnitSuccess(data, status) {
        units = data;
        RenderViewListUnit(); 
    }

    function RenderViewListUnit() {
        ClearListView();
        $.each(units, function (item) {
            $("#ListMenu tbody").append("<tr>" +
                    "<td>" + units[item].Kode + "</td>" +
                    "<td class='right'>" + units[item].Nama + "</td>" +
                    "<td class='center' id='Edit' onclick='dialogEdit(\"" + units[item].ModelGuid + "\")'></td>" +
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
        height: 300,
        width: 350,
        modal: true,
        buttons:
            {
                "Add":  function () {
                        kode = $("#add_kode");
                        nama = $("#add_nama");
                        SaveNewUnit();
                    },
                "Cancel": function () {
                        ClearAddForm();
                        $(this).dialog("close");
                    }
            }
    });

    function sendAddUnitToServer() {

        $.ajax({
            type: "GET",
            url: "/Unit/AddUnit",
            dataType: "json",
            data: { 'AddUnit': JSON.stringify(tambahUnit) },
            success: onAddUnitSuccess,
            error: onUpdateUnitError
        });
    }

    function onAddUnitSuccess(data, status) {
        addLocalModel();
        RenderViewListUnit();
        ClearAddForm();
        $("#add-dialog").dialog("close");
    }

    function addLocalModel() {
        units.push(tambahUnit);
    }

    function dialogEdit(id) {
        kode = $("#kode");
        nama = $("#nama");
        getUnitForEdit(id);
        showEditDialog();
    }

    function showEditDialog() {
        var options = {
            autoOpen: true,
            height: 300,
            width: 350,
            modal: true,
            buttons: {
                "Update": function () {
                    UpdateExistingUnit();
                },
                "Cancel": function () {
                    ClearEditForm();
                    $(this).dialog("close");
                }
            }

        };
        $("#edit-dialog").dialog(options);
    }

    function sendUpdateUnitToServer() {

        $.ajax({
            type: "GET",
            url: "/Unit/UpdateUnit",
            dataType: "json",
            data: { 'unit': JSON.stringify(updateUnitList) },
            success: onUpdateUnitSuccess,
            error : onUpdateUnitError
        });
    }

    function onUpdateUnitError(request, status, error) {
       var message = $(request.responseText)[1].text;
       alert(message);

    }
    function onUpdateUnitSuccess(data, status) {

        updateLocalModel();
        RenderViewListUnit();
        ClearEditForm();
        $("#edit-dialog").dialog("close");
    }

    function updateLocalModel() {

        $.each(units, function (item) {

            if (units[item].ModelGuid == updateUnitList.ModelGuid) {
                units[item] = updateUnitList;
                return;
            }
        });
    }

    function getUnitFromListUnit(id) {
        for (var e in units) {
            var value = units[e];
            if (value.ModelGuid == id) {
                return value;
            }
        }
        return null;
    }

    function getUnitForEdit(id) {
        updateUnitList = getUnitFromListUnit(id);
        setUpdateModelToEditUnitDialog()
    }

    function setUpdateModelToEditUnitDialog() {
        $("#kode").val(updateUnitList.Kode);
        $("#nama").val(updateUnitList.Nama);
    }

    function SaveNewUnit() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        if (IsExistData()) {
            updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
        }
        else {
            setNewUnitModel();
            sendAddUnitToServer();
        }
    }

    function setNewUnitModel() {
        tambahUnit = new Object();
        tambahUnit.Kode = $("#add_kode").val();
        tambahUnit.Nama = $("#add_nama").val();
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
        for (var e in units) {
            var value = units[e];
            if (value.Kode == kode.val() && value.Nama == nama.val()) {
                found = true;
            }
        }
        return found;
    }

    function UpdateExistingUnit() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        updateUnitList.Kode = $("#kode").val();
        updateUnitList.Nama = $("#nama").val();
        sendUpdateUnitToServer();
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




