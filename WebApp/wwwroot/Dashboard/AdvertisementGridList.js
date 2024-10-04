let baseApiUrl = "";
let currentId = null; // Variable to hold the current advertisement ID
$(document).ready(function () {
    ShowPreloader();
    baseApiUrl = $("#baseApiUrl").val();


    GetAllListings();
});


function GetAllListings() {


    postRequest('/Dashboard/GetallUserAdvertisementForApprovals', null, function (res) {

        if (res.status == 200) {
            HidePreloader();
            if (res.data != null) {

                $("#AppendApprovalListing").empty();
                $.each(res.data, function (i, v) {
                    $("#AppendApprovalListing").append(`
                    <tr>
                        <td>${v.userAdvertisementPackageID}</td>
                        <td><img src="${baseApiUrl + v.filePath}" alt="Advertisement Image" style="max-width: 60px; max-height: 60px;" /></td>
                        <td>${v.advertisementPackageName}</td>
                        <td>${v.advertisementPackageCost}</td>
                        <td>${v.advertisementPackageType}</td>
                        <td>${v.numberOfAdvertisement}</td>
                        <td>${v.status}</td>
                        <td>${v.username}</td>
                        <td>${v.createdOn}</td>
                        <td style="width: 115px; display: flex; justify-content: space-evenly; align-items: center;">
                            <button type="button" onclick="UpdateListingStatus(${v.userAdvertisementPackageID}, 'Approve');" title="Approve" class="btn btn-success btn-xs p-2"><i class="fa fa-check" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-danger btn-xs p-2" title="Reject" onclick="openRejectionModal(${v.userAdvertisementPackageID});"><i class="fa fa-ban" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-info btn-xs p-2" title="Pending" onclick="UpdateListingStatus(${v.userAdvertisementPackageID}, 'Pending');"><i class="fa fa-clock" aria-hidden="true"></i></button>
                            <button type="button" class="btn btn-primary btn-xs p-2" title="View" onclick='openViewDetailsModal(${JSON.stringify(v).replace(/'/g, "\\'")});'><i class="fa fa-eye" aria-hidden="true"></i></button>
                        </td>
                    </tr>`);
                });


                $('#TableApprovalListing').DataTable();


            }
        }
        if (res.status == 304) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {
            HidePreloader();
            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {
            HidePreloader();
            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

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
    $('#modalCreatedOn').text(ad.createdOn);

    const imageUrl = baseApiUrl + ad.filePath;

    // Create a temporary image element to load the image
    const tempImg = new Image();
    tempImg.src = imageUrl;

    tempImg.onload = function () {
        // Once the image is loaded, get the original dimensions
        const originalWidth = tempImg.naturalWidth;
        const originalHeight = tempImg.naturalHeight;

        // Set the dimensions in the modal
        $('#modalImageWidth').text(`${originalWidth}px`);
        $('#modalImageHeight').text(`${originalHeight}px`);

        // Now set the image source in the modal
        $('#modalImage').attr('src', imageUrl);
    };

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

