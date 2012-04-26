$(function () {
    window.setInterval(function () {
        var timeCounter = $("#timeCounter").html();
        var updateTime = eval(timeCounter) - eval(1);
        $("#timeCounter").html(updateTime);
        if (updateTime == 0) {
            window.location = ("/Account/LogOff");
        }
    }, 1000);
});


function RefreshSessionTimeOut(TimeSessionTimOut) {
    $('#timeCounter').html(TimeSessionTimOut);
};