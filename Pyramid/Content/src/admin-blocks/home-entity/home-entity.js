(function () {
    /*function home-entity.js*/
    var id = $("#HomeEntity_Id").val();
    
    $("#add-new-point").on("click", function () {
        var count = $(".home-entity__point-inputs").length;
        $.post("/HomeEntity/JsonGetNewPoint?id=" + id + "&count=" + count, function (data) {
            $(".home-entity__point-product-list").append(data.Data);
            var newpoint = $(' <div class="home-entity__point" data-ajaxid="' + data.Id + '">' + data.Id + 'точ.</div>');
            $(".home-entity__view-points").append(newpoint);
            RunDraggable();
        });
    });

    $(".js-btn-home-entity-add-category").on("click", function () {
        var count = $(".home-entity__category").length;
        $.post("/HomeEntity/GetTemplateCategory?entityId=" + id + "&count=" + count, function (data) {
            $(".home-entity__categories").append(data);
        })
    });

    $(".js-btn-home-entity-add-product").on("click", function () {
        var count = $(".home-entity__product").length;
        $.post("/HomeEntity/GetTemplateCategoryFromProduct?entityId=" + id + "&count=" + count, function (data) {
            $(".home-entity__products").append(data);
        })
    });

    $(".home-entity__products").on("change", ".js-category-from-product", function () {
        var thisElem = $(this);
        var pasteblock = thisElem.next(".js-product-select");
        $.post("/HomeEntity/GetProductTemplateDropDownListForCategoryId?id=" + thisElem.val() + "&index=" + thisElem.data("ajaxindex"), function (data) {
            pasteblock.html(data);
        })
    })
    $(".js-btn-update-banner").on("click", function () {
        $.post("/ImageManager/PartialSelectImage", function (data) {
            $('#banner-edit-modal .modal-body').html(data);
        })
        $("#banner-edit-modal").modal('show');
        
        $("#banner-edit-modal").on("click", ".btn-ajax-edit", function () {
            $("#Banner_Id").val($(this).data("ajaxid"));
            $(".home-entity__view-img").attr("src", $(this).data("url"));
            $("#banner-edit-modal").modal('hide');
        })
    });
    $(".home-entity__point-product-list").on("change", ".point-category", function () {
        var thisElem = $(this);
        var pasteblock = thisElem.next(".product-point");
        $.post("/Category/GetProductTemplateDropDownListForPointId?id=" + thisElem.val() + "&pointindex=" + thisElem.data("ajaxindex"), function (data) {
            pasteblock.html(data);
        })
    });
    $(".home-entity__point-product-list").on("click", ".js-btn-delete-point", function () {
        var pointid = $(this).data("ajaxid");
        
        var topasteblockDesc = $(".home-entity__point-product-list");
        var topasteblockView = $(".home-entity__view-points");
        $.post("/HomeEntity/DeletePoint/" + pointid, function (t) {
            $.post("/HomeEntity/PartialAllPoints?id=" + id + "&isview=false", function (data) {
                topasteblockDesc.html(data);
            })
            $.post("/HomeEntity/PartialAllPoints?id=" + id + "&isview=true", function (data) {
                topasteblockView.html(data);
            })
        })
    });
    
    function RunDraggable() {
        $(".home-entity__point").draggable({
            containment: "parent",
            stop: function (event, ui) {
                var t = this;
                var parent = $(this).parent();
                var width = parent.width();
                var height = parent.height();
                var id = $(ui.helper.context).data("ajaxid");
                SetPointInputsById(id,Math.round( ui.position.left * 100 / width), Math.round(ui.position.top * 100 / height))
                //$("#coordx").val(ui.position.left);
                //$("#coordy").val(ui.position.top);
            }

        });
    }
    RunDraggable();

    function SetPointInputsById(pointId,coordX,coordY) {
        var wrap = $("#point_" + pointId);
        var inputX = wrap.find(".coordx");
        var inputY = wrap.find(".coordy");
        inputX.val(coordX);
        inputY.val(coordY);
    }
})()