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

})();