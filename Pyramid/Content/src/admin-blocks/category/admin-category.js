; (function () {
     
    var categoryId = $("#Category_Id").val();
    $(".js-btn-category-add-filter").on("click", function () {

        $.post("/Category/GetTemplateFilter/" + categoryId, function (data) {
            $(".js-category-filters").append(data);
        });
    });
    $(".js-category-filters").on("click", ".js-btn-category-filter-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Category/DeleteFilter?id=" + categoryId + "&filterid=" + id, function (t) {
            $.post("/Category/GetAllFilter?filterid=" + categoryId, function (data) {
                $(".js-filter-all-enumvalues").html(data);
            });
        });
    })

})();