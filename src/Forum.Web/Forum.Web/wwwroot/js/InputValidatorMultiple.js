$(document).ready(function () {
    var selectedInputs = document.querySelectorAll("input[type='text'], textarea");
    var count = selectedInputs.length;
    $("input[type='text'], textarea").on('keyup', function () {
        let filled = 0;
        $("input[type='text'], textarea").each(function () {
            var trimmed = $(this).val().trim();
            if (trimmed.length == 0) {
                filled--;
            }
            else {
                filled++;
            }
        });

        if (filled == count)
            $("button[type='submit']").attr('disabled', false);
        else
            $("button[type='submit']").attr('disabled', 'disabled');
    });
});