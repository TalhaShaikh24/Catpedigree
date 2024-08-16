
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    $('#packagesAssignedtable').DataTable();
    GetAllDropdowns();





  




});


function GetAllDropdowns() {


    postRequest('/Dashboard/GetAllUsersForPricingPackages', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {



                $("#Users").empty();
                $("#Pricing").empty();
              


                
                $("#Pricing").append(`<option value="-1" disabled selected>Select Packages</option>`);
                $("#Users").append(`<option value="-1" disabled selected>Select User</option>`);
           
                $.each(res.data, function (i, v) {
                    $("#Users").append(`<option value="${v.userId}">${v.username}</option>`);
                });

            
         

                GetDatatable() 


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




function GetDatatable() {

    postRequest('/Dashboard/GetUserpackagesAssigned', null, function (res) {

        if (res.status == 200) {
            HidePreloader();
            if (res.data != null) {

                $('#packagesAssignedtable').DataTable().clear().draw();

                $.each(res.data, function (i, v) {
                    $('#packagesAssignedtable').DataTable().row.add([
                        v.userId,
                        v.username,
                        v.email,
                        v.contactNo,
                        v.name,
                        v.price
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



$("#Users").change(function () {

    var id = $(this).val();


    postRequest('/Dashboard/getAllPackagestoAssgin/' + id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {



                $("#Pricing").empty();




                $("#Pricing").append(`<option value="-1" disabled selected>Select Packages</option>`);
                
                $.each(res.data, function (i, v) {
                    $("#Pricing").append(`<option value="${v.packageID}">${v.name}</option>`);
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




});




$("#save").click(function () {

    var Users = $("#Users").val();
    var PricingpackageID = $("#Pricing").val();

    debugger;

    if (Users == null) {
        Swal.fire({
            title: "Error",
            text: 'Please Select the  user',
            icon: "error"
        });

        return;

    }

    if (PricingpackageID == null) {
        Swal.fire({
            title: "Error",
            text: 'Please Select the  Package',
            icon: "error"
        });

        return;

    }

    var obj = {

        UserID: Users,
        PackageID: Number(PricingpackageID),
    }


    postRequest('/Dashboard/AssignPackage', obj, function (res) {

        if (res.status == 200) {
            $(".preloader").hide()
            if (res.data != null) {


                packageID = 0;

                Swal.fire({
                    title: "Congrats",
                    text: res.responseMsg,
                    icon: "success"
                }).then(() => {
                
                });


                $("#Users").val('-1');


                GetDatatable();
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