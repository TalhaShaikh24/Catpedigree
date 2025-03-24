var filesToUpload = [];

let baseApiUrl = "";
let latitude = "";
let longitude = "";
$(document).ready(function () {
    $("html, body").animate({ scrollTop: 0 }, "slow");

    baseApiUrl = $("#baseApiUrl").val();

    $("#listingForm").validate({
        errorClass: "error",
        validClass: "valid",
        errorElement: "span",
        highlight: function (element) {
            $(element).addClass('error-border');
        },
        unhighlight: function (element) {
            $(element).removeClass('error-border');
        },
        rules: {
            Location: { required: true },
            Country: { required: true },
            State: { required: true },
            City: { required: true }
        },
        invalidHandler: function (event, validator) {
            // Scroll to #step-4 if the form is invalid
            $('html, body').animate({
                scrollTop: $('#step-4').offset().top
            }, 500);
        }
    });



    GetAllDropdowns();
    $('#Phone').intlTelInput({
        initialCountry: 'us',
        preferredCountries: ['us', 'gb', 'br', 'ru', 'cn', 'es', 'it'],
        autoPlaceholder: 'aggressive',
        separateDialCode: true,
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/12.1.6/js/utils.js"
    });


    initAutocomplete();
    onPlaceChanged();
})





function initAutocomplete() {
    const input = document.getElementById('Location');
    debugger;
    autocomplete = new google.maps.places.Autocomplete(input);

    // Set up the dropdown element
    dropdown = document.getElementById('places-dropdown');

    // Listen for place selection
    autocomplete.addListener('place_changed', onPlaceChanged);
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
    let streetAddress = "";
    let zipCode = "";

    // Extracting latitude and longitude
     latitude = place.geometry.location.lat();
     longitude = place.geometry.location.lng();

    for (const component of addressComponents) {
        const types = component.types;
        if (types.includes("locality") || types.includes("sublocality")) {
            city = component.long_name;
        }
        if (types.includes("administrative_area_level_1")) {
            state = component.short_name;
        }
        if (types.includes("country")) {
            country = component.long_name;
        }
        // Check for street number and route
        if (types.includes("street_number")) {
            streetAddress += component.long_name + " "; // Add street number
        }
        if (types.includes("route")) {
            streetAddress += component.long_name; // Add street name
        }
        if (types.includes("postal_code")) {
            zipCode = component.long_name; // Add zip code
        }
    }

    // Trim any extra whitespace from the street address
    streetAddress = streetAddress.trim();

    // Set the street address to the Location input
    $("#Location").val(streetAddress);

    // Assign values to respective inputs
    $("#State").val(state);
    $("#City").val(city);
    $("#Country").val(country);
    $("#ZipCode").val(zipCode);
}




$("#FeaturedFile").on('change', function (e) {


    let FeaturedFile = e.target.files[0];

    $('#FeaturedImageAppend').empty();
    $("#FeaturedImageAppend").append(`
                                  <div class="col-lg-4 mb-4">
                                     <div class="form_group file-input-one">

                                            <div class="img-thumbnail" style="width:50%!important">
                                            <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                            <img src="${URL.createObjectURL(FeaturedFile)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                                <div style="position:absolute;top:0px;right:0px;">
                                                  <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;">X</button>
                                                </div>
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
        if (filesToUpload.length < 6) {


            filesToUpload.push({
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
    for (let i = 0; i < filesToUpload.length; i++) {
        if (filesToUpload[i].FID === fidToRemove) {
            filesToUpload.splice(i, 1);
            break;
        }
    }
    GalleryView();
});

const GalleryView = () => {

    $('#GalleryFilesAppend').empty();
    for (let i = 0; i < filesToUpload.length; i++) {
        $("#GalleryFilesAppend").append(`
                                 <div class="col-lg-4 mb-4">
                             <div class="form_group file-input-one">
                                    <div class="img-thumbnail" style="width:50%!important">
                                    <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                             <img src="${URL.createObjectURL(filesToUpload[i].file)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                        <div style="position:absolute;top:0px;right:0px;">
                                              <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${filesToUpload[i].FID}" style="border-radius: 25px;">X</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`);
    }

};


$("#PackageId").change(function () {

    var pkgId = Number($(this).val());
    GetAllCategoriesByPackageId(pkgId);

});
$("#Category").change(function (e) {
    if ($("#Category option:selected").text().toUpperCase() == "PEDIGREE") {
        $("#PEDIGREE").show().append(`
                <div class="row">
                    <div class="col-lg-12">
                    <h4 class="title">PEDIGREE</h4>
                        <div class="form_group">
                            <label class="d-none" style="line-height: 5px;">PEDIGREE</label>
                            <small class="d-block"><strong>Upload a copy of your original pedigree</strong></small>
                            <small class="d-block mb-2">Please make sure the text on the pictures is readable and correctly aligned.</small>
                            <div class="form_group">
                                <input type="file" id="PedigreeFile" required class="form_control bg-white pt-4" style="opacity: 1!important; height: 70px;">
                                <div class="w-100" id="PedigreeImageViewAppend"></div>
                            </div>
                        </div>
                    </div>
                </div>
            `);
    } else {
        $("#PEDIGREE").hide().empty();
    }
});


$(document).on("change", "#PedigreeFile", function (e) {
    $("#PedigreeImageViewAppend").empty();
    $("#PedigreeImageViewAppend").append(`
                  <div class="col-lg-4 mb-4 pl-0">
                      <div class="mb-4 img-thumbnail" style="width:50%!important;">
                       <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                        <img src="${URL.createObjectURL(e.target.files[0])}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">
                      </div>
                 </div>
                 </div>`);
})

function GetAllDropdowns() {

    postRequest('/Listing/GetAllDropdowns', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#PackageId").empty();
                $("#Category").empty();
                $("#TypeOfCat").empty();
                $("#PromotionPackageId").empty();


                $("#PackageId").append(`<option value="-1" disabled selected>Select Packages</option>`);
                $("#PromotionPackageId").append(`<option value="-1" disabled selected>Select Packages</option>`);

                $("#TypeOfCat").append(`<option value="-1" disabled selected>Select Type Of Cat</option>`);

                $.each(res.data.item3, function (i, v) {
                    debugger;
                    $("#PackageId").append(`<option value="${v.packageID}">${v.name}-(${v.remainingListings == 999 ? "Unlimited" : v.remainingListings})</option>`);
                });

                $.each(res.data.item2, function (i, v) {
                    $("#TypeOfCat").append(`<option value="${v.id}">${v.catType}</option>`);
                });

                //$.each(res.data.item1, function (i, v) {
                //    $("#Category").append(`<option value="${v.id}">${v.categoryName}</option>`);
                //});
                debugger;
                if (res.data.item4.length > 0) {
                    $.each(res.data.item4, function (i, v) {
                        $("#PromotionPackageId").append(`<option value="${v.promotionPackagesID}">${v.name}  - (${v.packageCount})</option>`);
                    });
                }
                else {
                    $("#PromotionPackageId").append(`<option value="-1" disabled>You have no Promotion Package</option>`);
                }



                $('select').niceSelect('destroy');
                $('select').niceSelect();


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

$(document).on('change', '#VideoFile', function (e) {
    var file = e.target.files[0];
    var maxSizeMB = 30 * 1024 * 1024; // 30 MB in bytes
    var maxDuration = 30; // Max duration in seconds

   

    if (file) {
        // Check file size
        var fileSizeMB = file.size / (1024 * 1024); // Convert size to MB
        if (fileSizeMB > maxSizeMB) {
            Swal.fire({
                title: "Warning",
                text: "The file size is too large. Maximum allowed size is " + maxSizeMB + " MB.",
                icon: "warning",
                showCancelButton: false,
                confirmButtonColor: "#3085d6",
                allowOutsideClick: false,
                allowEscapeKey: true,
            });
            e.target.value = null;
            return;
        }

        // Check if the file type can be played
        var video = document.createElement('video');
        var canPlay = video.canPlayType(file.type);
        if (canPlay === '') {
            Swal.fire({
                title: "Error",
                text: "Cannot play this video type.",
                icon: "error"
            });
            e.target.value = null;
            return;
        }

        var url = URL.createObjectURL(file);
        video.preload = 'metadata';
        video.src = url;

        video.onloadedmetadata = function () {
            URL.revokeObjectURL(video.src);
            var duration = video.duration;
            debugger;
            if (duration > maxDuration) {
                Swal.fire({
                    title: "Warning",
                    text: "The video is too long. Maximum allowed duration is " + maxDuration + " seconds. And should not be more than 30mbs",
                    icon: "warning",
                    showCancelButton: false,
                    confirmButtonColor: "#3085d6",
                    allowOutsideClick: false,
                    allowEscapeKey: true,
                });
                e.target.value = null;
            } else {
                // Video is available
                $Step3nextButton = $('#step-3 button[onclick="nextStep()"]');
                $Step3nextButton.prop('disabled', true);// Disable button
                // Proceed with your existing postRequest call if the video duration is valid
                postRequest('/VideoPackages/VideoAvailablity', null, function (res) {
                    if (res.status == 200) {
                        if (res.data != null) {
                            if (res.data) {
                                // Video is available
                                $Step3nextButton.prop('disabled', false); // Enable button
                                // Video is available
                            } else {
                                e.target.value = null;

                                Swal.fire({
                                    title: "Warning!",
                                    text: res.responseMsg,
                                    icon: "warning",
                                    showCancelButton: false,
                                    confirmButtonColor: "#3085d6",
                                    allowOutsideClick: false,
                                    allowEscapeKey: true,
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        $Step3nextButton.prop('disabled', false); // Disable button
                                        redirectToVideoPackage();
                                    }
                                    
                                });
                            }
                        }
                    } else if ([304, 305, 401, 403, 320, 500, 600].includes(res.status)) {
                        Swal.fire({
                            title: res.status == 403 ? "Error" : "Warning",
                            text: res.responseMsg,
                            icon: res.status == 403 ? "error" : "warning"
                        });
                    }
                });
            }
        };

        video.onerror = function () {
            Swal.fire({
                title: "Error",
                text: "There was an error loading the video.",
                icon: "error"
            });
            e.target.value = null;
        };
    }
});

//$(document).on('change', '#VideoFile', function (e) {
//    postRequest('/VideoPackages/VideoAvailablity', null, function (res) {

//        if (res.status == 200) {

//            if (res.data != null) {

//                if (res.data) {


//                }
//                else {

//                    e.target.value = null;

//                    Swal.fire({
//                        title: "Warning!",
//                        text: res.responseMsg,
//                        icon: "warning",
//                        showCancelButton: false,
//                        confirmButtonColor: "#3085d6",
//                        allowOutsideClick: false,  // Disable outside click
//                        allowEscapeKey: true,
//                    }).then((result) => {
//                        console.log(result);  // Debugging: log the result to the console
//                        if (result.isConfirmed) {
//                            debugger;  // Debugger statement to pause execution for inspection
//                            window.open("/VideoPackages/VideoPlans", "_blank");

//                        }
//                    });

//                }

//            }
//        }
//        if (res.status == 304) {

//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: "error"
//            })
//        }
//        if (res.status == 305) {

//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: "error"
//            })
//        }
//        if (res.status == 401) {

//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: "error"
//            })
//        }
//        if (res.status == 403) {

//            Swal.fire(res.responseMsg, {
//                icon: "error",
//                title: "Error"
//            });
//        }
//        if (res.status == 320) {

//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: "error"
//            })
//        }
//        if (res.status == 500) {

//            Swal.fire({
//                title: "Error",
//                text: res.responseMsg,
//                icon: "error"
//            })
//        }
//        if (res.status == 600) {

//            Swal.fire({
//                title: "Warning",
//                text: res.responseMsg,
//                icon: "warning"
//            })

//        }
//    });

//});


$('#showpromotionpackage').change(function () {
    // this will contain a reference to the checkbox   
    if (this.checked) {
        // the checkbox is now checked
        $("#PromotionPackagegroup").show();

    } else {
        // the checkbox is now no longer checked

        $("#PromotionPackagegroup").hide();
    }
});

function redirectToVideoPackage() {
    window.open("/Home/Enhancement", "_blank");
}


function GetAllCategoriesByPackageId(pkgId) {
    postRequest('/Listing/GetAllCategoriesByPackageId?pkgId=' + pkgId, null, function (res) {

        if (res.status == 200 && res.data != null) {
            $("#Category").empty();
            $("#Category").append(`<option value="-1" disabled selected>Select Category</option>`);
            $.each(res.data, function (i, v) {
                $("#Category").append(`<option value="${v.id}">${v.categoryName}</option>`);
            });

            $("#Category").niceSelect('update');


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

function redirectToHome() {
    window.location.href = window.location.origin + "/Dashboard";
}

$("#Btn_Post_Listing").click(function () {

    if ($("#listingForm").valid()) {

    


    $Btn_Post_Listing = $('#Btn_Post_Listing');
    $Btn_Post_Listing.prop('disabled', true);

    debugger
    $(".preloader").show();
    let formData = new FormData();

    for (let i = 0; i < filesToUpload.length; i++) {

        formData.append("GalleryImageFiles", filesToUpload[i].file);
    }

    if ($('body').find('#PedigreeFile').length > 0) {

        formData.append("PedigreeFile", $("#PedigreeFile")[0].files[0]);
    }



    formData.append("FeatureImageFile", $("#FeaturedFile")[0].files[0]);
    formData.append("VideoFile", $("#VideoFile")[0].files[0]);
    formData.append("CategoryId", Number($("#Category").val()));
    formData.append("Title", $("#Title").val());
    formData.append("Country", $("#Country").val());
    formData.append("Location", $("#Location").val());
    formData.append("State", $("#State").val());
    formData.append("City", $("#City").val());
    formData.append("ZipCode", $("#ZipCode").val());
    formData.append("PackageId", Number($("#PackageId").val()));
    formData.append("Gender", $("#Gender").val());
    formData.append("Phone", $("#Phone").val());
    formData.append("Email", $("#Email").val());
    formData.append("BreerderName", $("#BreerderName").val());
    formData.append("TypeOfCat", Number($("#TypeOfCat").val()));
    formData.append("Age", $("#Age").val());
    formData.append("IsBreerderLicenseUpload", $('input[type=radio][name=IsBreerderLicenseUpload]:checked').val() == 'true' ? true : false);
    formData.append("ZoologicalNumber", $('input[type=checkbox][id=ZoologicalNumber]:checked').val() == 'true' ? true : false);

    // Handle new lines in Description
    let description = $("#Description").val().replace(/\n/g, "<br>"); // Convert new lines to <br>
    formData.append("Description", description);

    formData.append("Color", $("#Color").val());
    formData.append("IsVaccinated", 0);
    formData.append("IsCastration", $('input[name="IsCastration"]:checked').val() == "1" ? true : false);
    formData.append("IsSterilization", $('input[name="IsSterilization"]:checked').val() == "1" ? true : false);
    formData.append("IsPriceRequest", $('input[name="IsPriceRequest"]:checked').val() == "1" ? true : false);
    formData.append("Price", $('#Price').val());
    formData.append("PromotionPackageId", Number($('#PromotionPackageId').val()));
    formData.append("CatteryName", $('#CatteryName').val());

    // Get the selected country data
    let selectedCountryData = $("#Phone").intlTelInput("getSelectedCountryData");

    // Extract the ISO2 country code
    let countryCode = selectedCountryData.iso2;
    let countryDialCode = "+" + selectedCountryData.dialCode;

    debugger;

    formData.append('PhoneCode', countryCode);
    formData.append('CountryDialCode', countryDialCode);
    //formData.append('latitude', latitude);
    //formData.append('longitude', longitude);

    //Advertisement 

    formData.append('FamilyTreeMother', $("#FTMother").val());
    formData.append('FamilyTreeFather', $("#FTFather").val());
    formData.append('MotherTested', $("#MotherTested").val());


    formData.append('FatherTested', $("#FatherTested").val());

    formData.append('DateofBirth', $("#DataOFBirth").val());
    formData.append('PartOfAssociation', $("#PartOfAssociation").val());
    formData.append('Website', $("#Website").val());

    FilePostRequest('/Listing/AddListting', formData, function (res) {

        if (res.status == 200) {
            $(".preloader").hide();
            if (res.data != null) {

                Swal.fire({
                    title: "Congrats",
                    text: res.responseMsg,
                    icon: "success"
                })
                //    .then(() => {
                //    redirectToHome();
                //});

                //$(document).find("input").val(null);
                //$(document).find("select").val(null).niceSelect('update');
                //GetAllDropdowns();


                window.location.href ="/Dashboard/MyListing"

            }
            $Btn_Post_Listing.prop('disabled', false);
        }
        if (res.status == 304) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })

        }
        if (res.status == 305) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {
            $(".preloader").hide();
            $Btn_Post_Listing.prop('disabled', false);
            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });


    }

})



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

function FilePostRequest(url, requestData, handledata) {
    $.ajax({
        type: 'POST',
        processData: false,
        contentType: false,
        url: url,
        data: requestData,
        success: function (data, textStatus, xhr) {

            handledata(JSON.parse(data));
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




function showStep(step) {
    document.querySelectorAll('.step-form').forEach((el) => el.classList.remove('active'));
    document.getElementById(`step-${step}`).classList.add('active');
}

function nextStep() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    if (validateStep(currentStep)) {
        if (currentStep < totalSteps) {
            currentStep++;
            showStep(currentStep);
        }
    }
}

function prevStep() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    if (currentStep > 1) {
        currentStep--;
        showStep(currentStep);
    }
}

function validateStep(step) {
    let isValid = true;
    const stepForm = document.getElementById(`step-${step}`);
    const requiredFields = stepForm.querySelectorAll('[required]');

    clearValidation(requiredFields);

    requiredFields.forEach(field => {
        if (field.type === 'checkbox' && !field.checked) {
            isValid = false;
            field.classList.add('invalid');
        } else if (field.type === 'radio') {
            const radioGroup = stepForm.querySelectorAll(`input[name="${field.name}"]:checked`);
            if (radioGroup.length === 0) {
                isValid = false;
                field.classList.add('invalid');
            }
        } else if (field.type === 'file' && field.files.length === 0) {
            isValid = false;
            field.classList.add('invalid');
        } else if (field.tagName === 'SELECT' && (field.value === '' || field.value === '-1')) {
            isValid = false;
            field.classList.add('invalid');

            // Find the closest div with class 'nice-select' and apply border
            const niceSelectDiv = field.nextElementSibling; // Assuming the nice-select div is the next sibling
            if (niceSelectDiv && niceSelectDiv.classList.contains('nice-select')) {
                niceSelectDiv.style.border = '1px solid red'; // Example border style
            }
        }
        else if (field.value.trim() === '') {
            isValid = false;
            field.classList.add('invalid');
        } else if (field.id === 'Email' && !validateEmail(field.value)) {
            isValid = false;
            field.classList.add('invalid');
        }
    });

    return isValid;
}

function validateEmail(email) {
    const re = /^[^\s@@]+@@[^\s@@]+\.[^\s@@]+$/;
    return re.test(email.toLowerCase());
}

function clearValidation(elements) {
    elements.forEach((el) => el.classList.remove('invalid'));
}