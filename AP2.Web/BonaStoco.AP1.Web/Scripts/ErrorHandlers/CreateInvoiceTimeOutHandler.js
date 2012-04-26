var CreateInvoiceTimeOutHandler = (function () {
    var _guid;
    var _timeout = 60000;
    var queueLoadingElement = [];

    function handleException(guid) {
        _guid = guid;
        registerQueueLoadingElement(addLoading("Sedang proses..."));
    }

    function registerQueueLoadingElement(f) {
        if (f)
            queueLoadingElement.push(f);
    }

    function addLoading(msg) {
        var elementLoading = new ElementLoading();
        elementLoading.show(msg);
        var timeoutId = setTimeout(showErrorMessage, _timeout);

        var objException = new Object();
        objException.elementLoading = elementLoading;
        objException.timeoutId = timeoutId;
        return objException;
    }

    function disposeLoadingElement() {
        if (queueLoadingElement.length > 0) {
            queueLoadingElement[0].elementLoading.destroy();
            if (queueLoadingElement[0].timeoutId)
                clearTimeout(queueLoadingElement[0].timeoutId);
            if ($("#FailGetInvoice"))
                $("#FailGetInvoice").remove();
            queueLoadingElement.splice(0, 1);
            if (queueLoadingElement.length == 0) {
                $("#loadingElementContainer").remove();
            }
        }
    }

    function showErrorMessage() {
        var buttons = new Object();
        buttons[0] = new Object();
        buttons[0].Name = "Abaikan";
        buttons[0].Value = "ignore";
        buttons[1] = new Object();
        buttons[1].Name = "Ulangi";
        buttons[1].Value = "retry";
        errorMessage =
                            new ErrorMessage("FailGetInvoice",
                                             "Server sedang sibuk, ulang lagi?",
                                             buttons, function (data) {
                                                 if (data == "retry") {
                                                     setTimeout(showErrorMessage, _timeout);
                                                 }
                                                 else {
                                                     disposeLoadingElement();
                                                     invoiceHubs.unreg(_guid);
                                                 }
                                             });
        errorMessage.show();
    }

    return { handleException: handleException, disposeLoadingElement: disposeLoadingElement };
} ());