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
        if ($("#formFile")[0].files.length > 5) {
            alert("You can select only 5 images");
            this.form.reset();
        }

        if (Array.from($("#formFile")[0].files).some(x => (x.size > 5242880))) {
            alert("size error");
            this.form.reset();
        }

        if (Array.from($("#formFile")[0].files).some(x => !isImage(x.name))) {
            alert("type error");
            this.form.reset();
        }

    });
});

