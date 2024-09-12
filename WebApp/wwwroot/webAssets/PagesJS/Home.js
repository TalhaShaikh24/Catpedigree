
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    GetHomePageListings();
    GetTopPageListings();
    //GetVetRimmedPageListings();
    // GetHomeAdvertisments();
})


$(document).on("click", "#btnSearch", function () {
    // Get values from input fields
    var keyword = $("#keyword").val().trim(); // trim to remove leading/trailing whitespaces
    var categoryId = Number($("#categorySelect").val());
    var location = $("#location").val().trim(); // trim to remove leading/trailing whitespaces



    // Construct the URL
    var url = `/Listing/ViewListings?keyword=${encodeURIComponent(keyword)}&categoryId=${categoryId}&listingLocation=${encodeURIComponent(location)}`;

    // Redirect to the constructed URL
    window.location.href = url;
});


function GetTopPageListings() {

    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')
    debugger
    postRequest('/Listing/GetTopPageListings/' + curr, null, function (res) {

        if (res.status == 200) {

            if (res.data.length > 0) {

                $.each(res.data, function (index, item) {

                    var html = `
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="listing-item listing-grid-item-two mb-30 ${item.promotionName}">
                                <div class="listing-thumbnail">
                                    ${item.videoPath && item.videoPath.trim() !== "" ?
                                        `<div class="listing-play-box wow fadeInUp" style="height: 100%; visibility: visible; animation-name: fadeInUp;">
                                             <div class="play-content bg_cover text-center d-flex align-items-center justify-content-center h-100" style="border-radius:14px; background-image: url('${baseApiUrl + item.featureImagePath}');">
                                                 <a href="/Listing/SingleListing?listingId=${item.id}" target="_blank" class="video-popup"><i class="flaticon-play-button"></i></a>
                                                 ${item.price && item.price !== "" ?
                                                    `<span class="featured-btn price" data-price="${item.price}">${item.price}</span>` :
                                                    ''}
                                             </div>
                                         </div>` :
                                        `<a href="/Listing/SingleListing?listingId=${item.id}" class="w-100">
                                             <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                                             ${item.price && item.price !== "" ?
                                                `<span class="featured-btn price" data-price="${item.price}">${item.price}</span>` :
                                                ''}
                                         </a>`
                                     }
                                </div>
                                <div class="listing-content">
                                    <div class="title d-flex justify-content-between align-items-center mb-10">
                                        <span class="status st-close category_name" style="height:24px;">${item.categoryName}</span>
                                    </div>
                                    <h3 class="title">
                                        <a onclick="SingleListing(${item.id})">${item.title}</a>
                                    </h3>
                                    <p style="font-weight: ${item.propertiestoShow};" class="text_limit_2 d-none">${item.description}</p>
                                    <div class="listing-meta">
                                        <ul>
                                            <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                                        </ul>
                                        <button type="button" class="d-none btn btn-secondary w-100 mt-3" style="font-weight:bold!important;" onclick="RequestListingPrice(${item.id})">
                                            Request Price
                                            <span class="spinner-btn"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendTopListings').append(html);
                });
                $("#sectionTopListings").show();
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

    var selectedCurrency = localStorage.getItem('cur');
    debugger;

    updatePrices(selectedCurrency);

}

function GetVetRimmedPageListings() {
    postRequest('/Listing/GetVetRimmedPageListings', null, function (res) {

        if (res.status == 200) {

            if (res.data.length > 0) {

                $.each(res.data, function (index, item) {

                    var html = `
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="listing-item listing-grid-item-two mb-30 ${item.promotionName}">
                                <div class="listing-thumbnail">
                                   <a href="/Listing/SingleListing?listingId=${item.id}" class="w-100">
                                      <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                                   </a>
                                </div>
                                <div class="listing-content">
                                    <div class="title d-flex justify-content-between align-items-center mb-10">
                                        <span class="status st-close category_name" style="height:24px;">${item.categoryName}</span>
                                        <h4 class="status price" data-price="${item.price}">${item.price}</h4>
                                    </div>
                                    <h3 class="title">
                                        <a href="/Listing/SingleListing?listingId=${item.id}">${item.title}</a>
                                    </h3>
                                    <p style="font-weight: ${item.propertiestoShow};" class="text_limit_2 d-none">${item.description}</p>
                                    <div class="listing-meta">
                                        <ul>
                                            <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                                        </ul>
                                        <button type="button" class="d-none btn btn-secondary w-100 mt-3" style="font-weight:bold!important;" onclick="RequestListingPrice(${item.id})">
                                            Request Price
                                            <span class="spinner-btn"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendVetandRimmedListings').append(html);
                });

                $("#sectionVetRimmedListings").show();
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
function GetHomePageListings() {

    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')
  

    postRequest('/Listing/GetHomePageListings/' + curr, null, function (res) {

        if (res.status == 200) {

            if (res.data.length > 0) {

                $.each(res.data, function (index, item) {
                    var html = `
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="listing-item listing-grid-item-two mb-30 ${item.promotionName}">
                                <div class="listing-thumbnail">
                                    ${item.videoPath && item.videoPath.trim() !== "" ?
                        
                                 `
                                     <div class="listing-play-box wow fadeInUp" style="height: 100%; visibility: visible; animation-name: fadeInUp;">
                                         <div class="play-content bg_cover text-center d-flex align-items-center justify-content-center h-100" style="border-radius:14px; background-image: url('${ baseApiUrl + item.featureImagePath}');">
                                             <a href="/Listing/SingleListing?listingId=${item.id}" target="_blank" class="video-popup"><i class="flaticon-play-button"></i></a>
                                              ${item.price && item.price !== "" ?
                                                `<span class="featured-btn price" data-price="${item.price}">${item.price}</span>` :
                                                ''}
                                         </div>
                                     </div>` :
                                     `<a href="/Listing/SingleListing?listingId=${item.id}" class="w-100">
                                         <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                                         ${item.price && item.price !== "" ?
                                            `<span class="featured-btn price" data-price="${item.price}">${item.price}</span>` :
                                            ''}
                                     </a>`
                                        }
                                </div>
                                <div class="listing-content">
                                    <div class="title d-flex justify-content-between align-items-center mb-10">
                                        <span class="status st-close category_name" style="height:24px;">${item.categoryName}</span>
                                    </div>
                                    <h3 class="title">
                                        <a onclick="SingleListing(${item.id})">${item.title}</a>
                                    </h3>
                                    <p style="font-weight: ${item.propertiestoShow};" class="text_limit_2 d-none">${item.description}</p>
                                    <div class="listing-meta">
                                        <ul>
                                            <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                                        </ul>
                                        <button type="button" class="d-none btn btn-secondary w-100 mt-3" style="font-weight:bold!important;" onclick="RequestListingPrice(${item.id})">
                                            Request Price
                                            <span class="spinner-btn"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendListings').append(html);
                });


                GetHomePageBlogs();
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

            if (res.data.length > 0) {

                $.each(res.data, function (index, item) {

                    var html = `
                        <div class="col-lg-4 col-md-6 col-sm-12">
                            <div class="blog-post-item blog-post-item-three mb-40 wow fadeInUp">
                                <div class="post-thumbnail">
                                    <a href="/Blog/BlogDetails?Id=${item.blogID}"><img src="${baseApiUrl + item.featureImagePath}" alt="Blog Image"></a>
                                    <div class="post-date"><a href="#">${moment(item.createdOn).format("DD")}<span>${moment(item.createdOn).format("MMMM")}</span></a></div>
                                </div>
                                <div class="entry-content">
                                    <a href="#" class="cat-btn"><i class="ti-bookmark-alt"></i>Tours & Travel</a>
                                    <h3 class="title">
                                        <a href="/Blog/BlogDetails?Id=${item.blogID}">
                                            ${item.title}
                                        </a>
                                    </h3>
                                    <div class="post-shortDesc mb-10">
                                    <p>${item.shortDescription}</p>
                                    </div>
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


                var selectedCurrency = localStorage.getItem('cur');
                updatePrices(selectedCurrency);
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

function GetHomeAdvertisments() {

    postRequest('/Advertisement/GetHomeAdvertisments/' + 1, null, function (res) {

        if (res.status === 200 && res.data && res.data.length > 0) {
            res.data.forEach(function (item, index) {
                var html = `
            <div class="carousel-item ${index === 0 ? 'active' : ''}">
                <img class="d-block w-100" src="${baseApiUrl + item.paidAdvertisments}" alt="${item.alt}">
            </div>`;
                $('#carouselExampleIndicators .carousel-inner').append(html);
            });
        }
        else {
            $("#advertisemnetSection").hide();
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


function SingleListing(ListId) {
    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    window.location.href = `/Listing/SingleListing?listingId=${ListId}&currency=${curr}`;



}

function BuyPackage(pkgId) {

    var obj = {
        PackageID: Number(pkgId)
    }
    postRequest('/Packages/BuyPackage', obj, function (res) {

        if (res.status == 200) {

            if (res.data.length > 0) {


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


function RequestListingPrice(listingID) {
    // Get the button element
    var button = $('button[onclick="RequestListingPrice(' + listingID + ')"]');
    var spinner = button.find('.spinner-btn');

    // Show spinner and hide button text
    button.prop('disabled', true); // Disable button to prevent multiple clicks
    spinner.show();
    postRequest('/Listing/RequestListingPrice?listingID=' + listingID, null, function (res) {

        // Hide spinner and enable button
        spinner.hide();
        button.prop('disabled', false);
        if (res.status == 200 && res.data != null) {

            // Call the function to show the modal
            showModal(res.data);


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

// Function to open modal and display data
function showModal(data) {
    const modalContent = `
                <ul class="list-group">
                    <li class="list-group-item"><strong>Listing Email:</strong> ${data.listingEmail}</li>
                    <li class="list-group-item"><strong>Phone:</strong> ${data.phone}</li>
                    <li class="list-group-item"><strong>Breerder Name:</strong> ${data.breerderName}</li>
                    <li class="list-group-item"><strong>Author:</strong> ${data.firstname}</li>
                    <li class="list-group-item"><strong>Email:</strong> ${data.email}</li>
                </ul>
            `;
    document.getElementById('modal-content').innerHTML = modalContent;
    $('#dataModal').modal('show');
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