; (function () {
     
    var categoryId = $("#Category_Id").val();
    $(".js-btn-category-add-filter").on("click", function () {
        var count = $(".admin-category__filter").length;
        $.post("/Category/GetTemplateFilter?id=" + categoryId+"&count="+count, function (data) {
            $(".js-category-filters").append(data);
        });
    });
    $(".js-category-filters").on("click", ".js-btn-category-filter-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Category/DeleteFilter?id=" + categoryId + "&filterid=" + id, function (t) {
            $.post("/Category/GetAllFilter/" + categoryId, function (data) {
                $(".js-category-filters").html(data);
            });
        });
    })
    $(".btn-add-recommendations").on("click", function () {
        var count = $(".admin-category__recommendation").length;
        $.post("/Category/GetRecommendationTemplate?id=" + categoryId + "&count=" + count, function (data) {
            $(".js-recommendations").append(data);
        });
    });
    $(".js-recommendations").on("click", ".js-btn-category-recommendation-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Category/DeleteRecommendation?id=" + categoryId + "&recommendationid=" + id, function (t) {
            $.post("/Category/GetAllRecommendation/" + categoryId, function (data) {
                $(".js-recommendations").html(data); 
            });
        });
    })
})();