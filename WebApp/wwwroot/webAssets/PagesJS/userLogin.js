$("#Btn_Authentication").click(function () {


    let data = {
        Email: $("#usernameEmail").val(),
        Password: $("#password").val(),
    }

    postRequest('/Account/Authenticate', data, function (res) {
        switch (res.status) {
            case 200:
                handleSuccess(res);
                break;
            case 304:
            case 305:
            case 401:
            case 320:
            case 500:
                handleError(res.responseMsg, "error");
                break;
            case 403:
                handleError(res.responseMsg, "error", "Error");
                break;
            case 600:
                handleError(res.responseMsg, "warning", "Warning");
                break;
            default:
                handleError("Unexpected error occurred", "error");
                break;
        }
    });

    function handleSuccess(res) {
        if (res.data != null) {
            localStorage.setItem("username", res.data.dataObj.username);
            localStorage.setItem("profilePic", res.data.dataObj.profilePic);
            localStorage.setItem("role", res.data.dataObj.roleIds);
            localStorage.setItem("authToken", res.token);

            Swal.fire({
                title: "Login Successful!",
                text: res.responseMsg,
                icon: "success"
            }).then(() => {
                redirectToHome();
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