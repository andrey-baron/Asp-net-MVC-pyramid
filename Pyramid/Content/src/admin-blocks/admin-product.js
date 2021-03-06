﻿; (function () {
    var productId = $("#Product_Id").val();
    $(".btn-add-product-value").on("click", function () {
        var countProductValue = $(".admin-product__addition-value").length;
        $.post("/Product/GetEmptyTemplateProductValue?id=" + productId+"&count="+countProductValue, function (data) {
            $(".js-admin-productvalues").append(data);
        });
    });
    $(".js-admin-productvalues").on("click", ".btn-addition-value", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteProductValue/" + id, function (t) {
            $.post("/Product/GetAllProductValues?productId=" + productId, function (data) {
                $(".js-admin-productvalues").html(data);
            });
        });
    });

     
    $(".js-btn-add-product-enumvalue").on("click", function () {
        var countEnumValue = $(".admin-product-enumvalue").length;
        $.post("/Product/GetTemplateEnumValue?id=" + productId + "&count=" + countEnumValue, function (data) {
            $(".js-admin-product-enumvalues").append(data);
        });
    });
    $(".js-admin-product-enumvalues").on("click", ".btn-product-enum-value-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteEnumValue?id=" + productId + "&enumValueId=" + id, function (t) {
            $.post("/Product/GetAllEnumValues/" + productId, function (data) {
                $(".js-admin-product-enumvalues").html(data);
            });
        });
    })

    $("#addOrUpdateProductForm").submit(function () {
        var test = 1;
    });

    $(".js-btn-add-product-gallery").on("click", function () {
        var id = $(this).data("ajaxid");
        $.post("/ImageManager/PartialSelectImage/" + id, function (data) {
            $('#edit-gallery-modal .modal-body').html(data);
            $('#edit-gallery-modal').modal('show');
        })
    });
    $("#edit-gallery-modal").on("click", ".btn-ajax-edit", function () {
        var id = $(this).data("ajaxid");
        $.post("/Product/AddToGallery?id=" + productId + "&imageid=" + id, function (data) {
            $('.box-product-gallery').html(data);
            $('#edit-gallery-modal').modal('hide');
        })
    })

    $(".box-product-gallery").on("click", ".js-btn-gallery-ajax-delete", function () {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteToGallery?id=" + productId + "&imageid=" + id, function (data) {
            $('.box-product-gallery').html(data);
             
        })
    });


    $(".js-admin-product-enumvalues").on("change", ".js-product-filter-control", function () {
        var thisElem = $(this);
        var pasteblock = thisElem.next(".js-product-insert-filter-enumval");
        var indx = thisElem.data("index");
        $.post("/Product/GetProductTemplateDropDownListForFilterId?id=" + thisElem.val() + "&indx=" + indx, function (data) {
            pasteblock.html(data);
        })
    })
    /*recomendations 
    $(".js-btn-add-product-recommendation").on("click", function () {
        var countRecomendation = $(".admin-product-recommendation").length;
        $.post("/Product/GetTemplateRecomendation?id=" + productId + "&count=" + countRecomendation, function (data) {
            $(".js-admin-product-recommendations").append(data);
        });
    });
    $(".js-admin-product-recommendations").on("click", ".btn-product-recommendation-delete", function () {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteRecomendation?id=" + productId + "&recomendationId=" + id, function (data) {
            $('.box-product-gallery').html(data);

        })
    });*/
})();