
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

     GetAllBlogs();
})

function GetAllBlogs() {
    postRequest('/Blog/GetAllBlogs',null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#Blog").empty();
                $.each(res.data, function (i,v) {

                    debugger
                    $("#Blog").append(`<div class="blog-standard-wrapper pb-50">
                   
                    <div class="blog-post-item blog-post-item-four mb-50 wow fadeInUp">
                        <div class="post-thumbnail">
                            <a href="/Blog/BlogDetails?Id=${v.blogID}"><img class="w-100" src="${baseApiUrl+v.featureImagePath}" alt="Blog Image"></a>
                        </div>
                        <div class="entry-content">
                            <a href="javascript:void(0);" class="cat-btn">${moment(v.createdOn).format("DD MMMM - YYYY")}</a>
                            <div class="post-meta">
                                <ul>
                                    <li><span><i class="ti-bookmark-alt"></i><a href="javascript:void(0);">Tours & Travel</a></span></li>
                                    <li><span><i class="ti-comments-smiley"></i><a href="javascript:void(0);">${v.commentsCount} Comment</a></span></li>
                                    <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">By admin</a></span></li>
                                </ul>
                            </div>
                            <h3 class="title"><a href="/Blog/BlogDetails?Id=${v.blogID}">${v.title}</a></h3>
                            <p>${v.shortDescription}</p>
                            <a href="/Blog/BlogDetails?Id=${v.blogID}" class="btn-link">Continue Reading</a>
                        </div>
                    </div>
                   
                </div>`);
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

