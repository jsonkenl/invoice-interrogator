// 200 Invoice Table
/////////////////////////////////////////////////////////////
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


// Unprocessed Invoice Table
///////////////////////////////////////////////////////////
var unprocessedInvoice = $('#unprocessedInvoiceTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "visible": true, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        { "visible": false, "targets": ["_all"] }
    ],
    buttons: [
        'copy', 'excel'
    ]
});

unprocessedInvoice.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', unprocessedInvoice.table().container()));

var btn = $('#unprocessedInvoiceTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-primary');
btn.removeClass('btn-secondary');

var empty = 'All Invoices Have Been Processed'
$(".dataTables_empty").html(empty);


// Vendors Table
///////////////////////////////////////////////////////////
var vendors = $('#vendorTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "width": "120px", "targets": [0, 4] }
    ],
    buttons: [
        'copy', 'excel'
    ],
    order: [[1, 'asc']]
});

vendors.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', vendors.table().container()));

var btn = $('#vendorTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-primary');
btn.removeClass('btn-secondary');

// Accounts Table
///////////////////////////////////////////////////////////
var accts = $('#accountsTable').DataTable({
    lengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
    columnDefs: [
        { "width": "120px", "targets": [0, 2, 3] }
    ],
    buttons: [
        'copy', 'excel'
    ],
    order: [[0, 'asc']]
});

accts.buttons().container()
    .appendTo($('.dataTables_length:eq(0)', accts.table().container()));

var btn = $('#accountsTable_wrapper').find('.btn-secondary');
btn.addClass('btn-outline-primary');
btn.removeClass('btn-secondary');