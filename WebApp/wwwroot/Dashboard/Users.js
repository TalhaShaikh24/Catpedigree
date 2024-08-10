let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

  

    GetAllUsers();
})



function GetAllUsers() {
    postRequest('/Dashboard/GetAllUsers', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendUsers").empty();




                var rolesData = [];

                $.each(res.data.roles, function (i, v) {
                    rolesData.push({id:v.id,role:v.role})

                })



                function initializeSelect2() {
                    $('.select2').select2({
                        data: rolesData,
                        placeholder: 'Select roles',
                       
                    });
                }

         

                $.each(res.data.register, function (i, v) {
                    var rolesOptions = rolesData.map(role => {
                        return `
                        <option value="${role.id}" ${v.roles.includes(role.role) ? 'selected' : ''}>
                            ${role.role}
                        </option>
                        `;
                    }).join('');

                    // Determine which icon to show based on the isActive status
                    var statusBtn = v.isActive === "Active"
                        ? `<button type="button" class="btn btn-warning btn-xs p-2" title="Deactivate User" onclick="UpdateUserStatus(${v.userId});">
                            <i class="fa fa-ban" aria-hidden="true"></i>
                        </button>`
                        : `<button type="button" class="btn btn-info btn-xs p-2" title="Activate User" onclick="UpdateUserStatus(${v.userId});">
                            <i class="fa fa-check" aria-hidden="true"></i>
                        </button>`;

                    $("#AppendUsers").append(`
                        <tr>
                            <td>${v.firstname}</td>
                            <td>${v.lastname}</td>
                            <td>${v.username}</td>
                            <td>${v.email}</td>
                            <td>
                                <select class="form-control select2" multiple="multiple" data-user-id="${v.userId}">
                                    ${rolesOptions}
                                </select>
                            </td>
                            <td>${v.isActive}</td>
                            <td style="width: 115px; display: flex; justify-content: space-evenly; align-items: center;">
                                <a href="/Dashboard/UserEditProfile?id=${v.userId}" title="Edit Details" class="btn btn-success btn-xs p-2">
                                    <i class="fa fa-edit" aria-hidden="true"></i>
                                </a>
                                
                                ${statusBtn}
                                <button type="button" class="btn btn-danger btn-xs p-2" title="${v.isActive === 'Active' ? 'Delete' : 'Activate'}" onclick="${v.isActive === 'Active' ? `DeleteUser(${v.userId})` : `ActivateUser(${v.userId})`}">
                                    <i class='fa fa-trash' aria-hidden='true'></i>
                                </button>
                            </td>
                        </tr>
                    `);
                });




                
                initializeSelect2();

                $('.select2').each(function () {
                    $(this).next('.select2-container').css('width', '200px'); // Adjust the width as needed
                });
                $('.select2').on('change', function () {
                    var selectedValues = $(this).val();
                    var userId = $(this).data('user-id');


                    if (selectedValues.length>0) {


                        var obj = {
                            UserId: userId,
                            RolesIds: selectedValues.toString()
                        }

                        debugger;
                        postRequest('/Dashboard/UpdateUserRoles', obj, function (res) {

                            if (res.status == 200) {

                                if (res.data != null) {


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

                    }

            





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
        text: "This action will delete the user and it can not be undone!",
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

