$(document).ready(function () {
    var pasteBlock = $(".single__review-block");
    var productid = $("#single-productid").val();
    var startLink = "/Review/PartialGetReviewsByProductId?productid=" + productid;
    function GetReviews(link) {
        $.post(link, function (data) {
            pasteBlock.html(data);
        })
    }
   

    $(".single__review-block").on("click", ".pagination a", function (e) {
        e.preventDefault();
        var link = $(this).attr("href");
        GetReviews(link);
    })

    GetReviews(startLink);
});