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
    })

})();