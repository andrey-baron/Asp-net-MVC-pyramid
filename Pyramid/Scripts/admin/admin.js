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
;(function(){
    tinymce.init({


        // General options
        elements: "content_editor",
        language: "ru",
        plugins: "code,autolink,lists,spellchecker,pagebreak,table,save,,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,template",
        menu: {
            file: { title: 'File', items: 'newdocument' },
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            view: { title: 'View', items: 'visualaid' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
            tools: { title: 'Tools', items: 'code' }
        },
        selector: '.adminTextareaCommon',
    });

})();
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

})();

function reloadData() {
   
    this.selectorGaleryGrid = ".gallery-grid";
    $.post("/ImageManager/GetImages", function (data) {
        $(selectorGaleryGrid).html(data);
    });
};
; (function () {

       var filterId = $("#Filter_Id").val();
    $(".js-btn-filter-add-enumvalue").on("click", function () {

        $.post("/Filter/GetTemplateEnumValue?filterid=" + filterId, function (data) {
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