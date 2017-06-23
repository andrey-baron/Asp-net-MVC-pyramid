
; (function () {
    var id = $("#Event_Id").val();

    $(".btn-add-event-product").on("click", function () {
        var count = $(".event__product").length;
        $.post("/Event/TemplateCategoryFromEventProduct?eventId=" + id + "&count=" + count, function (data) {
            $(".event__products").append(data);
        })
    });
    $(".event__products").on("change", ".js-category-from-event-product", function () {
        var thisElem = $(this);
        var pasteblock = thisElem.next(".js-product-select");
        $.post("/HomeEntity/GetProductTemplateDropDownListForCategoryId?id=" + thisElem.val() + "&index=" + thisElem.data("ajaxindex"), function (data) {
            pasteblock.html(data);
        })
    });
    $(".btn-update-event-image").on("click", function () {
        $.post("/ImageManager/PartialSelectImage", function (data) {
            $('#edit-event-image-modal .modal-body').html(data);
        })
        $("#edit-event-image-modal").modal('show');
    })
    $("#edit-event-image-modal").on("click", ".btn-ajax-edit", function () {
        $("#EventImage_Id").val($(this).data("ajaxid"));
        $(".event__img-wrap img").attr("src", $(this).data("url"));
        $("#edit-event-image-modal").modal('hide');
    })
    $(".btn-remove-event-product").on("click", function () {
        var url = $(this).data("actionurl");
        var elem = $(this);
        $.post(url, function (data) {
            if (data.Result == true) {
                elem.parent().remove();
            }
        })
    })
})();
