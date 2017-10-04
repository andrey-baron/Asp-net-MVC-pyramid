; var jivoCustom = (function ($) {
    // приватная переменная
    var jivoWindow = $("#jivo-iframe-container","body");
    var jivoPreview = $(".globalClass_ET", "body");
    var openFlag = false;
    return { // методы доступные извне
        closeWindow: function () {
            jivoWindow.hide();
            openFlag = false;
        },
        open: function () {
            jivoWindow.show();
            jivo_api.open();
            openFlag = true;
        },
        isOpen: function () {
            return openFlag;
        }
         
    }
}($));