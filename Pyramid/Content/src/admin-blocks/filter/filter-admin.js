; (function () {
    /*filter function*/
       var filterId = $("#Filter_Id").val();
    $(".js-btn-filter-add-enumvalue").on("click", function () {
        var count =$(".admin-filter__enum-value").length;
        $.post("/Filter/GetTemplateEnumValue?filterid=" + filterId + "&count=" + count, function (data) {
            $(".js-filter-all-enumvalues").append(data);
        });
    });
    $(".js-filter-all-enumvalues").on("click", ".btn-filter-enum-value-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Filter/DeleteEnumValue?id=" + filterId + "&enumValueId=" + id, function (t) {
            $.post("/Filter/GetAllEnumValues?filterid=" + filterId, function (data) {
                $(".js-filter-all-enumvalues").html(data);
            });
        });
    })
})();