; (function () {
    $(".feedback-form").validate({
        rules: {
            "feedback.Name": {
                required: true
            },
            "feedback.Phone": {
                required: true,
            },
        messages: {
            "feedback.Name": {
                required: "Введите имя"
            },
            "feedback.Phone": {
                required: "Введите Телефон"
            },
        }

        }
    });

    $(".checkout-form").validate({
        rules: {
            Name: { 
                required: true
            },
            Phone: {
                required: true,
            },
            messages: {
                Name: {
                    required: "Введите имя"
                },
                Phone: {
                    required: "Введите Телефон"
                },
                
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
                Notify.generate('', 'Заявка отправленна', 1);
            });
            $("input", $(this)).val("");
        }

    })

})();