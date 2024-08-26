let baseApiUrl = "";
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
                "order": [[0, "desc"]]
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


$(document).on("click", "#btn_Listing_Edit", function (e) {
  

    postRequest('/Dashboard/GetListingDetailById/' + Number(e.currentTarget.dataset.id), null, function (res) {

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

                if (res.data.featureImagePath != null) {
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


                if (res.data.videoPath != null) {
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


                if (res.data.gallaryImagesPath != null) {
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


function UpdateListingStatus(id, status, reason="") {

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
