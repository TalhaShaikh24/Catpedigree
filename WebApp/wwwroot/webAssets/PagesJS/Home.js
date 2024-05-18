$(document).ready(function () {
    GetHomePageListings()
})

function GetHomePageListings() {
    postRequest('/Listing/GetHomePageListings', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {
                    
                    var html = `
                    <div class="col-lg-4 col-md-6 col-sm-12">
                        <div class="listing-item listing-grid-item-two mb-30">
                            <div class="listing-thumbnail">
                                <img src="${item.featureImagePath}" alt="Listing Image">
                            </div>
                            <div class="listing-content">
                                <h3 class="title">
                                    <span class="status st-close">${item.categoryName}</span>
                                    <a href="listing-details-1.html">${item.title}</a></h3>
                                <p>${item.description}</p>
                                <div class="listing-meta">
                                    <ul>
                                        <li><span><i class="ti-location-pin"></i>${item.location} ,${item.state} </span></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
            `;
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



function BuyPackage(pkgId) {

    var obj = {
        PackageID: Number(pkgId)
    }
    postRequest('/Packages/BuyPackage', obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

               
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