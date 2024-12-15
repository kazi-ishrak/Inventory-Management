$(function () {

    $('#DataTable_Projects').DataTable({
        "processing": true,
        "language": {
            "processing": '<i class="fa fa-spinner fa-spin fa-2x fa-fw text-primary"></i><span class="sr-only">Loading...</span>'
        },
        "serverSide": true,
        "order": [0, "desc"],
        "responsive": true,

        "ajax": {
            "url": "/Product/GetAll",
            "type": "POST",  
           
        },

        "columns": [
            { "data": "name", "name": "name" },
            { "data": "sku", "name": "sku" },
            { "data": "stock", "name": "stock" },
            { "data": "price", "name": "price" },
            { "data": "price", "name": "price" },
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
    $('#addProductModal').modal('show');
});

$('#submitProductButton').on('click', HandleFormSubmit);
$('#updateProjectButton').on('click', HandleUpdateFromSubmit);
function RowEdit(rowid) {

    $.ajax({
        url: `Product/GetById/${rowid}`,
        method: 'GET',
        contentType: 'application/json',

        success: function (data) {
            console.log(data);


            $('#productNameUpdate').val(data.name);
            $('#stockUpdate').val(data.stock);
            $('#priceUpdate').val(data.price);
            
            $('#rowEditModal').modal('show');

        },

    });
}
function HandleUpdateFromSubmit(e) {
    e.preventDefault();
    var updateFormData = {
        id: $('#projectIdHidden').val().toString(),
        project_id: $('#projectIdUpdate').val().toString(),
        type_id: $('#projectTypeIdUpdate').val(),
        code: $('#projectCodeUpdate').val(),
        name: $('#projectNameUpdate').val(),
        organization: $('#organizationUpdate').val(),
        api_token: $('#apiTokenUpdate').val(),
        hrm_identifier: $('#hrmEnabledUpdate').val() === 'Yes'
    };
    if (!updateFormData.project_id || !updateFormData.name || !updateFormData.api_token || !updateFormData.type_id) {
        alert('Please fill in all required fields.');
        $('#projectIdUpdate').addClass('required-field');
        $('#apiTokenUpdate').addClass('required-field');
        $('#projectNameUpdate').addClass('required-field');
        $('#projectTypeIdUpdate').addClass('required-field');
        return;
    }
    UpdateProjectData(updateFormData);
}


function UpdateProjectData(formData) {
    $.ajax({
        url: '/api/Projects/update',

        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            alert("Project successfully Updated.");
            $('#rowEditModal').modal('hide');
            $('#DataTable_Projects').DataTable().ajax.reload(null, false);
            console.log("API response:", response);
        },
        error: function (xhr, status, error) {
            console.error("Error Updating project:", xhr.responseText);
            alert("Failed to Update project: " + xhr.responseText);
        }
    });
}
function HandleFormSubmit(e) {
    e.preventDefault();
    var formdata = {
        project_id: $('#projectId').val().toString(),
        type_id: $('#projectTypeSelect').val().toString(),
        code: $('#projectCode').val(),
        name: $('#projectName').val(),
        organization: $('#organization').val(),
        api_token: $('#apiToken').val(),
        hrm_identifier: $('#hrmEnabled').val() === 'Yes'
    };
    if (!formdata.project_id || !formdata.name || !formdata.api_token || !formdata.type_id) {
        alert('Please fill in all required fields.');
        $('#projectId').addClass('required-field');
        $('#apiToken').addClass('required-field');
        $('#projectName').addClass('required-field');
        $('#projectTypeSelect').addClass('required-field');
        console.log(formdata.type_id);
        return;
    }

    InsertAProject(formdata);
}

function HandleFormSubmit(e) {
    e.preventDefault();  // Prevent the default form submission

    var formdata = {
        productName: $('#productName').val(),
        stock: $('#stock').val(),
        price: $('#price').val()
    };

    // Validate stock to be a positive integer
    if (!Number.isInteger(Number(formdata.stock)) || Number(formdata.stock) < 1) {
        alert('Please enter a valid stock number (positive integer).');
        $('#stock').addClass('required-field');  // Highlight the field if invalid
        return false;
    }

    // Validate price to be a positive decimal
    if (Number(formdata.price) <= 0) {
        alert('Please enter a valid price (positive number).');
        $('#price').addClass('required-field');  // Highlight the field if invalid
        return false;
    }

    // Assuming all validations are passed
    AddProduct(formdata); // A function to handle the AJAX request or form submission
}



function InsertAProject(formData) {

    $.ajax({
        url: '/api/Projects/insert',

        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            console.log("API response:", response);
            alert("Project successfully Created.");
            $('#addProjectModal').modal('hide');

            $('#DataTable_Projects').DataTable().ajax.reload(null, false);
            clearFormFields();
        },
        error: function (xhr, status, error) {
            console.error("Error inserting project:", xhr.responseText);
            alert("Failed to add project: " + xhr.responseText);
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