$(document).ready(function () {
    $("#commentText").on('keyup', function () {
        console.log(1);
        let empty = true;

        var trimmed = $(this).val().trim();
        empty = trimmed.length == 0;

        if (empty)
            $("#sendCommentBtn").attr('disabled', 'disabled');
        else
            $("#sendCommentBtn").attr('disabled', false);
    });
    $("#childCommentText").on('keyup', function () {
        console.log(1);
        let empty = true;

        var trimmed = $(this).val().trim();
        empty = trimmed.length == 0;

        if (empty)
            $("#sendChildCommentBtn").attr('disabled', 'disabled');
        else
            $("#sendChildCommentBtn").attr('disabled', false);
    });
});