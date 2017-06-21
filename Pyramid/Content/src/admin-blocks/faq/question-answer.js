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