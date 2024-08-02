
let baseApiUrl = "";
$(document).ready(function () {
    $("html, body").animate({ scrollTop: 0 }, "slow");

    baseApiUrl = $("#baseApiUrl").val();

    GetAllDropdowns();

    $('#CouponCodePertentage').on('input', function () {
     

        this.value = this.value.replace(/[^0-9]/g, '');

        var value = $(this).val();
        
        if (value >= 5 && value <=  100) {


            $('#message').text('');
        } else {

            $('#message').text('Number Should Be B/W 5 to 100!').css('color', 'red');
        }




    });




    
})


function GetAllDropdowns() {

    postRequest('/Dashboard/GetAllUsers', null, function (res) {

        if (res.status == 200) {

            debugger;
            if (res.data != null) {

                $("#UserId").empty();
                

                $("#UserId").append(`<option value="-1"  selected>Select User</option>`);
                
                
                $.each(res.data, function (i, v) {
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

    var value = $("#CouponCodePertentage").val();

    if (value >= 5 && value <= 100) {
        var obj = {

            DiscountPercentage: Number($("#CouponCodePertentage").val()),
            UserId: $("#UserId").val(),
            CouponCode: "",
            UserName:""


        }
        postRequest('/Dashboard/AddCouponsCodes', obj, function (res) {

            if (res.status == 200) {

                if (res.data != null) {
                    Swal.fire({
                        title: "Good job!",
                        text: res.responseMsg,
                        icon: "success"
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
    } else {

        $('#message').text('Number Should Be B/W 5 to 100!').css('color', 'red');
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
        }
    });
}