// Page Loading
$(document).ready(function () {
    window.onload = function () {
        $('#pageLoader').fadeOut('fast', function () { $('#pageLoader').remove(); });
    }
});

// Invoice Table
var table = $('#invoiceTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "visible": true, "targets": [0,1,2,3,4,5,6,7,8,9]},
        { "visible": false, "targets": ["_all"] }
    ]
});

new $.fn.dataTable.Buttons(table, {
    buttons: [
        'copy', 'excel'
    ]
});

table.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', table.table().container()));

var btn = $('#invoiceTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-secondary');
btn.removeClass('btn-secondary');