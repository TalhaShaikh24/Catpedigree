
//$(function ($) {
//    $('[data-numeric]').payment('restrictNumeric');
//    $('.cc-number').payment('formatCardNumber');
//    $('.cc-exp').payment('formatCardExpiry');
//    $('.cc-cvc').payment('formatCardCVC');
//    $.fn.toggleInputError = function (erred) {
//        this.parent('.form-group').toggleClass('has-error', erred);
//        return this;
//    };
//    $('form').submit(function (e) {
//        e.preventDefault();
//        var cardType = $.payment.cardType($('.cc-number').val());
//        $('.cc-number').toggleInputError(!$.payment.validateCardNumber($('.cc-number').val()));
//        $('.cc-exp').toggleInputError(!$.payment.validateCardExpiry($('.cc-exp').payment('cardExpiryVal')));
//        $('.cc-cvc').toggleInputError(!$.payment.validateCardCVC($('.cc-cvc').val(), cardType));
//        $('.cc-brand').text(cardType);
//        $('.validation').removeClass('text-danger text-success');
//        $('.validation').addClass($('.has-error').length ? 'text-danger' : 'text-success');
//    });
//});



let baseApiUrl = "";

let packageID = 0;
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    getAllPackages()
})

function getAllPackages() {


    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    postRequest('/Packages/GetAllPackages/' + curr, null, function (res) {
        debugger;
        if (res.status == 200) {

            if (res.data != null) {
                debugger
                $.each(res.data, function (index, item) {

                    if (item.name != "SINGLE") {
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
                        var pricingPackageDetails = JSON.parse(item.pricingPackageDetails);

                        var html = `
                <div class="col-12 col-lg-3">
                    <div class="pricing-table pedigree">
                        <h3>${item.name}*</h3>
                        <p class="price">Now for € ${item.price.toFixed(2)}** / <span>year</span></p>
                        <p class="description">Key Features:</p>
                        <div class="accordion">
            `;
                        // Loop through pricingPackageDetails to add the dynamic headings and descriptions
                        $.each(pricingPackageDetails, function (detailIndex, detailItem) {
                            html += `
                    <div class="option">
                        <input type="checkbox" id="Starter*-${detailIndex + 1}_${item.packageID}" class="toggle">
                        <label class="title1" for="Starter*-${detailIndex + 1}_${item.packageID}">
                            <span class="green">
                                <span id="hs_cos_wrapper_widget_1724084820212_" class="hs_cos_wrapper hs_cos_wrapper_widget hs_cos_wrapper_type_icon" style="" data-hs-cos-general-type="widget" data-hs-cos-type="icon">
                                    <svg version="1.0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" aria-hidden="true">
                                        <g id="check-circle${detailIndex + 1}_layer">
                                            <path d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z"></path>
                                        </g>
                                    </svg>
                                </span>
                            </span> ${detailItem.Headings}
                        </label>
                        <div class="content">
                            <p>${detailItem.Descriptions}</p>
                        </div>
                    </div>
                `;
                        });

                        // Close the HTML structure
                        html += `
                        </div>
                        <button class="button" type="button" onClick="Payment(${item.packageID})">
                            <p class="button-pt">BUY NOW!</p>
                        </button>
                    </div>
                </div>
            `;


                        $('.pricing-table-slider').append(html);
                    }
                    else {

                        $("#singleLisitngPackage").append(`    
                        <button type="button" class="main-btn mt-5 " style="font-size: 20px;" onClick="Payment(${item.packageID})">
                            I'll rather buy my listings <br>
                        <span style="font-size: 15px;">separate for €7,50 per Listing</span>
                        </button>`);
                    }

               
                });



                var selectedCurrency = localStorage.getItem('cur');
                //updatePrices(selectedCurrency);
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




    var obj = {
        PurchasedProductID: Number(packageID),
        PriceId: 'price_1PWKweKR3yBF1l8fXM3cjclV',
        packageType: 'pricing',
        

    }
    postRequest('/Payment/createcheckoutsession', obj, function (res) {
        debugger
        if (res.status == 200) {
            $(".preloader").hide()
            if (res.data != null) {


                packageID = 0;


                const sessionId = res.data.id;
                const stripe = Stripe('pk_test_51M9O4qKR3yBF1l8fXy3z9Vvtnn8A5e4frQt5lJgfpPOBcBMx6ZZFG93mpFCWgN0EjYXL0l7ioxvtSA07AJzUUOJX00XWJkik2w'); // Replace with your Publishable Key
                stripe.redirectToCheckout({ sessionId });

            
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
            $(".preloader").hide();
            Swal.fire({
                title: "info",
                text: "You need to login to purchase the package.",
                icon: "info",
                showCancelButton: true,
                confirmButtonText: "Log In",
                cancelButtonText: "Cancel",
                allowOutsideClick: false
            }).then((result) => {
                if (result.isConfirmed) {
                    // Redirect to the login page
                    window.location.href = '/Home/login'; // Update with your login URL
                }
            });
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
 //   $("#paymentModal").modal('show');


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
        debugger
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