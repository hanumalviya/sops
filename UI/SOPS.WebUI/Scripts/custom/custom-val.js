// validate input elements on focus change
$(document).ready(function () {
    $('input[data-val="true"]').blur(function() {
        $(this).valid();
    });
});