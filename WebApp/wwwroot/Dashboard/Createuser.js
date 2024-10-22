let baseApiUrl = "";
$(document).ready(function () {




    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: new Date(1800, 10 - 1, 25),//最小值                
        yearRange: '-110:+20',
        maxDate: 0

    });
    $("#registerForm").validate({
       
            rules: {
                firstname: {
                    required: true,
                    minlength: 2
                },
                lastname: {
                    required: true,
                    minlength: 2
                },
                email: {
                    required: true,
                    email: true
            },
            password: {
                    required: true,
                  
                },
                username: {
                    required: true,
                    minlength: 5
                },
                contactNo: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 15
                },
                datepicker: {
                    required: true,
                    date: true
                },
                address: {
                    required: true
                },
                country: {
                    required: true
                },
                province: {
                    required: true
                },
                city: {
                    required: true
                },
                zoologicalNumber: {
                    required: true,
                    digits: true
                },
                FbProfile: {
                    url: true
                },
                InProfile: {
                    url: true
                },
                TwProfile: {
                    url: true
                },
                profilePic: {
                    required: true,
                    extension: "jpg|jpeg|png|gif"
                },
                breederLicense: {
                    required: true,
                    extension: "pdf|jpg|jpeg|png"
                },
                profileInfo: {
                    required: true,
                    minlength: 10
                }
            },
            messages: {
                firstname: {
                    required: "Please enter your first name",
                    minlength: "First name must be at least 2 characters long"
                },
                lastname: {
                    required: "Please enter your last name",
                    minlength: "Last name must be at least 2 characters long"
                },
                email: {
                    required: "Please enter your email address",
                    email: "Please enter a valid email address"
                },
                password: {
                    required: "Please enter the password ",
                 
                },
                username: {
                    required: "Please enter a username",
                    minlength: "Username must be at least 5 characters long"
                },
                contactNo: {
                    required: "Please enter your contact number",
                    digits: "Only digits are allowed",
                    minlength: "Contact number must be at least 10 digits",
                    maxlength: "Contact number cannot be more than 15 digits"
                },
                datepicker: {
                    required: "Please select your date of birth",
                    date: "Please enter a valid date"
                },
                profilePic: {
                    required: "Please upload your profile picture",
                    extension: "Only image files are allowed"
                },
                breederLicense: {
                    required: "Please upload your breeder license",
                    extension: "Only pdf, jpg, jpeg, png files are allowed"
                }
            }
    });





    $("html, body").animate({ scrollTop: 0 }, "slow");

    baseApiUrl = $("#baseApiUrl").val();

    GetAllDropdowns();


    initAutocomplete();
    onPlaceChanged();


})

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

function GetAllDropdowns() {

    postRequest('/Dashboard/GetAllRoles', null, function (res) {

        if (res.status == 200) {

            debugger;
            if (res.data != null) {


                $("#Roles").empty();


                $("#Roles").append(`<option value="-1"  selected>Select Roles</option>`);


                $.each(res.data, function (i, v) {
                    $("#Roles").append(`<option value="${v.id}"  >${v.role}</option>`);
                });

                $('#Roles').selectpicker();

                // Remove the first option (index 0) if it is "Select User"
                $('#Roles option').eq(0).remove(); // Change index if needed

                // Refresh the selectpicker to update UI
                $('#Roles').selectpicker('refresh');

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




$('#save').click(function () {

    if ($("#registerForm").valid()) {
        $(".preloader").show();
        var selectedValues = $("#Roles").val();  // This gives you the array ['1', '2', '3', '4', '1004']
        var commaSeparatedString = selectedValues.join(', ');  // Join array elements into a comma-separated string
        debugger;
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
        formData.append('Roles', commaSeparatedString);
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




        FilePostRequest('/Dashboard/AddUser', formData, function (res) {

            if (res.status == 200) {

                debugger;
                if (res.data != null) {


                    $(".preloader").hide()
                    Swal.fire({
                        title: "Success!",
                        text: "User Created Successfully",
                        icon: "success"
                    }).then(() => {

                        window.location.href = '/Dashboard/Users';
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
                $(".preloader").hide()
                Swal.fire({
                    title: "Success!",
                    text: response.message,
                    icon: "success"
                }).then(() => {
                    window.location.href = '/Dashboard/Users';
                });
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



