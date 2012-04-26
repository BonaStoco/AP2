var TimeOutHandler = (function () {
    var queueLoadingElement = [];
    var _timeout = 60000;

    function handleException(msg) {
        if (!msg)
            msg = "Sedang Proses...";

        registerQueueLoadingElement(addLoading(msg));
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
            queueLoadingElement.splice(0, 1);
            if (queueLoadingElement.length == 0) {
                $("#loadingElementContainer").remove();
            }
        }
    }

    function showErrorMessage() {
        var buttons = new Object();
        buttons[1] = new Object();
        buttons[1].Name = "Tutup";
        buttons[1].Value = "retry";
        errorMessage = new ErrorMessage("EventUnsuccess",
                                        "Koneksi sedang bermasalah, namun faktur anda tetap diproses.",
                                        buttons, function (data) {
                                            if (data == "retry") {
                                                errorMessage.Destroy();
                                            }
                                        });
        errorMessage.show();
    }

    return { handleException: handleException, disposeLoadingElement: disposeLoadingElement };
} ());

