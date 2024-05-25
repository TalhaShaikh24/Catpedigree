$(document).ready(function () {
    $('#summernote').summernote({
        height:650
    });

    GetAllAdminBLogs();
})



function GetAllAdminBLogs() {
    postRequest('/Dashboard/GetAllAdminBLogs', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendBlogs").empty();
                $.each(res.data, function (i, v) {

                    debugger
                    $("#AppendBlogs").append(`
                      <tr>
                      <td><p style=" text-overflow: ellipsis; width:300px; overflow: hidden;overflow: hidden; position: relative; display: inline-block; text-overflow: ellipsis; white-space: nowrap; ">${v.title}</p></td>
                      <td><p style=" text-overflow: ellipsis; overflow: hidden; width:500px;  overflow: hidden; position: relative; display: inline-block; text-overflow: ellipsis; white-space: nowrap; ">${v.shortDescription}</p></td>
                      <td>${v.commentsCount}</td>
                      <td>${v.username}</td>
                      <td>${moment(v.createdOn).format("DD-MMMM-YYYY")}</td>
                      <td><button class="btn btn-info btn-xs" data-id="${v.blogID}"">View Comments</button></td>
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


$("#Btn_BlogSubmit").click(function () {


    let formData = new FormData(); 

    formData.append("Title", $("#title").val());
    formData.append("ShortDescription", $("#shortdescription").val());
    formData.append("Content", $('#summernote').summernote("code"));
    formData.append("FeatureImage", $("#featuredFile")[0].files[0]);

    FilePostRequest('/Dashboard/AddBlog',formData, function (res) {

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

