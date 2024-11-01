
$(document).ready(async function () {
    await GetAllUsersDropdown();
    await GetAllRoleByUserIdChange(0);
    await GetAllPermissionScreensDropdown();
    await GetAllRoleScreenPermissions();
    await GetAllRole();
})

function GetAllUsersDropdown() {

    postRequest('/Dashboard/GetAllUsersDropdown',null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#ddlUsers").empty();
                $.each(res.data, function (i, v) {

                    

                    $("#ddlUsers").append(`<option value="${v.userId}">${v.username}</option>`);

                });

                $(".selectpicker").selectpicker("refresh");

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

function GetAllRoleByUserIdChange(UserId = 0) {

    postRequest('/Dashboard/GetAllRoleDropdownByUserId/' + UserId, null, function (res) {

        if (res.status == 200) {

             $("#ddlRoles").empty();

            if (res.data != null && res.data.length > 0) {

                $.each(res.data, function (i, v) {

                    $("#ddlRoles").append(`<option value="${v.id}">${v.role}</option>`);

                });

                $(".selectpicker").selectpicker("refresh")
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

function GetAllPermissionScreensDropdown() {

    postRequest('/Dashboard/GetAllPermissionScreensDropdown', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#ddlScreens").empty();
                $.each(res.data, function (i, v) {

                    

                    $("#ddlScreens").append(`<option value="${v.screenId}">${v.screenName}</option>`);

                });

                $(".selectpicker").selectpicker("refresh")
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

function GetAllRole() {


    if ($.fn.DataTable.isDataTable('#RoleTable')) {
        $('#RoleTable').DataTable().destroy();
    }

    $('#RoleTable').DataTable({
        responsive: true,
        lengthChange: true,
        processing: true, // Show progress bar
        serverSide: false, // Process server-side
        searching: true, // Enable searching (filter)
        orderMulti: false, // Disable multiple column ordering
        pageLength: 10,
        orderClasses: false,
        order: [[0, 'desc']],
        language: {
            search: "Search:",
            processing: "Processing...",
            lengthMenu: "Display _MENU_ records",
            info: "Showing _START_ to _END_ of _TOTAL_ entries"
        },
        ajax: {
            url: "/Dashboard/GetAllRoleDropdownByUserId/0'",
            type: "POST",
            dataType: "json",
            dataSrc: function (data) {

                // Check for a valid status and return data accordingly
                if (data.status === 200) {

                    return data.data; // Make sure this is the correct path to your data array
                } else {
                    console.error("Error fetching data:", data.message);
                    return []; // Return an empty array if the status is not 200
                }


            },
            error: function (xhr, error, thrown) {
                console.error("Failed to fetch data:", error, thrown);
                console.error("Response:", xhr.responseText);
            }
        },
        columns: [
            { data: 'id' },
            { data: 'role' }
        ]
    });
}

function GetAllRoleScreenPermissions() {
    // Destroy any existing instance of DataTable on permissionsTable
    if ($.fn.DataTable.isDataTable('#permissionsTable')) {
        $('#permissionsTable').DataTable().destroy();
    }

    $('#permissionsTable').DataTable({
        responsive: true,
        lengthChange: true,
        processing: true, // Show progress bar
        serverSide: false, // Process server-side
        searching: true, // Enable searching (filter)
        orderMulti: false, // Disable multiple column ordering
        pageLength: 10,
        orderClasses: false,
        order: [[0, 'desc']],
        language: {
            search: "Search:",
            processing: "Processing...",
            lengthMenu: "Display _MENU_ records",
            info: "Showing _START_ to _END_ of _TOTAL_ entries"
        },
        ajax: {
            url: "/Dashboard/GetAllRoleScreenPermissions",
            type: "POST",
            dataType: "json",
            dataSrc: function (data) {
                // Check for a valid status and return data accordingly
                if (data.status === 200) {
                    return data.data; // Make sure this is the correct path to your data array
                } else {
                    console.error("Error fetching data:", data.message);
                    return []; // Return an empty array if the status is not 200
                }
            },
            error: function (xhr, error, thrown) {
                console.error("Failed to fetch data:", error, thrown);
                console.error("Response:", xhr.responseText);
            }
        },
        columns: [
            { data: 'role' },
            {
                data: 'screenName', // Adjust according to your actual data field
                render: function (data, type, row) {
                    // Split the data string into an array
                    const screenNames = data.split(','); // Split by comma

                    // Map over the array to create badges
                    return screenNames.map(name => `
                        <span class="badge badge-primary">${name.trim()}</span>
                    `).join(' '); // Join badges with space
                },
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex align-items-center">
                        <button class="btn btn-primary btn-sm mr-1" onclick="EditPermission('${row.screenIds}', ${row.userId}, ${row.roleId})">Edit</button>
                         |
                        <button class="btn btn-danger btn-sm ml-1" onclick="DeletePermission(${row.userId})">Delete</button>
                        </div>
                        `;
                }
            }

        ]
    });
}



$("#Btn_CreateRoleSubmit").click(function () {


    var obj = {
        Id: 0,
        Role: $("#textRole").val()
    }


    postRequest('/Dashboard/CreateRole', obj, async function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#textRole").val('');

                await GetAllRole();
                await GetAllRoleByUserIdChange();

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


$("#Btn_RoleScreenPermissionSubmit").click(function () {


    var obj = {
        RoleId: Number($("#ddlRoles").selectpicker("val")),
        ScreenIds: $("#ddlScreens").selectpicker("val").join(","),
        UserId: Number($("#ddlUsers").selectpicker("val"))
    }

    postRequest('/Dashboard/AddRoleScreenPermission', obj, async function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#ddlRoles").empty().attr("disabled", true).val("");
                $("#ddlScreens").attr("disabled",true).val("");
                $("#ddlUsers").val("");
                $(".selectpicker").selectpicker("refresh");

                await GetAllRoleScreenPermissions();

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


async function EditPermission(screenId, userId, roleId) {

 $("#ddlUsers").selectpicker("val", userId);

 $("#ddlRoles").selectpicker("val", roleId);

 const screenIdsArray = screenId.split(",");

 $("#ddlScreens").selectpicker("val", screenIdsArray);

 $(".selectpicker").selectpicker("refresh");
    
}

function DeletePermission(userId) {
    Swal.fire({
        title: "Are you sure?",
        text: "If you proceed, permissions for all roles associated with this user will be removed. Do you want to continue?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete all permissions",
        cancelButtonText: "Cancel",
    }).then((result) => {
        if (result.isConfirmed) {
            postRequest('/Dashboard/DeletePermission/' + userId, null, function (res) {
                if (res.status == 200) {
                    GetAllRoleScreenPermissions();
                } else if ([304, 305, 401, 320, 500].includes(res.status)) {
                    Swal.fire({
                        title: "Error",
                        text: res.responseMsg,
                        icon: "error"
                    });
                } else if (res.status == 403) {
                    Swal.fire({
                        title: "Error",
                        text: res.responseMsg,
                        icon: "error"
                    });
                } else if (res.status == 600) {
                    Swal.fire({
                        title: "Warning",
                        text: res.responseMsg,
                        icon: "warning"
                    });
                }
            });
        }
    });
}


