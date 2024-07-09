let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllDashbaord();

})
function GetAllDashbaord() {
    postRequest('/Dashboard/GetAllDashboard', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                var IndexZero = JSON.parse(res.data)[0];
                $("#TotalListingText").text(IndexZero.TotalListing);
                $("#ActiveListingText").text(IndexZero.ActiveListing);
                $("#ExpiredListingText").text(IndexZero.ExpiredListing);
                $("#PendingListingText").text(IndexZero.TotalPendingListing);
                $("#ApprovedListingText").text(IndexZero.TotalApprovedListing);
                $("#RejectedListingText").text(IndexZero.TotalRejectedListing);
                $("#ActivePlanText").text(IndexZero.TotalActivePlan);
                $("#ExpiredPlanText").text(IndexZero.TotalExpiredPlan);
                $("#AppendMyPackages").empty();

                for (let i = 1; i <= JSON.parse(res.data).length - 1; i++) {
                    debugger
                    $("#AppendMyPackages").append(`
                        <tr>
                                <td>${JSON.parse(res.data)[i].Name}</td>
                                <td>${JSON.parse(res.data)[i].AllowedListings == 999 ? "UNLIMITED" : JSON.parse(res.data)[i].AllowedListings}</td>
                                <td>${JSON.parse(res.data)[i].RemainingListings == 999 ? "UNLIMITED" : JSON.parse(res.data)[i].RemainingListings}</td>
                        </tr>`);
                }

                $('#TableMyPackages').DataTable();

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

