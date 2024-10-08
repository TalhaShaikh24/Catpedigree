let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllDashbaord();

})
function GetAllDashbaord() {
    postRequest('/Dashboard/GetAllDashboard', null, function (res) {

        if (res.status == 200) {
            debugger
            if (res.data.jsonObject != null) {

                var IndexZero = JSON.parse(res.data.jsonObject)[0];
                $("#TotalListingText").text(IndexZero.TotalListing);
                $("#ActiveListingText").text(IndexZero.ActiveListing);
                $("#ExpiredListingText").text(IndexZero.ExpiredListing);
                $("#PendingListingText").text(IndexZero.TotalPendingListing);
                $("#ApprovedListingText").text(IndexZero.TotalApprovedListing);
                $("#RejectedListingText").text(IndexZero.TotalRejectedListing);
                $("#ActivePlanText").text(IndexZero.TotalActivePlan);
                $("#ExpiredPlanText").text(IndexZero.TotalExpiredPlan);

                $("#TotalAdvertisementPackages").text(IndexZero.TotalAdvertisementPackages);
                $("#TotalUtlizedAds").text(IndexZero.TotalUtlizedAds);
                $("#TotalAdsApproved").text(IndexZero.TotalAdsApproved);

                $("#TotalAdsReject").text(IndexZero.TotalAdsReject);
                
                $("#TotalAdsPending").text(IndexZero.TotalAdsPending);


                $("#AppendMyPackages").empty();

                for (let i = 1; i <= JSON.parse(res.data.jsonObject).length - 1; i++) {
                    debugger
                    $("#AppendMyPackages").append(`
                        <tr>
                                <td>${JSON.parse(res.data.jsonObject)[i].Name}</td>
                                <td>${JSON.parse(res.data.jsonObject)[i].AllowedListings == 999 ? "UNLIMITED" : JSON.parse(res.data.jsonObject)[i].AllowedListings}</td>
                                <td>${JSON.parse(res.data.jsonObject)[i].RemainingListings == 999 ? "UNLIMITED" : JSON.parse(res.data.jsonObject)[i].RemainingListings}</td>
                        </tr>`);
                }

                $('#TableMyPackages').DataTable();
               
                for (let i = 1; i <= res.data.assignPromotionPackage.length - 1; i++) {
                 
                    $("#AppendMyPromotionPackages").append(`
                        <tr>
                                <td>${res.data.assignPromotionPackage[i].promotionPackageName}</td>
                                <td>${moment(res.data.assignPromotionPackage[i].subscriptionDate).format("DD - MMMM - YYYY") }</td>
                               <td>${moment(res.data.assignPromotionPackage[i].expiryDate).format("DD - MMMM - YYYY") }</td>

                               </tr>`);
                }
               
                $('#TableMyPromotionPackages').DataTable();


                debugger;
                $.each(res.data.advertisementPackage, function (i, v) {
                    debugger;
                    $("#AppendApprovalListing").append(`
                                                <tr>

                                                   <td>${v.userAdvertisementPackageID}</td>
                                                   <td>${v.advertisementPackageName}</td>
                                                   <td>${v.advertisementPackageCost}</td>
                                                   <td>${v.advertisementPackageType}</td>
                                                   <td>${v.numberOfAdvertisement}</td>
                                                   <td>${v.status}</td>
                                                 
                                                   
                                                   <td>${moment(v.createdOn).format("DD - MMMM - YYYY") }</td>
                                                   <td> <img src="${baseApiUrl + v.filePath}" style="

    height: 51px;
    width: 51px;
" />   </td>
                                                   

                                                </tr>`);

                });

                $('#TableApprovalListing').DataTable();

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

