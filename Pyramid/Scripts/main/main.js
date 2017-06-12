

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
        values: [curminVal, curmaxVal],
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
})();
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
        var serializeData = form.serialize();
        $.post("/Review/AddReview/", serializeData, function (data) {
            if (data.Status == "Ok") {

            }
        });
    })
})($);