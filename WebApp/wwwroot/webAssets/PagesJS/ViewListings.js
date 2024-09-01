let baseApiUrl = "";
let pageNumber = 1;
const pageSize = 8;
let varTotalCount = 0;
let varCurrentCount = 0;

$(document).ready(function () {
    GetAllCatType();
})

// Get the URL parameters
let urlParams = new URLSearchParams(window.location.search);

// Extract values from parameters and assign them to variables
let keyword = urlParams.get('keyword');
let categoryId = Number(urlParams.get('categoryId'));
let listingLocation = urlParams.get('listingLocation');

async function loadMore() {
    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')


    var obj = {
        PageNumber: pageNumber,
        PageSize: pageSize,
        CategoryId: categoryId,
        Keyword: keyword,
        Location: listingLocation,
        Currency: curr
    }


    

    const response = await fetch('/Listing/GetAllListingByFilters', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });
    const data = await response.json();

    if (data.data) {
        const { listings, totalCount, currentCount } = data.data;
        varTotalCount = totalCount;
        varCurrentCount = currentCount;

        // $('#appendListings').empty();
        $.each(listings, function (index, item) {

            var html = `
                            <div class="col-lg-4 col-md-6 col-sm-12" >
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
                                        <button type="button" class="btn btn-secondary w-100 mt-3" style="font-weight:bold!important;" onclick="RequestListingPrice(${item.id})">
                                            Request Price
                                            <span class="spinner-btn"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            </div>`;
            $('#appendListings').append(html);
            $("#load-more").show();
        });




        var selectedCurrency = localStorage.getItem('cur');
        updatePrices(selectedCurrency);

        pageNumber++;
        updateCountDisplay();

        if (varCurrentCount >= varTotalCount) {
            document.getElementById('load-more').style.display = 'none';
        }
    }
    else {
        $('#appendListings').append(`<h4>No Listing found..</h4>`);
    }
}

async function filteringSearch() {

    var sliderValues = $("#slider-range").slider("values");
    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')


    var obj = {
        PageNumber: 1,
        PageSize: 40,
        CategoryId: Number($("#categoryFilter").val()),
        Keyword: $("#keywordFilter").val(),
        Location: $("#locationFilter").val(),
        State: $("#stateFilter").val(),
        City: $("#cityFilter").val(),
        TypeOfCat: Number($("#breedFilter").val()),
        PriceMin: sliderValues[0],
        PriceMax: sliderValues[1],
        Currency: curr

    }
    debugger
    const response = await fetch('/Listing/GetAllListingByFilters', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });
    const data = await response.json();

    if (data.data) {
        const { listings, totalCount, currentCount } = data.data;
        varTotalCount = totalCount;
        varCurrentCount = currentCount;

        $('#appendListings').empty();

        $.each(listings, function (index, item) {

            var html = `
             <div class="col-lg-4 col-md-6 col-sm-12" >
                 <div class="listing-item listing-grid-item-two mb-30  ${item.promotionName}">
                     <div class="listing-thumbnail">
                        <a href="/Listing/SingleListing?listingId=${item.id}" class="w-100">
                           <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                        </a>
                     </div>
                     <div class="listing-content">
                         <div class="title d-flex justify-content-between align-items-center mb-10">
                             <span class="status st-close category_name" style="height:24px;">${item.categoryName}</span>
                              <h4 class="status   price" data-price="${item.price}"> ${item.price}</h4>
                         </div>
                         <h3 class="title">
                             <a href="/Listing/SingleListing?listingId=${item.id}">${item.title}</a>
                         </h3>
                         <p class="text_limit_2 d-none" style="font-weight: ${item.propertiestoShow};">${item.description}</p>
                         <div class="listing-meta">
                             <ul>
                                 <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                             </ul>
                             <button type="button" class="btn btn-secondary w-100 mt-3" style="font-weight:bold!important;" onclick="RequestListingPrice(${item.id})">
                                 Request Price
                                 <span class="spinner-btn"></span>
                             </button>
                         </div>
                     </div>
                 </div>
             </div>`;
            $('#appendListings').append(html);
            $("#load-more").show();
        });


        

        var selectedCurrency = localStorage.getItem('cur');
        updatePrices(selectedCurrency);

        pageNumber++;
        updateCountDisplay();

        if (varCurrentCount >= varTotalCount) {
            document.getElementById('load-more').style.display = 'none';
        }
    }
    else {
        $('#load-more').hide();
        $('#appendListings').empty();
        $('#appendListings').append(`<h4>No Listing found..</h4>`);
    }
}

function updateCountDisplay() {
    const countDisplay = document.getElementById('count-display');
    countDisplay.textContent = `${varCurrentCount}/${varTotalCount}`;
}

document.addEventListener('DOMContentLoaded', () => {
    baseApiUrl = $("#baseApiUrl").val();
    loadMore();
    GetSidebarAdvertisments();
});


function GetAllCatType() {
    postRequest('/Listing/GetAllCatType', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (i, v) {

                    $("#appendCatTypes").after(`
                      <option value="${v.id}">${v.catType}</option>
                      `);

                });
                $('#breedFilter').niceSelect('destroy');

                $('#breedFilter').niceSelect();

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


function GetSidebarAdvertisments() {
    postRequest('/Advertisement/GetSidebarAdvertisments/' + 2, null, function (res) {
        if (res.status == 200) {
            if (res.data != null && res.data.length > 0) {
                $('.sidebar-hide').hide();
                var $imageElement = $('#advertisement-image');
                var imageUrls = res.data.map(item => baseApiUrl + item.paidAdvertisments.replace(/\\/g, '/'));
                var currentIndex = 0;

                function showNextImage() {
                    $imageElement.attr('src', imageUrls[currentIndex])
                        .removeClass('show');

                    setTimeout(() => {
                        $imageElement.addClass('show');
                    }, 50); // Small delay to ensure the image loads and transition applies

                    currentIndex = (currentIndex + 1) % imageUrls.length;
                }

                // Start the image rotation
                showNextImage();
                setInterval(showNextImage, 3000); // Change image every 3 seconds
            }
        } else {
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: res.status == 600 ? "warning" : "error"
            });
        }
    });
}

function SingleListing(ListId) {
    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    window.location.href = `/Listing/SingleListing?listingId=${ListId}&currency=${curr}`;



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
