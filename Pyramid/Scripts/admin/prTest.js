 var manualUploader = new qq.FineUploader({
        element: document.getElementById('fine-uploader-manual-trigger'),
        template: 'qq-template-manual-trigger',
        request: {
            endpoint: '/ImageManager/Upload'
        },
        thumbnails: {
            placeholders: {
                waitingPath: '/Content/img-file-uploader/waiting-generic.png',
                //            notAvailablePath: '/Content/img-file-uploader/not_available-generic.png'
            }
        },
        autoUpload: false,
        callbacks: {

            onComplete: function (id, fileName, responseJSON) {
                setTimeout(function () {
                    $(".qq-upload-list-selector").html("");
                }, 3000);
                return false;
            },
            onUpload: function (id, name) {
                var test = 7;
                alert(name);
            }
        },
        debug: true
    });

    qq(document.getElementById("trigger-upload")).attach("click", function () {
        manualUploader.uploadStoredFiles();
    });