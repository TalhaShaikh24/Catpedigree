
var WaterMarkURL = "";
$(document).ready(function () {

    //$(".gallery").magnificPopup({
    //    delegate: "a",
    //    type: "image",
    //    tLoading: "Loading image #%curr%...",
    //    mainClass: "mfp-img-mobile",
    //    gallery: {
    //        enabled: true,
    //        navigateByImgClick: true,
    //        preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
    //    },
    //    image: {
    //        tError: '<a href="%url%">The image #%curr%</a> could not be loaded.'
    //    }
    //});

    GetAllGallery();

});



function GetAllGallery() {

    postRequest("/Listing/GetAllGallary", null, function (res) {

        if (res.status == 200) {

            $(".grid").html("");

            $.each(res.data, function (index, imageData) {

                const $div = $('<div></div>');

                    // Example of applying classes dynamically based on the index
                    if (index % 5 === 0) {
                        $div.addClass('wide');
                    } else if (index % 3 === 0) {
                        $div.addClass('tall');
                    } else if (index % 7 === 0) {
                        $div.addClass('big');
                    }
                const $a = $('<a></a>')
                    .attr('href', imageData.filePath)
                    .attr('data-fancybox', 'gallery')
                    .attr('data-caption', imageData.fileName);

                // Create the first image element
                const $img1 = $('<img>', {
                    class: 'img-watermark',
                    src: 'https://localhost:7297/webassets/images/logo/logo-1.png',
                    alt: 'f46a1_Cattery Monti Della Meta (14).png'
                });

                // Create the second image element (adjust attributes as needed)
                const $img2 = $('<img>', {
                    src: imageData.filePath,  // Assuming imageData.filePath is defined
                    alt: imageData.fileName
                });

                // Append the first image to the link
                $a.append($img1);
                $a.append($img2);

                // Append the link to the div
                $div.append($a);

                // Append the div to the grid-wrapper
                $('#grid-wrapper').append($div);

                

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



