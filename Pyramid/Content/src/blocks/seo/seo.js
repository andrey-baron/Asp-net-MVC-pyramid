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