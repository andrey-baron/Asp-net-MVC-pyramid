; (function () {
    var productId = $("#Product_Id").val();
    $(".btn-add-product-value").on("click", function () {
        
        $.post("/Product/GetEmptyTemplateProductValue/" + productId, function (data) {
            $(".admin-product__addition-values").append(data);
        });
    });
    $(".admin-product__addition-values").on("click", ".btn-addition-value", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteProductValue/" + id, function (t) {
            $.post("/Product/GetAllProductValues?productId=" + productId, function (data) {
                $(".admin-product__addition-values").html(data);
            });
        });
    });


    $(".js-btn-add-product-enumvalue").on("click", function () {

        $.post("/Product/GetTemplateEnumValue/" + productId, function (data) {
            $(".js-admin-product-enumvalues").append(data);
        });
    });
    $(".js-admin-product-enumvalue").on("click", ".btn-product-enum-value-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteEnumValue?id=" + productId + "&enumValueId=" + id, function (t) {
            $.post("/Product/GetAllEnumValues/" + productId, function (data) {
                $(".js-admin-product-enumvalues").html(data);
            });
        });
    })

    

})();