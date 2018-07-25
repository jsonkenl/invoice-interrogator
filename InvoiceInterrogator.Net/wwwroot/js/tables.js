// 200 Invoice Table
/////////////////////////////////////////////////////////////
$(document).ready(function () {
    $('#invoiceTable').DataTable();
});

var invoice = $('#invoiceTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "visible": true, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        { "visible": false, "targets": ["_all"] }
    ],
    buttons: [
        'copy', 'excel'
    ]
});

invoice.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', invoice.table().container()));

var btn = $('#invoiceTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-primary');
btn.removeClass('btn-secondary');

var msg = 'Only displaying the most recent 200 invoices. ';
var link = '<a href="/Tables/AllInvoices">View All</a>';
$(".dt-buttons").append('<span class="msg-200">' + msg + link + '</span>');

var empty = 'No data available. Upload some invoices to get started.'
$(".dataTables_empty").html(empty);


// All Invoice Table
///////////////////////////////////////////////////////////
$(document).ready(function () {
    $('#allInvoiceTable').DataTable();
});

var everyInvoice = $('#allInvoiceTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "visible": true, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        { "visible": false, "targets": ["_all"] }
    ],
    buttons: [
        'copy', 'excel'
    ]
});

everyInvoice.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', everyInvoice.table().container()));

var btn = $('#allInvoiceTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-primary');
btn.removeClass('btn-secondary');