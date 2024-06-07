$(document).ready(function () {
    GetHomePageListings();
    GetHomePageBlogs();
})


$(document).on("click", "#btnSearch", function () {
    // Get values from input fields
    var keyword = $("#keyword").val().trim(); // trim to remove leading/trailing whitespaces
    var categoryId = Number($("#categorySelect").val());
    var location = $("#location").val().trim(); // trim to remove leading/trailing whitespaces

    // Basic validation
    if (keyword === "") {
        alert("Please enter a keyword.");
        return;
    }

    // Construct the URL
    var url = `/Listing/ViewListings?keyword=${encodeURIComponent(keyword)}&categoryId=${categoryId}&listingLocation=${encodeURIComponent(location)}`;

    // Redirect to the constructed URL
    window.location.href = url;
});


function GetHomePageListings() {
    postRequest('/Listing/GetHomePageListings', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {

                 

                    var html = `
                        <div class="col-lg-4 col-md-6 col-sm-12" >
                            <div class="listing-item listing-grid-item-two mb-30" style="border: ${item.propertiestoShow};">
                                <div class="listing-thumbnail">
                                    <img src="https://localhost:7280/${item.featureImagePath}" alt="Listing Image">
                                </div>
                                <div class="listing-content">
                                    <h3 class="title">

                                        <span class="status st-close">${item.categoryName}</span>
                                        <a href="/Listing/SingleListing?listingId=${item.id}">${item.title}</a></h3>
                                    <p style="font-weight: ${item.propertiestoShow};">${item.description}</p>
                                    <div class="listing-meta">
                                        <ul>
                                            <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendListings').append(html);
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


function GetHomePageBlogs() {
    postRequest('/Blog/GetHomePageBlogs', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {

                    var html = `
                        <div class="col-lg-4 col-md-6 col-sm-12">
                            <div class="blog-post-item blog-post-item-three mb-40 wow fadeInUp">
                                <div class="post-thumbnail">
                                    <a href="blog-details.html"><img src="https://localhost:7280/${item.featureImagePath}" alt="Blog Image"></a>
                                    <div class="post-date"><a href="#">${moment(item.createdOn).format("DD")}<span>${moment(item.createdOn).format("MMMM")}</span></a></div>
                                </div>
                                <div class="entry-content">
                                    <a href="#" class="cat-btn"><i class="ti-bookmark-alt"></i>Tours & Travel</a>
                                    <h3 class="title">
                                        <a href="/Blog/BlogDetails?Id=${item.blogID}">
                                            ${item.title}
                                        </a>
                                    </h3>
                                    <div class="post-meta">
                                        <ul>
                                            <li><span><i class="ti-comments-smiley"></i><a href="javascript:void(0)">${item.commentsCount} Comments</a></span></li>
                                            <li><span><i class="ti-id-badge"></i><a href="#">By admin</a></span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendBlogs').append(html);
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



function BuyPackage(pkgId) {

    var obj = {
        PackageID: Number(pkgId)
    }
    postRequest('/Packages/BuyPackage', obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

               
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