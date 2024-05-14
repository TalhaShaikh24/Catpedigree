$(document).ready(function () {
    getAllPackages()
})

function getAllPackages() {
    postRequest('/Packages/GetAllPackages', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {
                    var categories = [];
                    if (item.categoryNames.includes(',')) {
                        categories = item.categoryNames.split(',').map(function (category) {
                            return category.trim();
                        });
                    } else {
                        categories.push(item.categoryNames.trim());
                    }
                    var colorClasses = ['blue', 'magenta']; // Define your color classes
                    var colorClass = colorClasses[index % colorClasses.length]; // Cycle through the color classes
                    var html = `
                    <div class="col-md-4 col-sm-6 mb-5">
                        <div class="pricingTable ${colorClass}">
                            <div class="pricingTable-header">
                                <h3 class="title">${item.name}</h3>
                            </div>
                            <div class="price-value">
                                <span class="amount">€${item.price.toFixed(2)}</span>
                            </div>
                            <h4 class="mb-4">Key Features:</h4>
                            <ul class="pricing-content">
                                <li>Advertising Limit: ${item.isUnlimited == true ? 'UNLIMITED' : item.allowedListings}</li>
                                <li>Expiry Time: ${item.duration} Days</li>
                            </ul>
                            <h4 class="mb-4">Categories:</h4>
                            <ul class="pricing-content">
                               ${categories.map(category => `<li>${category}</li>`).join('')}
                            </ul>
                            <p class="mx-4 mb-4">${item.description}</p>
                            <div class="pricingTable-signup">
                                <a href="javascript:void(0)"  onClick="BuyPackage(${item.packageID})">Sign Up</a>
                            </div>
                        </div>
                    </div>
            `;
                    $('#pricingContainer').append(html);
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