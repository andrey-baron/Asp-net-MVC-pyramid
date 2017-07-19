$(document).ready(function () {
    //var pasteBlock = $(".single__review-block");
    //var productid = $("#single-productid").val();
    //var startLink = "/Review/PartialGetReviewsByProductId?productid=" + productid;
    //function GetReviews(link) {
    //    $.post(link, function (data) {
    //        pasteBlock.html(data);
    //    })
    //}
   

    $(".single__review-block").on("click", ".pagination a", function (e) {
        e.preventDefault();
        var link = $(this).attr("href");
        GetReviews(link);
    })

    // GetReviews(startLink);

    $(".js-review-star").on("click", function (e) {
        if (!$(this).hasClass("star-on")) {
            $(this).removeClass("star-off").addClass("star-on");
        }
        $(this).prevAll(".js-review-star").removeClass("star-off").addClass("star-on")
        $(this).nextAll(".js-review-star").removeClass("star-on").addClass("star-off");
        $("#rating-from-review").val($(this).data("rating"));
    });
});