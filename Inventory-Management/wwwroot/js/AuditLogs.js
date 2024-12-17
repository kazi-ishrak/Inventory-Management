$(function () {
    $('#DataTable_AuditLogs').DataTable({
        "processing": true,
        "language": {
            "processing": '<i class="fa fa-spinner fa-spin fa-2x fa-fw text-primary"></i><span class="sr-only">Loading...</span>'
        },
        "serverSide": true,
        "order": [[5, "desc"]],
        "responsive": true,
        "ajax": {
            "url": "/AuditLog/GetAll",
            "type": "POST"
        },
        "columns": [
            { "data": "id", "name": "id" },
            { "data": "productId", "name": "productId" },
            { "data": "changeType", "name": "changeType" },
            { "data": "quantity", "name": "quantity" },
            { "data": "userId", "name": "userId" },
            {
                "data": "timestamp",
                "orderable": true,
                "render": function (data) {
                    return formatDatetime(data);
                }
            }
        ],
        "lengthMenu": [10, 25, 50],
        "pageLength": 10,
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
        "searching": true,
        "info": true,
        "paging": true
    });
});

function formatDatetime(data) {
    var dateObj = new Date(data);
    var day = ("0" + dateObj.getDate()).slice(-2);
    var month = ("0" + (dateObj.getMonth() + 1)).slice(-2);
    var year = dateObj.getFullYear();
    var hours = ("0" + dateObj.getHours()).slice(-2);
    var minutes = ("0" + dateObj.getMinutes()).slice(-2);
    var seconds = ("0" + dateObj.getSeconds()).slice(-2);
    return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`;
}
