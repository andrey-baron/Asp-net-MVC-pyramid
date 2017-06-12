;
(function () {
    /*question-answer function*/
    var faqId = $("#Faq_Id").val();
    $(".js-btn-add-question-answer").on("click", function () {
        $.post("/Faq/AddNewDefaultAndGetAll/" + faqId, function (data) {
            $(".faq__question-answer-values").html(data);
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