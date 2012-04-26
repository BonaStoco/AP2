var CommonErrorHandler = (function () {
    var disposeHandlers = [];
    function show(error) {
        var errorMessage = new CommonErrorMessage();
        var message;
        if (error.ErrorMessage) {
            if (error.Username) {
                message = error.ErrorMessage + " (by : " + error.Username + ")";
            }
            else {
                message = error.ErrorMessage;
            }
        } else {
            message = error;
        }
        errorMessage.show(message);
        TimeOutHandler.disposeLoadingElement();

        //        while (disposeHandlers.length > 0) {
        //            disposeHandlers.pop()();
        //        }
    }

    //    function registerDisposeHandler(f) {
    //        if (f)
    //            disposeHandlers.push(f);
    //    }

    return { show: show };
} ());