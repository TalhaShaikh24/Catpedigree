let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    $("#formAddUsefulLink").validate({
        rules: {
            uploadImage: {
                required: true
            },
            url: {
                required: true,
                url: true
            }
        },
        messages: {
            uploadImage: {
                required: "Please upload an image."
            },
            url: {
                required: "Please enter a URL.",
                url: "Please enter a valid URL."
            }
        },
        submitHandler: function (form) {
            form.submit(); // Submit the form if valid
        }
    });

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
            {
                data: 'usefulLinkFilePath',
                render: function (data, type, row) {
                    return `<img src="${baseApiUrl + data}" alt="Image" style="width:50px; height:50px;"/>`;
                }
                
            },
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
                            <button class="btn btn-info btn-md mx-2 EditUsefulLink" title="Edit" data-id="${data.id}" data-usefulLinkFilePath="${data.usefulLinkFilePath}" data-url="${data.url}">
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
        order: [[0, 'desc']],
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
$(document).on("click", ".EditUsefulLink", function () {
    const id = $(this).data("id");
    const usefulLinkFilePath = $(this).data("usefullinkfilepath");
    const url = $(this).data("url");

    // Set the values in the modal
    $("#modalUsefulLinkId").val(id);
    $("#modalUrl").val(url);

    // Set the current image source
    const imageUrl = baseApiUrl + usefulLinkFilePath;
    $("#currentImage").attr("src", imageUrl).show(); // Show the current image
    $("#modalUsefulFilPath").val(""); // Clear the file input

    $("#editUsefulLinkModal").modal("show");
});

$(document).on('click','#Btn_UsefulLinkSubmit', function () {

    if ($("#formAddUsefulLink").valid()) {
        let formData = new FormData();

        formData.append("UsefulLinkFile", $("#uploadImage")[0].files[0]);
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
                    $('#formAddUsefulLink')[0].reset(); // Reset the form
                    GetAllUsefulLinks();

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

   

});

// Update useful link
$(document).on('click', '#Btn_UpdateUsefulLink', function () {
    let formData = new FormData();
    const id = Number($("#modalUsefulLinkId").val());
    formData.append("Id", id);

    // Check if a new file is selected
    const newFileInput = $("#modalUsefulFilPath")[0];
    if (newFileInput.files.length > 0) {
        formData.append("UsefulLinkFile", newFileInput.files[0]);
    }
    formData.append("Url", $("#modalUrl").val());
    FilePostRequest('/Dashboard/UpdateUsefulLink', formData, function (res) {
        if (res.status == 200) {

            if (res.data != null) {

                debugger

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })
                
                
                GetAllUsefulLinks();
                $('#editCategoryForm')[0].reset(); // Reset the form
                $("#editUsefulLinkModal").modal("hide");
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

// Update displayed image when a new file is selected
$(document).on('change', '#modalUsefulFilPath', function () {
    const file = this.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            $("#currentImage").attr("src", e.target.result).show(); // Update image source and show it
        };
        reader.readAsDataURL(file); // Convert the file to a data URL
    } else {
        $("#currentImage").hide(); // Hide if no file is selected
    }
});
function DeleteUsefulLinkById(Id) {

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            postRequest('/Dashboard/DeleteUsefulLinkById?Id=' + Id, null, function (res) {

                if (res.status == 200) {

                    if (res.data != null) {

                        Swal.fire({
                            title: "Success",
                            text: res.responseMsg,
                            icon: "success"
                        });

                        GetAllUsefulLinks();
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
    })
   


}