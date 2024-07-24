
$(document).ready(function () {

   
    GetAllGellary();

});




function GetAllGellary() {

    postRequest("/Dashboard/GetAllVideosGallery", null, function (res) {

        if (res.status == 200) {

            $(".gallery").html("");

            $.each(res.data, function (i, v) {

                $(".gallery").append(`<div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                                        <div class="col-lg-3 col-md-4 col-xs-6 thumb">
            <video width="320" height="240" controls>
                <source src="${v.filePath}" type="video/mp4">
                Your browser does not support the video tag.
            </video>
                                                </div>`);

            });

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

