function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}

function isImage(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
        case 'png':
            return true;
    }
    return false;
}

$('document').ready(() => {
    $("#formFile").on("change", function () {
        if ($("#formFile")[0].files[0].size > 5242880) {
            alert("File is too big");
            this.form.reset();
        }
        if (!isImage($("#formFile")[0].files[0])) {
            alert("Invalid file type");
            this.form.reset();
        }
    });
});
