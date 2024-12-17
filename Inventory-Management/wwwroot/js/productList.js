$(function () {
    $('#DataTable_Products').DataTable({
        processing: true,
        language: {
            processing: '<i class="fa fa-spinner fa-spin fa-2x fa-fw text-primary"></i><span class="sr-only">Loading...</span>'
        },
        serverSide: true,
        order: [[5, "desc"]],
        responsive: true,
        ajax: {
            url: "/Product/GetAll",
            type: "POST"
        },
        columns: [
            { data: "name", name: "name" },
            { data: "sku", name: "sku" },
            { data: "stock", name: "stock" },
            { data: "price", name: "price" },
            {
                data: "categories",
                name: "categories",
                render: function (data) {
                    if (Array.isArray(data) && data.length > 0) {
                        return data.map(category => `<li>${category.name}</li>`).join('');
                    }
                    return "<span>No Category</span>";
                }
            },
            {
                data: "created_at",
                render: formatDatetime
            },
            {
                data: "updated_at",
                render: formatDatetime
            },
            {
                data: null,
                width: "10%",
                render: function (data, type, row) {
                    return `<a href="#" onclick="RowEdit(${row.id})" class="btn btn-outline-light btn-sm text-primary"><i class="fa fa-pencil text-warning"></i></a>
                            <a href="#" onclick="RowDelete(${row.id})" class="btn btn-outline-light btn-sm text-danger"><i class="fa fa-trash text-danger"></i></a>`;
                }
            }
        ],
        columnDefs: [
            { className: "dt-center", targets: "_all" }
        ]
    });
});

$('#openModalButton').on('click', function () {
    $('#addProductModal').modal('show');
});

$('#submitProductButton').on('click', function (e) {
    e.preventDefault();
    const formData = {
        id: 0,
        name: $('#productName').val(),
        sku: $('#sku').val(),
        stock: parseInt($('#stock').val()),
        price: parseFloat($('#price').val()),
        created_at: new Date().toISOString(),
        updated_at: new Date().toISOString()
    };
    $.ajax({
        url: "http://localhost:5217/Product/Create",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(formData),
        success: function () {
            alert("Product created successfully");
            $('#addProductModal').modal('hide');
            $('#DataTable_Products').DataTable().ajax.reload();
        },
        error: function () {
            alert("Error creating product");
        }
    });
});

function RowEdit(id) {
    $.ajax({
        url: `Product/GetById/${id}`,
        method: "GET",
        success: function (data) {
            $('#productIdHidden').val(data.id);
            $('#productNameUpdate').val(data.name);
            $('#skuUpdate').val(data.sku); // Ensure SKU is populated
            $('#stockUpdate').val(data.stock);
            $('#priceUpdate').val(data.price);
            $('#created_atHidden').val(data.created_at); // Store created_at for later use
            $('#updated_atHidden').val(data.updated_at); // Store updated_at for later use
            $('#rowEditModal').modal('show');
        }
    });
}

$('#updateProductButton').on('click', function (e) {
    e.preventDefault();

    const formData = {
        id: parseInt($('#productIdHidden').val()),
        name: $('#productNameUpdate').val(),
        sku: $('#skuUpdate').val(), // SKU will be included in the request
        stock: parseInt($('#stockUpdate').val()),
        price: parseFloat($('#priceUpdate').val()),
        created_at: $('#created_atHidden').val(), // Include created_at from hidden field
        updated_at: new Date().toISOString() // Set updated_at to current timestamp
    };

    $.ajax({
        url: "http://localhost:5217/Product/Update",
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify(formData),
        success: function () {
            alert("Product updated successfully");
            $('#rowEditModal').modal('hide');
            $('#DataTable_Products').DataTable().ajax.reload();
        },
        error: function () {
            alert("Error updating product");
        }
    });
});



function RowDelete(id) {
    if (confirm("Are you sure you want to delete this product?")) {
        $.ajax({
            url: `http://localhost:5217/Product/Delete?id=${id}`,
            method: "DELETE",
            success: function () {
                alert("Product deleted successfully");
                $('#DataTable_Products').DataTable().ajax.reload();
            },
            error: function () {
                alert("Error deleting product");
            }
        });
    }
}

function formatDatetime(data) {
    const date = new Date(data);
    const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' };
    return date.toLocaleDateString('en-US', options);
}
