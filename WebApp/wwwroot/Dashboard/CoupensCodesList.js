


$(document).ready(function () {

    GetAllListings()
})
function GetAllListings() {


    postRequest('/Dashboard/GetCouponCodes', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                // Destroy existing DataTable instance if it exists
                if ($.fn.DataTable.isDataTable('#TableCoupensCodesListing')) {
                    $('#TableCoupensCodesListing').DataTable().clear().destroy();
                }

                // Initialize DataTable instance
                var table = $('#TableCoupensCodesListing').DataTable();

                // Clear any existing rows before adding new ones
                $("#AppendCoupensCodesListing").empty();

                // Iterate over the response data and add rows to the DataTable
                $.each(res.data, function (i, v) {
                    table.row.add([
                        v.couponCode,
                        v.discountPercentage + '%',
                        v.userName,
                        v.couponsDays,
                        v.isActive,
                        v.isExpired,
                        v.usedBy,
                        v.createdOn,
                        `<div style="width: 115px; display: flex; justify-content: space-evenly; align-items: center;">
            <button type="button" class="btn btn-danger btn-xs p-2" title="Delete" onclick="UpdateExpireStatus(${v.couponID});"><i class="fa fa-trash" aria-hidden="true"></i></button>
            <button type="button" class="btn btn-info btn-xs p-2" title="Pending" onclick="UpdateActiveStatus(${v.couponID});"><i class="fa fa-clock" aria-hidden="true"></i></button>
         </div>`
                    ]);
                });

                // Redraw the DataTable to display the new data
                table.draw();

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



function UpdateExpireStatus(id) {

    debugger
    FilePostRequest(`/Dashboard/IsExpireCoupens?Id=${id}`, null, function (res) {

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
function UpdateActiveStatus(id ) {

    debugger
    FilePostRequest(`/Dashboard/ActiveDeactiveCode?Id=${id}`, null, function (res) {

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

