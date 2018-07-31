// Page Loading
$(document).ready(function () {
    window.onload = function () {
        $('#pageLoader').fadeOut('fast', function () { $('#pageLoader').remove(); });
    }
});
