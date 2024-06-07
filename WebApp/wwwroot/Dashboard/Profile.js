let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    GetProfileDetail();

});


$("#profilePic").change(function (e) {

    $("#AppendProfilePic").empty().append(`<div style="width:150px;height:150px;">
             <img  src="${URL.createObjectURL(e.target.files[0])}" id="profilePicPath" class="img-fluid img-thumbnail" data-img-url="${e.target.files[0].name}"/>
          </div>`);

});



$("#breederLicense").change(function (e) {

    $("#AppendBreederLicense").empty().append(`<div style="width:150px;height:150px;">
          <img src="${URL.createObjectURL(e.target.files[0])}" id="profilePicPath" class="img-fluid img-thumbnail" data-img-url="${e.target.files[0].name}"/>
        </div>`);

});




function GetProfileDetail() {

    FilePostRequest('/Dashboard/GetProfileDetailById', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#firstname").val(res.data.firstname);
                $("#lastname").val(res.data.lastname);
                $("#email").val(res.data.email);
                $("#password").val(res.data.password);
                $("#contactNo").val(res.data.contactNo);
                $("#address").val(res.data.address);
                $("#zoologicalNumber").val(res.data.zoologicalNumber);
                $("#profileInfo").val(res.data.profileInfo);
                $("#username").val(res.data.username);

                debugger;

                $("#AppendProfilePic").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl+res.data.profilePicPath}" id="profilePicPath" class="img-fluid img-thumbnail" data-img-url="${res.data.profilePicPath}"/>
               </div>`);

                $("#AppendBreederLicense").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl+res.data.breederLicensePath}" id="breederLicensePath" class="img-fluid img-thumbnail" data-img-url="${res.data.breederLicensePath}"/>
                </div>`);

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



$('#updateBtn').click(function () {

    var formData = new FormData();

    formData.append('Firstname', $('#firstname').val());
    formData.append('Lastname', $('#lastname').val());
    formData.append('Email', $('#email').val());
    formData.append('Username', $('#username').val());
    formData.append('Password', $('#password').val());
    formData.append('ContactNo', $('#contactNo').val());
    formData.append('Address', $('#address').val());
    formData.append('ProfileInfo', $('#profileInfo').val());
    formData.append('ZoologicalNumber', $('#zoologicalNumber').val());
    formData.append('ProfilePicPath', $("#profilePicPath").attr("data-img-url"));
    formData.append('BreederLicensePath', $("#breederLicensePath").attr("data-img-url"));

    // Append ProfilePic if exists
    var profilePic = $("#profilePic")[0].files[0];
    if (profilePic) {
        formData.append('ProfilePic', profilePic);
    }

    // Append BreederLicense if exists
    var breederLicense = $("#breederLicense")[0].files[0];
    if (breederLicense) {
        formData.append('BreederLicense', breederLicense);
    }

    FilePostRequest('/Dashboard/UpdateProfile', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
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

});

