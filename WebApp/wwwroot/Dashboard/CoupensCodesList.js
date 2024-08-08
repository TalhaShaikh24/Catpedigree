


$(document).ready(function () {

    GetAllListings()
})
function GetAllListings() {


    postRequest('/Dashboard/GetCouponCodes', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendCoupensCodesListing").empty();
                $.each(res.data, function (i, v) {

                    $("#AppendCoupensCodesListing").append(`
                                                <tr>

                                                   <td>${v.couponCode}</td>
                                                   <td>${v.discountPercentage+ '%'}</td>
                                                   <td>${v.userName}</td>
                                                   <td>${v.isActive}</td>
                                                   <td>${v.isExpired}</td>
                                                 
                                                   <td>${v.usedBy}</td>
                                                 

                                                   <td>${v.createdOn}</td>
                                                           <td style=" width: 115px; display: flex; justify-content: space-evenly; align-items: center;">
                                                                   <button type="button" class="btn btn-danger btn-xs p-2" title="Reject" onclick="UpdateExpireStatus(${v.couponID});"><i class="fa fa-ban" aria-hidden="true"></i></button>
                                                                 <button type="button" class="btn btn-info btn-xs p-2" title="Pending" onclick="UpdateActiveStatus(${v.couponID});"><i class="fa fa-clock" aria-hidden="true"></i></button>
                                                    </td>

                                                </tr>`);

                });

                $('#TableCoupensCodesListing').DataTable();


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

