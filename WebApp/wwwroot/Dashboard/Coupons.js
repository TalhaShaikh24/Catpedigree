
let baseApiUrl = "";
$(document).ready(function () {
    $("#frmCoupon").validate({
        rules: {
            Name: {
                required: true
            },
            CouponCodePertentage: {
                required: true,
                number: true,
                min: 5,
                max: 100
            }
        },
        messages: {
            Name: {
                required: "Please enter the coupon name."
            },
            CouponCodePertentage: {
                required: "Please enter a coupon code percentage.",
                number: "Please enter a valid number.",
                min: "The percentage must be at least 5.",
                max: "The percentage cannot exceed 100."
            }
        },
        errorElement: "p",
        errorClass: "error"
    });





    $("html, body").animate({ scrollTop: 0 }, "slow");

    baseApiUrl = $("#baseApiUrl").val();

    GetAllDropdowns();

    //$('#CouponCodePertentage').on('input', function () {
     

    //    this.value = this.value.replace(/[^0-9]/g, '');

    //    var value = $(this).val();
        
    //    if (value >= 5 && value <=  100) {


    //        $('#message').text('');
    //    } else {

    //        $('#message').text('Number should be between 5 to 100!').css('color', 'red');
    //    }




    //});




    
})


function GetAllDropdowns() {

    postRequest('/Dashboard/GetAllUsers', null, function (res) {

        if (res.status == 200) {

            debugger;
            if (res.data != null) {

                $("#UserId").empty();
                

                $("#UserId").append(`<option value="-1"  selected>Select User</option>`);
                
                
                $.each(res.data.register, function (i, v) {
                    $("#UserId").append(`<option value="${v.userId}" data-email=${v.email} >${v.firstname} (${v.lastname})</option>`);
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







$("#save").click(function () {

    if ($("#frmCoupon").valid()) {
        // Show spinner and disable button
        $("#BtnSpinner").removeClass("d-none");
        $("#BtnText").text("Submitting...");
        $(this).prop("disabled", true);
        var value = $("#CouponCodePertentage").val();

       
            var obj = {

                DiscountPercentage: Number($("#CouponCodePertentage").val()),
                UserId: $("#UserId").val(),
                CouponCode: "",
                UserName: "",
                CouponName: $("#Name").val()


            }
            postRequest('/Dashboard/AddCouponsCodes', obj, function (res) {
                // Hide spinner and enable button
                $("#BtnSpinner").addClass("d-none");
                $("#BtnText").text("Submit");
                $("#Btn_BlogUpdate").prop("disabled", false);
                if (res.status == 200) {

                    if (res.data != null) {
                        Swal.fire({
                            title: "Good job!",
                            text: res.responseMsg,
                            icon: "success"
                        });

                        window.location.href = "/Dashboard/CouponsList"



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

    

});



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
            // Hide spinner and enable button
            $("#BtnSpinner").addClass("d-none");
            $("#BtnText").text("Submit");
            $("#Btn_BlogUpdate").prop("disabled", false);
        }
    });
}