var Destinetions;
var updateDestinetionList;
var DestinetionAfterUpdate;
var tambahDestinetion;
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
            url: "/Cargo/ListDestinetion",
            dataType: "json",
            success: onLoadingAllDestinetionSuccess,
            error : onUpdateDestinetionError
        });
    }

    function onLoadingAllDestinetionSuccess(data, status) {
        Destinetions = data;
        RenderViewListDestinetion(); 
    }

    function RenderViewListDestinetion() {
        $("tbody#bodyList").empty();
        $.each(Destinetions, function (item) {
            $("tbody#bodyList").append("<tr class='bodyTable'>" +
                    "<td>" + Destinetions[item].Kode + "</td>" +
                    "<td class='right'>" + Destinetions[item].Nama + "</td>" +
                    "<td class='center' id='Edit' onclick='dialogEdit(\"" + Destinetions[item].Guid + "\")'></td>" +
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
                        SaveNewDestinetion();
                    },
                Cancel: function () {
                        ClearAddForm();
                        $(this).dialog("close");
                    }
            }
    });

    function sendAddDestinetionToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/AddDestinetion",
            dataType: "json",
            data: { 'data': JSON.stringify(tambahDestinetion) },
            success: onAddDestinetionSuccess,
            error: onUpdateDestinetionError
        });
    }

    function onAddDestinetionSuccess(data, status) {
        addLocalModel();
        RenderViewListDestinetion();
        ClearAddForm();
        $("#add-dialog").dialog("close");
    }

    function addLocalModel() {
        Destinetions.push(tambahDestinetion);
    }

    function dialogEdit(id) {
        kode = $("#kode");
        nama = $("#nama");
        getDestinetionForEdit(id);
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
                    UpdateExistingDestinetion();
                },
                Cancel: function () {
                    ClearEditForm();
                    $(this).dialog("close");
                }
            }

        };
        $("#edit-dialog").dialog(options);
    }

    function sendUpdateDestinetionToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/UpdateDestinetion",
            dataType: "json",
            data: { 'data': JSON.stringify(updateDestinetionList) },
            success: onUpdateDestinetionSuccess,
            error : onUpdateDestinetionError
        });
    }

    function onUpdateDestinetionError(request, status, error) {
       var message = $(request.responseText)[1].text;
       alert(message);

    }
    function onUpdateDestinetionSuccess(data, status) {

        updateLocalModel();
        RenderViewListDestinetion();
        ClearEditForm();
        $("#edit-dialog").dialog("close");
    }

    function updateLocalModel() {

        $.each(Destinetions, function (item) {

            if (Destinetions[item].Guid == updateDestinetionList.Guid) {
                Destinetions[item] = updateDestinetionList;
                return;
            }
        });
    }

    function getDestinetionFromListDestinetion(id) {
        for (var e in Destinetions) {
            var value = Destinetions[e];
            if (value.Guid == id) {
                return value;
            }
        }
        return null;
    }

    function getDestinetionForEdit(id) {
        updateDestinetionList = getDestinetionFromListDestinetion(id);
        setUpdateModelToEditDestinetionDialog()
    }

    function setUpdateModelToEditDestinetionDialog() {
        $("#kode").val(updateDestinetionList.Kode);
        $("#nama").val(updateDestinetionList.Nama);
    }

    function SaveNewDestinetion() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        if (IsExistData()) {
            updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
        }
        else {
            setNewDestinetionModel();
            sendAddDestinetionToServer();
        }
    }

    function setNewDestinetionModel() {
        tambahDestinetion = new Object();
        tambahDestinetion.Kode = $("#add_kode").val();
        tambahDestinetion.Nama = $("#add_nama").val();
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
        for (var e in Destinetions) {
            var value = Destinetions[e];
            if (value.Kode == kode.val() && value.Nama == nama.val()) {
                found = true;
            }
        }
        return found;
    }

    function UpdateExistingDestinetion() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        updateDestinetionList.Kode = $("#kode").val();
        updateDestinetionList.Nama = $("#nama").val();
        sendUpdateDestinetionToServer();
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




