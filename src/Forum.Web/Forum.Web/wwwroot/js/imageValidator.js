function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}

function isImage(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'bmp':
        case 'png':
            return true;
    }
    return false;
}

$("#file").on("change", function () {
    if ($("#file")[0].files[0].size > 5242880){
        alert("File is too big");
        this.form.reset();
    }
    if (!isImage($("#file")[0].files[0])) {
        alert("Invalid file type");
        this.form.reset();
    }
});