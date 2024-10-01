
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

var AdvertisementPackageID = 0;

let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
   getAll();

})

$(document).on('click','#scrollToPricing',function () {
    // Scroll to the div with ID 'target'
    $('html, body').animate({
        scrollTop: $('#targetPricing').offset().top
    }, 1000); // Duration in milliseconds
});

function getAll() {


    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    debugger;
    postRequest('/Advertisement/GetAdvertisementPackage/' + curr, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {


                    //var costs = [];

                    //if (item.promotionCosts.length > 0) {


                    //    for (var i = 0; i < item.promotionCosts.length; i++) {

                    //        costs.push({
                    //            daysNumber: item.promotionCosts[i].daysNumber,
                    //            cost: item.promotionCosts[i].cost
                    //        });
                    //    }
                    //}
                    //else {
                    //    costs.push({
                    //        daysNumber: '',
                    //        cost: ''
                    //    });

                    //}


                    var colorClasses = ['blue', 'magenta']; // Define your color classes
                    var colorClass = colorClasses[index % colorClasses.length]; // Cycle through the color classes
                    var html = `


    <div class="col-12 col-md-4 ">
        <div class="pricing-table" style="min-height: 435px;">


            <h3>${item.advertisementPackageName}</h3>
            <p class="price">
                From € ${item.advertisementPackageCost} / <span>year</span>
            </p>


            <ul class="list-features">

                <li>
                    <span class="green">
                        <span id="hs_cos_wrapper_widget_1724520991266_"
                              class="hs_cos_wrapper hs_cos_wrapper_widget hs_cos_wrapper_type_icon" style=""
                              data-hs-cos-general-type="widget" data-hs-cos-type="icon">
                            <svg version="1.0"
                                 xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" aria-hidden="true">
                                <g id="check-circle1_layer">
                                    <path d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z">
                                    </path>
                                </g>
                            </svg>
                        </span>
                    </span> ${item.advertisementPackageType}
                </li>
            </ul>

            <a href="javascript:void(0)" class="buypackage" data-packageid="${item.advertisementPackageID}">
                <p class="button-pt">
                    BUY NOW!
                </p>
            </a>
    
            </a>
        </div>


    </div>

            `;

            //        var html = `
            //        <div class="col-md-4 col-sm-6 mb-5">
            //            <div class="pricingTable ${colorClass}">
            //                <div class="pricingTable-header">
            //                    <h3 class="title">${item.advertisementPackageName}</h3>
            //                </div>
                            
            //                <p class="mx-4 mb-4">${item.advertisementPackageType}</p>
            //                <h4 class="mb-4">Costs:</h4>
            //                 <ul class="pricing-content" id="costs${item.advertisementPackageID}">
            //                   <li>${item.numberOfAdvertisement} Number of Advertisement + <span class="price"></span>  ${item.advertisementPackageCost}</li>
            //                </ul>

            //                <div class="pricingTable-signup">
            //                    <a href="javascript:void(0)" class="buypackage" data-packageid="${item.advertisementPackageID}" >Buy Now</a>
                           
            //                    </div>
            //            </div>
            //        </div>
            //`;
                    $('.pricing-table-slider').append(html);
                });

                debugger;
                var selectedCurrency = localStorage.getItem('cur');
                SinglePriceListing(selectedCurrency);
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
                title: "Oops",
                text: "Please log in as a Business Advertiser!",
                icon: "warning",
                dangerMode: true,
                showCancelButton: true, // Show the cancel button
                confirmButtonText: "Login",
                cancelButtonText: "Cancel",
                confirmButtonColor: "#3085d6", // Optional: Change button color
                cancelButtonColor: "#d33", // Optional: Change cancel button color
            }).then((result) => {
                if (result.isConfirmed) {
                    // Redirect to login page or perform login action
                    window.location.href = '/Home/login'; // Adjust the URL to your login page
                }
            });

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






function BuypromotionPackage(pkgId) {
    //if ($('input[name="inlineRadioOptions"]:checked').val() == undefined) {

    //    Swal.fire({
    //        title: "Error",
    //        text: "Select any days Plan",
    //        icon: "error"
    //    });

    //    return;

    //}
    var obj = {
        AdvertisementPackageID: Number(pkgId),

        //  Days: parseInt($('input[name="inlineRadioOptions"]:checked').val())
    }
    postRequest('/Dashboard/BuyAdvertisementPackage', obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                Swal.fire({
                    title: "Good job!",
                    text: res.responseMsg,
                    icon: "success"
                });

                clear();

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
            $(".preloader").hide();
            Swal.fire({
                title: "info",
                text: "You need to login as business advertiser to purchase the package.",
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



$(document).on('click', '.buypackage', function () {
    AdvertisementPackageID = $(this).attr('data-packageid');
    Payment(AdvertisementPackageID)

    //$.ajax({
    //    type: 'POST',
    //    contentType: 'application/json;charset=utf-8',
    //    dataType: "json",
    //    url: "/Advertisement/CheckCookiesData",
    //  //  data: JSON.stringify(requestData),
    //    success: function (data, textStatus, xhr) {

    //        debugger;


    //        if (data) {

    //            $("#paymentModal").modal('show');


    //            AdvertisementPackageID = $(this).attr('data-packageid');

    //        }
    //        else {

    //            Swal.fire({
    //                title: "Oops",
    //                text: "Please log in as a Business Advertiser!",
    //                icon: "warning",
    //                dangerMode: true,
    //                showCancelButton: true, // Show the cancel button
    //                confirmButtonText: "Login",
    //                cancelButtonText: "Cancel",
    //                confirmButtonColor: "#3085d6", // Optional: Change button color
    //                cancelButtonColor: "#d33", // Optional: Change cancel button color
    //            }).then((result) => {
    //                if (result.isConfirmed) {
    //                    // Redirect to login page or perform login action
    //                    window.location.href = '/Home/login'; // Adjust the URL to your login page
    //                }
    //            });

    //        }



    //    },
    //    error: function (xhr, textStatus, errorThrown) {
    //        Swal.fire({
    //            title: "Error",
    //            text: "Something Went Wrong!",
    //            icon: "error",
    //            dangerMode: true,
    //        })
    //    }
    //});










});


$("#makepayment").click(function () {

    var expireDate = $('#cc-exp').val();
    // Parse the expire date
    var expireMonth = '';
    var expireYear = '';
    var parts = expireDate.split('/');
    var obj = {
        AdvertisementPackageID: Number(AdvertisementPackageID),

        CardNumber: $("#cc-number").val(),
        expireMonth: parseInt(parts[0]),
        expireYear: parseInt(parts[1]),
        cvc: $("#cc-cvc").val()
    }
    postRequest('/Dashboard/BuyAdvertisementPackage', obj, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                Swal.fire({
                    title: "Good job!",
                    text: res.responseMsg,
                    icon: "success"
                });

                clear();

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

});


function GetPromotionCost(pkgId) {


    postRequest('/PromotionPackage/GetPromotionCost/' + pkgId, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger;


                for (var i = 0; i < res.data.length; i++) {

                    $("#costslist").append(` 
                    <div class="form-check form-check-inline">
              <input class="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio1${res.data[i].promotionCostID}" value="${res.data[i].promotionCostID}">
                 <label class="form-check-label" for="inlineRadio1${res.data[i].promotionCostID}">${res.data[i].daysNumber} days - $ ${res.data[i].cost}  </label>
                    </div>
                    <br/>
      `);

                }


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





//$("#BuyPP").click(function () {

//    BuypromotionPackage(Promotionpackageid)

//})

function Payment(pkgId) {
    packageID = Number(pkgId);



    var obj = {
        PurchasedProductID: Number(packageID),
        PriceId: 'price_1PWKweKR3yBF1l8fXM3cjclV',
        packageType: 'Advertisement'


    }
    postRequest('/Payment/createcheckoutsession', obj, function (res) {
        debugger
        if (res.status == 200) {
            $(".preloader").hide()
            if (res.data != null) {


                packageID = 0;

                AdvertisementPackageID = 0;
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
                text: "You need to login as business advertiser to purchase the package.",
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



function clear() {

    AdvertisementPackageID = 0;

    $("#paymentModal").modal('hide');
    //  $('#exampleModalCenter').modal('hide');

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