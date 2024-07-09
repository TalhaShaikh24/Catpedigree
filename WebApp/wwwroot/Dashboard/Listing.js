let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllListings();
});


function GetAllListings() {


    postRequest('/Dashboard/GetAllListings', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendApprovalListing").empty();
                $.each(res.data, function (i, v) {

                    $("#AppendApprovalListing").append(`
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
                                                   <td>${v.gender}</td>
                                                   <td>${v.description}</td>
                                                   <td>${v.age}</td>
                                                   <td>${v.zoologicalNumber}</td>
                                                   <td>${v.categoryId}</td>
                                                   <td>${v.packageId}</td>

                                            <td>${v.promotionName}</td>
                                                   <td>${v.isActive}</td>
                                                   <td>${v.status}</td>
                                                   <td>${v.createdBy}</td>
                                                   <td>${v.createdOn}</td>
                                                           <td style=" width: 115px; display: flex; justify-content: space-evenly; align-items: center;">
                                                                <button type="button" onclick="UpdateListingStatus(${v.id}, 'Approve');" title="Approve" class="btn btn-success btn-xs p-2"><i class="fa fa-check" aria-hidden="true"></i></button>
                                                                    <button type="button" class="btn btn-danger btn-xs p-2" title="Reject" onclick="UpdateListingStatus(${v.id}, 'Reject');"><i class="fa fa-ban" aria-hidden="true"></i></button>
                                                                 <button type="button" class="btn btn-info btn-xs p-2" title="Pending" onclick="UpdateListingStatus(${v.id}, 'Pending');"><i class="fa fa-clock" aria-hidden="true"></i></button>
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



function UpdateListingStatus(id, status) {

    debugger
    FilePostRequest(`/Dashboard/UpdateListingStatus?Id=${id}&Status=${status}`, null, function (res) {

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


