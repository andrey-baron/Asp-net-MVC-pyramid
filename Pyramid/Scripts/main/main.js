var jivoCustom = (function ($) {
    // приватная переменная
    var jivoWindow = $("#jivo-iframe-container", "body");
    var jivoPreview = $(".globalClass_ET", "body");
    jivoPreview.hide();
    var openFlag = false;
    return { // методы доступные извне
        closeWindow: function () {
            jivoWindow.css({
                "z-index": "-100",
                opacity: 0
            });
            openFlag = false;
            jivo_api.close();
            jivoPreview.hide();
        },
        open: function () {
            jivoWindow.show();
            jivo_api.open();
            openFlag = true;
            jivoWindow.css({
                "z-index": "1",
                opacity: 1
            });
        },
        isOpen: function () {
            return openFlag;
        },
        init: function () {
            jivoWindow = $("#jivo-iframe-container", "body");
            jivoPreview = $(".globalClass_ET", "body");
            jivoPreview.hide();
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
    jivoCustom.closeWindow();
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



(function () {

    $(".call-to-action__form-wrap").hide();
    $(".call-to-action").on("click",function () {
        $(this).next(".call-to-action__form-wrap").toggle( 400 );

    });
})();
;(function () {
    var maxVal=$("#MaxPrice").val();
    var minVal = $("#MinPrice").val();

    var curmaxVal = $("#CurrentMaxPrice").val();
    var curminVal = $("#CurrentMinPrice").val();
    $(".sliderUI").slider({
        
        max:maxVal ,
        //min: minVal,
        range: true,
        values: [curminVal, curmaxVal == 0 ? maxVal : curmaxVal],
        step:1,
        start: function (event, ui) {
            var values= $( ".sliderUI" ).slider( "values" );
            $(".spinnerUI1").spinner("value", values[0]);
            $(".spinnerUI2").spinner("value", values[1]);
        },
        change: function (event, ui) {
            var values = $(".sliderUI").slider("values");
            $(".spinnerUI1").spinner("value", values[0]);
            $(".spinnerUI2").spinner("value", values[1]);
        }
    });
     
    $(".spinnerUI1").spinner({
        incremental: true,
               
        numberFormat: "n",
        change: function (event, ui) {
           
            $(".sliderUI").slider("values", 0, this.value);
        }
    });
    $(".spinnerUI2").spinner({
        incremental: false,


        numberFormat: "n",
        change: function (event, ui) {
            $(".sliderUI").slider("values", 1, this.value);
        }
    });

    $(".spinnerUI1").spinner("value", curminVal);

    $(".spinnerUI2").spinner("value", curmaxVal == 0 ? maxVal : curmaxVal);

    $(".subcategory__list-item-link_open").on("click", function (e) {
        var ulList = $(this).closest(".subcategory__list");
        var additional= ulList.children(".subcategory__list-item_additional");
        additional.toggle("slow");
        var sign = $(this).find(".subcategory__sign");
        if (sign.text()=="+") {
            sign.text("-");
        } else {
            sign.text("+");
        }
    })

   
})();

;
var isPartial = $(".cart__container").hasClass("cart__container_small") ? true : false;
var isFullPage = $(".cart__container").hasClass("cart__container_full") ? true : false;

var reloadUrl =  "/Cart/PartialGetCard" ;
var reloadUrlPartial =  "/Cart/PartialShortCart";
(function () {
    
    $(".cart__container").on("click", ".cart__remove-item", function () {
        var productId = $(this).data("ajaxid");
        $.post("/Cart/RemoveItem/" + productId, function (removeData) {
            UpdateCartPoint(removeData);
            if (isPartial) {
                $.post(reloadUrlPartial, function (data) {
                    UpdateShortCart(data);
                })
            }
            if (isFullPage) {
                $.post(reloadUrl, function (data) {
                    UpdateCart(data);
                })
            }
            
        })
    });
  

    $(".cart__container").on("spinstop",".js-cart-spinner", function (event, ui) {
        var ajaxid = $(this).data("ajaxid");
        quantity = $(this).spinner( "value" );
        $.post("/Cart/SetQuantityToCart?productId=" + ajaxid + "&quantity=" + quantity, function (data) {
            SoccessAdd(data);
        })
    });
  
    $(".js-add-to-cart").on("click", function (e) {
        var ajaxid = $(this).data("ajaxid");
        var ajaxtitle = $(this).data("ajaxtitle");
        var singleQuantity = $(".js-single-spinner");
        var quantity = 1;
        if (singleQuantity.length!=0) {
            quantity = $(singleQuantity[0]).val();
        }
        $.post("/Cart/AddToCart?productId="+ajaxid+"&quantity="+quantity, function (data) {
            SoccessAdd(data); 
        })
        Notify.generate('', 'Товар успешно добавлен в корзину', 1);
    });

})();

function UpdateCartPoint(responseObj) {
    $(".header__count-items").html(responseObj["AllAmount"]);
}
function UpdateShortCart(data) {
    $(".cart__container_small").html(data);
    $(".js-cart-spinner").spinner({
        min: 1,
    });
}
function UpdateCart(data) {
    $(".cart__container_full").html(data);
    $(".js-cart-spinner").spinner({
        min: 1,
    });
}
function SoccessAdd(response) {
    if (isPartial) {
        $.post(reloadUrlPartial, function (data) {
            UpdateShortCart(data);
        })
    }
    if (isFullPage) {
        $.post(reloadUrl, function (data) {
            UpdateCart(data);
        })
    }
    UpdateCartPoint(response);
  
}
(function () {

    $('.ft-grid').masonry({
        itemSelector: '.ft-item',
        columnWidth: '.grid-sizer',
        percentPosition: true
    });

     //$(".footer__catalog-products").hide();
     $(".footer__switch").on("click",function () {
     $(".footer__catalog-products").toggle( 400 );
     });
})();
(function ($) {

    $('.home-slider__wrap').bxSlider({
        //mode: 'vertical',
        controls: true,
        auto: true, 
        slideMargin: 0,
        infiniteLoop: true,
        /*nextText: '',
        prevText: '',*/
        pager: true,
    });
}($));

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
        if ($('textarea[Name="Content"]', form).val() == '') {
            showSubmitResult(form, true, "Напишите отзыв");
            return;
        }
        if ($('input[Name="Name"]', form).val() == '') {
            showSubmitResult(form, true, 'Введите имя');
            return; 
        }
        form.find("#review-text")
       
            var serializeData = form.serialize();
            $.post("/Review/AddReview/", serializeData, function (data) {
                showSubmitResult(form, false, 'Отзыв отправлен');
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

function showSubmitResult(form, wasError, message) {
    var elem = $('.result-info', form);
    elem.stop().css('opacity', 0).val(message);
    if (wasError) {
        elem.addClass('error');
    } else {
        elem.removeClass('error');
    }
    elem.html(message);
    elem.animate({ opacity: 1 }, 500);
    if (!wasError) {
        $('input', form).val('');
        $('textarea', form).val('');
        setTimeout(function () {
            $.magnificPopup.close();
        }, 5000);
    }
}
;(function () {
$(".js-toggle-content").on("click", function (e) {
    var parent = $(this).parent();
    var isOpenClass = "seo__content-wrap_is-open";
    if (parent.hasClass(isOpenClass)) {
        parent.removeClass(isOpenClass);
    } else {
        parent.addClass(isOpenClass);
    }
})

})();