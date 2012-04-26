var CommonErrorMessage = function () {
    this.show = function (msg) {
        var listFakturDiv = $('.listfaktur');
        if (!listFakturDiv) return;

        var children = listFakturDiv.parent().children();
        var existingErrDivContainer;
        for (var i = 0; i < children.length; i++) {
            if (children[i] && children[i].id == "errorMessageContainer") {
                existingErrDivContainer = children[i];
                break;
            }
        }
        if (!existingErrDivContainer) {
            createErrorMessageElement(msg).insertAfter($('.listfaktur'));
        }
        else {
            addError(msg);
        }
    }

    function addError(msg) {
        var errors = $("#errorMessageContainer").children();
        var sameError;
        for (var i = 0; i < errors.length; i++) {
            if (errors[i].firstChild.innerHTML === msg) {
                sameError = errors[i];
                break;
            }
        }

        if (!sameError) {
            createDivErrorMessage(msg).appendTo("#errorMessageContainer");
        }
    }

    function createErrorMessageElement(msg) {
        var divErrorMessageContainer = $('<div>', { 'class': 'errorMessageContainer', 'id': 'errorMessageContainer' });
        createDivErrorMessage(msg).appendTo(divErrorMessageContainer);
        return divErrorMessageContainer;
    }

    function createDivErrorMessage(msg) {
        var divErrorMessage = $('<div>', { id: 'commonError', 'class': 'commonError' });

        var spanErrorMessage = $('<span>', { id: 'spanErrorMessage' });
        if (msg)
            spanErrorMessage.text(msg);

        var buttonDeleteError = $('<div>', { id: 'deleteError', 'text': 'X' });
        buttonDeleteError.click(function () {
            $(this).parent().remove();
            var numberOfErrors = $("#errorMessageContainer").children().length;
            if (numberOfErrors == 0) {
                $("#errorMessageContainer").remove();
            }
        });

        spanErrorMessage.appendTo(divErrorMessage);
        buttonDeleteError.appendTo(divErrorMessage);

        return divErrorMessage;
    }
}