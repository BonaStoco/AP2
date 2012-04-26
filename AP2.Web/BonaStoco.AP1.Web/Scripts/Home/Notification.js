// Notification Barang Baru
function LoadProductPending() {
    $.ajax({
        type: "GET",
        url: "/RequestProduct/CountPendingRequestedProduct",
        dataType: "json",
        success: InsertProductPendingToTable
    });
}

function InsertProductPendingToTable(data) {
    if (data > 0) {
        $("#DataNotification").append("<div class='notif BorderOrange' id='BarangPending'>Total Barang Pending (<span class='RedColor'>" + data + "</span>)</div>");
        $("#BarangPending").click(RedirectToVerifikasiBarangBaru).css('cursor', 'pointer');
    }
}

function RedirectToVerifikasiBarangBaru() {
    document.location ="../RequestProduct/Index";
}

// Notification Pengiriman Barang Barang

function LoadPendingGRN() {
    $.ajax({
        type: "GET",
        url: "/VerifikasiPengirimanBarang/FindAllPendingGRN",
        dataType: "json",
        success: InsertGRNPendingToTable
    });
}
function InsertGRNPendingToTable(data) {
    if (data.length > 0) {
        $("#DataNotification").append("<div class='notif BorderOrange' id='GRNPending'>Total Pengiriman Pending (<span class='RedColor'>" + data.length + "</span>)</div>");
        $("#GRNPending").click(RedirectToVerifikasiPengirimanBarang).css('cursor', 'pointer');
    }
}
function RedirectToVerifikasiPengirimanBarang() {
    CreateModalDialogGRN("Daftar Pengiriman Barang Pending");
    $.ajax({
        type: "GET",
        url: "/VerifikasiPengirimanBarang/FindAllPendingGRN",
        dataType: "json",
        success: InsertDataGRNToTable
    });
}

// Notification Retur Barang
function LoadPendingRET() {
    $.ajax({
        type: "GET",
        url: "/VerifikasiReturBarang/FindAllPendingRET",
        dataType: "json",
        success: InsertRETPendingToTable
    });
}
function InsertRETPendingToTable(data) {
    if (data.length > 0) {
        $("#DataNotification").append("<div class='notif BorderOrange' id='RETPending'>Total Retur Pending (<span class='RedColor'>" + data.length + "</span>)</div>");
        $("#RETPending").click(RedirectToVerifikasiReturBarang).css('cursor', 'pointer');
    }
}
function RedirectToVerifikasiReturBarang() {
    CreateModalDialogGRN("Daftar Retur Barang Pending");
    $.ajax({
        type: "GET",
        url: "/VerifikasiReturBarang/FindAllPendingRET",
        dataType: "json",
        success: InsertDataRETToTable
    });
}