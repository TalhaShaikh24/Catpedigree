let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

  

    GetAllBlogCategories();
})






    function GetAllBlogCategories() {

    // Check if the DataTable is already initialized
    if ($.fn.DataTable.isDataTable('#TableBlogCategories')) {
        // Destroy the existing DataTable
        $('#TableBlogCategories').DataTable().clear().destroy();
    }

    $('#TableBlogCategories').DataTable({
        ajax: {
            url: '/Dashboard/GetAllBlogCategories',
            type: 'POST',
            dataSrc: function (res) {
                if (res.status === 200) {
                    return res.data || []; // Return the data or an empty array
                } else {
                    handleErrorResponse(res);
                    return []; // Return empty if there's an error
                }
            }
        },
        "columns": [
            { "data": "categoryName" },
            { "data": "description" },
            {
                "data": "createdOn",
                "render": function (data) {
                    return moment(data).format("DD-MMMM-YYYY");
                }
            },
            {
                "data": null,
                "render": function (data) {
                    return `
                        <div style="display: flex; justify-content: start; align-items: center;">
                            <button class="btn btn-info btn-md mx-2 edit-category" title="Edit" data-id="${data.categoryId}" data-name="${data.categoryName}" data-description="${data.description}">
                                <i class="fa fa-edit"></i>
                            </button>
                            <button type="button" class="btn btn-danger btn-md mx-2" title="Delete" onclick="DeleteBlogCategory(${data.categoryId})">
                                <i class="fa fa-trash"></i>
                            </button>
                        </div>`;
                }
            }
        ],
        // Optional: You can customize the DataTable here
        order: [[3, 'desc']],
        paging: true,
        searching: true,
        ordering: true,
        // Add other DataTable options as needed
    });

    function handleErrorResponse(res) {
        HidePreloader();
        Swal.fire({
            title: "Error",
            text: res.responseMsg,
            icon: res.status >= 400 && res.status < 500 ? "error" : "warning"
        });
    }

    // Optionally, you might want to show a preloader while fetching data
    $(document).on('processing.dt', function (e, settings, processing) {
        if (processing) {
            ShowPreloader();
        } else {
            HidePreloader();
        }
    });



}


$("#Btn_BlogCategorySubmit").click(function () {


    var obj = {
        CategoryName: $("#categoryName").val(),
        Description: $("#description").val()
    }

  
    postRequest('/Dashboard/AddBlogCategory', obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })
                GetAllBlogCategories();

            }
        }
        if (res.status == 304) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {

            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {

            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });


});


// Update changes to blog category
$("#Btn_Update_Listing").click(function () {
    const id = $("#modalCategoryId").val();
    const name = $("#modalCategoryName").val();
    const description = $("#modalDescription").val();

    const obj = {
        Id: id,
        CategoryName: name,
        Description: description
    };

    postRequest('/Dashboard/UpdateBlogCategory', obj, function (res) {
        if (res.status == 200) {

            if (res.data != null) {

                debugger

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })
                $("#editCategoryModal").modal("hide");
                GetAllBlogCategories();

            }
        }
        if (res.status == 304) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {

            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {

            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });
});

function DeleteBlogCategory(Id) {

    postRequest('/Dashboard/DeleteBlogCategory?Id=' + Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                GetAllBlogCategories();
            }
        }
        if (res.status == 304) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {

            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {

            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {

            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });


}