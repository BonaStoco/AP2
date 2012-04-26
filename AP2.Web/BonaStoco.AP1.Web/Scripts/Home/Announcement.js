function Announcement() {
    $.ajax({
        type: "GET",
        url: "/Home/GetNewInfo",
        dataType: "json",
        success: SetNewInfo
    });
}

function SetNewInfo(data) {
    $.each(data, function (item) {
    var tanggal = data[item].Tanggal;
    var headline = data[item].Headline;
    var story = data[item].Story;

    $("#announcement").append("<div id='headline' class='BorderOrange'>" + headline + "</div>" +
                              "<div id='story'>" + story + "</div>" +
                              "<div id='tanggal'>-" + formatDate(tanggal) + "-</div></div>");
    });
}

function formatDate(jsondate) {
    var someDate = new Date(+jsondate.replace(/\/Date\((-?\d+)\)\//gi, "$1"));
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth()+1;
    var yyyy = today.getFullYear();
    if(dd<10){dd='0'+dd}
    if(mm<10){mm='0'+mm}
    return dd+'/'+mm+'/'+yyyy;
}