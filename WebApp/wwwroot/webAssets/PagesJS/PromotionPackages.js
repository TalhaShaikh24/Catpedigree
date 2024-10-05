
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


var Promotionpackageid = 0;

let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    getAll()
})

function getAll() {


    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    postRequest('/PromotionPackage/GetAllPromotionPackages/' + curr, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $.each(res.data, function (index, item) {
                    debugger;

                    var colorClasses = ['blue', 'magenta']; // Define your color classes
                    var colorClass = colorClasses[index % colorClasses.length]; // Cycle through the color classes

                    // Assuming the 'item.description' contains multiple features separated by a specific delimiter like ';'
                    var featureList = item.details ? item.details.split('?') : []; // Example: "Feature 1;Feature 2;Feature 3"
                    debugger;
                    var html = `
    <div class="col-12 col-md-4 ">
        <div class="pricing-table enhancement">
            <h3>${item.name}</h3>
            <p class="price">
                €  ${item.costs}
            </p>
            <p class="description">
                ${item.description} <!-- Display the main description -->
            </p>

            <ul class="list-features">
                ${featureList.map(function (feature) {
                        return `
                    <li>
                        <span class="green">
                            <span id="hs_cos_wrapper_module_1724188129826_" class="hs_cos_wrapper hs_cos_wrapper_widget hs_cos_wrapper_type_icon">
                                <svg version="1.0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" aria-hidden="true">
                                    <g id="check-circle_layer">
                                        <path d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z"></path>
                                    </g>
                                </svg>
                            </span>
                        </span> ${feature.trim()}
                    </li>
                    `;
                    }).join('')} <!-- Dynamically generate the list of features -->
            </ul>

            <a href="javascript:void(0)" class="buypackage" data-packagetitle="${item.name}" data-days="${item.ppcid}" data-packageid="${item.promotionPackagesID}">
                <p class="button-pt">BUY NOW!</p>
            </a>
        </div>
    </div>
    `;

                    $('.pricing-table-slider').append(html);
                });

                var html = `<div class="col-12 col-md-4 ">
                <div class="pricing-table enhancement">
    

                    <h3>Coming Soon</h3>
                    <p class="price">
                      €5* 
                    </p>
                    <p class="description">
                      
                    </p>
                    
                    <ul class="list-features">
                            
                      <li><span class="green">
                    <span id="hs_cos_wrapper_module_1724188129826_" class="hs_cos_wrapper hs_cos_wrapper_widget hs_cos_wrapper_type_icon" style="" data-hs-cos-general-type="widget" data-hs-cos-type="icon"><svg version="1.0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" aria-hidden="true"><g id="check-circle1_layer"><path d="M504 256c0 136.967-111.033 248-248 248S8 392.967 8 256 119.033 8 256 8s248 111.033 248 248zM227.314 387.314l184-184c6.248-6.248 6.248-16.379 0-22.627l-22.627-22.627c-6.248-6.249-16.379-6.249-22.628 0L216 308.118l-70.059-70.059c-6.248-6.248-16.379-6.248-22.628 0l-22.627 22.627c-6.248 6.248-6.248 16.379 0 22.627l104 104c6.249 6.249 16.379 6.249 22.628.001z"></path></g></svg></span>        
                        </span>Promotion Background</li>
                      
                            
                            
                    </ul>
                    
                    <a href="#"><p class="button-pt">
                      Package Deals!
                      </p></a>
                  </div>
            </div>`;


               // $('.pricing-table-slider').append(html);
                var selectedCurrency = localStorage.getItem('cur');
               // updatePrices(selectedCurrency);
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

function BuypromotionPackage(pkgId) {
  
}



$(document).on('click', '.buypackage', function () {


   


     Promotionpackageid = $(this).attr('data-packageid');


    var packagetitle = $(this).attr('data-packagetitle');
    var days = $(this).attr('data-days');
    debugger;
    Payment(Promotionpackageid, days);

   // $("#costslist").empty();

   // if (packagetitle == "Video") {


   //     $("#paymentModal").modal('show');
   // }

   // else {
    
    
   //GetPromotionCost(Promotionpackageid);

   // }



});

function GetPromotionCost(pkgId) {


    var obj = {

        Id:pkgId,
        currency:localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')
    }

    postRequest('/PromotionPackage/GetPromotionCost', obj, function (res) {
        debugger;
        if (res.status == 200) {

            if (res.data != null) {

                debugger;


                for (var i = 0; i < res.data.length; i++) {
                    var daysText = res.data[i].daysNumber === 0 ? "For the duration of your advertisement" : `${res.data[i].daysNumber} days`;

                    $("#costslist").append(` 
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio1${res.data[i].promotionCostID}" value="${res.data[i].promotionCostID}">
                            <label class="form-check-label" for="inlineRadio1${res.data[i].promotionCostID}">${daysText} - <span class="price"></span> ${res.data[i].cost + " €"}  </label>
                        </div>
                        <br/>
                    `);
                }

                $('#exampleModalCenter').modal('show');

            }



            var selectedCurrency = localStorage.getItem('cur');
            //updatePrices(selectedCurrency);
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
                text: "Please login to buy this package",
                icon: "warning"
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





$("#BuyPP").click(function () {

    if ($('input[name="inlineRadioOptions"]:checked').val() == undefined) {

        Swal.fire({
            title: "Error",
            text: "Select any days Plan",
            icon: "error"
        });

        return;

    }

    $("#paymentModal").modal('show');
   // BuypromotionPackage(Promotionpackageid)

})


$("#makepayment").click(function () {
 

    debugger
    var expireDate = $('#cc-exp').val();
    // Parse the expire date
    var expireMonth = '';
    var expireYear = '';
    var parts = expireDate.split('/');
    var obj = {
        PromotionPackagesID: Number(Promotionpackageid),

        Days: $('input[name="inlineRadioOptions"]:checked').val() == undefined? 0: parseInt($('input[name="inlineRadioOptions"]:checked').val()),
        CardNumber: $("#cc-number").val(),
        expireMonth: parseInt(parts[0]),
        expireYear: parseInt(parts[1]),
        cvc: $("#cc-cvc").val()
    }
    postRequest('/PromotionPackage/BuyPackage', obj, function (res) {

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
})








function clear() {

    Promotionpackageid = 0;
    $('#exampleModalCenter').modal('hide');
    $("#paymentModal").modal('hide');
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





function Payment(pkgId, days) {
    packageID = Number(pkgId);



    debugger;

    var obj = {
        PurchasedProductID: Number(packageID),
        PriceId: 'price_1PWKweKR3yBF1l8fXM3cjclV',
        packageType: 'PromotionPackage',
        Days: Number(days)

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
