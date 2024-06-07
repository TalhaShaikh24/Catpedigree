
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    var urlParams = new URLSearchParams(window.location.search);

    var blogId = urlParams.get("Id");

    if (blogId)
    {
        BlogDetails(blogId);

    }

});


function BlogDetails(Id) {

    postRequest('/Blog/GetAllBlogDetails/' + Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
               
                $("#BlogDetails_Append").empty();
                $("#Comments_Append").empty();

                $.each(res.data.item1, function (i, v) {
                    $("#BlogDetails_Append").append(`
                        <div class="post-thumbnail">
                            <img class="w-100" src="${baseApiUrl+v.featureImagePath}" alt="Blog Image">
                        </div>
                        <div class="entry-content">
                            <div class="post-meta">
                                <ul>
                                    <li><span><i class="ti-bookmark-alt"></i><a href="javascript:void(0);">Tours & Travel</a></span></li>
                                    <li><span><i class="ti-comments-smiley"></i><a href="javascript:void(0);">${v.commentsCount} Comment</a></span></li>
                                    <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">By admin</a></span></li>
                                    <li><span><i class="ti-calendar"></i><a href="javascript:void(0);">${moment(v.createdOn).format("DD MMMM - YYYY")}</a></span></li>
                                </ul>
                            </div>

                              <h3 class="title">${v.title}</h3>

                              ${v.content}
                            <div class="comments-area"></div>
                       </div>`);

                    $(".comments-title").text(`Comment (${v.commentsCount})`);
                    $("#HDBLOGID").val(v.blogID);
                });

                $.each(res.data.item2, function (i, v) {

                    debugger
                    $("#Comments_Append").append(`
                    <li class="comment">
                        <div class="comment-avatar">
                            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRWMDER8CUNhcQ4xBIWth31rBxkrquqpehmYg&amp;s" alt="comment author one">
                        </div>
                        <div class="comment-wrap">
                            <div class="comment-author-content">
                                <span class="author-name">${v.userName}<span class="date">${moment(v.commentDate).format("DD MMMM - YYYY")}</span></span>
                                <p>${v.commentText}</p>
                                ${(() => {
                                            let repliesHtml = '';
                        $.each(res.data.item3, function (i, r) {
                            debugger
                                                if (v.id == r.commentId && v.userId == r.userId) {
                                                    repliesHtml += `
                                                <div class="comment">
                                                    <div class="comment-avatar">
                                                        <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRWMDER8CUNhcQ4xBIWth31rBxkrquqpehmYg&amp;s" alt="comment author one">
                                                    </div>
                                                    <div class="comment-wrap">
                                                        <div class="comment-author-content">
                                                            <span class="author-name d-block">Reply</span>
                                                            <span class="author-name">By Admin<span class="date">${moment(r.replyDate).format("DD MMMM - YYYY")}</span></span>
                                                            <p>${r.replyText}</p>
                                                        </div>
                                                    </div>
                                                </div>`;
                                                }
                                            });
                                            return repliesHtml;
                                        })()}
                            </div>
                        </div>
                    </li>`);

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


$("#SubmitReview").click(function () {

    var obj = {

        BlogID: $("#HDBLOGID").val(),
        CommentText: $("#Comment").val()
    }
    postRequest('/Blog/AddComment',obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                var urlParams = new URLSearchParams(window.location.search);

                var blogId = urlParams.get("Id");

                if (blogId) {

                   BlogDetails(blogId);

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
});


function postRequest(url, requestData, handledata) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        dataType: "json",
        url: url,
        data: JSON.stringify(requestData),
        success: function (data, textStatus, xhr) {

            handledata(data);
        },
        error: function (xhr, textStatus, errorThrown) {
            Swal.fire({
                title: "Error",
                text: "Something Went Wrong!",
                icon: "error",
                dangerMode: true,
            })
        }
    });
}