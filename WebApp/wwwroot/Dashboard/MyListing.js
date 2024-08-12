var filesToUpload = [];
var FeaturedFileUpload = [];
var VideoFileUpload = [];
var PedigreeFileUpload = [];
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    GetAllDropdowns();
    $('#Phone').intlTelInput({
        initialCountry: 'br',
        preferredCountries: ['us', 'gb', 'br', 'ru', 'cn', 'es', 'it'],
        autoPlaceholder: 'aggressive',
        separateDialCode: true,
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/12.1.6/js/utils.js"
    });
});


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




                GetAllMyListings()

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


function GetAllMyListings() {

    postRequest('/Dashboard/GetAllMyListings', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendMyListings").empty();

                if ($.fn.DataTable.isDataTable('#TableMyListings')) {
                    $('#TableMyListings').DataTable().destroy();
                }
                debugger
         

                $.each(res.data, function (i, v) {

                    debugger
                    $("#AppendMyListings").append(`
                                        <tr>
                                           <td>${v.id}</td>
                                           <td>${v.title}</td>
                                           <td>${v.location}</td>
                                           <td>${v.state}</td>
                                           <td>${v.city}</td>
                                           <td>${v.isBreerderLicenseUpload}</td>
                                           <td>${v.phone}</td>
                                           <td>${v.email}</td>
                                           <td>${v.breerderName}</td>
                                           <td>${v.typeOfCat}</td>
                                           <td>${v.weigth}</td>
                                           <td>${v.color}</td>
                                           <td>${v.price}</td>
                                           <td>${v.isVaccinated}</td>
                                           <td>${v.zoologicalNumber}</td>
                                           <td>${v.gender}</td>
                                           <td>${v.age}</td>
                                           <td>${v.categoryId}</td>
                                           <td>${v.packageId}</td>
                                            <td>${v.promotionName}</td>
                                            <td>${v.catteryName}</td>
                                           <td>${v.isActive}</td>
                                           <td>${v.createdBy}</td>
                                           <td>${v.createdOn}</td>
                                           <td><button id="btn_Listing_Edit" type="button" class="btn btn-xs btn-info mr-2" data-id="${v.id}"><i class="fa fa-edit"></i></button>|<button id="btn_Listing_Delete" type="button" class="btn btn-xs btn-danger ml-2" data-id="${v.id}"><i class="fa fa-trash"></i></button></td>
                                        </tr>`);

                });

              
                $('#TableMyListings').DataTable({
                    "order": [[0, "desc"]]
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



$(document).on("click", "#btn_Listing_Delete", function (e) {

    postRequest('/Dashboard/DeleteListingById?Id=' + Number(e.currentTarget.dataset.id), null, function (res) {

        if (res.status == 200) {

            Swal.fire({
                title: "Success",
                text: res.responseMsg,
                icon: "success"
            })
                GetAllMyListings();
              
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
$(document).on("click", "#btn_Listing_Edit", function (e) {

  postRequest('/Dashboard/GetListingDetailById/'+ Number(e.currentTarget.dataset.id), null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {


                debugger

                var promises = [];
                filesToUpload = [];
                FeaturedFileUpload = [];
                VideoFileUpload = [];
                $("#HDID").val(res.data.id);
                $("#PackageId").val(res.data.packageId);
                $("#Category").val(res.data.categoryId).change();
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
                debugger

                if (res.data.pedigreeFilePath != null) {
                    $.each(res.data.pedigreeFilePath.split(","), function (i, v) {

                        var Path = baseApiUrl + v.replace(/\\/g, "/");

                        var promise = new Promise(function (resolve, reject) {

                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    debugger

                                    PedigreeFileUpload = new File([blob], v.split("\\")[1], { type: blob.type });


                                    resolve();
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                        promises.push(promise);
                    });
                }

                if (res.data.featureImagePath != null)
                {
                    $.each(res.data.featureImagePath.split(","), function (i, v) {

                        debugger

                        var Path = baseApiUrl + v.replace(/\\/g, "/");

                        var promise = new Promise(function (resolve, reject) {

                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    debugger

                                    FeaturedFileUpload = new File([blob], v.split("\\")[1], { type: blob.type });


                                    resolve();
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                        promises.push(promise);
                    });
                }


                if (res.data.videoPath != null)
                {
                    $.each(res.data.videoPath.split(","), function (i, v) {

                        debugger

                        var Path = baseApiUrl + v.replace(/\\/g, "/");

                        var promise = new Promise(function (resolve, reject) {

                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    debugger

                                    VideoFileUpload = new File([blob], v.split("\\")[1], { type: blob.type });


                                    resolve();
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                        promises.push(promise);
                    });

                }


                if (res.data.gallaryImagesPath != null)
                {
                    $.each(res.data.gallaryImagesPath.split(","), function (i, v) {

                        var Path = baseApiUrl + v.replace(/\\/g, "/");

                        var promise = new Promise(function (resolve, reject) {

                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    debugger

                                    var myFileID = "FID" + (1000 + Math.floor(Math.random() * 9000));

                                    var file = new File([blob], v.split("\\")[1], { type: blob.type });

                                    debugger

                                    filesToUpload.push({
                                        file: file,
                                        size: file.size,
                                        FID: myFileID,
                                        name: file.name
                                    });
                                    resolve();
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                        promises.push(promise);
                    });

                }
                Promise.all(promises).then(function () {

                    $("#FeaturedFile").change();
                    $("#PedigreeFile").change();
                    GalleryView();
                }).catch(function (error) {
                    console.error("One or more AJAX requests failed:", error);
                });

                $("#UpdateListingModal").modal("show");

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


$("#FeaturedFile").on('change', function (e) {

    if (e.target.files[0] != undefined) {
        FeaturedFileUpload = [];
        FeaturedFileUpload = e.target.files[0];
    }

    $('#FeaturedImageAppend').empty();
    $("#FeaturedImageAppend").append(`
                                         <div class="col-lg-4 mb-4">
                                         <div class="form_group file-input-one">
                                                <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                                                <img src="${URL.createObjectURL(FeaturedFileUpload)}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">
                                                    <div style="position:absolute;top:5px;right:5px;">
                                                      <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                                                  
                                                </div>
                                            </div>
                                        </div>
                                    </div>`);

});


$("#VideoFile").on('change', function (e) {

    if (e.target.files[0] != undefined) {
        VideoFileUpload = [];
        VideoFileUpload = e.target.files[0];
    }
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
                                       
                                        <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                                 <img src="${URL.createObjectURL(filesToUpload[i].file)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                            <div style="position:absolute;top:5px;right:5px;">
                                             <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${filesToUpload[i].FID}"style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                                           
                                        </div>
                                    </div>
                                </div>
                            </div>`);
    }

};


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
    }
    else {

        $("#PEDIGREE").hide().empty();
    }
});



$(document).on("change", "#PedigreeFile", function (e) {

    if (e.target.files[0] != undefined) {
        PedigreeFileUpload = [];
        PedigreeFileUpload = e.target.files[0];
    }
    $("#PedigreeImageViewAppend").empty();
    $("#PedigreeImageViewAppend").append(`
                  <div class="col-lg-4 mb-4 pl-0">
                      <div class="mb-4 img-thumbnail" style="width:50%!important;">
                       <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                        <img src="${URL.createObjectURL(PedigreeFileUpload)}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">
                      </div>
                 </div>
                 </div>`);
})



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


$("#Btn_Update_Listing").click(function () {

    debugger

    let formData = new FormData();

    for (let i = 0; i < filesToUpload.length; i++) {

        formData.append("GalleryImageFiles", filesToUpload[i].file);
    }

    if ($('body').find('#PedigreeFile').length > 0) {

        formData.append("PedigreeFile", $("#PedigreeFile")[0].files[0]);
    }

    formData.append("FeatureImageFile", FeaturedFileUpload);
    formData.append("VideoFile", VideoFileUpload);
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
  //  formData.append("Weigth", $("#Weigth").val());
  //  formData.append("Color", $("#Color").val());
  //  formData.append("IsVaccinated", $('input[name="IsVaccinated"]:checked').val());
    formData.append("Price", $('#Price').val());

    //update code changes 
    formData.append("IsCastration", $('input[name="IsCastration"]:checked').val() == "1" ? true : false);
    formData.append("IsSterilization", $('input[name="IsSterilization"]:checked').val() == "1" ? true : false);
    formData.append("CatteryName", $('#CatteryName').val());

    FilePostRequest('/Dashboard/UpdateListing', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $('#FeaturedImageAppend').empty();
                $('#GalleryFilesAppend').empty();
                filesToUpload = [];
                FeaturedFileUpload = [];
                VideoFileUpload = [];

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


