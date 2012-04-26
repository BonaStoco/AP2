var Customers;
var updateCustomerList;
var CustomerAfterUpdate;
var tambahCustomer;
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
            url: "/Cargo/ListCustomer",
            dataType: "json",
            success: onLoadingAllCustomerSuccess,
            error : onUpdateCustomerError
        });
    }

    function onLoadingAllCustomerSuccess(data, status) {
        Customers = data;
        RenderViewListCustomer(); 
    }

    function RenderViewListCustomer() {
        $("tbody#bodyList").empty();
        $.each(Customers, function (item) {
            $("tbody#bodyList").append("<tr class='bodyTable'>" +
                    "<td>" + Customers[item].Kode + "</td>" +
                    "<td class='right'>" + Customers[item].Nama + "</td>" +
                    "<td class='center' id='Edit' onclick='dialogEdit(\"" + Customers[item].Guid + "\")'></td>" +
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
                        SaveNewCustomer();
                    },
                Cancel: function () {
                        ClearAddForm();
                        $(this).dialog("close");
                    }
            }
    });

    function sendAddCustomerToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/AddCustomer",
            dataType: "json",
            data: { 'data': JSON.stringify(tambahCustomer) },
            success: onAddCustomerSuccess,
            error: onUpdateCustomerError
        });
    }

    function onAddCustomerSuccess(data, status) {
        addLocalModel();
        RenderViewListCustomer();
        ClearAddForm();
        $("#add-dialog").dialog("close");
    }

    function addLocalModel() {
        Customers.push(tambahCustomer);
    }

    function dialogEdit(id) {
        kode = $("#kode");
        nama = $("#nama");
        getCustomerForEdit(id);
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
                    UpdateExistingCustomer();
                },
                Cancel: function () {
                    ClearEditForm();
                    $(this).dialog("close");
                }
            }

        };
        $("#edit-dialog").dialog(options);
    }

    function sendUpdateCustomerToServer() {

        $.ajax({
            type: "GET",
            url: "/Cargo/UpdateCustomer",
            dataType: "json",
            data: { 'data': JSON.stringify(updateCustomerList) },
            success: onUpdateCustomerSuccess,
            error : onUpdateCustomerError
        });
    }

    function onUpdateCustomerError(request, status, error) {
       var message = $(request.responseText)[1].text;
       alert(message);

    }
    function onUpdateCustomerSuccess(data, status) {

        updateLocalModel();
        RenderViewListCustomer();
        ClearEditForm();
        $("#edit-dialog").dialog("close");
    }

    function updateLocalModel() {

        $.each(Customers, function (item) {

            if (Customers[item].Guid == updateCustomerList.Guid) {
                Customers[item] = updateCustomerList;
                return;
            }
        });
    }

    function getCustomerFromListCustomer(id) {
        for (var e in Customers) {
            var value = Customers[e];
            if (value.Guid == id) {
                return value;
            }
        }
        return null;
    }

    function getCustomerForEdit(id) {
        updateCustomerList = getCustomerFromListCustomer(id);
        setUpdateModelToEditCustomerDialog()
    }

    function setUpdateModelToEditCustomerDialog() {
        $("#kode").val(updateCustomerList.Kode);
        $("#nama").val(updateCustomerList.Nama);
    }

    function SaveNewCustomer() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        if (IsExistData()) {
            updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
        }
        else {
            setNewCustomerModel();
            sendAddCustomerToServer();
        }
    }

    function setNewCustomerModel() {
        tambahCustomer = new Object();
        tambahCustomer.Kode = $("#add_kode").val();
        tambahCustomer.Nama = $("#add_nama").val();
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
        for (var e in Customers) {
            var value = Customers[e];
            if (value.Kode == kode.val() && value.Nama == nama.val()) {
                found = true;
            }
        }
        return found;
    }

    function UpdateExistingCustomer() {
        bValid1 = checkNullValidate(kode, kode.val(), "Kode Tidak Boleh Kosong.");
        bValid2 = checkNullValidate(nama, nama.val(), "Nama Tidak Boleh Kosong.");
        if (!(bValid1 && bValid2)) {
            return;
        }
        updateCustomerList.Kode = $("#kode").val();
        updateCustomerList.Nama = $("#nama").val();
        sendUpdateCustomerToServer();
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




