

$(document).ready(function () {


    GetAllVendors();
});



function GetAllVendors() {


    postRequest('/Vendor/GetAllVendors', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {


                $.each(res.data, function (index, item) {




                    var html = `
                        <div class="col-lg-4 col-md-6 col-sm-12" >
                            <div class="listing-item listing-grid-item-two mb-30">
                                <div class="listing-thumbnail">
                                    <img  src="https://localhost:7280/${item.featureImagePath}" alt="Listing Image">
                                </div>
                                <div class="listing-content">
                                    <h3 class="title">

                                        <span class="status st-close"></span>
                                        <a href="/Listing/SingleListing?listingId=${item.userId}">${item.username}</a></h3>
                                 
                                    <div class="listing-meta">
                                        <ul>
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