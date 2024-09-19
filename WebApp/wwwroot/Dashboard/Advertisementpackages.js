
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

var AdvertisementPackageID = 0;

let baseApiUrl = "";
$(document).ready(function () {
   // ShowPreloader();
    baseApiUrl = $("#baseApiUrl").val();
     getAll()
})

function getAll() {


    var curr = localStorage.getItem('cur') == null ? 'EUR' : localStorage.getItem('cur')

    debugger;
    postRequest('/Dashboard/GetAdvertisementPackagesDashboard/' + curr, null, function (res) {

        if (res.status == 200) {
            HidePreloader();
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

                    //<div class="col-md-4 col-sm-6 mb-5">
                    //    <div class="pricingTable ${colorClass}">
                    //        <div class="pricingTable-header">
                    //            <h3 class="title">${item.advertisementPackageName}</h3>
                    //        </div>

                    //        <p class="mx-4 mb-4">${item.advertisementPackageType}</p>
                    //        <h4 class="mb-4">Costs:</h4>
                    //        <ul class="pricing-content" id="costs${item.advertisementPackageID}">
                    //            <li>${item.numberOfAdvertisement} Number of Advertisement + <span class="price"></span>  ${item.advertisementPackageCost}</li>
                    //        </ul>

                    //        <div class="pricingTable-signup">
                    //            <a href="javascript:void(0)" class="buypackage" data-packageid="${item.advertisementPackageID}" >Buy Now</a>

                    //        </div>
                    //    </div>
                    //</div>
                    var html = `
                
                    <div class="col-12 col-md-4 ">
                        <div class="pricing-table" style="min-height: 435px;">


                    <h3>${item.advertisementPackageName}</h3>
                   
                        <ul class="pricing-content" id="costs${item.advertisementPackageID}">
                           <span class="pricess">From</span>  <span class="price"></span>    <span class="pricess">    ${item.advertisementPackageCost} </span>  <span > / Month</span>
                         </ul>


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
                            </span> Your Advertisement on every page of our website
                        </li>
                    </ul>
                    <a href="javascript:void(0)" class="buypackage"ata-packageid="${item.advertisementPackageID}">
                        <p class="button-pt">
                            BUY NOW!
                        </p>
                    </a>
                </div>

                </div>

                

            `;
                    $('#AdvertisementpackagesContainer').append(html);
                });

                debugger;
                var selectedCurrency = localStorage.getItem('cur');
                SinglePriceListing(selectedCurrency);
            }
        }
        if (res.status == 304) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {
            HidePreloader();
            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {
            HidePreloader();
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



$(document).on('click', '.buypackage', function () {










    $("#paymentModal").modal('show');


    AdvertisementPackageID = $(this).attr('data-packageid');

 
        




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