

(function () {

    $(".call-to-action__form-wrap").hide();
    $(".call-to-action").on("click",function () {
        $(this).next(".call-to-action__form-wrap").toggle( 400 );

    });
})();
(function () {

    $('.ft-grid').masonry({
        itemSelector: '.ft-item',
        columnWidth: '.grid-sizer',
        percentPosition: true
    });

     $(".footer__catalog-products").hide();
     $(".footer__switch").on("click",function () {
     $(".footer__catalog-products").toggle( 400 );
     });
})();
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
})($);
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
