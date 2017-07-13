(function ($) {
   /* $(".product__item").mouseover(function (e) {

        $(this).css({"text-align": "left"});
        $(this).children(".product__item-img").stop().animate({
            width:"100px",
        },300,function (e) {
            $(".product__item-title").css({"width":"230px"});
        });

    });

    $(".product__item").mouseout(function (e) {
        $(this).css({"text-align": "left"});
        $(this).children(".product__item-img").stop().animate({
            width:"100%",
        },300,function (e) {
            $(".product__item-title").css({"width":"inherit"});
        });

    });*/

    var singleProductId = $('#single-productid').val();

    $('#single-bxslider').bxSlider({
        pagerCustom: '#single-bx-pager'
    });
    $(".js-single-spinner").spinner({
        min:1,
    });

    $("#save-btn-single-review").on("click", function () {
        var form = $('#FormAddReview');
        var serializeData = form.serialize();
        $.post("/Review/AddReview/", serializeData, function (data) {
            if (data.Status == "Ok") {

            }
        });
    })

    $(".single__add-val-item_open-link").on("click", function () {
        $(".single__add-val-item_additional").toggle("slow");
        var sign = $(this).find(".single__sign-all");
        if (sign.text() == "+") {
            sign.text("-");
        } else {
            sign.text("+");
        }
    })
})($);