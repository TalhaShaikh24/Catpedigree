let baseApiUrl = "";
var table;
let paginationIndex = 0;
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    // Get the query string from the current URL
    var queryString = window.location.search;

    // Create a URLSearchParams object
    var urlParams = new URLSearchParams(queryString);

    // Get the value of the 'paginationIndex' query parameter
    

    
        paginationIndex = Number(urlParams.get('paginationIndex'));
  

    GetAllUsers();
})




function GetAllUsers() {
    
        // Destroy and reinitialize the DataTable
        $("#TableUsers").DataTable().destroy();
        table = $("#TableUsers").DataTable({
            "responsive": true,
            "lengthChange": true,
            "processing": true, // Show progress bar
            "serverSide": false, // Process server-side
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
                "url": "/Dashboard/GetAllUsers",
                "type": "POST",
                "dataType": "json",
                "dataSrc": function (data) {
                    if (data.status === 200) {
                        const registerData = data.data.register || [];
                        const rolesData = data.data.roles || [];

                        // Mapping roles data to each user
                        registerData.forEach(user => {
                            user.rolesOptions = rolesData.map(role => {
                                return {
                                    id: role.id,
                                    role: role.role,
                                    selected: user.roles.includes(role.role)
                                };
                            });
                        });

                        return registerData;
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
                { "data": "firstname", "name": "firstname", "autoWidth": true },
                { "data": "lastname", "name": "lastname", "autoWidth": true },
                { "data": "username", "name": "username", "autoWidth": true },
                { "data": "email", "name": "email", "autoWidth": true },
                {
                    "data": "rolesOptions",
                    "name": "roles",
                    "autoWidth": true,
                    "render": function (data, type, full, meta) {
                        const options = data.map(role => {
                            return `<option value="${role.id}" ${role.selected ? 'selected' : ''}>${role.role}</option>`;
                        }).join('');
                        return `<select class="form-control select2" multiple="multiple" data-user-id="${full.userId}">${options}</select>`;
                    }
                },
                { "data": "isActive", "name": "isActive", "autoWidth": true },
                {
                    "data": "userId",
                    "name": "userId",
                    "autoWidth": true,
                    "render": function (data, type, full, meta) {
                        const statusBtn = full.isActive === "Active"
                            ? `<button type="button" class="btn btn-warning btn-xs p-2" title="Deactivate User" onclick="UpdateUserStatus(${data});">
                            <i class="fa fa-ban" aria-hidden="true"></i>
                        </button>`
                            : `<button type="button" class="btn btn-info btn-xs p-2" title="Activate User" onclick="UpdateUserStatus(${data});">
                            <i class="fa fa-check" aria-hidden="true"></i>
                        </button>`;
                        return `
                        <div style="display: flex; justify-content: space-evenly; align-items: center;">
                            <a onClick="editUserToPage(${data})" title="Edit Details" class="btn btn-success btn-xs p-2">
                                <i class="fa fa-edit" aria-hidden="true"></i>
                            </a>
                            ${statusBtn}
                            <button type="button" class="btn btn-danger btn-xs p-2" title="${full.isActive === 'Active' ? 'Delete' : 'Activate'}" onclick="${full.isActive === 'Active' ? `DeleteUser(${data})` : `ActivateUser(${data})`}">
                                <i class='fa fa-trash' aria-hidden='true'></i>
                            </button>
                        </div>`;
                    }
                }
            ],
            "drawCallback": function (settings) {
                $('.select2').select2();
                $('.select2').each(function () {
                    $(this).next('.select2-container').css('width', '200px'); // Adjust the width as needed
                });

                $('.select2').on('change', function () {
                    var selectedValues = $(this).val();
                    var userId = $(this).data('user-id');

                    if (selectedValues.length > 0) {
                        var obj = {
                            UserId: userId,
                            RolesIds: selectedValues.toString()
                        };

                        postRequest('/Dashboard/UpdateUserRoles', obj, function (res) {
                            if (res.status === 200) {
                                Swal.fire({
                                    title: "Success",
                                    text: res.responseMsg,
                                    icon: "success"
                                });
                            } else {
                                Swal.fire({
                                    title: "Error",
                                    text: res.responseMsg,
                                    icon: "error"
                                });
                            }
                        });
                    }
                });
            }
        });

        // Ensure that the table is fully initialized before calling `page` method
    table.on('init.dt', function () {
        table.page(paginationIndex).draw('page'); // Go to page 2 (index is 1 for the second page)
        });
    
}

function editUserToPage(userId) {

    window.location.href = `/Dashboard/UserEditProfile?id=${userId}&paginationIndex=${table.page.info().page}`;
}



function UpdateUserStatus(id) {



    postRequest('/Dashboard/UpdateActiveInActiveUser/' + id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })

                GetAllUsers();

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
function DeleteUser(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This action will delete the user and all the related data  which can not be undone!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            postRequest('/Dashboard/DeleteUser/' + id, null, function (res) {

                if (res.status == 200) {

                    if (res.data != null) {

                        debugger

                        Swal.fire({
                            title: "Success",
                            text: res.responseMsg,
                            icon: "success"
                        })

                        GetAllUsers();

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

        } else {
            // User clicked "Cancel"
            console.log('Action canceled');
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

