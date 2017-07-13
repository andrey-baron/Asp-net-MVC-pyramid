 //var manualUploader = new qq.FineUploader({
 //       element: document.getElementById('fine-uploader-manual-trigger'),
 //       template: 'qq-template-manual-trigger',
 //       request: {
 //           endpoint: '/ImageManager/Upload'
 //       },
 //       thumbnails: {
 //           placeholders: {
 //               waitingPath: '/Content/img-file-uploader/waiting-generic.png',
 //               //            notAvailablePath: '/Content/img-file-uploader/not_available-generic.png'
 //           }
 //       },
 //       autoUpload: true,
 //       callbacks: {

 //           onComplete: function (id, fileName, responseJSON) {
 //               reloadData();
 //               setTimeout(function () {
 //                   $(".qq-upload-list-selector").html("");
 //               }, 3000);
 //               return false;
 //           },
 //           onUpload: function (id, name) {
 //               var test = 7;
 //               alert(name);
 //           }
 //       },
 //       debug: false
 //   });

 //   qq(document.getElementById("trigger-upload")).attach("click", function () {
 //       manualUploader.uploadStoredFiles();
 //   });

$('#fine-uploader-manual-trigger').fineUploader({
    template: 'qq-template-manual-trigger',
    request: {
        endpoint: '/ImageManager/Upload'
    },
    thumbnails: {
        placeholders: {
            waitingPath: '/Content/img-file-uploader/waiting-generic.png',
            notAvailablePath: '/Content/img-file-uploader/not_available-generic.png'
        }
    },
    autoUpload: true,
    interceptSubmit: false,
    allowXdr: true,
    debug: false,
    // multiple: false
}).on('complete', function (event, id, name, responseJSON) {
    var tmp = responseJSON;
    reloadData();
    setTimeout(function () {
        $(".qq-upload-list-selector").html("");
    }, 3000);
    event.preventDefault();
});;

$('#trigger-upload').click(function () {
    $('#fine-uploader-manual-trigger').fineUploader('uploadStoredFiles');
});
