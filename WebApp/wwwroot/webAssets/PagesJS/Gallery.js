
var WaterMarkURL = "";
$(document).ready(function () {
    $('.loader-gallery').show();
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

    postRequest("/Gallery/GetAllGallery", null, function (res) {

        if (res.status == 200) {
            $.each(res.data, function (index, imageData) {
                // Create the <a> tag with appropriate attributes
                const $a = $('<a></a>')
                    .attr('href', imageData.filePath)
                    .attr('data-fancybox', 'gallery')
                    .attr('data-caption', imageData.fileName);

                // Create the <img> tag
                const $img = $('<img />')
                    .attr('src', imageData.filePath)
                    .attr('alt', imageData.fileName);

                // Append the <img> tag to the <a> tag
                $a.append($img);

                // Create the grid item and append the <a> tag inside it
                const $gridItem = $('<div class="grid-item"></div>').append($a);

                // Append the grid item to the grid wrapper
                $("#grid-wrapper").append($gridItem);
            });

            // Initialize Masonry layout after images are loaded
            // Initialize Masonry layout after images are loaded
            var $grid = $('.grid').imagesLoaded(function () {
                $grid.masonry({
                    itemSelector: '.grid-item',
                    percentPosition: true,
                    columnWidth: '.grid-sizer'
                });

                // Hide the loader after a 3-second delay
                setTimeout(function () {
                    $('.loader-gallery').fadeOut(); // Use fadeOut for a smoother effect
                    $('.grid').css({
                        "visibility": "visible"
                    });
                }, 3000);
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



