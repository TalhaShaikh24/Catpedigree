let baseApiUrl = "";

let autocomplete;
let dropdown;
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true
    });

    $("#vendor").prop("checked", true);
    $('#registerBtn').click(function () {

        $(".preloader").show();

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
        formData.append('RoleId', Number($("input[type=radio][name=userType]:checked").val()));
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
      
        $.ajax({
            url: '/Account/RegisterUser',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                handleSuccess(response);
            },
            error: function (xhr, status, error) {
                handleError(xhr);
            }
        });

        function handleSuccess(response) {
            $(".preloader").hide()
            Swal.fire({
                title: "Success!",
                text: "Congratulations! Your registration was successful.",
                icon: "success"
            }).then(() => {
                redirectToHome();
            });
        }

        function handleError(xhr) {
            $(".preloader").hide()
            let errorMessage = "Oops! Something went wrong.";
            if (xhr.responseText) {
                errorMessage = xhr.responseText;
            }
            Swal.fire({
                title: "Oops!",
                text: errorMessage,
                icon: "error"
            });
        }

        function redirectToHome() {
            window.location.href = window.location.origin;
        }

    });


    initAutocomplete();
    onPlaceChanged();

});



function initAutocomplete() {
    const input = document.getElementById('address');
    debugger;
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


$(document).on("change", "input[type=radio][name=userType]", function () {
    if ($(this).val() === '2') {
        $("#zoologicalNumberField").show();
        $("#breederLicenseField").show();
    } else {
        $("#zoologicalNumberField").hide();
        $("#breederLicenseField").hide();
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