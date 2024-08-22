let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    $('#blogTags').select2({
        tags: true
    });

    $('#summernote').summernote({
        height:650
    });


    GetAllBlogCategories();
    GetAllAdminBLogs();
})



function GetAllBlogCategories() {



    postRequest('/Dashboard/GetAllAdminBlogCategories', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendCategories").empty();
                $.each(res.data, function (i, v) {

                    
                    $("#blogCategory").append(`
                      <option value="${v.id}">${v.categoryName}</option>
                      `);

                });

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


function GetAllAdminBLogs() {
    debugger;
    $("#TableBlogs").DataTable().destroy();
    $("#TableBlogs").DataTable({
        "responsive": true,
        "lengthChange": true,
        "processing": true, // for show progress bar
        "serverSide": false, // for process server side
        "searching": true, // Enable searching (filter)
        "orderMulti": false, // Disable multiple column ordering
        "pageLength": 10,
        "orderClasses": false,
        "language": {
            "search": "Search:",
            "processing": "Processing...",
            "lengthMenu": "Display _MENU_ records",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries"
        },
        "ajax": {
            "url": "/Dashboard/GetAllAdminBLogs",
            "type": "POST",
            "dataType": "json",
            "dataSrc": function (data) {
                if (data.status === 200) {
                    return data.data;
                }

                let title, icon;
                switch (data.status) {
                    case 304:
                    case 305:
                    case 401:
                    case 403:
                    case 320:
                    case 500:
                        title = "Error";
                        icon = "error";
                        break;
                    case 600:
                        title = "Warning";
                        icon = "warning";
                        break;
                    default:
                        title = "Error";
                        icon = "error";
                        break;
                }

                Swal.fire({
                    title: title,
                    text: data.responseMsg,
                    icon: icon
                });

                return []; // Return an empty array if there is an error
            }
        },
        "columns": [
            {
                "data": "title",
                "name": "title",
                "autoWidth": true
            },
            {
                "data": "shortDescription",
                "name": "shortDescription",
                "autoWidth": true
            },
            {
                "data": "commentsCount",
                "name": "commentsCount",
                "autoWidth": true
            },
            {
                "data": "username",
                "name": "username",
                "autoWidth": true
            },
            {
                "data": "createdOn",
                "name": "createdOn",
                "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return `<span>${moment(full.createdOn).format("DD - MMMM - YYYY")}</span>`;
                }
            },
            {
                "data": "blogID",
                "name": "blogID",
                "autoWidth": true,
                "render": function (data, type, row) {
                    // Example roleIds string (you should replace this with the actual value from your cookie)
                    let roleIds = RoleIds; // For demonstration
                    debugger;
                    // Check if 'Blogger' exists in the roleIds string
                    let isBlogger = roleIds.split(',').includes('Blogger') || roleIds.split(',').includes('Admin');

                    // Build the HTML string with conditional rendering
                    return `
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <a class="btn btn-success btn-md" title="Comments" href="/Dashboard/Comments?Id=${data}">
                            <i class="fa fa-comment"></i>
                            ${row.unreadComments > 0 ? `<span class="badge badge-danger navbar-badge">${row.unreadComments}</span>` : ''}
                        </a>

                        ${isBlogger ? `
                        <a class="btn btn-info btn-md" title="Edit" href="/Dashboard/EditBlog?Id=${data}">
                            <i class="fa fa-edit"></i>
                        </a>` : ``}

                        <button type="button" class="btn btn-danger btn-md" title="Delete" onclick="BlogDeleteById(${data})">
                            <i class="fa fa-trash"></i>
                        </button>
                    </div>`;
                }

            }
        ],
        "order": [[2, 'desc']]
    });
}

function BlogDeleteById(Id) {
    postRequest('/Dashboard/BlogDeleteById?Id=' + Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })

                GetAllAdminBLogs();


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

    $("#Btn_BlogSubmit").click(function () {


        let formData = new FormData();

        formData.append("Title", $("#title").val());
        formData.append("ShortDescription", $("#shortdescription").val());
        formData.append("BlogCategoryId", $("#blogCategory").val());
        formData.append("Tags", String($("#blogTags").val()));
        formData.append("Content", $('#summernote').summernote("code"));
        formData.append("FeatureImage", $("#featuredFile")[0].files[0]);

        FilePostRequest('/Dashboard/AddBlog', formData, function (res) {

            if (res.status == 200) {

                if (res.data != null) {

                    debugger

                    Swal.fire({
                        title: "Success",
                        text: res.responseMsg,
                        icon: "success"
                    })


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

