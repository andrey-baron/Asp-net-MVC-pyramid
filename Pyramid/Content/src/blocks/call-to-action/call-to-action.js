

(function () {

    $(".call-to-action__form-wrap").hide();
    $(".call-to-action").on("click",function () {
        $(this).next(".call-to-action__form-wrap").toggle( 400 );

    });
})();