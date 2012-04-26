var StockOpnameHubs = (function () {
    var updateHeaderTarget;
    var updateItemTarget;
    var addItemTarget;
    var itemAdded;
    var stockOpnameId;

    function updateHeader(data) {
        if (!updateHeaderTarget) return;
        updateHeaderTarget(data);
    }

    function updateItem(data) {
        //   if (!updateItemTarget) return;
        //   updateItemTarget(data);
        ItemUpdater(data);
    }

    function addItem(data) {
        if (addItemTarget) return;
        addItemTarget(data);
    }
    function showError(error) {
        CommonErrorHandler.show(error);
    }

    function registerUpdateHeader(f) {
        updateHeaderTarget = f;
    }

    function registerUpdateItem(f) {
        updateItemTarget = f;
    }

    function registerAddItem(f) {
        addItemTarget = f;
    }

    function unreg(stockOpnameId) {
        stockOpnameHub.unreg(stockOpnameId);
        stockOpnameId = null;
    }
    function register(stockOpnameId) {
        stockOpnameHub.register(stockOpnameId);
        stockOpnameId = stockOpnameId;
    }

    $.connection.hub.url = "http://localhost:51850/signalR";
    var stockOpnameHub = $.connection.stockOpnameHub;
    stockOpnameHub.updateHeader = updateHeader;
    stockOpnameHub.updateItem = updateItem;
    stockOpnameHub.itemAdded = itemAdded
    stockOpnameHub.showError = showError;
    $.connection.hub.start();

    return { registerUpdateHeader: registerUpdateHeader,
        register: register,
        registerAddItem: registerAddItem,
        unreg: unreg
    };
} ());

