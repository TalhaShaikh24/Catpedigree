
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

    GetAllVideoPackages()
})

function GetAllVideoPackages() {
    postRequest('/VideoPackages/GetAllVideoPackages', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                debugger

                $.each(res.data, function (index, item) {
                    
                    var colorClasses = ['blue', 'magenta']; // Define your color classes
                    var colorClass = colorClasses[index % colorClasses.length]; // Cycle through the color classes
                    var html = `
                    <div class="col-md-4 col-sm-6 mb-5">
                        <div class="pricingTable ${colorClass}">
                            <div class="pricingTable-header">
                                <h3 class="title">${item.packageName}</h3>
                            </div>
                            <div class="price-value">
                                <span class="amount">€${item.price.toFixed(2)}</span>
                            </div>
                            <p class="mx-4 mb-4">${item.description}</p>
                            <div class="pricingTable-signup">
                                <a href="javascript:void(0)"  onClick="Payment(${item.id})">Buy Now</a>
                            </div>
                        </div>
                    </div>`;

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

function Payment(pkgId) {
    packageID = Number(pkgId);

    $("#paymentModal").modal('show');


}

$("#makepayment").click(function () {

  //  $(".preloader").show();

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
        cvc: $("#cc-cvc").val()
    }
    debugger;
    postRequest('/VideoPackages/BuyPackage', obj, function (res) {
        
        if (res.status == 200) {

            if (res.data != null) {

                packageID = 0;



                $("#paymentModal").modal('hide');

          //      $(".preloader").hide();

                Swal.fire({
                    title: "Congratulations!",
                    text: "You have successfuly buy a Video Package",
                    icon: "success",
                    showCancelButton: false,
                    confirmButtonColor: "#3085d6",
                    allowOutsideClick: false,  // Disable outside click
                    allowEscapeKey: true,
                }).then((result) => {
                    console.log(result);  // Debugging: log the result to the console
                    if (result.isConfirmed) {
                        window.close();

                    }
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
                title: "Info",
                text: res.responseMsg,
                icon: "info"
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