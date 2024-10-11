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

                $("#UserTotalPromotionPackage").text(IndexZero.UserTotalPromotionPackage);
                $("#TotalUsedPromotionPackages").text(IndexZero.TotalUsedPromotionPackages);
                $("#TotalUnUsedPromotionPackages").text(IndexZero.TotalUnUsedPromotionPackages);
                $("#AvailableBoldPackages").text(IndexZero.AvailableBoldPackages);
                $("#AvailableTopPlusPackages").text(IndexZero.AvailableTopPlusPackages);
                $("#AvailableRimmedPackages").text(IndexZero.AvailableRimmedPackages);

                $("#AvailableBackgroundPackages").text(IndexZero.AvailableBackgroundPackages);

                $("#AvailableTopPackages").text(IndexZero.AvailableTopPackages);
                $("#AvailableVideoAdvertsPackages").text(IndexZero.AvailableVideoAdvertsPackages);
                

                $("#AppendMyPackages").empty();

                const jsonObject = JSON.parse(res.data.jsonObject);

                $.each(jsonObject, function (index, item) {
                    if (index === 0) return; // Skip index 0

                    $("#AppendMyPackages").append(
                        `<tr>
                            <td>${item.Name}</td>
                            <td>${item.AllowedListings === 999 ? "UNLIMITED" : item.AllowedListings}</td>
                            <td>${item.RemainingListings === 999 ? "UNLIMITED" : item.RemainingListings}</td>
                        </tr>`
                                    );
                });


                $('#TableMyPackages').DataTable();
               
                $.each(res.data.assignPromotionPackage, function (index, item) {
                    $("#AppendMyPromotionPackages").append(`
                    <tr>
                        <td>${item.promotionPackageName} (${item.packageCount})</td>
                        <td>${moment(item.subscriptionDate).format("DD - MMMM - YYYY")}</td>
                        <td>${moment(item.expiryDate).format("DD - MMMM - YYYY")}</td>
                    </tr>`);
                });

               
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

