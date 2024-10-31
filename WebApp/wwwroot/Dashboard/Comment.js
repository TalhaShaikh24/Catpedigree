let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    var urlParams = new URLSearchParams(window.location.search);

    var blogId = urlParams.get("Id");

    if (blogId) {

       GetAllComments(blogId);

    }
   
})

function GetAllComments(blogId) {
    postRequest('/Dashboard/GetAllCommentsByBlogId?Id='+blogId, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendComment").empty();
                $.each(res.data, function (i, v) {

                    debugger
                    $("#AppendComment").append(`
                      <tr>
                      <td style="width: 65%;">${v.commentText}</td>
                      <td>${v.userName}</td>
                      <td>${moment(v.createdOn).format("DD-MMMM-YYYY")}</td>
                      <td><div style="display: flex; justify-content: space-between; align-items: center;">

                      <a class="btn btn-success btn-md" href="/Dashboard/Replies?Id=${v.id}" title="View Reply"><i class="fa fa-eye"></i></a>

                      <button class="btn btn-info btn-md" title="Reply" onclick="Reply(${v.id},${v.userId})"><i class="fa fa-reply"></i></button>

                      <button type="button" class="btn btn-danger btn-md" title="Delete" onclick="DeleteCommentById(${v.id})"><i class="fa fa-trash"></i></button></div></td>

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

function Reply(Id,userId) {
    $("#HDCID").val(Id);
    $("#HDUID").val(userId);
    $("#ReplyModal").modal("show");
}

function SubmitReply() {


    var obj = {

        CommentId:$("#HDCID").val(),
        UserId:$("#HDUID").val(),
        ReplyText:$("#ReplyText").val(),
    }
    postRequest('/Dashboard/SendReply',obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

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


}

function DeleteCommentById(Id) {

    postRequest('/Dashboard/DeleteCommentById?Id='+Id,null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                var urlParams = new URLSearchParams(window.location.search);

                var blogId = urlParams.get("Id");

                if (blogId) {

                    GetAllComments(blogId);

                }
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


