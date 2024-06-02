var filesToUpload = [];
$(document).ready(function () {

    GetAllDropdowns();
    $('#Phone').intlTelInput({
        initialCountry: 'br',
        preferredCountries: ['us', 'gb', 'br', 'ru', 'cn', 'es', 'it'],
        autoPlaceholder: 'aggressive',
        separateDialCode: true,
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/12.1.6/js/utils.js"
    });
})

$("#FeaturedFile").on('change', function (e) {

    debugger

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

        filesToUpload.push({
            file: myFile,
            size: myFile.size,
            FID: myFileID,
            name: myFile.name
        });
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


$("#Btn_Post_Listing").click(function () {

    debugger

    let formData = new FormData();

    for (let i = 0; i < filesToUpload.length; i++) {

        formData.append("GalleryImageFiles", filesToUpload[i].file);
    }

    if ($('body').find('#PedigreeFile').length > 0) {

        formData.append("PedigreeFile", $("#PedigreeFile")[0].files[0]);
    }
    formData.append("FeatureImageFile", $("#FeaturedFile")[0].files[0]);
    formData.append("VideoFile", $("#VideoFile")[0].files[0]);
    formData.append("CategoryId", $("#Category").val());
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
    formData.append("Weigth", $("#Weigth").val());
    formData.append("Color", $("#Color").val());
    formData.append("IsVaccinated",$('input[name="IsVaccinated"]:checked').val());
    formData.append("Price",$('#Price').val());
    FilePostRequest('/Listing/AddListting', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                $(document).find("input").val(null);
                $(document).find("select").val(null).niceSelect('update');

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


$("#Category").change(function (e) {
    if ($("#Category option:selected").text().toUpperCase() == "PEDIGREE") {
        $("#PEDIGREE").show().append(`
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



                $("#PackageId").append(`<option value="-1" disabled selected>Select Packages</option>`);
                $("#Category").append(`<option value="-1" disabled selected>Select Category</option>`);
                $("#TypeOfCat").append(`<option value="-1" disabled selected>Select Type Of Cat</option>`);

                $.each(res.data.item3, function (i, v) {
                    $("#PackageId").append(`<option value="${v.packageID}">${v.name}-(${v.remainingListings})</option>`);
                });

                $.each(res.data.item2, function (i, v) {
                    $("#TypeOfCat").append(`<option value="${v.id}">${v.catType}</option>`);
                });

                $.each(res.data.item1, function (i, v) {
                    $("#Category").append(`<option value="${v.id}">${v.categoryName}</option>`);
                });


                $('select').niceSelect('update');


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
    postRequest('/VideoPackages/VideoAvailablity', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                if (res.data)
                {


                }
                else
                {

                    e.target.value = null;

                    Swal.fire({
                        title: "Warning!",
                        text: res.responseMsg,
                        icon: "warning",
                        showCancelButton: false,
                        confirmButtonColor: "#3085d6",
                        allowOutsideClick: false,  // Disable outside click
                        allowEscapeKey: true,
                    }).then((result) => {
                        console.log(result);  // Debugging: log the result to the console
                        if (result.isConfirmed) {
                            debugger;  // Debugger statement to pause execution for inspection
                            window.location.href = "/VideoPackages/VideoPlans";
                        }
                    });

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