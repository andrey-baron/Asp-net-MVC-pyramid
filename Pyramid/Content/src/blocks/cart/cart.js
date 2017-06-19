
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
        var singleQuantity = $(".js-single-spinner");
        var quantity = 1;
        if (singleQuantity!="undefind") {
            quantity = singleQuantity.val();
        }
        $.post("/Cart/AddToCart?productId="+ajaxid+"&quantity="+quantity, function (data) {
            SoccessAdd(data);
        })
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