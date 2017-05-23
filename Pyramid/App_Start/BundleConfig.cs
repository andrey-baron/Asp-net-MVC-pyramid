using System.Web;
using System.Web.Optimization;

namespace Pyramid
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
"~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminscript").Include(
                     "~/Scripts/admin/admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                     "~/Scripts/tinymce/tinymce.min.js"));
            
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/css/bootstrap.min.css"));
            
                 bundles.Add(new StyleBundle("~/Content/cssreset").Include(
                     "~/Content/css/reset.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                     "~/Content/css/admin.css"));

            bundles.Add(new StyleBundle("~/Content/jQuery-File-Upload").Include(
                    "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                   "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css",
                   "~/Content/blueimp-gallery2/css/blueimp-gallery.css",
                     "~/Content/blueimp-gallery2/css/blueimp-gallery-video.css",
                       "~/Content/blueimp-gallery2/css/blueimp-gallery-indicator.css"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-File-Upload").Include(
                     //<!-- The Templates plugin is included to render the upload/download listings -->
                     "~/Scripts/jQuery.FileUpload/vendor/jquery.ui.widget.js",
                       "~/Scripts/jQuery.FileUpload/tmpl.min.js",
//<!-- The Load Image plugin is included for the preview images and image resizing functionality -->
"~/Scripts/jQuery.FileUpload/load-image.all.min.js",
//<!-- The Canvas to Blob plugin is included for image resizing functionality -->
"~/Scripts/jQuery.FileUpload/canvas-to-blob.min.js",
//"~/Scripts/file-upload/jquery.blueimp-gallery.min.js",
//<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
"~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
//<!-- The basic File Upload plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
//<!-- The File Upload processing plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
//<!-- The File Upload image preview & resize plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
//<!-- The File Upload audio preview plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-audio.js",
//<!-- The File Upload video preview plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-video.js",
//<!-- The File Upload validation plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
//!-- The File Upload user interface plugin -->
"~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js"


));
        }
    }
}
