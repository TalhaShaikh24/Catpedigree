
let baseApiUrl = "";
let paginationIndex;
$(document).ready(function () {
    $("html, body").animate({ scrollTop: 0 }, "slow");

    baseApiUrl = $("#baseApiUrl").val();


    // Get the query string from the current URL
    var queryString = window.location.search;

    // Create a URLSearchParams object
    var urlParams = new URLSearchParams(queryString);

    // Get the value of the 'id' query parameter
    var id = urlParams.get('id');
    paginationIndex = urlParams.get('paginationIndex');

    GetProfileDetails(id);

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true
    });


    initAutocomplete();
    onPlaceChanged();


})


function GetProfileDetails(id) {

    postRequest('/Dashboard/UserEdit/' + id, null, function (res) {

        debugger;
        if (res.status == 200) {

            if (res.data != null) {



                if (res.data.register != null) {

                    $("#firstname").val(res.data.register.firstname);
                    $("#lastname").val(res.data.register.lastname);
                    $("#email").val(res.data.register.email);
                    //$("#password").val(res.data.register.password);
                    $("#contactNo").val(res.data.register.contactNo);
                    $("#address").val(res.data.register.address);
                    $("#zoologicalNumber").val(res.data.register.zoologicalNumber);
                    $("#profileInfo").val(res.data.register.profileInfo);
                    $("#username").val(res.data.register.username);
                    $("#country").val(res.data.register.country);
                    $("#province").val(res.data.register.province);
                    $("#city").val(res.data.register.city);



                    $("#datepicker").datepicker('setDate', new Date(res.data.dateofBirth))


                    $("#FbProfile").val(res.data.register.faceBook)
                    $("#InProfile").val(res.data.register.insta)
                    $("#TwProfile").val(res.data.register.twitter)






                    var newprofilePicPath = res.data.register.profilePicPath.replace(/~/g, '');

                    $("#AppendProfilePic").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl + newprofilePicPath}" id="profilePicPath" class="img-fluid img-thumbnail" data-img-url="${res.data.register.profilePicPath}"/>
               </div>`);

                    var newbreederLicensePath = res.data.breederLicensePath.replace(/~/g, '');

                    $("#AppendBreederLicense").empty().append(`<div style="width:150px;height:150px;">
                  <img src="${baseApiUrl + newbreederLicensePath}" id="breederLicensePath" class="img-fluid img-thumbnail" data-img-url="${res.data.register.breederLicensePath}"/>
                </div>`);

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







$('#updateBtn').click(function () {



    // Get the query string from the current URL
    var queryString = window.location.search;

    // Create a URLSearchParams object
    var urlParams = new URLSearchParams(queryString);

    // Get the value of the 'id' query parameter
    var id = urlParams.get('id');

    var formData = new FormData();

    formData.append('UserId', id);
    formData.append('Firstname', $('#firstname').val());
    formData.append('Lastname', $('#lastname').val());
    formData.append('Email', $('#email').val());
    formData.append('Username', $('#username').val());

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

    FilePostRequest('/Dashboard/UpdateUserProfile', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                
                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                }).then(function () {
                    window.location.href = `/Dashboard/Users?paginationIndex=${paginationIndex}`;
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


function goBack() {
    
    window.location.href = `/Dashboard/Users?paginationIndex=${paginationIndex}`;
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