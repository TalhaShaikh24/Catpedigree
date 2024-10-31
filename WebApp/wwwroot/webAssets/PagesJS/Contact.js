let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    $("#contactForm").validate({
        rules: {
            department: {
                required: true
            },
            name: {
                required: true,
                minlength: 2
            },
            lastname: {
                required: true,
                minlength: 2
            },
            phone: {
                required: true,
                minlength: 10,
                maxlength: 15
            },
            email: {
                required: true,
                email: true
            },
            subject: {
                required: true
            },
            message: {
                required: true,
                minlength: 10
            }
        },
        messages: {
            department: {
                required: "Please select department"
            },
            name: {
                required: "Please enter your first name",
                minlength: "Your first name must consist of at least 2 characters"
            },
            lastname: {
                required: "Please enter your last name",
                minlength: "Your last name must consist of at least 2 characters"
            },
            phone: {
                required: "Please enter your phone number",
                minlength: "Your phone number must be at least 10 digits",
                maxlength: "Your phone number must be less than 15 digits"
            },
            email: {
                required: "Please enter a valid email address",
                email: "Please enter a valid email address"
            },
            subject: {
                required: "Please enter the subject of your message"
            },
            message: {
                required: "Please enter your message",
                minlength: "Your message must be at least 10 characters long"
            }
        },
        errorElement: "div",
        errorPlacement: function (error, element) {
            error.addClass("help-block");
            element.parents(".form_group").append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).parent().addClass("has-error");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parent().removeClass("has-error");
        },
        submitHandler: function (form) {
            // Handle form submission here, e.g., send data via AJAX
            alert("Form successfully submitted!");
        }
    });


});




$(document).on('click', '#btnSubmitContact', function() {


    var obj = {
        Department: $("#department").val(),
        Firstname: $("#name").val(),
        Lastname: $("#lastname").val(),
        Phone: $("#phone").val(),
        Email: $("#email").val(),
        Subject: $("#subject").val(),
        Message: $("#message").val()
    }

    if ($("#contactForm").valid()) {
        postRequest('/Contact/AddContact', obj, function (res) {

            if (res.status == 200) {

                if (res.data != null) {
                    document.getElementById('contactForm').reset();
                    debugger

                    Swal.fire({
                        title: "Success",
                        text: res.responseMsg,
                        icon: "success"
                    })

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