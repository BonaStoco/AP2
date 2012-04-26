var ErrorMessage = function (name, message, buttons, actionResult) {
    var dialogErrorMsg = $('<div>', { id: name, 'class': 'dialogErrorMsg' });
    var backgroudErrorMsg = $('<div>', { 'class': 'backgroudErrorMsg' });
    var dialogAreaErrorMsg = $('<div>', { 'class': 'dialogAreaErrorMsg' });
    var bodyErrorMsg = $('<div>', { text: message, 'class': 'bodyErrorMsg' });
    var footer = $('<div>', { 'class': 'errorMsgFooter' });

    for (var i in buttons) {
        buttons[i] = $('<div>', { 'class': 'buttonErrorMsg ' + buttons[i].Value + '', text: buttons[i].Name, 'value': buttons[i].Value });
    }

    this.show = function () {
        var dialogErrors = $('.dialogErrorMsg');
        if (dialogErrors.length > 0)
            dialogErrors.remove();

        dialogErrorMsg.appendTo(document.body);
        backgroudErrorMsg.appendTo(dialogErrorMsg);
        dialogAreaErrorMsg.appendTo(dialogErrorMsg);
        bodyErrorMsg.appendTo(dialogAreaErrorMsg);
        footer.appendTo(bodyErrorMsg);
        for (var i in buttons) {
            buttons[i].appendTo(footer);
        }
        triggerEvent();


        var winH = $(window).height();
        var winW = $(window).width();
        $(dialogAreaErrorMsg).css('top', winH / 2 - dialogAreaErrorMsg.height() / 2);
        $(dialogAreaErrorMsg).css('left', winW / 2 - dialogAreaErrorMsg.width() / 2);

        dialogErrorMsg.show();
    }
    var triggerEvent = function () {
        $(".buttonErrorMsg").click(function (ev) {
            dialogErrorMsg.remove();
            actionResult($(this).attr('value'));
        });
    }

    this.Destroy = function () {
        dialogErrorMsg.remove();
    }
}