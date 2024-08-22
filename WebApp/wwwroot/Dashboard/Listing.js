let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllListings();
});


function GetAllListings() {


    postRequest('/Dashboard/GetAllListings', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                $('#TableApprovalListing').DataTable().destroy();
                $("#AppendApprovalListing").empty();
                $.each(res.data, function (i, v) {

                    var statusIcon = "";
                    if (v.status == "Approve") {
                        statusIcon = '<span class="badge badge-info">Approved</span>';
                    }

                    else if (v.status == "Reject") {
                        statusIcon = '<span class="badge badge-danger">Rejected</span>'
                    }

                    else {

                        statusIcon = '<span class="badge badge-warning">Pending</span>'
                    }

                    $("#AppendApprovalListing").append(`
                                                <tr>

                                                   <td>${v.id}</td>
                                                   <td>${statusIcon}</td>
                                                   <td>${v.title}</td>
                                                   <td>${v.location}</td>
                                                   <td>${v.state}</td>
                                                   <td>${v.city}</td>
                                                   <td>${v.isBreerderLicenseUpload}</td>
                                                   <td>${v.phone}</td>
                                                   <td>${v.email}</td>
                                                   <td>${v.breerderName}</td>
                                                   <td>${v.typeOfCat}</td>
                                                   <td>${v.gender}</td>
                                                   <td>${v.age}</td>
                                                   <td>${v.zoologicalNumber}</td>
                                                   <td>${v.categoryId}</td>
                                                   <td>${v.packageId}</td>
                                                   <td>${v.promotionName}</td>
                                                   <td>${v.isActive}</td>
                                                   <td>${v.createdBy}</td>
                                                   <td>${moment(v.createdOn).format("DD - MMMM - YYYY") }</td>
                                                   <td style="display: flex; justify-content: space-evenly; align-items: center;">
                                                    <button type="button" class="btn btn-success btn-xs p-2 mx-1" onclick="UpdateListingStatus(${v.id}, 'Approve');" title="Approve"><i class="fa fa-check" aria-hidden="true"></i></button>
                                                    <button type="button" class="btn btn-info btn-xs p-2 mx-1" title="Reject" onclick="showReasonModal(${v.id}, 'Reject');"><i class="fa fa-ban" aria-hidden="true"></i></button>
                                                    <button type="button" class="btn btn-warning btn-xs p-2 mx-1" title="Pending" onclick="UpdateListingStatus(${v.id}, 'Pending');"><i class="fa fa-clock" aria-hidden="true"></i></button>
                                                    <button type="button" class="btn btn-danger btn-xs p-2 mx-1" id="btn_Listing_Delete" title="Delete Listing" data-id="${v.id}"><i class="fa fa-trash"></i></button>
                                                    </td>

                                                </tr>`);

                });

                $('#TableApprovalListing').DataTable({
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
