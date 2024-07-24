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

                    debugger
                    $("#AppendCategories").append(`
                      <tr>
                      <td>${v.categoryName}</td>
                      <td>${v.description}</td>
                      <td>${moment(v.createdOn).format("DD-MMMM-YYYY")}</td>

                      <td><div style="display: flex; justify-content: start; align-items: center;">

                      <button class="btn btn-info btn-md mx-2" title="Reply" id="EditReply" data-id="${v.id}"><i class="fa fa-edit"></i></button>

                      <button type="button" class="btn btn-danger btn-md mx-2" title="Delete" onclick="DeleteReplyById(${v.id})"><i class="fa fa-trash"></i></button></div></td>

                      </tr>`);

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

