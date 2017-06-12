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