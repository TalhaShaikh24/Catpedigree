let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    $("#datepicker").datepicker();
    GetProfileDetail();


    initAutocomplete();
    onPlaceChanged();


});


function initAutocomplete() {
    const input = document.getElementById('address');
    
    autocomplete = new google.maps.places.Autocomplete(input);

    // Set up the dropdown element
    dropdown = document.getElementById('places-dropdown');

    // Listen for place selection
    autocomplete.addListener('place_changed', onPlaceChanged);
}

function onPlaceChanged() {
    const place = autocomplete.getPlace();
    if (!place.geometry) {
        console.log("No details available for input: '" + place.name + "'");
        return;
    }

    const addressComponents = place.address_components;
    let city = "";
    let state = "";
    let country = "";

    for (const component of addressComponents) {
        const types = component.types;
        if (types.includes("locality")) {
            city = component.long_name;
        }
        if (types.includes("administrative_area_level_1")) {
            state = component.short_name;
        }
        if (types.includes("country")) {
            country = component.long_name;
        }
    }

    // Log city, state, and country

    $("#country").val(country);
    $("#province").val(state);
    $("#city").val(city);


}

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
                $("#country").val(res.data.country);
                $("#province").val(res.data.province);
                $("#city").val(res.data.city);



                debugger;
                $("#datepicker").datepicker('setDate', new Date(res.data.dateofBirth))


                $("#FbProfile").val(res.data.faceBook)
                $("#InProfile").val(res.data.insta)
                $("#TwProfile").val(res.data.twitter)

                




                var newprofilePicPath = res.data.profilePicPath.replace(/~/g, '');

                $("#AppendProfilePic").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl + newprofilePicPath}" id="profilePicPath" class="img-fluid img-thumbnail" data-img-url="${res.data.profilePicPath}"/>
               </div>`);

                var newbreederLicensePath = res.data.breederLicensePath.replace(/~/g, '');

                $("#AppendBreederLicense").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl + newbreederLicensePath}" id="breederLicensePath" class="img-fluid img-thumbnail" data-img-url="${res.data.breederLicensePath}"/>
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
    formData.append('Country', $('#country').val());
    formData.append('City', $('#city').val());
    formData.append('Province', $('#province').val());
    formData.append('DateofBirth', $("#datepicker").val());
    formData.append('FaceBook', $("#FbProfile").val());
    formData.append('Insta', $("#InProfile").val());
    formData.append('Twitter', $("#TwProfile").val());

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




const togglePassword = document.querySelector('#togglePassword');
const password = document.querySelector('#password');

togglePassword.addEventListener('click', function () {
    // Toggle the type attribute
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);

    // Toggle the icon
    this.classList.toggle('fa-eye');
    this.classList.toggle('fa-eye-slash');
})