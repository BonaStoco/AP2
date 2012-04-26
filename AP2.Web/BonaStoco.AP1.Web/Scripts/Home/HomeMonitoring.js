var Active = new Array();
var NonActive = new Array();
var ActiveNonTransaction = new Array();
var Day = new Array();
var month = new Array();
var date;
var _bandara;

function GetDataMonitoring(bandara) {
    _bandara = bandara;
    $.ajax({
        type: "GET",
        url: "/MonitoringDashBoard/MonitoringDashBoardByPeriode",
        data: {"bandara" : bandara},
        dataType: "json",
        success: ShowDataMonitoring
    });
}

function ShowDataMonitoring(data) {
    SetData(data);
    DetailMonitoring(_bandara);
}

function SetData(data) {
    $.each(data, function (item) {
        date = data[item].DateString;
        Day.push(data[item].Day);
        month.push(data[item].Month);
        Active.push([date, data[item].Active]);
        NonActive.push([date, data[item].NonActive]);
        ActiveNonTransaction.push([date, data[item].ActiveNonTransaction]);
    });
    return data;
}

//function getMonthName(bulan) {
//    monthName = new Array("JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER");
//    return monthName[bulan];
//}

function DetailMonitoring(dataMonitoring) {
    chart = new Highcharts.Chart({
        chart: {
            renderTo: 'DataMonitoring'
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
            lineWidth: 2,
            marker: {
                radius: 4
            },
            data: NonActive
        },
        {
            name: 'Active Non Transaction',
            lineWidth: 2,
            marker: {
                radius: 4
            },
            data: ActiveNonTransaction
        }]
    });
}

function ShowDataTenan(date, status, bandara) {
    if (status.toLowerCase() == "active") {
        $.ajax({
            type: "GET",
            url: "/MonitoringDashBoard/GetTenanAktifHome",
            data: { "date": date, "bandara": bandara },
            dataType: "json",
            success:InsertTenanToTable            
        });
         CreateModalDialog("List Tenan Active");
        
    }
    else if (status.toLowerCase() == "non active") {       
        $.ajax({
            type: "GET",
            url: "/MonitoringDashBoard/GetTenanNonAktifHome",
            data: { "date": date, "bandara": bandara },
            dataType: "json",
            success: InsertTenanToTable

        });
        CreateModalDialog("List Tenan Non Active");
       
    } else {

    $.ajax({
        type: "GET",
        url: "/MonitoringDashBoard/GetTenanActiveNonTransactionHome",
        data: { "date": date, "bandara": bandara },
        dataType: "json",
        success: InsertTenanToTable               
             
    });
    CreateModalDialog("List Tenan Active Non Transaction");
    }
}