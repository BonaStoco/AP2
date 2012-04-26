var listPartGroup;
var editedPartGroupModel;
var newPartGroup;
var tips;
var Kode;
var Nama;
var bValid1;
var bValid2;

$(document).ready(function () {
    loadListPartGroup();
});

$("#dialog-overlay").ajaxStart(function () {
    $(this).show();
}).ajaxStop(function () {
    $(this).hide();
});

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
    for (var e in listPartGroup) {
        var value = listPartGroup[e];
        if (value.Kode == Kode.val() && value.Nama == Nama.val()) {
            found = true;
        }
    }
    return found;
}

function loadListPartGroup()
{
     $.ajax({
                type: "GET",
                url: "/PartGroup/FindListPartGroupByTenanId",
                dataType: "json",
                success: loadAllListPartGroup,
                error: onErrorLoadingPartGroup
           });
}

function onErrorLoadingPartGroup(request, status, error) {
    var message = $(request.responseText)[1].text;
    alert(message);
}

function loadAllListPartGroup(data, status) {
    listPartGroup = data;
    RenderingPartGroup(listPartGroup);

}

function RenderingPartGroup() {
    ClearListView();
    $.each(listPartGroup, function(item) {
        $("#users tbody").append("<tr>" +
			"<td>" + listPartGroup[item].Kode + "</td>" +
			"<td>" + listPartGroup[item].Nama + "</td>" +
            "<td id='Edit' onclick='editGroup(\"" + listPartGroup[item].ModelGuid + "\")'></td>" +
		"</tr>");
    });
}

function getPartGroupFromList(id) {
    for (var e in listPartGroup) {
        var value = listPartGroup[e];
        if (value.ModelGuid == id) {
            return value;
        }
    }
    return null;
}

function initPartGroupForEdit(id) {
    editedPartGroupModel = getPartGroupFromList(id);
    setPartGroupValueToEditor();
}

function setPartGroupValueToEditor() {
    Kode = $("#edit-kode");
    Nama = $("#edit-nama");
    if (!(editedPartGroupModel == null)) {
        Kode.val(editedPartGroupModel.Kode);
        Nama.val(editedPartGroupModel.Nama);
    }
    else {
        alert("Silakan muat ulang halaman ini karena terjadi kesalahan");
        return;
        Kode.val("");
        Nama.val("");
    }
}

function editGroup(id) {
    
    initPartGroupForEdit(id);
    showEditDialog();
}

function showEditDialog() {
    var options = {
        autoOpen: true,
        height: 300,
        width: 350,
        modal: true,
        buttons: {
            "Update": UpdateExistingPartGroup,
            "Cancel": function () {
                clearEditForm();
                $(this).dialog("close");
            }
        }
    };
    $("#edit-dialog").dialog(options);
}

function UpdateExistingPartGroup() {
    bValid1 = checkNullValidate(Kode, Kode.val(), "Kode Tidak Boleh Kosong.");
    bValid2 = checkNullValidate(Nama, Nama.val(), "Nama Tidak Boleh Kosong.");
    if (!(bValid1 && bValid2)) {
        return;
    }
    editedPartGroupModel.Kode = $("#edit-kode").val();
    editedPartGroupModel.Nama = $("#edit-nama").val();
    RequestUpdatePartGroup();    
}

function RequestUpdatePartGroup() 
{
    $.ajax({
        type: "GET",
        url: "/PartGroup/UpdatePartGroup",
        dataType: "json",
        data: { 'partGroup': JSON.stringify(editedPartGroupModel) },
        success: onUpdatePartGroupSuccess,
        error: onUpdateOrAddPartGroupError
    });
}

function onUpdatePartGroupSuccess() {
    updateLocalListModel();
    RenderingPartGroup();
    clearEditForm();
    $("#edit-dialog").dialog("close");
}

function onUpdateOrAddPartGroupError(request, status, error) {
    var message = $(request.responseText)[1].text;
    alert(message);
}

function updateLocalListModel() {

    $.each(listPartGroup, function (item) {

        if (listPartGroup[item].ModelGuid == editedPartGroupModel.ModelGuid) {
            listPartGroup[item] = editedPartGroupModel;
            return;
        }
    });
}

function ClearListView() {
    var table = document.getElementById("users");
    while (table.rows.length > 1) {
        table.deleteRow(1);
    }
}

function showAddPartGroupDialog() {
    var options = {
        autoOpen: true,
        height: 300,
        width: 350,
        modal: true,
        buttons: {
            "Add": SaveNewPartGroup,
            "Cancel": function () {
                clearAddForm();
                $(this).dialog("close");
            }
        }
    };
    $("#add-dialog").dialog(options);
}

function addPartGroup() {
    Kode = $("#add-kode");
    Nama = $("#add-nama");
    showAddPartGroupDialog();
}

function SaveNewPartGroup() 
{
    bValid1 = checkNullValidate(Kode, Kode.val(), "Kode Tidak Boleh Kosong.");
    bValid2 = checkNullValidate(Nama, Nama.val(), "Nama Tidak Boleh Kosong.");
    if (!(bValid1 && bValid2)) {
        return;
    }
    if (IsExistData()) {
        updateTips("Data Yang Anda Masukkan Sudah Terdaftar");
    }
    else {
        setNewPartGroupModel();
        RequestNewPartGroup();
    }
}
function setNewPartGroupModel() {
    newPartGroup = new Object();
    newPartGroup.Kode = Kode.val();
    newPartGroup.Nama = Nama.val();
}

function addLocalPartGroupModel() {
   listPartGroup.push(newPartGroup);
}

function RequestNewPartGroup() {
    $.ajax({
        type: "GET",
        url: "/PartGroup/AddPartGroup",
        dataType: "json",
        data: { 'partGroup': JSON.stringify(newPartGroup) },
        success: onAddPartGroupSuccess,
        error: onUpdateOrAddPartGroupError   
    });
}

function onAddPartGroupSuccess(data,status) {
    addLocalPartGroupModel();
    RenderingPartGroup();
    clearAddForm();
    $("#add-dialog").dialog("close");
}

function clearAddForm() {
    $("p").text("");
    $("p").removeClass("validateTips");
    Kode.removeClass("ui-state-error");
    Nama.removeClass("ui-state-error");
    $("#add-kode").val("");
    $("#add-nama").val("");
    $("#add-model-guid").val("");
    $("#add-tenant-id").val("");
}

function clearEditForm() {
    $("p").text("");
    $("p").removeClass("validateTips");
    Kode.removeClass("ui-state-error");
    Nama.removeClass("ui-state-error");
    $("#edit-kode").val("");
    $("#edit-nama").val("");
}





