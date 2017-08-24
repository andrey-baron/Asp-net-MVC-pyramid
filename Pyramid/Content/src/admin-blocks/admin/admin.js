;(function(){
    tinymce.init({
        language: "ru",
        selector: ".adminTextareaCommon",
        plugins: [
			"advlist autolink lists link image charmap hr anchor pagebreak",
			"searchreplace wordcount visualblocks visualchars",
			"insertdatetime media nonbreaking save table contextmenu directionality",
			"template paste textcolor code spellchecker"
        ],
        toolbar1: "insertfile undo redo | styleselect fontsizeselect | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | spellchecker | forecolor backcolor",
        fontsize_formats: "8px 10px 12px 14px 18px 24px 36px",
        convert_urls: false,
        allow_script_urls: true,
        extended_valid_elements: "a[*]",
        spellchecker_languages: "Russian=ru,English=en",
        spellchecker_language: "ru",
        spellchecker_rpc_url: "http://speller.yandex.net/services/tinyspell?options=12",
         
        // General options
        /*elements: "content_editor",
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
        selector: '.adminTextareaCommon',*/
    });

    $(".datepickerCommon").datepicker({
        gotoCurrent: true,
        dateFormat: "dd.mm.yy"
    });

})();