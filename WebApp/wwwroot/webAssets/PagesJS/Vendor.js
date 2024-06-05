var profilepic = "#profilepic";
var username = "#username";
var emailid = "#emailid";


$(document).ready(function () {


    var idValue = getQueryParam('id');
    GetvendorDetails(idValue);
});


function getQueryParam(param) {
    var urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param);
}

function GetvendorDetails(id) {


    postRequest('/Vendor/GetVednorDataAndList/' + id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger;

                $(emailid).text(res.data.vendorInfo.email);
                $(username).text(res.data.vendorInfo.username);
              
                $(profilepic).attr("src", "https://localhost:7280/UploadImages\\" + res.data.vendorInfo.profilePicPath);
                $.each(res.data.listings, function (index, item) {


                    var html = `
                        <div class="col-lg-4 col-md-6 col-sm-12" >
                            <div class="listing-item listing-grid-item-two mb-30" style="border: ${item.propertiestoShow};">
                                <div class="listing-thumbnail">
                                    <img src="https://localhost:7280/${item.featureImagePath}" alt="Listing Image">
                                </div>
                                <div class="listing-content">
                                    <h3 class="title">

                                        <span class="status st-close">${item.categoryName}</span>
                                        <a href="/Listing/SingleListing?listingId=${item.id}">${item.title}</a></h3>
                                    <p style="font-weight: ${item.propertiestoShow};">${item.description}</p>
                                    <div class="listing-meta">
                                        <ul>
                                            <li><span><i class="ti-location-pin"></i>${item.location}, ${item.state}</span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    $('#appendListings').append(html);
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