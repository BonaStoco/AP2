var dataMonitoring;
var chart;
var Day = new Array();
var Active = new Array();
var NonActive = new Array();
var tahun;
var bulan;
var _bandara;
var date;


//function OpenDialogAktif() {

//    $("#DialogAktif").dialog({
//        autoOpen: false,
//        width: 1000,
//        height: 450,
//        modal: true
//    });
//}

//function OpenDialogNonAktif() {

//    $("#DialogNonAktif").dialog({
//        autoOpen: false,
//        width: 1000,
//        height: 450,
//        modal: true
//    });
//}

function LoadDataUsingAjax(bandara) {
    _bandara = bandara;
    $.ajax({
        type: "GET",
        url: "/MonitoringDashBoard/MonitoringDashBoardByPeriode",
        data:{"bandara":bandara},
        dataType: "json",
        success: showStatesDataMonitoring
    });
}
function showStatesDataMonitoring(data, status) {
    dataMonitoring = data;
    SortDataByDate();
//    showDataInTable(dataMonitoring);
    updateDetailMonitoring(dataMonitoring);
}

function SortDataByDate() {
    $.each(dataMonitoring, function (item) {
        date = dataMonitoring[item].DateString;
        Day.push(dataMonitoring[item].Day);
        //month.push(dataMonitoring[item].Month);
        Active.push([date, dataMonitoring[item].Active]);
        NonActive.push([date, dataMonitoring[item].NonActive]);
    });
}

//function showDataInTable(dataMonitoring) {
//    var i = 1;
//    $.each(dataMonitoring, function (data) {
//        $('#aktif' + dataMonitoring[data].Day).text(dataMonitoring[data].Active).click(function () { showdialog(dataMonitoring[data].Day, bulan, tahun, bandara); }).css('cursor', 'pointer')
//        $('#nonaktif' + dataMonitoring[data].Day).text(dataMonitoring[data].NonActive).click(function () { ShowDialogNonAktif(dataMonitoring[data].Day, bulan, tahun, bandara); }).css('cursor', 'pointer')
//    }
//    );
//}

//function showdialog(hari, bulan, tahun, bandara) {
//    $("#IsiDialogAktif").empty();
//    $("#IsiDialogAktif").append("<table id='aktif' width='100%' style='font-size:12px;'><thead id = 'head'><tr><th>Tenan Id</th><th>Nama Tenan</th><th>Gate</th></tr></thead>" +
//                       "<tbody id='tenanaktif'></tbody></table>");
//    $("#hari").val(hari);
//    $("#bulan").val(bulan);
//    $("#tahun").val(tahun);
//    $("#bandara").val(bandara);
//    $.ajax({
//        type: "GET",
//        url: "/MonitoringDashBoard/DetailTenanAktif",
//        data: { 'hari': hari, 'bulan' : bulan, 'tahun' : tahun, 'bandara' : bandara},
//        dataType: "json",
//        success: showTenanAktifDialog
//    });

//}

//function showTenanAktifDialog(data) {
//    $("#DialogAktif").dialog("open");
//    if(data == null || data.length == 0)
//        return null;
//    $.each(data, function(item) {
//        $("#tenanaktif").append("<tr>" +
//                                "<td>" + data[item].TenanId + "</td>" +
//                                "<td>" + data[item].TenanName + "</td>" +
//                                "<td>" + data[item].Gate + "</td>" +
//                                "</tr>");
//    });
//}

//function ShowDialogNonAktif(hari, bulan, tahun, bandara) {
//    $("#IsiDialogNonAktif").empty();
//    $("#IsiDialogNonAktif").append("<table width='100%' style='font-size:12px;'><thead><tr><th>Tenan Id</th><th>Nama Tenan</th><th>Gate</th></tr></thead>" +
//                       "<tbody id='tenannonaktif'></tbody></table>");
//    $("#hari1").val(hari);
//    $("#bulan1").val(bulan);
//    $("#tahun1").val(tahun);
//    $("#bandara1").val(bandara);
//    $.ajax({
//        type: "GET",
//        url: "/MonitoringDashBoard/DetailTenanNonAktif",
//        data: { 'hari': hari, 'bulan': bulan, 'tahun': tahun, 'bandara': bandara },
//        dataType: "json",
//        success: showTenanNonAktifDialog
//    });
//}
//function showTenanNonAktifDialog(data) {
//    $("#DialogNonAktif").dialog("open");
//    if (data == null || data.length == 0)
//        return null;
//    $.each(data, function (item) {
//        $("#tenannonaktif").append("<tr>" +
//                                "<td>" + data[item].TenanId + "</td>" +
//                                "<td>" + data[item].TenanName + "</td>" +
//                                "<td>" + data[item].Gate + "</td>" +
//                                "</tr>");
//    });
//}

//function getMonthName(bulan) {
//    monthName = new Array("JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER");
//    return monthName[bulan];
//}

function updateDetailMonitoring(dataMonitoring) {
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'container'
        },

        title: {
            text: ''
        },

        xAxis: {
            type: 'datetime',
            tickInterval: 7 * 24 * 3600 * 1000, // one week
            tickWidth: 0,
            gridLineWidth: 1,
            labels: {
                align: 'left',
                x: 3,
                y: 3
            }
        },

        yAxis: [{ // left y axis
            title: {
                text: null
            },
            labels: {
                align: 'left',
                x: -10,
                y: 0,
                formatter: function () {
                    return Highcharts.numberFormat(this.value, 0);
                }
            },
            showFirstLabel: false
        }],

        legend: {
            align: 'left',
            verticalAlign: 'top',
            y: 0,
            x: 20,
            floating: true,
            borderWidth: 0
        },

        tooltip: {
            formatter: function () {
                return '<b>' + this.series.data[this.x].name + '</b><br/>' +
               this.series.name + ': ' + this.y + ' Tenant';
            }
        },

        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            ShowDataTenan(this.series.data[this.x].name, this.series.name,_bandara);
                        }
                    }
                },
                marker: {
                    lineWidth: 1
                }
            }
        },

        series: [{
            name: 'Active',
            lineWidth: 2,
            marker: {
                radius: 4
            },
            data: Active
        }, {
            name: 'Non Active',
            data: NonActive
        }]
    });
}

function ShowDataTenan(date, status, bandara) {
    if (status.toLowerCase() == "active") {
        CreateModalDialog("List Tenan Active");
        $.ajax({
            type: "GET",
            url: "/MonitoringDashBoard/GetTenanAktifHome",
            data:{"date": date, "bandara":bandara},
            dataType: "json",
            success: InsertTenanToTable
        });
    }
    else {
        CreateModalDialog("List Tenan Non Active");
        $.ajax({
            type: "GET",
            url: "/MonitoringDashBoard/GetTenanNonAktifHome",
            data: { "date": date, "bandara": bandara },
            dataType: "json",
            success: InsertTenanToTable
        });
    }
}