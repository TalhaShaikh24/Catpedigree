let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllDropdowns();

    $("#PromotionPackage").change(function () {
        var selectedValue = $(this).find("option:selected").text().toLowerCase();

        if (selectedValue.includes("video")) {
            $("#videoUploadSection").show();
        } else {
            $("#videoUploadSection").hide();
        }
    });

  
})

function GetAllDropdowns() {

    postRequest('/Dashboard/GetListing_ProdictionPackages', null, function (res) {

        if (res.status == 200) {
            debugger;
            if (res.data != null) {

                $("#PromotionPackage").empty();
              

                $("#Listing").empty();
              
            
                if (res.data.item1.length > 0) {
                    $.each(res.data.item1, function (i, v) {
                        $("#PromotionPackage").append(`<option value="${v.promotionPackagesID}">${v.name}  (${v.packageCount})</option>`);
                    });
                }
                else {
                    $("#PromotionPackage").append(`<option value="-1" disabled>You don't have any Promotion Package</option>`);
                }

                $.each(res.data.item2, function (i, v) {
                    $("#Listing").append(`<option value="${v.id}">${v.title}</option>`);
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




$("#save").click(function () {
    debugger

   
    var data = {
        PromotionPackageId: Number($("#PromotionPackage").val()),
        Id: Number($("#Listing").val())
    };

    // Check if the selected package requires video upload
    if ($("#PromotionPackage").find("option:selected").text().toLowerCase().includes("video")) {
        var videoFile = $("#videoUpload")[0].files[0];
        if (!videoFile) {
            Swal.fire({
                title: "Oops",
                text: "Video upload is required for this promotion package.",
                icon: "warning"
            });
            return;
        }

        // Check file size (30 MB = 30 * 1024 * 1024 bytes)
        if (videoFile.size > 30 * 1024 * 1024) {
            Swal.fire({
                title: "Oops",
                text: "Video file must be less than 30 MB.",
                icon: "warning"
            });
            return;
        }

        // Create a temporary video element to check duration
        var videoElement = document.createElement("video");
        videoElement.src = URL.createObjectURL(videoFile);
        videoElement.onloadedmetadata = function () {
            if (videoElement.duration > 30) {
                Swal.fire({
                    title: "Oops",
                    text: "Video duration must be 30 seconds or less.",
                    icon: "warning"
                });
                return;
            }

            // If all validations pass, proceed with the API call
            var formData = new FormData();
            formData.append("PromotionPackageId", data.PromotionPackageId);
            formData.append("Id", data.Id);
            formData.append("videoFile", videoFile);

            FilePostRequest('/Dashboard/Assgin_PromotionPackage_to_List', formData, function (res) {

                if (res.status == 200) {

                    if (res.data != null) {

                        Swal.fire({
                            title: "Success",
                            text: res.responseMsg,
                            icon: "success"
                        });
                        GetAllDropdowns();
                        var urlParams = new URLSearchParams(window.location.search);


                    }

                    // Clear all input fields
                    $("input[type='text'], input[type='email'], input[type='number'], textarea").val('');
                    $("select").prop('selectedIndex', 0); // Reset select elements to the first option
                    $("#videoUploadSection").hide(); // Optionally hide the video upload section
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
                        title: "Oops",
                        text: res.responseMsg,
                        icon: "warning"
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
        };

        // Load video metadata
        videoElement.load();
    }
    else {
        debugger
        // For packages not requiring video upload
        var formData = new FormData();
        formData.append("PromotionPackageId", data.PromotionPackageId);
        formData.append("Id", data.Id);
        FilePostRequest('/Dashboard/Assgin_PromotionPackage_to_List', formData, function (res) {

            if (res.status == 200) {

                if (res.data != null) {

                    Swal.fire({
                        title: "Success",
                        text: res.responseMsg,
                        icon: "success"
                    });
                    GetAllDropdowns();
                    var urlParams = new URLSearchParams(window.location.search);


                }

                // Clear all input fields
                $("input[type='text'], input[type='email'], input[type='number'], textarea").val('');
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
                    title: "Oops",
                    text: res.responseMsg,
                    icon: "warning"
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
