$(document).ready(function () {

    const cookieBox = $(".wrapperCookies");

    const executeCodes = () => {
        // Check if the consent cookie is not set
        if (!getCookie("CookieConsent")) {
            cookieBox.addClass("show");
        }

        $("#acceptBtn").click(function () {
            $.ajax({
                url: "/cookieconsent/SetConsent", // Check this URL matches your controller route
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ consent: true }),
                success: function (response) {
                    console.log('Consent set successfully:', response);
                    cookieBox.removeClass("show"); // Optionally hide the consent box on success
                    setCookie("CookieConsent", "true", 365); // Set the cookie on client side as well
                },
                error: function (xhr, textStatus, errorThrown) {
                    console.error('Error setting consent:', errorThrown);
                }
            });
        });

        function setCookie(name, value, days) {
            var expires = "";
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                expires = "; expires=" + date.toUTCString();
            }
            document.cookie = name + "=" + (value || "") + expires + "; path=/";
        }

        function getCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) === ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
            }
            return null;
        }
    };

    $(window).on("load", executeCodes); // Ensure executeCodes runs on page load
});
