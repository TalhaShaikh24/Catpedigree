let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();


    GetAllDropdowns();

})

function GetAllDropdowns() {

    postRequest('/Dashboard/GetPromotionPackagesWithDaysRes', null, function (res) {


        debugger;
        if (res.status == 200) {
           
            if (res.data != null) {

                $("#UserId").empty();


                $("#Promotionpackage").empty();



         
                if (res.data.assignPromotionPackages.length > 0) {

                    $("#Promotionpackage").append(`<option value="-1">Select Promotion Package</option>`);

                    $.each(res.data.assignPromotionPackages, function (i, v) {
                        debugger;
                        $("#Promotionpackage").append(`<option value="${v.ppcid}">${v.promotionPackageName}</option>`);
                    });
                }
                else {
                    $("#Promotionpackage").append(`<option value="-1" disabled> No Promotion Package data</option>`);
                }

                $("#UserId").append(`<option value="-1">Select user</option>`);

                $.each(res.data.users, function (i, v) {
                    $("#UserId").append(`<option value="${v.userId}">${v.username}</option>`);
                });

                debugger;

                $('select').niceSelect('update');

                GetDatatable();
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




$("#save").click(function () {


    if ($("#UserId").val()=="-1") {
        Swal.fire({
            title: "Error",
            text: "Select and User",
            icon: "error"
        });

        return;
    }
    if ($("#Promotionpackage").val()=="-1") {
        Swal.fire({
            title: "Error",
            text: "Select and Promotion Package",
            icon: "error"
        });

        return;
    }

    var data = {

        PPCID: parseInt($("#Promotionpackage").val()) ,

        userId: parseInt($("#UserId").val()),

        PromotionPackageName:"",

        CreatedBy:0

    }

    debugger;
    postRequest('/Dashboard/AssignPromotionPackageToUser', data, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });
                GetAllDropdowns();
                var urlParams = new URLSearchParams(window.location.search);


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



function GetDatatable() {

    postRequest('/Dashboard/getAllUsersPromotionPackages', null, function (res) {

        if (res.status == 200) {
            HidePreloader();
            if (res.data != null) {

                $('#packagesAssignedtable').DataTable().clear().draw();

                $.each(res.data, function (i, v) {
                    $('#packagesAssignedtable').DataTable().row.add([
                        // v.userId,
                        v.username,
                        v.promotionPackageName.replace('(0 Days)', ''), // Replacing (0 Days) with an empty string
                        v.subscriptionDate,
                        v.createdOn,
                        // v.price
                    ]).draw();
                });



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
