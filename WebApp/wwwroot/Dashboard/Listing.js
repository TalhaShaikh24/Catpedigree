var GalleryFilesUpload = [];
var FeaturedFileUpload = null;
var PedigreeFileUpload = null;
let baseApiUrl = "";
let latitude = "";
let longitude = "";

$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    // Define and call the async function
    (async function () {
        try {
            await GetAllDropdowns();
            await GetAllListingFiltersDashboard();
            await GetAllListings();
        } catch (error) {
            console.error('Error:', error);
        }
    })();
    $('#Phone').intlTelInput({
        initialCountry: 'br',
        preferredCountries: ['us', 'gb', 'br', 'ru', 'cn', 'es', 'it'],
        autoPlaceholder: 'aggressive',
        separateDialCode: true,
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/12.1.6/js/utils.js"
    });


});


$('#UpdateListingModal').on('shown.bs.modal', function () {
    setTimeout(function () {
        initAutocomplete(); // Initialize autocomplete after a slight delay
    }, 100);
});

var autocomplete;

function initAutocomplete() {
    var input = document.getElementById('Location');
    if (input) {
        autocomplete = new google.maps.places.Autocomplete(input);
        autocomplete.addListener('place_changed', onPlaceChanged);
    } else {
        console.error('Location input not found.');
    }
}

function onPlaceChanged() {
    const place = autocomplete.getPlace();
    if (!place.geometry) {
        console.log("No details available for input: '" + place.name + "'");
        return;
    }

    const addressComponents = place.address_components;
    let city = "";
    let state = "";
    let country = "";

    // Extracting latitude and longitude
    const latitude = place.geometry.location.lat();
    const longitude = place.geometry.location.lng();

    for (const component of addressComponents) {
        const types = component.types;
        if (types.includes("locality")) {
            city = component.long_name;
        }
        if (types.includes("administrative_area_level_1")) {
            state = component.short_name;
        }
        if (types.includes("country")) {
            country = component.long_name;
        }
    }

    // Log city, state, and country
    console.log('City:', city);
    console.log('State:', state);
    console.log('Country:', country);

    $("#State").val(state);
    $("#City").val(city);
}



function GetAllDropdowns() {


    postRequest('/Dashboard/GetAllDropdowns', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {


                debugger

                $("#PackageId").empty();
                $("#Category").empty();
                $("#TypeOfCat").empty();



                $("#PromotionPackageId").empty();

                $("#PackageId").append(`<option value="-1" disabled selected>Select Packages</option>`);
                $("#Category").append(`<option value="-1" disabled selected>Select Category</option>`);
                $("#TypeOfCat").append(`<option value="-1" disabled selected>Select Type Of Cat</option>`);
                $("#PromotionPackageId").append(`<option value="-1" disabled selected>Select Packages</option>`);

                $.each(res.data.item3, function (i, v) {
                    $("#PackageId").append(`<option value="${v.packageID}">${v.name}</option>`);
                });

                $.each(res.data.item2, function (i, v) {
                    $("#TypeOfCat").append(`<option value="${v.id}">${v.catType}</option>`);
                });

                $.each(res.data.item1, function (i, v) {
                    $("#Category").append(`<option value="${v.id}">${v.categoryName}</option>`);
                });




                GetAllListings()

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

function GetAllListingFiltersDashboard() {


    postRequest('/Dashboard/GetAllListingFiltersDashboard', null, function (res) {

        if (res.status == 200 && res.data != null) {
            // Populate filter dropdowns using the separate API response
            populateFilterOptions('#filterCategoryName', res.data.item1, 'categoryName');
            populateFilterOptions('#filterCatType', res.data.item2, 'catType');
            populateFilterOptions('#filterPackageName', res.data.item3, 'name');
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
function GetAllListings() {

    postRequest('/Dashboard/GetAllListings', null, function (res) {

        if (res.status == 200 && res.data != null) {
            // Check if the DataTable exists and destroy it if it does
            if ($.fn.DataTable.isDataTable('#TableApprovalListing')) {
                $('#TableApprovalListing').DataTable().destroy();
            }

            $("#AppendApprovalListing").empty();

            $.each(res.data, function (i, v) {
                var statusIcon = "";
                if (v.status == "Approve") {
                    statusIcon = '<span class="badge badge-info">Approved</span>';
                } else if (v.status == "Reject") {
                    statusIcon = '<span class="badge badge-danger">Rejected</span>';
                } else {
                    statusIcon = '<span class="badge badge-warning">Pending</span>';
                }

                $("#AppendApprovalListing").append(`
                    <tr>
                        <td>${v.id}</td>
                        <td>${statusIcon}</td>
                        <td>${v.title}</td>
                        <td>${v.email}</td>
                        <td>${v.catType}</td>
                        <td>${v.categoryName}</td>
                        <td>${v.packageName}</td>
                        <td>${v.promotionName}</td>
                        <td>${v.isActive}</td>
                        <td>${moment(v.createdOn).format("DD - MMMM - YYYY")}</td>
                        <td style="display: flex; justify-content: space-evenly; align-items: center;">
                            <button id="btn_Listing_Edit" type="button" class="btn btn-info btn-xs p-2 mx-1" data-id="${v.id}"><i class="fa fa-edit"></i></button>
                            <button type="button" class="btn btn-success btn-xs p-2 mx-1" onclick="UpdateListingStatus(${v.id}, 'Approve');" title="Approve"><i class="fa fa-check" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-info btn-xs p-2 mx-1" title="Reject" onclick="showReasonModal(${v.id}, 'Reject');"><i class="fa fa-ban" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-warning btn-xs p-2 mx-1" title="Pending" onclick="UpdateListingStatus(${v.id}, 'Pending');"><i class="fa fa-clock" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-danger btn-xs p-2 mx-1" id="btn_Listing_Delete" title="Delete Listing" data-id="${v.id}"><i class="fa fa-trash"></i></button>
                        </td>
                    </tr>`);
            });

            // Initialize DataTable
            var table = $('#TableApprovalListing').DataTable({
                "order": [[8, 'desc']], // Assuming the createdOn column is the 9th column (index 8)
                "columnDefs": [
                    {
                        "targets": 8, // Index of the createdOn column
                        "type": "date",
                        "render": function (data, type, row) {
                            // Convert the date format to something sortable
                            return moment(data, "DD - MMMM - YYYY").format("YYYY-MM-DD");
                        }
                    }
                ]
            });

            // Apply custom filtering for dropdowns
            $('#filterStatus, #filterCatType, #filterCategoryName, #filterPackageName').on('change', function () {
                table.draw();
            });

            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var status = $('#filterStatus').val();
                    var catType = $('#filterCatType').val();
                    var categoryName = $('#filterCategoryName').val();
                    var packageName = $('#filterPackageName').val();

                    var rowStatus = $(table.row(dataIndex).node()).find('td:eq(0) span').text().trim();
                    var rowCatType = data[3] || '';
                    var rowCategoryName = data[4] || '';
                    var rowPackageName = data[5] || '';

                    if ((status === '' || rowStatus === status) &&
                        (catType === '' || rowCatType === catType) &&
                        (categoryName === '' || rowCategoryName === categoryName) &&
                        (packageName === '' || rowPackageName === packageName)) {
                        return true;
                    }
                    return false;
                }
            );
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

// Function to populate filter options dynamically
function populateFilterOptions(selector, options, key) {
    $(selector).empty().append('<option value="">All</option>'); // Reset options
    $.each(options, function (index, option) {
        $(selector).append(`<option value="${option[key]}">${option[key]}</option>`);
    });
}

// Function to show modal and handle form submission
function showReasonModal(id, status) {

    $('#reasonModal').modal('show');

    $('#submitReason').off('click').on('click', function () {
        var reason = $('#reason').val();

        if (reason.trim() === '') {
            Swal.fire({
                title: 'Error',
                text: 'Reason cannot be empty.',
                icon: 'error'
            });
            return;
        }

        // Call the function to update listing status with the reason
        UpdateListingStatus(id, status, reason);

        // Close the modal
        $('#reasonModal').modal('hide');
    });
}


function UpdateListingStatus(id, status, reason = "") {

    FilePostRequest(`/Dashboard/UpdateListingStatus?Id=${id}&Status=${status}&Reason=${encodeURIComponent(reason)}`, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                GetAllListings();
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


$(document).on("click", "#btn_Listing_Delete", function (e) {
    const listingId = Number(e.currentTarget.dataset.id);

    // Show confirmation dialog
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!'
    }).then((result) => {
        if (result.isConfirmed) {

            // Proceed with the deletion if confirmed
            postRequest('/Dashboard/DeleteListingById?Id=' + Number(listingId), null, function (res) {

                if (res.status == 200) {

                    Swal.fire({
                        title: "Success",
                        text: res.responseMsg,
                        icon: "success"
                    })
                    GetAllListings();

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
    });
});


$(document).on("click", "#btn_Listing_Edit", function (e) {

    $("#status_reason").text('')

    GalleryFilesUpload = [];
    FeaturedFileUpload = null;
    PedigreeFileUpload = null;

    const statusReasonElement = document.querySelector('#status_reason');

    // Remove any existing <p> element after .status_reason
    const existingParagraph = statusReasonElement.nextElementSibling;

    if (existingParagraph && existingParagraph.tagName === 'P') {

        existingParagraph.remove();
    }

    postRequest('/Dashboard/GetListingDetailById/' + Number(e.currentTarget.dataset.id), null, function (res) {


        if (res.status == 200) {

            if (res.data != null) {

                if (res.data.status == "Approve") {
                    $("#status_reason")
                        .text("Approved")
                        .removeClass() // Remove all previous classes
                        .addClass("badge badge-info");
                }
                else if (res.data.status == "Reject") {
                    $("#status_reason")
                        .text('Rejected')
                        .removeClass() // Remove all previous classes
                        .addClass("badge badge-danger")
                        .css({ "line-height": "normal" });

                    // Append the reason if it is not null
                    if (res.data.reason != null) {
                        $("#status_reason").after('<p class="m-0 mx-2" style="color:red">' + res.data.reason + '</p>');
                    }
                }
                else {
                    $("#status_reason")
                        .text('pending')
                        .removeClass() // Remove all previous classes
                        .addClass("badge badge-warning");
                }


                $("#HDID").val(res.data.id);
                $("#PackageId").val(res.data.packageId);
                $("#Title").val(res.data.title);
                $("#Gender").val(res.data.gender);
                $("#TypeOfCat").val(res.data.typeOfCat);
                $("#Age").val(res.data.age);
                $("#Description").val(res.data.description);
                $("#Location").val(res.data.location);
                $("#State").val(res.data.state);
                $("#City").val(res.data.city);
                $("#Phone").val(res.data.phone);
                $("#Email").val(res.data.email);
                $("#BreerderName").val(res.data.breerderName);
                $('#check1').prop("Checked", !res.data.isBreerderLicenseUpload);
                $('#check2').prop("Checked", !res.data.isBreerderLicenseUpload);
                $("#ZoologicalNumber").prop("checked", res.data.zoologicalNumber);
                if (res.data.isVaccinated) $('input[name="IsVaccinated"][value="1"]').prop('checked', true); else $('input[name="IsVaccinated"][value="0"]').prop('checked', true);
                $("#Price").val(res.data.price);
                $("#Weigth").val(res.data.weigth);
                $("#Color").val(res.data.color);


                //update code changes 
                if (res.data.isSterilization) $('input[name="IsSterilization"][value="1"]').prop('checked', true); else $('input[name="IsSterilization"][value="0"]').prop('checked', true);
                if (res.data.isCastration) $('input[name="IsCastration"][value="1"]').prop('checked', true); else $('input[name="IsCastration"][value="0"]').prop('checked', true);

                $("#CatteryName").val(res.data.catteryName);

                $('#GalleryFilesAppend').empty();
                $("#PedigreeImageViewAppend").empty();
                $('#FeaturedImageAppend').empty();
                let countryCode = res.data.phoneCode;

                // Set the country code to change the flag and dial code
                $("#Phone").intlTelInput("setCountry", countryCode);
                latitude = res.data.latitude;
                longitude = res.data.longitude;

                if (res.data.pedigreeFilePath) {

                    const filePaths = res.data.pedigreeFilePath.split(",");
                    const promises = filePaths.map((filePath) => {
                        const Path = baseApiUrl + filePath.replace(/\\/g, "/");

                        return new Promise((resolve, reject) => {
                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    const fileName = filePath.split("\\").pop(); // Get the file name

                                    PedigreeFileUpload = new File([blob], fileName, { type: blob.type });

                                    resolve(PedigreeFileUpload);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                    });

                    Promise.all(promises)
                        .then((files) => {

                            $("#Category").val(res.data.categoryId).change();
                            $("#PedigreeFile").val(null).change();
                        })
                        .catch((error) => {
                            console.log("One or more AJAX requests failed:", error);
                        });
                }
                else {

                    $("#Category").val(res.data.categoryId).change();
                }



                if (res.data.featureImagePath) {

                    const filePaths = res.data.featureImagePath.split(",");
                    const promises = filePaths.map((filePath) => {
                        const Path = baseApiUrl + filePath.replace(/\\/g, "/");

                        return new Promise((resolve, reject) => {
                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {

                                    const fileName = filePath.split("\\").pop(); // Get the file name

                                    FeaturedFileUpload = new File([blob], fileName, { type: blob.type });

                                    resolve(FeaturedFileUpload);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                    });

                    Promise.all(promises)
                        .then((files) => {
                            $("#FeaturedFile").val(null).change();
                        })
                        .catch((error) => {
                            console.log("One or more AJAX requests failed:", error);
                        });
                }


                if (res.data.gallaryImagesPath) {



                    const galleryPaths = res.data.gallaryImagesPath.split(",");

                    const galleryPromises = galleryPaths.map((galleryPath) => {

                        const Path = baseApiUrl + galleryPath.replace(/\\/g, "/");

                        return new Promise((resolve, reject) => {
                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    const myFileID = "FID" + (1000 + Math.floor(Math.random() * 9000));
                                    const fileName = galleryPath.split("\\").pop(); // Get the file name
                                    const file = new File([blob], fileName, { type: blob.type });

                                    // Push the file and related info into the global array
                                    GalleryFilesUpload.push({
                                        file: file,
                                        size: file.size,
                                        FID: myFileID,
                                        name: file.name
                                    });

                                    resolve(file);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                    });

                    Promise.all(galleryPromises)
                        .then((files) => {



                            $("#Gallery_Files").val(null).change();


                        })
                        .catch((error) => {
                            console.error("One or more AJAX requests failed:", error);
                        });
                }





            }

            $("#UpdateListingModal").modal("show");
         

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


})


$("#FeaturedFile").on('change', function (e) {

    if (e.target.files[0] != undefined) {
        FeaturedFileUpload = e.target.files[0];
    }

    $('#FeaturedImageAppend').empty();
    $("#FeaturedImageAppend").append(`<div class="col-lg-4 mb-4">
                                         <div class="form_group file-input-one">
                                                <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                                                <img src="${URL.createObjectURL(FeaturedFileUpload)}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">
                                                    <div style="position:absolute;top:10px;right:10px;">
                                                      <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                                                </div>
                                                 <div style="position:absolute;top:10px;right:46px;">
                                                      <button class="btn btn-primary btn-xs btn_downloadfile" data-url="${URL.createObjectURL(FeaturedFileUpload)}" style="border-radius: 25px;width:30px;height:30px;"><i class="fa fa-download"></i></button>
                                                 </div>
                                            </div>
                                        </div>
                                    </div>`);

});


$(document).on('click', '.Featured-remove-button', function () {
    $("#FeaturedImageAppend").children().remove();
    $("#FeaturedFile").val(null);
});


$("#Gallery_Files").on('change', function (e) {

    for (let i = 0; i < e.target.files.length; i++) {

        let myFile = e.target.files[i];
        let myFileID = "FID" + (1000 + Math.random() * 9000).toFixed(0);
        if (GalleryFilesUpload.length < 6) {

            GalleryFilesUpload.push({
                file: myFile,
                size: myFile.size,
                FID: myFileID,
                name: myFile.name
            });
        }
        else {
            Swal.fire({
                title: "Error",
                text: "Only 6 Files Can be Uploaded",
                icon: "error"
            })
        }

    }

    GalleryView();

    e.target.value = null;
});

$(document).on('click', '.gallery-remove-button', function () {
    var fidToRemove = $(this).data('fid');
    for (let i = 0; i < GalleryFilesUpload.length; i++) {
        if (GalleryFilesUpload[i].FID === fidToRemove) {
            GalleryFilesUpload.splice(i, 1);
            break;
        }
    }
    GalleryView();
});


const GalleryView = () => {

    $('#GalleryFilesAppend').empty();

    for (let i = 0; i < GalleryFilesUpload.length; i++) {

        $("#GalleryFilesAppend").append(`
                <div class="col-lg-4 mb-4">
                    <div class="form_group file-input-one">
                        <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                            <img src="${URL.createObjectURL(GalleryFilesUpload[i].file)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                            <div style="position:absolute;top:5px;right:5px;">
                                <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${GalleryFilesUpload[i].FID}" style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                            </div>
                             <div style="position:absolute;top:5px;right:40px;">
                                <button class="btn btn-primary btn-xs btn_downloadfile" data-url="${URL.createObjectURL(GalleryFilesUpload[i].file)}" style="border-radius: 25px;width:30px;height:30px;"><i class="fa fa-download"></i></button>
                            </div>
                        </div>
                    </div>
                </div>
            `);
    }

};


$("#Category").change(function (e) {

    if ($("#Category option:selected").text().toUpperCase() == "PEDIGREE") {

        $("#PEDIGREE").show().empty().append(`
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form_group">
                                    <label class="d-block" style="line-height: 5px;">PEDIGREE</label>
                                    <small class="d-block"><strong>Upload a copy of your original pedigree</strong></small>
                                    <small class="d-block mb-2">Please make sure the text on the pictures is readable and correctly aligned.</small>
                                    <div class="form_group">
                                        <input type="file" id="PedigreeFile" class="form_control bg-white pt-4" style="opacity: 1!important; height: 70px;">
                                        <div class="w-100" id="PedigreeImageViewAppend"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);

        if (PedigreeFileUpload) {
            $("#PedigreeFile").change()
        };
    }
    else {

        $("#PEDIGREE").hide().empty();
    }
});

$(document).on("change", "#PedigreeFile", function (e) {

    debugger

    if (e.target.files[0] != undefined) {
        PedigreeFileUpload = e.target.files[0];
    }
    $("#PedigreeImageViewAppend").empty();
    $("#PedigreeImageViewAppend").append(`
                  <div class="col-lg-4 mb-4 pl-0">
                      <div class="mb-4 img-thumbnail" style="width:50%!important;position:relative;">
                       <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                        <img src="${URL.createObjectURL(PedigreeFileUpload)}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">

                         <div style="position:absolute;top:5px;right:5px;">
                          <button class="btn btn-primary btn-xs btn_downloadfile" data-url="${URL.createObjectURL(PedigreeFileUpload)}" style="border-radius: 25px;width:30px;height:30px;"><i class="fa fa-download"></i></button>
                        </div>
                      </div>

                 </div>
                 </div>`);
})


$("#Btn_Update_Listing").click(function () {

    debugger

    let formData = new FormData();

    for (let i = 0; i < GalleryFilesUpload.length; i++) {

        formData.append("GalleryImageFiles", GalleryFilesUpload[i].file);
    }


    debugger

    if ($('body').find('#PedigreeFile').length > 0) {

        if ($("#PedigreeFile")[0].files[0] != undefined) {

            formData.append("PedigreeFile", $("#PedigreeFile")[0].files[0]);
        }
        else {
            formData.append("PedigreeFile", PedigreeFileUpload);
        }
    }

    formData.append("FeatureImageFile", FeaturedFileUpload);
    formData.append("VideoFile", []);
    formData.append("CategoryId", $("#Category").val());
    formData.append("Id", $("#HDID").val());
    formData.append("Title", $("#Title").val());
    formData.append("Location", $("#Location").val());
    formData.append("State", $("#State").val());
    formData.append("City", $("#City").val());
    formData.append("PackageId", $("#PackageId").val());
    formData.append("Gender", $("#Gender").val());
    formData.append("Phone", $("#Phone").val());
    formData.append("Email", $("#Email").val());
    formData.append("BreerderName", $("#BreerderName").val());
    formData.append("TypeOfCat", $("#TypeOfCat").val());
    formData.append("Age", $("#Age").val());
    formData.append("IsBreerderLicenseUpload", $('input[type=radio][name=IsBreerderLicenseUpload]:checked').val());
    formData.append("ZoologicalNumber", $("#ZoologicalNumber").val());
    formData.append("Description", $("#Description").val());
    formData.append("Price", $('#Price').val());

    //update code changes 
    formData.append("IsCastration", $('input[name="IsCastration"]:checked').val() == "1" ? true : false);
    formData.append("IsSterilization", $('input[name="IsSterilization"]:checked').val() == "1" ? true : false);
    formData.append("CatteryName", $('#CatteryName').val());
    // Get the selected country data
    let selectedCountryData = $("#Phone").intlTelInput("getSelectedCountryData");

    // Extract the ISO2 country code
    let countryCode = selectedCountryData.iso2;
    debugger;
    formData.append('PhoneCode', countryCode);
    formData.append('latitude', latitude);
    formData.append('longitude', longitude);
    FilePostRequest('/Dashboard/UpdateListing', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $('#FeaturedImageAppend').empty();
                $('#GalleryFilesAppend').empty();
                $("#PedigreeImageViewAppend").empty();
                GalleryFilesUpload = [];
                FeaturedFileUpload = null;
                PedigreeFileUpload = null;

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                $("#UpdateListingModal").modal("hide");

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

})


$(document).on('click', '.btn_downloadfile', function () {
    const url = $(this).data('url');
    const link = document.createElement('a');
    link.href = url;
    link.download = 'image.png';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
});