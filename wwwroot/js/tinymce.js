window.tinyHelper = {
    init: function (selector, dotNetRef) {

        tinymce.init({
            language: 'pt_BR',
            language_url: '/tinymce/js/tinymce/langs/pt_BR.js',

            selector: selector,
            height: 400,

            license_key: 'gpl',

            menubar: 'file edit view insert format tools table help',

            plugins: `
                anchor autolink charmap codesample code directionality
                emoticons fullscreen help image insertdatetime link lists
                media preview searchreplace table visualblocks visualchars wordcount
            `,

            toolbar: `
                undo redo | blocks fontfamily fontsize |
                bold italic underline strikethrough |
                alignleft aligncenter alignright alignjustify |
                bullist numlist outdent indent |
                link image media table |
                forecolor backcolor |
                removeformat code fullscreen
            `,

            toolbar_mode: 'sliding',

            branding: false,
            promotion: false,
            statusbar: true,

            image_caption: true,

            image_class_list: [
                { title: 'Normal', value: 'img-normal' },
                { title: 'Esquerda (texto ao redor)', value: 'img-left' },
                { title: 'Direita (texto ao redor)', value: 'img-right' },
                { title: 'Centralizada', value: 'img-center' }
            ],

            // ===== BASE64 + DRAG + PASTE =====
            paste_data_images: true,
            automatic_uploads: false,
            images_upload_handler: null,

            file_picker_types: 'image',

            file_picker_callback: function (cb) {

                const input = document.createElement('input');
                const DEFAULT_WIDTH = 300;
                input.type = 'file';
                input.accept = 'image/*';

                input.onchange = function () {
                    const file = this.files[0];
                    const reader = new FileReader();

                    reader.onload = function () {

                        tinymce.activeEditor.insertContent(
                            `<img src="${reader.result}"  width="${DEFAULT_WIDTH}" style="max-width:${DEFAULT_WIDTH}px; height:auto;" />`
                        );
                    };

                    reader.readAsDataURL(file);
                };

                input.click();
            },

            setup: function (editor) {
                editor.on('change keyup input', function () {
                    const value = editor.getContent();
                    window.tinyValue = value;
                    dotNetRef.invokeMethodAsync('AtualizarConteudo', value);
                });
            }
        });
    },

    getValue: function () {
        return window.tinyValue || "";
    },

    setContent: function (selector, content) {
        tinymce.get(selector.replace('#', '')).setContent(content || '');
    }
};

window.openFilePicker = (id) => {
    const el = document.getElementById(id);
    if (el) el.click();
    return;
};