
var WaterMarkURL = "";
$(document).ready(function () {

    $(".gallery").magnificPopup({
        delegate: "a",
        type: "image",
        tLoading: "Loading image #%curr%...",
        mainClass: "mfp-img-mobile",
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">The image #%curr%</a> could not be loaded.'
        }
    });

    GetAllGallery();

});



function GetAllGallery() {

    postRequest("/Dashboard/GetAllGallary", null, function (res) {

        if (res.status == 200) {

            $(".gallery").html("");

            $.each(res.data, function (i, v) {

                $(".gallery").append(`<div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                                       
                                                         <a href="${v.filePath}">
                                                        <figure class="position-relative">
                                                             <img class="img-fluid img-thumbnail" src="${v.filePath}" alt="${v.fileName}">
                                                            
                                                     </figure>
                                                      
                                                    </a>
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


function loadImage(src) {
    return new Promise(function (resolve, reject) {
        var img = new Image();
        img.crossOrigin = "anonymous";
        img.onload = function () {
            resolve(img);
        };
        img.onerror = function () {
            reject(new Error('Failed to load image: ' + src));
        };
        img.src = src;
    });
}



