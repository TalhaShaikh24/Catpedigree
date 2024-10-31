
var WaterMarkURL = "";
var watermarkImageSrc = "/webassets/images/watermarks/watermark-3.png";
$(document).ready(function () {


    $('#WaterMarkModal').on('hidden.bs.modal', function () {
        $('#position').prop('selectedIndex', 0); // Reset to the first option
    });

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
 

    GetAllGellary();

});

$("#UploadWaterMark").change(function () {

    var file = document.getElementById('UploadWaterMark').files[0];

    WaterMarkURL = window.URL.createObjectURL(file);

    var img = new Image();
    img.src = WaterMarkURL;
   // img.crossOrigin = "Anonymous";

    img.onload = function () {
        var width = img.naturalWidth;
        var height = img.naturalHeight;
        $('#watermarkWidth').val(width);
    };
});



$('#addWatermark, #UploadWaterMark, #position, #offsetX, #offsetY, #watermarkWidth, #watermarkOpacity').on('change keyup', function () {
    var mainImageSrc = $("#mainImage").attr("value");
    //var watermarkImageSrc = WaterMarkURL;
    var watermarkImageSrc = "/webassets/images/watermarks/watermark-3.png";

    Promise.all([
        loadImage(mainImageSrc),
        loadImageWatermark(watermarkImageSrc)
    ]).then(function (images) {
        debugger;
        var mainImage = images[0];
        var watermarkImage = images[1];

        
    //    mainImage.crossOrigin = "Anonymous";

        var canvas = document.createElement('canvas');
      
        var ctx = canvas.getContext('2d');

        canvas.width = mainImage.width;
        canvas.height = mainImage.height;

        ctx.clearRect(0, 0, canvas.width, canvas.height);

        ctx.drawImage(mainImage, 0, 0);
       // ctx.crossOrigin = true;
         
        var watermarkWidth = parseInt($('#watermarkWidth').val()) || watermarkImage.width;
        var aspectRatio = watermarkImage.width / watermarkImage.height;
        var watermarkHeight = watermarkWidth / aspectRatio;
        var watermarkOpacity = parseFloat($('#watermarkOpacity').val());
        var offsetX = parseInt($('#offsetX').val()) || 0;
        var offsetY = parseInt($('#offsetY').val()) || 0;
        var position = $('#position').val();

        if (watermarkOpacity < 0 || watermarkOpacity > 1 || isNaN(watermarkOpacity)) {
            alert("Opacity must be a number between 0 and 1");
            return;
        }

        var watermarkX, watermarkY;

        switch (position) {
            case 'top-left':
                watermarkX = offsetX;
                watermarkY = offsetY;
                break;
            case 'top-center':
                watermarkX = (canvas.width - watermarkWidth) / 2;
                watermarkY = offsetY;
                break;
            case 'top-right':
                watermarkX = canvas.width - watermarkWidth - offsetX;
                watermarkY = offsetY;
                break;
            case 'center-left':
                watermarkX = offsetX;
                watermarkY = (canvas.height - watermarkHeight) / 2;
                break;
            case 'center':
                watermarkX = (canvas.width - watermarkWidth) / 2;
                watermarkY = (canvas.height - watermarkHeight) / 2;
                break;
            case 'center-right':
                watermarkX = canvas.width - watermarkWidth - offsetX;
                watermarkY = (canvas.height - watermarkHeight) / 2;
                break;
            case 'bottom-left':
                watermarkX = offsetX;
                watermarkY = canvas.height - watermarkHeight - offsetY;
                break;
            case 'bottom-center':
                watermarkX = (canvas.width - watermarkWidth) / 2;
                watermarkY = canvas.height - watermarkHeight - offsetY;
                break;
            case 'bottom-right':
                watermarkX = canvas.width - watermarkWidth - offsetX;
                watermarkY = canvas.height - watermarkHeight - offsetY;
                break;
            default:
                watermarkX = offsetX;
                watermarkY = offsetY;
                break;
        }

        ctx.globalAlpha = watermarkOpacity;
        ctx.drawImage(watermarkImage, watermarkX, watermarkY, watermarkWidth, watermarkHeight);

        var resultImageSrc = canvas.toDataURL('image/jpeg', 0.7);
        $("#mainImage").attr("src", resultImageSrc);
    }).catch(function (error) {
        console.error("Error adding watermark: ", error);
    });
});


function GetAllGellary() {

    postRequest("/Dashboard/GetAllGallary", null, function (res) {

        if (res.status == 200) {

            $(".gallery").html("");

            $.each(res.data, function (i, v) {

                $(".gallery").append(`<div class="col-lg-3 col-md-4 col-xs-6 thumb">
                                                        <div class="CheckBoxSelection shadow-sm">
                                                           <input type="checkbox" value="${v.fileName}"/>
                                                        </div>
                                                         <a href="${v.filePath}">
                                                        <figure class="position-relative">
                                                             <img class="img-fluid img-thumbnail" src="${v.filePath}" alt="${v.fileName}">
                                                            
                                                     </figure>
                                                      
                                                    </a>
                                                    <div class="Watermarkbutton">
                                                                <button type="button" class="btn btn-info btn-sm" id="ShowModalWatermark" data-filename="${v.fileName}" data-imageurl="${v.filePath}">Add Water Mark</button>
                                                        </div>
                                                </div>`);

            });
            $('.thumb').hover(
                function () {
                    $(this).find('img').css({
                        '-webkit-filter': 'grayscale(0)',
                        'filter': 'grayscale(0)'
                    });
                },

            );
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


$('#replaceImage').on('click', function () {

    debugger;

    var base64String = $("#mainImage").attr("src");

    var matches = base64String.match(/^data:image\/(png|jpg|jpeg);base64,(.*)$/);
    var mimeType = matches[1];
    var base64Data = matches[2];

    var filename = $("#mainImage").attr("alt");

    var byteCharacters = atob(base64Data);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var blob = new Blob([byteArray], { type: 'image/' + mimeType });

    var formData = new FormData();
    formData.append('file', blob, filename);

    FilePostRequest(`/Dashboard/replaceFileGallery`, formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                $("#WaterMarkModal").modal("hide");
                GetAllGellary();
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
});


$(document).on("click", "#ShowModalWatermark", function () {

    $('#UploadWaterMark').val('');


    var ImageUrl = $(this).attr("data-imageurl");

    var filename = $(this).attr("data-filename");

    $("#WaterMarkModal").modal("show");

    $("#apppendImage").empty().append(`
                        <a href="javascript:void(0)">
                            <figure>
                             <img id="mainImage" class="img-fluid " src="${ImageUrl}" value="${ImageUrl}" alt="${filename}">
                            </figure>
                         </a>`);
    $("#UploadWaterMark").change();

})


function loadImageWatermark(src) {
    debugger;
    return new Promise(function (resolve, reject) {
        const img = new Image();
      
        img.onload = function () {
            resolve(img);
        };
        img.onerror = function (error) {
            reject(new Error('Failed to load image: ' + src + " " + error));
        };



        img.src = src;


    });
}

function loadImage(src) {
    
    return new Promise(function (resolve, reject) {
        const img = new Image();
        img.crossOrigin = 'anonymous';
     
        img.onload = function () {
            resolve(img);
        };
        img.onerror = function (error) {
            reject(new Error('Failed to load image: ' + src + " " + error));
        };


        let oldUrl = src;
        let newUrl = oldUrl.replace(/\/UploadImages\//, '/api/images/').replace(/v=\d+/, 'v=638540547168663316');
      
     
        img.src = newUrl;


    });
}



$('#UploadNewFile').on('change', function () {
    var files = document.getElementById('UploadNewFile').files;




    var maxSize = 30 * 1024 * 1024; // 30 MB in bytes

    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        if (file.size > maxSize) {

            Swal.fire({
                title: "Warning",
                text: "The file size is too large. Maximum allowed size is " + maxSize + " MB.",
                icon: "warning",
                showCancelButton: false,
                confirmButtonColor: "#3085d6",
                allowOutsideClick: false,
                allowEscapeKey: true,
            });
            e.target.value = null;

            break;
            return;
        }
    }


    var formData = new FormData();

    for (var i = 0; i < files.length; i++) {
        formData.append('files', files[i]); // Use 'files[]' to handle multiple files on the server side
    }


    debugger;
    FilePostRequest(`/Dashboard/UploadNewGalleryOnly`, formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });
                GetAllGellary();
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
});



$("#masterCheckbox").change(function () {
    if ($(this).is(":checked")) {
        $(".CheckBoxSelection input[type='checkbox']").prop("checked", true);
    } else {
        $(".CheckBoxSelection input[type='checkbox']").prop("checked", false);
    }
});


$("#btn-saveGallery").click(function () {

    debugger
    var checkedValues = [];
    $(".CheckBoxSelection input[type='checkbox']:checked").each(function () {
        checkedValues.push($(this).val());
    });

    if (checkedValues.length > 0) {
        postRequest("/Dashboard/UploadSelectedGalleryPath/" + checkedValues.join(", "), null, function (res) {

            if (res.status == 200) {


                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
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
    else {

        Swal.fire({
            title: "Warning",
            text: "Please Select Gallery Images",
            icon: "warning"
        })

    }

});

$("#btn-deleteGallery").click(function () {
    debugger;
    var checkedValues = [];
    $(".CheckBoxSelection input[type='checkbox']:checked").each(function () {
        checkedValues.push($(this).val());
    });

    if (checkedValues.length > 0) {
        postRequest("/Dashboard/DeleteSelectedGalleryPath/" + checkedValues.join(","), null, function (res) {

            if (res.status == 200) {


                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                GetAllGellary();

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
    else {

        Swal.fire({
            title: "Warning",
            text: "Please Select Gallery Images",
            icon: "warning"
        })

    }

});
