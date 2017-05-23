;(function () {
    

    $(".gallery-grid").on("click", ".btn-ajax-delete", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/ImageManager/delete/" + id, function () {
            reloadData()
        })
    });

    $(".gallery-grid").on("click", ".btn-ajax-edit", function (e) {
        var id = $(this).data("ajaxid");
        $.post("/ImageManager/PartialBodyModal/" + id, function (data) {
            $('#edit-modal .modal-body').html(data);
            $('#edit-modal').modal('show');
        })
    });

    $('#edit-modal').on('hidden.bs.modal', function (e) {
        
        var th = this;
        $(this).find(".modal-body").html("...");
    })
    $(".modal").on("click", ".btn-modal-save", function (e) {
        var modal = $(this).closest(".modal");
        var form = modal.find(".partial-form-im");
        var id = form.find("input#Id").val();
        
        var data = $(this).closest(".modal").find(".partial-form-im").serialize();
        $.post("/ImageManager/AddOrUpdate/" + id,data, function (data) {
            $('#edit-modal .modal-body').html("...");
            $('#edit-modal').modal('hide');
            reloadData()
        })
    })
  
    $(".js-btn-thumbnail-edit").on("click", function () {
        $.post("/ImageManager/PartialSelectImage", function (data) {
            $('#edit-thumbnail-modal .modal-body').html(data);
        })
        $("#edit-thumbnail-modal").modal('show');
    })
    $("#edit-thumbnail-modal").on("click", ".btn-ajax-edit", function () {
        $("#ThumbnailId").val($(this).data("ajaxid"));
        $(".admin-product__thumbnail img").attr("src", $(this).data("url"));
        $("#edit-thumbnail-modal").modal('hide');
    })
    //$(".qq-upload-list-selector")
    

})();

function reloadData() {
   
    this.selectorGaleryGrid = ".gallery-grid";
    $.post("/ImageManager/GetImages", function (data) {
        $(selectorGaleryGrid).html(data);
    });
};