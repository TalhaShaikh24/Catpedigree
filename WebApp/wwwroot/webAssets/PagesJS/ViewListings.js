let baseApiUrl = "";
let pageNumber = 1;
const pageSize = 8;
let varTotalCount = 0;
let varCurrentCount = 0;




$(document).ready(function () {
    baseApiUrl = $("#baseApiUrl").val();
    (async function () {
        try {
          
            await GetAllListingMarkers();
            await loadMore();
            await GetAllAdsForViewListings();
            await GetAllCatType();
            
        } catch (error) {
            console.error('Error:', error);
        }
    })();
}); 



// Get the URL parameters
let urlParams = new URLSearchParams(window.location.search);

// Extract values from parameters and assign them to variables
let keyword = urlParams.get('keyword');
let categoryId = Number(urlParams.get('categoryId'));
let listingLocation = urlParams.get('listingLocation');



async function loadMore() {
    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur');

    var obj = {
        PageNumber: pageNumber,
        PageSize: pageSize,
        CategoryId: categoryId,
        Keyword: keyword,
        Location: listingLocation,
        Currency: curr
    };

    // Show the skeleton loader
    $('#skeleton-loader').show();
    $('#appendListings').hide(); // Optionally hide existing listings

    try {
        const response = await fetch('/Listing/GetAllListingByFilters', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(obj)
        });
        const data = await response.json();

        // Hide the skeleton loader
        $('#skeleton-loader').hide();
        $('#appendListings').show(); // Show listings area again

        if (data.data) {
            const { listings, totalCount, currentCount } = data.data;
            varTotalCount = totalCount;
            varCurrentCount = currentCount;

            $.each(listings, function (index, item) {
                var html = `
                    <div class="col-lg-4 col-md-6 col-sm-12">
                        <div class="listing-item listing-grid-item-two mb-30 ${item.promotionName}">
                            <div class="listing-thumbnail">
                                ${item.videoPath && item.videoPath.trim() !== "" ? `
                                    <div class="listing-play-box wow fadeInUp" style="height: 100%; visibility: visible; animation-name: fadeInUp;">
                                        <div class="play-content bg_cover text-center d-flex align-items-center justify-content-center h-100" style="border-radius:14px; background-image: url('${baseApiUrl + item.featureImagePath}');">
                                            <a onclick = "SingleListing(${item.id})"}" href="${item.price && item.price !== "" ? `/Listing/SingleListing?listingId=${item.id}` : '#'}" target="_blank" class="video-popup">
                                                <i class="flaticon-play-button"></i>
                                            </a>
                                            ${item.isPriceRequest === true ? `
                                                <span class="featured-btn" onclick="RequestListingPrice(${item.id})">Request Price</span>
                                            ` : `
                                                <span class="featured-btn price" data-price="${item.price}">${item.price}</span>
                                            `}
                                        </div>
                                    </div>
                                ` : `
                                    <a onclick = "SingleListing(${item.id})"}" href="${item.price && item.price !== "" ? `/Listing/SingleListing?listingId=${item.id}` : '#'}" class="w-100">
                                        <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                                        ${item.isPriceRequest === true ? `
                                            <span class="featured-btn" onclick="RequestListingPrice(${item.id})">Request Price</span>
                                        ` : `
                                            <span class="featured-btn price" data-price="${item.price}">${item.price}</span>
                                        `}
                                    </a>
                                `}
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
                                        <li><span><i class="ti-location-pin"></i>${[item.location, item.state, item.city, item.country].filter(Boolean).join(', ')}</span></li>
                                    </ul>
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
        } else {
            $('#appendListings').append(`<h4>No Listing found..</h4>`);
        }
    } catch (error) {
        console.error("Error fetching listings:", error);
        // Handle error accordingly
        $('#skeleton-loader').hide();
        $('#appendListings').show();
        $('#appendListings').append(`<h4>Error loading listings. Please try again later.</h4>`);
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
                    <div class="col-lg-4 col-md-6 col-sm-12">
                        <div class="listing-item listing-grid-item-two mb-30 ${item.promotionName}">
                            <div class="listing-thumbnail">
                                ${item.videoPath && item.videoPath.trim() !== "" ? `
                                    <div class="listing-play-box wow fadeInUp" style="height: 100%; visibility: visible; animation-name: fadeInUp;">
                                        <div class="play-content bg_cover text-center d-flex align-items-center justify-content-center h-100" style="border-radius:14px; background-image: url('${baseApiUrl + item.featureImagePath}');">
                                            <a onclick = "SingleListing(${item.id})"}" href="${item.price && item.price !== "" ? `/Listing/SingleListing?listingId=${item.id}` : '#'}" target="_blank" class="video-popup">
                                                <i class="flaticon-play-button"></i>
                                            </a>
                                            ${item.isPriceRequest === true || item.price == "0" ? `
                                                <span class="featured-btn" onclick="RequestListingPrice(${item.id})">Request Price</span>
                                            ` : `
                                                <span class="featured-btn price" data-price="${item.price}">${item.price}</span>
                                            `}
                                        </div>
                                    </div>
                                ` : `
                                    <a onclick = "SingleListing(${item.id})"}" href="${item.price && item.price !== "" ? `/Listing/SingleListing?listingId=${item.id}` : '#'}" class="w-100">
                                        <img src="${baseApiUrl + item.featureImagePath}" alt="Listing Image">
                                        ${item.isPriceRequest === true || item.price == "0" ? `
                                            <span class="featured-btn" onclick="RequestListingPrice(${item.id})">Request Price</span>
                                        ` : `
                                            <span class="featured-btn price" data-price="${item.price}">${item.price}</span>
                                        `}
                                    </a>
                                `}
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

function GetAllCatType() {
    postRequest('/Listing/GetAllCatType', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                debugger;
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

//function GetSidebarAdvertisments() {
//    postRequest('/Advertisement/GetSidebarAdvertisments/' + 2, null, function (res) {
//        if (res.status == 200) {
//            if (res.data != null && res.data.length > 0) {
//                $('.sidebar-hide').hide();
//                var $imageElement = $('#advertisement-image');
//                var imageUrls = res.data.map(item => baseApiUrl + item.paidAdvertisments.replace(/\\/g, '/'));
//                var currentIndex = 0;

//                function showNextImage() {
//                    $imageElement.attr('src', imageUrls[currentIndex])
//                        .removeClass('show');

//                    setTimeout(() => {
//                        $imageElement.addClass('show');
//                    }, 50); // Small delay to ensure the image loads and transition applies

//                    currentIndex = (currentIndex + 1) % imageUrls.length;
//                }

//                // Start the image rotation
//                showNextImage();
//                setInterval(showNextImage, 3000); // Change image every 3 seconds
//            }
//        } else {
//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: res.status == 600 ? "warning" : "error"
//            });
//        }
//    });
//}

function GetAllAdsForViewListings() {
    postRequest('/Advertisement/GetAllAdsForViewListings', null, function (res) {

        if (res.status == 200) {

            if (res.data && res.data.leftSidebar && res.data.rightSidebar) {
                // Fallback URLs
                const fallbackLeft = `${baseApiUrl}UploadAdvertisements/YourAd.gif`;
                const fallbackRight = `${baseApiUrl}UploadAdvertisements/YourAdRight.gif`;

                // Left Sidebar Videos
                if (res.data.leftSidebar.length > 0 && res.data.leftSidebar[0].filePath) {
                    $("#leftAdvertisement-video-1").append(`
                        <video autoplay muted loop>
                            <source src="${baseApiUrl + res.data.leftSidebar[0].filePath.replace(/\\/g, '/')}" type="video/mp4">
                            Your browser does not support the video tag.
                        </video>
                    `);
                }
                else {
                    $("#leftAdvertisement-video-1").append(`
                        <img src="${fallbackLeft}" alt="Advertisement">
                    `);
                }

                if (res.data.leftSidebar.length > 1 && res.data.leftSidebar[1].filePath) {
                    $("#leftAdvertisement-video-2").append(`
                        <video autoplay muted loop>
                            <source src="${baseApiUrl + res.data.leftSidebar[1].filePath.replace(/\\/g, '/')}" type="video/mp4">
                            Your browser does not support the video tag.
                        </video>
                    `);
                }
                else {
                    $("#leftAdvertisement-video-2").append(`
                        <img src="${fallbackRight}" alt="Advertisement">
                    `);
                }

                // Left Sidebar Images
                for (let i = 2; i <= 4; i++) {
                    const imgSrc = (res.data.leftSidebar.length > i && res.data.leftSidebar[i].filePath)
                        ? baseApiUrl + res.data.leftSidebar[i].filePath.replace(/\\/g, '/')
                        : fallbackLeft;

                    $(`#leftAdvertisement-image-${i - 1}`).attr('src', imgSrc).on('error', function () {
                        $(this).attr('src', fallbackLeft);
                    });
                }

                // Right Sidebar Images
                for (let i = 0; i <= 4; i++) {
                    const imgSrc = (res.data.rightSidebar.length > i && res.data.rightSidebar[i].filePath)
                        ? baseApiUrl + res.data.rightSidebar[i].filePath.replace(/\\/g, '/')
                        : fallbackRight;

                    $(`#rigtAdvertisement-image-${i + 1}`).attr('src', imgSrc).on('error', function () {
                        $(this).attr('src', fallbackRight);
                    });
                }
            } else {
                console.error("Response data is incomplete or null");
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
    //const modalContent = `
    //    <ul class="list-group">
    //        <li class="list-group-item"><strong>Listing Email:</strong> ${data.listingEmail}</li>

    //    </ul>
    //`;

    //document.getElementById('modal-content').innerHTML = modalContent;

    //// Update the footer with the buttons
    //const modalFooter = `
    //    <button type="button" class="btn btn-primary" onclick="location.href='mailto:${data.listingEmail}'">Send Email</button>
    //    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
    //`;

    //document.querySelector('#dataModal .modal-footer').innerHTML = modalFooter;
    //$('#dataModal').modal('show');

    // Create the mailto link with the email address
    const mailtoLink = `mailto:${data.listingEmail}`;

    // Open the mail client directly
    window.location.href = mailtoLink;
}

function GetAllListingMarkers() {
    const savedAddress = localStorage.getItem('userAddress');

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(position => {
            const userLocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            localStorage.removeItem('userAddress'); // Clear saved address if location is allowed
            loadMarkers(userLocation);
        }, () => {
            // If geolocation fails or is blocked
            if (savedAddress) {
                getCoordinatesFromAddress(savedAddress);
            } else {
                handleLocationError();
            }
        });
    } else {
        // Geolocation not supported
        handleLocationError();
    }
}

function handleLocationError() {
    // Prompt user for address
    Swal.fire({
        title: 'OOPS,',
        text: 'Unfortunately we cannot find out your location. Would you be so kind to enter, that way we can help you to the best of our abilities',
        input: 'text',
        showCancelButton: true,
        confirmButtonText: 'Submit',
        preConfirm: (address) => {
            if (!address) {
                Swal.showValidationMessage('You need to enter an address');
            } else {
                return address;
            }
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const address = result.value;
            localStorage.setItem('userAddress', address); // Save the address
            getCoordinatesFromAddress(address);
        }
    });
}

function getCoordinatesFromAddress(address) {
    const geocodeUrl = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}`;

    fetch(geocodeUrl)
        .then(response => response.json())
        .then(data => {
            if (data.length > 0) {
                const userLocation = {
                    lat: parseFloat(data[0].lat),
                    lng: parseFloat(data[0].lon)
                };
                loadMarkers(userLocation);
            } else {
                Swal.fire({
                    title: "Error",
                    text: "Geocoding failed: Address not found.",
                    icon: "error"
                });
            }
        })
        .catch(error => {
            Swal.fire({
                title: "Error",
                text: "An error occurred while fetching geocoding data.",
                icon: "error"
            });
        });
}

function loadMarkers(userLocation) {
    postRequest('/Listing/GetAllListingMarkers', null, function (res) {
        if (res.status === 200) {
            if (res.data != null) {
                const map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 12,
                    center: userLocation
                });

                // Add the user's location marker
                new google.maps.Marker({
                    position: userLocation,
                    map: map,
                    title: 'Your Location',
                    icon: {
                        url: 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'
                    }
                });

                const customIcon = {
                    url: 'https://unpkg.com/leaflet@1.3.4/dist/images/marker-icon.png',
                    scaledSize: new google.maps.Size(25, 41),
                };

                const infoWindow = new google.maps.InfoWindow();
                let isInfoWindowOpen = false; // Flag to track InfoWindow state

                res.data.forEach(marker => {
                    const lat = marker.latitude;
                    const lng = marker.longitude;
                    const location = marker.location || "";
                    const city = marker.city || "";
                    const state = marker.state || "";
                    const country = marker.country || "";

                    const googleMarker = new google.maps.Marker({
                        position: { lat: parseFloat(lat), lng: parseFloat(lng) },
                        map: map,
                        title: `${location}, ${city}, ${state}, ${country}`,
                        icon: customIcon
                    });

                    // Add hover event to show info window
                    googleMarker.addListener('mouseover', () => {
                        infoWindow.setContent(createInfoWindowContent(marker));
                        infoWindow.open(map, googleMarker);
                        isInfoWindowOpen = true; // Set flag when InfoWindow is open
                    });

                    // Add mouseout event for the marker
                    googleMarker.addListener('mouseout', () => {
                        if (!isInfoWindowOpen) {
                            infoWindow.close();
                        }
                    });

                    // Prevent the InfoWindow from closing when hovering over it
                    infoWindow.addListener('domready', () => {
                        const infowindowDiv = document.querySelector('.gm-style-iw');
                        if (infowindowDiv) {
                            infowindowDiv.addEventListener('mouseover', () => {
                                isInfoWindowOpen = true; // Keep InfoWindow open
                            });
                            infowindowDiv.addEventListener('mouseout', () => {
                                isInfoWindowOpen = false; // Set flag when mouse leaves
                                setTimeout(() => {
                                    if (!isInfoWindowOpen) {
                                        infoWindow.close(); // Close the InfoWindow if not hovered
                                    }
                                }, 100); // Allow time for mouse to move from marker to InfoWindow
                            });
                        }
                    });

                    // Add click event to focus on the marker
                    googleMarker.addListener('click', () => {
                        googleMarker.setAnimation(google.maps.Animation.BOUNCE);
                        map.setZoom(14);
                        map.setCenter(googleMarker.getPosition());

                        setTimeout(() => {
                            googleMarker.setAnimation(null);
                        }, 1400);
                    });
                });
            }
        }

        handleResponseErrors(res);
    });
}

function createInfoWindowContent(marker) {
    return `
       <div style="width: 250px; max-height: 320px; overflow-y: auto; font-family: Arial, sans-serif;">
            <a onclick = "SingleListing(${marker.id})" href="${`/Listing/SingleListing?listingId=${marker.id}`}" class="w-100">
            <img src="${baseApiUrl + marker.featureImagePath}" alt="${marker.title}" style="width: 150px; margin-bottom:10px; height: auto;"/>
            <h5 style="font-weight:600!important">${marker.title}</h4>
            </a>
            <span class="badge badge-red mb-2">${marker.categoryName}</span>
            <p>Location: ${marker.location}</p>
        </div>
    `;
}

function handleResponseErrors(res) {
    switch (res.status) {
        case 304:
        case 305:
        case 401:
        case 403:
        case 320:
        case 500:
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            });
            break;
        case 600:
            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            });
            break;
        default:
            console.log('Unhandled status: ', res.status);
            break;
    }
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
