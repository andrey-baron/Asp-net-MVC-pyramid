var jivoCustom = (function ($) {
    // приватная переменная
    var jivoWindow = $("#jivo-iframe-container", "body");
    var jivoPreview = $(".globalClass_ET", "body");
    //jivoPreview.hide();
    var openFlag = false;
    return { // методы доступные извне
        closeWindow: function () {
            //jivoWindow.css({
            //    "z-index": "-100",
            //    opacity: 0
            //});
            openFlag = false;
            jivo_api.close();
            //jivoPreview.hide();
        },
        open: function () {
            //jivoWindow.show();
            jivo_api.open();
            openFlag = true;
            //jivoWindow.css({
            //    "z-index": "1",
            //    opacity: 1
            //});
        },
        isOpen: function () {
            return openFlag;
        },
        init: function () {
            jivoWindow = $("#jivo-iframe-container", "body");
            jivoPreview = $(".globalClass_ET", "body");
            //jivoPreview.hide();
            openFlag = false;
        }

    }
}($));

var jivo_onLoadCallback = function () {
    jivoCustom.init();
    $(".header__consultant").on("click", function () {
        if (jivoCustom.isOpen() == true) {
            jivoCustom.closeWindow();
        } else {
            jivoCustom.open();
        }
    });
};
var jivo_onClose = function () {
    //jivoCustom.closeWindow();
};
; (function () {

    $(".feedback-form").validate({
        rules: {
            "feedback.Name": {
                required: true
            },
            "feedback.Phone": {
                required: true,
            },
            Confirm_terms: {
                required: true,
            }
        },
        messages: {
            "feedback.Name": {
                required: "Введите имя"
            },
            "feedback.Phone": {
                required: "Введите Телефон"
            },
            Confirm_terms: {
                    required: "Необходимо согласие"
                }
        }
    

        }
    );

    $(".checkout-form, .checkout-form-one-click").validate({
        rules: {
            Name: { 
                required: true
            },
            Phone: {
                required: true,
            },
            Confirm_terms: {
                required: true,
            }
        },
            messages: {
                Name: {
                    required: "Введите имя"
                },
                Phone: {
                    required: "Введите Телефон"
                },
                Confirm_terms: {
                    required: "Необходимо согласие"
                }
            }

    });
    $(".feedback-form").on("submit", function (e) {
        e.preventDefault();
        $(this).validate();
        var formRes = $(this).serialize();
        var amountError = $("input.error", $(this));
        if (amountError.length == 0) {
            $.post("/FeedBack/Send", formRes, function (data) {
                Notify.generate('', 'Заявка отправленна', 1);
            });
            $("input", $(this)).val("");
        }

    });
    $(".checkout-form").on("submit", function (e) {
        e.preventDefault();
        $(this).validate();
        var formRes = $(this).serialize();
        var amountError = $("input.error", $(this));
        if (amountError.length == 0) {
            $.post("/Cart/Checkout", formRes, function (data) {
                //$(".js-result-insert").html(data);
                Notify.generate('', 'Заявка отправленна', 1);
            });
            $("input", $(this)).val("");
        }

    })
    $(".checkout-form-one-click").on("submit", function (e) {
        e.preventDefault();
        $(this).validate();
        var formRes = $(this).serialize();
        var amountError = $("input.error", $(this));
        if (amountError.length == 0) {
            $.post("/Cart/CheckoutOneClick", formRes, function (data) {
                //$(".js-result-insert").html(data);
                Notify.generate('', 'Заявка отправленна', 1);
            });
            $("input", $(this)).val("");
        }

    })
})();
