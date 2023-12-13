$(document).ready(function () {
    $("input[type='text']").on('keyup', function () {
        let empty = true;

        $("input[type='text']").each(function () {
            var trimmed = $(this).val().trim();
            empty = trimmed.length == 0;
        });

        if (empty)
            $("button[type='submit']").attr('disabled', 'disabled');
        else
            $("button[type='submit']").attr('disabled', false);
    });
});