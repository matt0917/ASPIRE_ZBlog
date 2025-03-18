window.tinymceInterop = {
    initEditor: (editorId) => {
        tinymce.init({
            selector: `#${editorId}`,
            menubar: false,
            plugins: 'advlist autolink lists link image charmap print preview anchor searchreplace visualblocks code fullscreen insertdatetime media table paste code help wordcount',
            toolbar: 'undo redo | formatselect | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            branding: false,
            height: 300
        });
    },

    reloadEditor: () => {
        if (tinymce.activeEditor) {
            tinymce.activeEditor.destroy();
        }
        setTimeout(() => {
            tinymce.init({ selector: 'textarea' });
        }, 500);
    }
};
