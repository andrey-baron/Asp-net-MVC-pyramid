$(document).ready(function () {

    $('.decoration-work__wrap-video').magnificPopup({
        delegate: '.decoration-work__popup-youtube',
        disableOn: 700,
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        }
    });

});