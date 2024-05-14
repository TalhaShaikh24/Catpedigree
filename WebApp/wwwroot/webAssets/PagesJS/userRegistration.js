 $(document).ready(function () {
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
                // Handle success response
                console.log(response);
            },
            error: function (xhr, status, error) {
                // Handle error
                console.error(xhr.responseText);
            }
        });
    });
 });