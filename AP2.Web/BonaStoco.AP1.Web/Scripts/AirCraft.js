var AirCrafts;
var updateAirCraftList;
var AirCraftAfterUpdate;
var tambahAirCraft;
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
            url: "/Cargo/ListAirCraft",
            dataType: "json",
            success: onLoadingAllAirCraftSuccess,
            error : onUpdateAirCraftError
        });
    }

    function onLoadingAllAirCraftSuccess(data, status) {
        AirCrafts = data;
        RenderViewListAirCraft(); 
    }

    function RenderViewListAirCraft() {
        $("tbody#bodyList").empty();
        $.each(AirCrafts, function (item) {
            $("tbody#bodyList").append("<tr class='bodyTable'>" +
                    "<td>" + AirCrafts[item].Kode + "</td>" +
                    "<td class='right'>" + AirCrafts[item].Nama + "</td>" +
                    "<td class='center' id='Edit' onclick='dialogEdit(\"" + AirCrafts[item].Guid + "\")'></td>" +
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
                        SaveNewAirCraft();
                    },
                Cancel: function () {
                        ClearAddForm();
                        $(this).dialog("close");
                    }
            }
    });

    function sendAddAirCraftToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/AddAirCraft",
            dataType: "json",
            data: { 'data': JSON.stringify(tambahAirCraft) },
            success: onAddAirCraftSuccess,
            error: onUpdateAirCraftError
        });
    }

    function onAddAirCraftSuccess(data, status) {
        addLocalModel();
        RenderViewListAirCraft();
        ClearAddForm();
        $("#add-dialog").dialog("close");
    }

    function addLocalModel() {
        AirCrafts.push(tambahAirCraft);
    }

    function dialogEdit(id) {
        kode = $("#kode");
        nama = $("#nama");
        getAirCraftForEdit(id);
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
                    UpdateExistingAirCraft();
                },
                Cancel: function () {
                    ClearEditForm();
                    $(this).dialog("close");
                }
            }

        };
        $("#edit-dialog").dialog(options);
    }

    function sendUpdateAirCraftToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/UpdateAirCraft",
            dataType: "json",
            data: { 'data': JSON.stringify(updateAirCraftList) },
            success: onUpdateAirCraftSuccess,
            error : onUpdateAirCraftError
        });
    }

    function onUpdateAirCraftError(request, status, error) {
       var message = $(request.responseText)[1].text;
       alert(message);

    }
    function onUpdateAirCraftSuccess(data, status) {

        updateLocalModel();
        RenderViewListAirCraft();
        ClearEditForm();
        $("#edit-dialog").dialog("close");
    }

    function updateLocalModel() {

        $.each(AirCrafts, function (item) {

            if (AirCrafts[item].Guid == updateAirCraftList.Guid) {
                AirCrafts[item] = updateAirCraftList;
                return;
            }
        });
    }

    function getAirCraftFromListAirCraft(id) {
        for (var e in AirCrafts) {
            var value = AirCrafts[e];
            if (value.Guid == id) {
                return value;
            }
        }
        return null;
    }

    function getAirCraftForEdit(id) {
        updateAirCraftList = getAirCraftFromListAirCraft(id);
        setUpdateModelToEditAirCraftDialog()
    }

    function setUpdateModelToEditAirCraftDialog() {
        $("#kode").val(updateAirCraftList.Kode);
        $("#nama").val(updateAirCraftList.Nama);
    }

    function SaveNewAirCraft() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        if (IsExistData()) {
            updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
        }
        else {
            setNewAirCraftModel();
            sendAddAirCraftToServer();
        }
    }

    function setNewAirCraftModel() {
        tambahAirCraft = new Object();
        tambahAirCraft.Kode = $("#add_kode").val();
        tambahAirCraft.Nama = $("#add_nama").val();
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
        for (var e in AirCrafts) {
            var value = AirCrafts[e];
            if (value.Kode == kode.val() && value.Nama == nama.val()) {
                found = true;
            }
        }
        return found;
    }

    function UpdateExistingAirCraft() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        updateAirCraftList.Kode = $("#kode").val();
        updateAirCraftList.Nama = $("#nama").val();
        sendUpdateAirCraftToServer();
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




