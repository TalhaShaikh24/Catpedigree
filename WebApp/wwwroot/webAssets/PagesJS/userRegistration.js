let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    $('#registerBtn').click(function () {
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
            Swal.fire({
                title: "Congrats!",
                text: response.responseMsg,
                icon: "success"
            }).then(() => {
                redirectToHome();
            });
        }

        function handleError(xhr) {
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