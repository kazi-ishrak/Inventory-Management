$(function () {

    $('#DataTable_Categories').DataTable({
        "processing": true,
        "language": {
            "processing": '<i class="fa fa-spinner fa-spin fa-2x fa-fw text-primary"></i><span class="sr-only">Loading...</span>'
        },
        "serverSide": true,
        "order": [1, "desc"],
        "responsive": true,

        "ajax": {
            "url": "/Category/GetAll",
            "type": "POST",

        },

        "columns": [
            { "data": "name", "name": "name" },

            {
                "data": "created_at", "orderable": true,
                "render": function (data, type, row, meta) {
                    return formatDatetime(data);
                },
            },
            {
                "data": "updated_at", "orderable": true,
                "render": function (data, type, row, meta) {
                    return formatDatetime(data);
                },
            },
            {
                "data": null,
                "width": "10%",
                "render": function (data, type, row, meta) {
                    return '<a href="#" onclick="RowEdit(' + row.id + ')" class="btn btn-outline-light btn-sm text-primary"> <i class="fa fa-pencil text-warning" aria-hidden="true"></i></a>' +
                        ' <a href="#" onclick="RowDelete(' + row.id + ')" class="btn btn-outline-light btn-sm text-danger"> <i class="fa fa-trash text-danger" aria-hidden="true"></i></a>';
                }
            },

        ],

        select: true,
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }
        ],
    });
});

$('#openModalButton').on('click', function () {
    $('#addCategoryModal').modal('show');
});

$('#submitCategoryButton').on('click', HandleFormSubmit);
$('#UpdateCategoryButton').on('click', HandleUpdateFormSubmit);
function RowEdit(rowid) {

    $.ajax({
        url: `/Category/GetById?id=${rowid}`,
        method: 'GET',
        contentType: 'application/json',
        success: function (data) {
            $('#categoryNameUpdate').val(data.name);
            $('#categoryIdHidden').val(data.id);
            $('#updateCategoryModal').modal('show');

        },

    });
}

function RowDelete(rowid) {

    $.ajax({
        url: `/Category/Delete?id=${rowid}`,
        method: 'DELETE',
        contentType: 'application/json',
        success: function (data) {
            alert("Category successfully Deleted.");
            $('#DataTable_Categories').DataTable().ajax.reload(null, false);
        },

    });
}



function HandleUpdateFormSubmit(e) {
    e.preventDefault();  // Prevent the default form submission
    var now = new Date();
    let formdata = {
        id: $('#categoryIdHidden').val(),
        name: $('#categoryNameUpdate').val()
    };

    if (formdata.name == "") {
        alert('Please enter a Category Name.');
        $('#categoryNameUpdate').addClass('required-field');  // Highlight the field if invalid
        return false;
    }
    console.log(formdata);
    UpdateProductData(formdata);
}


function UpdateProductData(formData) {
    $.ajax({
        url: '/Category/Update',

        method: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            alert("Category successfully Updated.");
            $('#updateCategoryModal').modal('hide');
            $('#DataTable_Categories').DataTable().ajax.reload(null, false);

        },
        error: function (xhr, status, error) {
            alert("Failed to Update Category: " + xhr.responseText);
        }
    });
}

function HandleFormSubmit(e) {
    e.preventDefault();  // Prevent the default form submission
    var now = new Date();
    var formdata = {
        name: $('#categoryName').val(),
    

    };

    if ( formdata.name == "") {
        alert('Please enter a Category Name.');
        $('#categoryName').addClass('required-field');  // Highlight the field if invalid
        return false;
    }

    // Assuming all validations are passed
    AddProduct(formdata); // A function to handle the AJAX request or form submission
}

function AddProduct(formData) {

    $.ajax({
        url: '/Category/Create',

        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {

            console.log("API response:", response);
            alert("Category successfully Added.");
            $('#addCategoryModal').modal('hide');
            $('#DataTable_Categories').DataTable().ajax.reload(null, false);

        },
        error: function (xhr, status, error) {
            console.error("Error adding Category:", xhr.responseText);
            alert("Failed to add Category: " + xhr.responseText);
        }
    });
}




function formatDatetime(data) {
    var dateObj = new Date(data);
    var day = ("0" + dateObj.getDate()).slice(-2);
    var month = ("0" + (dateObj.getMonth() + 1)).slice(-2);
    var year = dateObj.getFullYear().toString().slice(-2);
    var hours = ("0" + dateObj.getHours()).slice(-2);
    var ampm = hours >= 12 ? "PM" : "AM";
    hours = hours % 12;
    hours = hours ? hours : 12;
    var minutes = ("0" + dateObj.getMinutes()).slice(-2);
    var seconds = ("0" + dateObj.getSeconds()).slice(-2);
    return day + "." + month + "." + year + " " + hours + ":" + minutes + ":" + seconds + " " + ampm;
}