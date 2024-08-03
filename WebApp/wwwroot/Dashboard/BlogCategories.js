let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

  

    GetAllBlogCategories();
})



function GetAllBlogCategories() {
    postRequest('/Dashboard/GetAllBlogCategories', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendCategories").empty();
                $.each(res.data, function (i, v) {
                    $("#AppendCategories").append(`
                      <tr>
                      <td>${v.categoryName}</td>
                      <td>${v.description}</td>
                      <td>${moment(v.createdOn).format("DD-MMMM-YYYY")}</td>
                      <td>
                        <div style="display: flex; justify-content: start; align-items: center;">
                          <button class="btn btn-info btn-md mx-2" title="Edit" id="EditCategory" data-id="${v.id}" data-name="${v.categoryName}" data-description="${v.description}"><i class="fa fa-edit"></i></button>
                          <button type="button" class="btn btn-danger btn-md mx-2" title="Delete" onclick="DeleteBlogCategory(${v.id})"><i class="fa fa-trash"></i></button>
                        </div>
                      </td>
                      </tr>`);
                });

                // Attach click event to edit buttons
                $("#AppendCategories").on("click", "#EditCategory", function () {
                    const id = $(this).data("id");
                    const name = $(this).data("name");
                    const description = $(this).data("description");
                    $("#modalCategoryId").val(id);
                    $("#modalCategoryName").val(name);
                    $("#modalDescription").val(description);
                    $("#editCategoryModal").modal("show");
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