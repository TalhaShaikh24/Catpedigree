$(document).ready(function () {

    var urlParams = new URLSearchParams(window.location.search);

    var Id = urlParams.get("Id");

    if (Id) {

        GetAllReplyByCommentId(Id);

    }
   
})

function GetAllReplyByCommentId(Id) {
    postRequest('/Dashboard/GetAllReplyByCommentId?Id='+Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendReplies").empty();
                $.each(res.data, function (i, v) {

                    debugger
                    $("#AppendReplies").append(`
                      <tr>
                      <td style="width: 65%;">${v.replyText}</td>
                  
                      <td>${moment(v.replyDate).format("DD-MMMM-YYYY")}</td>

                      <td style="width: 116px;"><div style="display: flex; justify-content: space-between; align-items: center;">

                      <button class="btn btn-info btn-md" title="Reply" id="EditReply" data-id="${v.id},${v.userId},${v.commentId}"><i class="fa fa-edit"></i></button>

                      <button type="button" class="btn btn-danger btn-md" title="Delete" onclick="DeleteReplyById(${v.id})"><i class="fa fa-trash"></i></button></div></td>

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

$(document).on("click","#EditReply",function () {
    debugger

    var Id = $(this).attr("data-id").split(",");

    if (Id.length > 0) {

        $("#HDRID").val(Id[0]);
        $("#HDUID").val(Id[1]);
        $("#HDCID").val(Id[2]);
        $("#ReplyText").val($(this).parent().parent().parent().find("td:eq(0)").text());
        $("#UpdateReplyModal").modal("show");
    }

});

function UpdateReply() {


        var obj = {
            Id: $("#HDRID").val(),
            CommentId: $("#HDCID").val(),
            UserId: $("#HDUID").val(),
            ReplyText: $("#ReplyText").val(),
        }
        postRequest('/Dashboard/UpdateReply', obj, function (res) {

            if (res.status == 200) {

                if (res.data != null) {

                    Swal.fire({
                        title: "Success",
                        text: res.responseMsg,
                        icon: "success"
                    });

                    var urlParams = new URLSearchParams(window.location.search);

                    var Id = urlParams.get("Id");

                    if (Id) {

                        GetAllReplyByCommentId(Id);

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

function DeleteReplyById(Id) {

    postRequest('/Dashboard/DeleteReplyId?Id='+Id,null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                var urlParams = new URLSearchParams(window.location.search);

                var Id = urlParams.get("Id");

                if (Id) {

                    GetAllReplyByCommentId(Id);

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


