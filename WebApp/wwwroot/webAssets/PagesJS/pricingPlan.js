
$(function ($) {
    $('[data-numeric]').payment('restrictNumeric');
    $('.cc-number').payment('formatCardNumber');
    $('.cc-exp').payment('formatCardExpiry');
    $('.cc-cvc').payment('formatCardCVC');
    $.fn.toggleInputError = function (erred) {
        this.parent('.form-group').toggleClass('has-error', erred);
        return this;
    };
    $('form').submit(function (e) {
        e.preventDefault();
        var cardType = $.payment.cardType($('.cc-number').val());
        $('.cc-number').toggleInputError(!$.payment.validateCardNumber($('.cc-number').val()));
        $('.cc-exp').toggleInputError(!$.payment.validateCardExpiry($('.cc-exp').payment('cardExpiryVal')));
        $('.cc-cvc').toggleInputError(!$.payment.validateCardCVC($('.cc-cvc').val(), cardType));
        $('.cc-brand').text(cardType);
        $('.validation').removeClass('text-danger text-success');
        $('.validation').addClass($('.has-error').length ? 'text-danger' : 'text-success');
    });
});



let baseApiUrl = "";

let packageID = 0;
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    getAllPackages()
})

function getAllPackages() {


    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    postRequest('/Packages/GetAllPackages/' + curr, null, function (res) {

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
                                <span class="amount price" data-price='${item.price.toFixed(2)}'>${item.price.toFixed(2)}</span>
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
                                <a href="javascript:void(0)"  onClick="Payment(${item.packageID})">Buy Now</a>
                            </div>
                        </div>
                    </div>
            `;
                    $('#pricingContainer').append(html);
                });



                var selectedCurrency = localStorage.getItem('cur');
                updatePrices(selectedCurrency);
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


function Payment(pkgId) {
    packageID = Number(pkgId);

    $("#paymentModal").modal('show');


}


$("#makepayment").click(function () {

    $(".preloader").show();

    var expireDate = $('#cc-exp').val();
    // Parse the expire date
    var expireMonth = '';
    var expireYear = '';
    var parts = expireDate.split('/');

    var obj = {
        PackageID: Number(packageID),
        CardNumber: $("#cc-number").val(),
        expireMonth: parseInt(parts[0]),
        expireYear: parseInt(parts[1]),
        cvc: $("#cc-cvc").val(),
        CouponCode: $("#CouponCode").val()

    }
    postRequest('/Packages/BuyPackage', obj, function (res) {

        if (res.status == 200) {
            $(".preloader").hide()
            if (res.data != null) {


                packageID = 0;

                Swal.fire({
                    title: "Congrats",
                    text: res.responseMsg,
                    icon: "success"
                }).then(() => {
                    redirectToAddListing();
                });

            }
        }
        if (res.status == 304) {
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {
            $(".preloader").hide()
            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {
            $(".preloader").hide()
            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });
})

function BuyPackage(pkgId) {
  
}

function redirectToAddListing() {
    window.location.href = window.location.origin + "/Listing";
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