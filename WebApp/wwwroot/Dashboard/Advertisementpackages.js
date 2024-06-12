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
                               <li>${item.numberOfAdvertisement} Number of Advertisement + $  ${item.advertisementPackageCost}</li>
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


    AdvertisementPackageID = $(this).attr('data-packageid');

    BuypromotionPackage(AdvertisementPackageID)

    //$("#costslist").empty();



 //   GetPromotionCost(Promotionpackageid);





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