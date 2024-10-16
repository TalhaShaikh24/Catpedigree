let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

  

  GetAllUsefulLinks();
})






function GetAllUsefulLinks() {

    // Check if the DataTable is already initialized
    if ($.fn.DataTable.isDataTable('#TableUsefulLinks')) {
        // Destroy the existing DataTable
        $('#TableUsefulLinks').DataTable().clear().destroy();
    }

    $('#TableUsefulLinks').DataTable({
        ajax: {
            url: '/Dashboard/GetAllUsefulLinks',
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
            { "data": "id" },
            { "data": "usefulLinkFilePath" },
            { "data": "url" },
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
                            <button class="btn btn-info btn-md mx-2 EditUsefulLink" title="Edit" data-id="${data.id}" data-filepath="${data.usefulLinkFilePath}" data-url="${data.url}">
                                <i class="fa fa-edit"></i>
                            </button>
                            <button type="button" class="btn btn-danger btn-md mx-2" title="Delete" onclick="DeleteUsefulLinkById(${data.id})">
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


// Attach click event to edit buttons
$(document).on("click", "#EditUsefulLink", function () {
    const id = $(this).data("id");
    const filepath = $(this).data("filepath");
    const url = $(this).data("url");
    $("#modalUsefulLinkId").val(id);
    $("#modalUsefulFilPath").val(filepath);
    $("#modalUrl").val(url);
    $("#editCategoryModal").modal("show");
});

$(document).on('click','#Btn_UsefulLinkSubmit', function () {

    let formData = new FormData();

    formData.append("UsefulLinkFile", $("#uplaodImage")[0].files[0]);
    formData.append("Url", $("#url").val());

    debugger;

    FilePostRequest('/Dashboard/AddUsefulLink', formData, function (res) {

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