
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

    baseApiUrl = $("#baseApiUrl").val();
    getAll()
})

function getAll() {
    postRequest('/Dashboard/GetAdvertisementPackage', null, function (res) {

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
                    <div class="col-md-4 col-sm-6 mb-5">
                        <div class="pricingTable ${colorClass}">
                            <div class="pricingTable-header">
                                <h3 class="title">${item.advertisementPackageName}</h3>
                            </div>
                            
                            <p class="mx-4 mb-4">${item.advertisementPackageType}</p>
                            <h4 class="mb-4">Costs:</h4>
                             <ul class="pricing-content" id="costs${item.advertisementPackageID}">
                               <li>${item.numberOfAdvertisement} Number of Advertisement + £  ${item.advertisementPackageCost}</li>
                            </ul>

                            <div class="pricingTable-signup">
                                <a href="javascript:void(0)" class="buypackage" data-packageid="${item.advertisementPackageID}" >Buy Now</a>
                           
                                </div>
                        </div>
                    </div>
            `;
                    $('#AdvertisementpackagesContainer').append(html);
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


    //$('#exampleModalCenter').modal('show');


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