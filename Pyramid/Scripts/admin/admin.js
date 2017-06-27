; (function () {
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

    $(".js-btn-gallery-ajax-delete").on("click", function () {
        var id = $(this).data("ajaxid");
        $.post("/Product/DeleteToGallery?id=" + productId + "&imageid=" + id, function (data) {
            $('.box-product-gallery').html(data);
            
        })
    });
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

    $(".datepickerCommon").datepicker({
        gotoCurrent: true,
        dateFormat: "dd.mm.yy"
    });

})();
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

})();

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

;
(function () {
    /*question-answer function*/

    var faqId = $("#Faq_Id").val();
    
    $(".js-btn-add-question-answer").on("click", function () {
    var count = $(".admin-product__addition-value").length;
        $.post("/Faq/AddNewDefault?id=" + faqId+"&count="+count, function (data) {
            $(".faq__question-answer-values").append(data);
        });
        
    });
    $(".faq__question-answer-values").on("click", ".js-btn-QuestionAnswer-delete", function () {
        var id = $(this).data("ajaxid");
        $.post("/Faq/DeleteQuestionAnswer/" + id, function (data) {
            $.post("/Faq/PartialGetAllQuestionAnswer/" + faqId, function (data) {
                $(".faq__question-answer-values").html(data);
            });
            
        });
    });
    

})();
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
    $(".home-entity__products").on("click", ".js-btn-home-entity-delete-product", function () {

        var ajaxId = $(this).data("ajaxid");
        $.post("/HomeEntity/DeleteProduct?id=" + id + "&productId=" + ajaxId, function (data) {
            $(".home-entity__products").html(data);
        });
    });
    $(".home-entity__categories").on("click", ".js-btn-home-entity-delete-category", function () {

        var ajaxId = $(this).data("ajaxid");
        $.post("/HomeEntity/DeleteCategory?id=" + id + "&categoryId=" + ajaxId, function (data) {
            $(".home-entity__categories").html(data); 
        });
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