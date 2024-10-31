let baseApiUrl = "";
let currentId = null; // Variable to hold the current advertisement ID
$(document).ready(function () {
    ShowPreloader();
    baseApiUrl = $("#baseApiUrl").val();


    GetAllListings();
});


function GetAllListings() {

    // Check if the DataTable is already initialized
    if ($.fn.DataTable.isDataTable('#TableApprovalListing')) {
        // Destroy the existing DataTable
        $('#TableApprovalListing').DataTable().clear().destroy();
    }

    $('#TableApprovalListing').DataTable({
        ajax: {
            url: '/Dashboard/GetallUserAdvertisementForApprovals',
            type: 'POST',
            dataSrc: function (res) {
                if (res.status === 200) {
                    return res.data || []; // Return the data or an empty array
                } else {
                    handleErrorResponse(res);
                    return []; // Return empty if there's an error
                }
            }
        },
        columns: [
            { data: 'userAdvertisementPackageID' },
            {
                data: 'filePath', render: function (data, type, row) {
                    const mediaUrl = baseApiUrl + data;
                    // Check if the advertisement package name indicates it's a video ad
                    if (row.advertisementPackageName.toLowerCase().trim().includes('video ad')) {
                        // Return a video element for video ads
                        return `<i class="fas fa-play" style="color:#ffd74e; font-size:26px"></i>`;
                    } else {
                        // Return an image element for other ads
                        return `<img src="${mediaUrl}" alt="Advertisement Image" style="max-width: 60px; max-height: 60px;" />`;
                    }
                }
            },
            { data: 'advertisementPackageName' },
            { data: 'advertisementPackageCost' },
            { data: 'advertisementPackageType' },
            { data: 'numberOfAdvertisement' },
            {
                data: 'status', render: function (data) {
                    // Assign badge classes based on status
                    let badgeClass = '';
                    switch (data) {
                        case 'Approve':
                            badgeClass = 'badge bg-success';
                            break;
                        case 'Pending':
                            badgeClass = 'badge bg-warning';
                            break;
                        case 'Reject':
                            badgeClass = 'badge bg-danger';
                            break;
                        default:
                            badgeClass = 'badge bg-info';
                    }
                    return `<span class="${badgeClass}">${data}</span>`;
                }
            },
            { data: 'username' },
            {
                data: 'createdOn', render: function (data) {
                    return moment(data).format("DD-MMM-YYYY");
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return `
                    <div style="display: flex; justify-content: space-evenly; align-items: center;">
                        <button type="button" onclick="UpdateListingStatus(${row.userAdvertisementPackageID}, 'Approve');" title="Approve" class="mx-1 btn btn-success btn-xs p-2"><i class="fa fa-check" aria-hidden="true"></i></button>
                        <button type="button" class="mx-1 btn btn-info btn-xs p-2" title="Reject" onclick="openRejectionModal(${row.userAdvertisementPackageID});"><i class="fa fa-ban" aria-hidden="true"></i></button>
                        <button type="button" class="mx-1 btn btn-warning btn-xs p-2" title="Pending" onclick="UpdateListingStatus(${row.userAdvertisementPackageID}, 'Pending');"><i class="fa fa-clock" aria-hidden="true"></i></button>
                        <button type="button" class="mx-1 btn btn-primary btn-xs p-2" title="View" onclick='openViewDetailsModal(${JSON.stringify(row).replace(/'/g, "\\'")});'><i class="fa fa-eye" aria-hidden="true"></i></button>
                        <button type="button" class="mx-1 btn btn-danger btn-xs p-2" title="Delete" onclick="DeleteAdvertising(${row.userAdvertisementPackageID});"><i class="fa fa-trash" aria-hidden="true"></i></button>
                    </div>`;
                }
            }
        ],
        // Optional: You can customize the DataTable here
        order: [[0, 'desc']],
        paging: true,
        searching: true,
        ordering: true,
        // Add other DataTable options as needed
    });

    function handleErrorResponse(res) {
        HidePreloader();
        Swal.fire({
            title: "Error",
            text: res.responseMsg,
            icon: res.status >= 400 && res.status < 500 ? "error" : "warning"
        });
    }

    // Optionally, you might want to show a preloader while fetching data
    $(document).on('processing.dt', function (e, settings, processing) {
        if (processing) {
            ShowPreloader();
        } else {
            HidePreloader();
        }
    });

   

}


function openViewDetailsModal(ad) {
    debugger;

    currentId = ad.userAdvertisementPackageID; // Set the current ID
    $('#modalPackageId').text(ad.userAdvertisementPackageID);
    $('#modalPackageName').text(ad.advertisementPackageName);
    $('#modalPackageCost').text(ad.advertisementPackageCost);
    $('#modalPackageType').text(ad.advertisementPackageType);
    $('#modalNumberOfAds').text(ad.numberOfAdvertisement);
    $('#modalStatus').text(ad.status);
    $('#modalUsername').text(ad.username);
    $('#modalCreatedOn').text(moment(ad.createdOn).format("DD-MMM-YYYY"));

    const mediaUrl = baseApiUrl + ad.filePath;

    // Check if the file is a video
    if (ad.advertisementPackageName.toLowerCase().trim().includes('video ad')) {
        // If it's a video, show the video element
        $('#modalImage').hide(); // Hide the image element
        $('#modalVideo').attr('src', mediaUrl).show(); // Show and set the video source

        $("#lblImage").text("Video");
        $("#trWidth").hide();
        $("#trHeight").hide();
    } else {
        // If it's an image, show the image element
        const tempImg = new Image();
        tempImg.src = mediaUrl;

        tempImg.onload = function () {
            const originalWidth = tempImg.naturalWidth;
            const originalHeight = tempImg.naturalHeight;

            $('#modalImageWidth').text(`${originalWidth}px`);
            $('#modalImageHeight').text(`${originalHeight}px`);
            $('#modalImage').attr('src', mediaUrl).show(); // Show and set the image source
            $('#modalVideo').hide(); // Hide the video element
        };
    }

    $('#viewDetailsModal').modal('show');
}




function openRejectionModal(id) {
    $('#viewDetailsModal').modal('hide'); // Hide the view details modal
    $('#rejectionReasonModal').modal('show'); // Show the rejection modal

    $('#submitRejection').off('click').on('click', function () {
        const reason = $('#rejectionReason').val().trim();
        if (reason) {
            UpdateListingStatus(id, 'Reject', reason);
            $('#rejectionReasonModal').modal('hide');
            $('#rejectionReason').val(''); // Clear the input
        } else {
            Swal.fire({
                title: "Warning",
                text: "Please provide a reason for rejection.",
                icon: "warning"
            });
        }
    });
}



function UpdateListingStatus(id, status, reason = null) {
    let url = `/Dashboard/UserAdvertisementStatus?Id=${id}&Status=${status}`;
    if (status === 'Reject' && reason) {
        url += `&Reason=${encodeURIComponent(reason)}`;
    }

    FilePostRequest(url, null, function (res) {
        debugger
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



function DeleteAdvertising(Id) {

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
            postRequest('/Dashboard/DeleteAdvertisingById?Id=' + Number(Id), null, function (res) {

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
}