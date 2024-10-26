
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    var urlParams = new URLSearchParams(window.location.search);

    var blogId = urlParams.get("Id");

    if (blogId) {
        ShowDetails(blogId);

    }

});


function ShowDetails(Id) {

    postRequest('/Show/GetAllShowDetails/' + Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger;
                $("#BlogDetails_Append").append(`


                        <h1 >${res.data.title}</h1>
                        <div class="post-thumbnail">
                            <img class="w-100" src="${baseApiUrl + res.data.featureImagePath}" alt="Blog Image">
                        </div>
                        <div class="entry-content">
                            <div class="post-meta d-none">
                                <ul>
                                    <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">${res.data.username}</a></span></li>
                                    <li><span><i class="ti-calendar"></i><a href="javascript:void(0);">${moment(res.data.createdOn).format("DD MMMM - YYYY")}</a></span></li>
                                </ul>
                            </div>


                              ${res.data.content}
                         
                       </div>`);

                console.log(res.data.gallaryImagePath)
                $("#HDBLOGID").val(res.data.showId);


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