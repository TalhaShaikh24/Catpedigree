
$("#btn_forgotPassword").click(function () {
    $(".preloader").show()

    let data = {
        Email: $("#email").val(),
    }

    postRequest('/Account/ForgotPassword', data, function (res) {
        switch (res.status) {
            case 200:
                handleSuccess(res);
                break;
            case 304:
            case 305:
            case 401:
            case 320:
            case 500:
                $(".preloader").hide()
                handleError(res.responseMsg, "error");
                break;
            case 403:
                $(".preloader").hide()
                handleError(res.responseMsg, "error", "Error");
                break;
            case 600:
                $(".preloader").hide()
                handleError(res.responseMsg, "warning", "Warning");
                break;
            default:
                $(".preloader").hide()
                handleError("Unexpected error occurred", "error");
                break;
        }
    });


});




$("#btn_resetPassword").click(function () {
    $(".preloader").show()

    let data = {
        Email: $("#email").val(),
        Password: $("#newPassword").val(),
        VerificationCode: $("#verificationCode").val()
    }

    postRequest('/Account/ResetPassword', data, function (res) {
        $(".preloader").hide()
        switch (res.status) {
            case 200:
                Swal.fire({
                    title: "Success!",
                    text: res.responseMsg,
                    icon: "success"
                }).then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        redirectToHome();
                    }
                });

                break;
            case 304:
            case 305:
            case 401:
            case 320:
            case 500:
                $(".preloader").hide()
                handleError(res.responseMsg, "error");
                break;
            case 403:
                $(".preloader").hide()
                handleError(res.responseMsg, "error", "Error");
                break;
            case 600:
                $(".preloader").hide()
                handleError(res.responseMsg, "warning", "Warning");
                break;
            default:
                $(".preloader").hide()
                handleError("Unexpected error occurred", "error");
                break;
        }
    });
});



function setupPasswordToggle(toggleBtn, passwordField) {
    debugger;
    toggleBtn.addEventListener('click', function () {
        const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordField.setAttribute('type', type);
        this.classList.toggle('fa-eye');
        this.classList.toggle('fa-eye-slash');
    });
}

// Setup for password field 1
const togglePassword1 = document.querySelector('#togglePassword1');
const password1 = document.querySelector('#verificationCode');
setupPasswordToggle(togglePassword1, password1);

// Setup for password field 2
const togglePassword2 = document.querySelector('#togglePassword2');
const password2 = document.querySelector('#newPassword');
setupPasswordToggle(togglePassword2, password2);

// Setup for password field 3
const togglePassword3 = document.querySelector('#togglePassword3');
const password3 = document.querySelector('#confirmPassword');
setupPasswordToggle(togglePassword3, password3);




function handleSuccess(res) {
    $(".preloader").hide()
    if (res.data != null) {
        $("#forgotPasswordDiv").hide();
        $("#resetPasswordDiv").show();
        Swal.fire({
            title: "Success!",
            text: res.responseMsg,
            icon: "success"
        });
    }
}

function handleError(message, icon, title = "Error") {
    Swal.fire({
        title: title,
        text: message,
        icon: icon
    });
}

function redirectToHome() {
    window.location.href = window.location.origin;
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
            $(".preloader").hide()
            Swal.fire({
                title: "Error",
                text: "Something Went Wrong!",
                icon: "error",
                dangerMode: true,
            })
        }
    });
}