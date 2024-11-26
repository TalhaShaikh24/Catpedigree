let baseApiUrl = "";
var GalleryFilesUpload = [];
var FeaturedFileUpload = null;
$(document).ready(function () {
    ShowPreloader();
    baseApiUrl = $("#baseApiUrl").val();

    $('#blogTags').select2({
        tags: true,
        tokenSeparators: [',', ' ']
    });

    $('#summernote').summernote({
        height: 650,
        fontSizes: ['8', '9', '10', '11', '12', '13', '14', '15', '16', '18', '20', '22', '24', '28', '32', '36', '40', '48', '50', '52', '54', '56', '58', '60'],
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'clear']],
            //['fontname', ['fontname']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['table', ['table']],
            ['insert', ['link', 'picture', 'hr']],
            ['view', ['fullscreen', 'codeview']],
            ['help', ['help']]
        ],
        callbacks: {
            onImageUpload: function (files) {
                uploadImage(files[0]);
            }
        }
    });

    function uploadImage(file) {
        let formData = new FormData();
        formData.append('file', file);

        fetch(`/Dashboard/UploadBlogImage`, {  // Adjust the URL to match the API endpoint
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                let imageUrl = data.data;  // Assuming your server responds with the image URL
                $('#summernote').summernote('insertImage', imageUrl);
            })
            .catch(error => {
                console.error('Error uploading image:', error);
            });
    }
    var urlParams = new URLSearchParams(window.location.search);
    var Id = urlParams.get("Id");
    getById(Id);



    $('#title').on('blur keyup', function () {
        const title = $(this).val().trim();
        if (title === '') {
            $('#titleError').show();
        } else {
            $('#titleError').hide();
        }
    });



})


function getById(id) {
    debugger;
    postRequest('/Dashboard/GetShowbyID?Id=' + parseInt(id), null, function (res) {
        ShowPreloader();
        if (res.status == 200) {
            HidePreloader();
            if (res.data != null) {
                // Populate Select2 with tags

                // Populate other fields
                $('#ShowId').val(res.data.showId);
                $('#title').val(res.data.title);

                // Populate Summernote editor
                $('#summernote').summernote("code", res.data.content);



                if (res.data.featureImagePath) {

                    const filePaths = res.data.featureImagePath.split(",");
                   
                    const promises = filePaths.map((filePath) => {
                        const Path = baseApiUrl + filePath.replace(/\\/g, "/");
                        debugger;
                        return new Promise((resolve, reject) => {
                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {

                                    const fileName = filePath.split("\\").pop(); // Get the file name

                                    FeaturedFileUpload = new File([blob], fileName, { type: blob.type });

                                    resolve(FeaturedFileUpload);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                    });

                    Promise.all(promises)
                        .then((files) => {

                            $("#featuredFile").val(null).change();
                        })
                        .catch((error) => {
                            console.log("One or more AJAX requests failed:", error);
                        });
                }


                if (res.data.gallaryImagePath) {



                    const galleryPaths = res.data.gallaryImagePath.split(",");

                    const galleryPromises = galleryPaths.map((galleryPath) => {

                        const Path = baseApiUrl + galleryPath.replace(/\\/g, "/");

                        return new Promise((resolve, reject) => {
                            $.ajax({
                                url: Path,
                                type: "GET",
                                xhrFields: {
                                    responseType: "blob"
                                },
                                success: function (blob) {
                                    const myFileID = "FID" + (1000 + Math.floor(Math.random() * 9000));
                                    const fileName = galleryPath.split("\\").pop(); // Get the file name
                                    const file = new File([blob], fileName, { type: blob.type });

                                    // Push the file and related info into the global array
                                    GalleryFilesUpload.push({
                                        file: file,
                                        size: file.size,
                                        FID: myFileID,
                                        name: file.name
                                    });

                                    resolve(file);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    console.error("Error fetching image:", errorThrown);
                                    reject(errorThrown);
                                }
                            });
                        });
                    });

                    Promise.all(galleryPromises)
                        .then((files) => {



                            $("#Gallery_Files").val(null).change();


                        })
                        .catch((error) => {
                            console.error("One or more AJAX requests failed:", error);
                        });
                }



            }

            console.log(GalleryFilesUpload);

        }
        if (res.status == 304) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 305) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 401) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 403) {
            HidePreloader();
            Swal.fire(res.responseMsg, {
                icon: "error",
                title: "Error"
            });
        }
        if (res.status == 320) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 500) {
            HidePreloader();
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: "error"
            })
        }
        if (res.status == 600) {
            HidePreloader();
            Swal.fire({
                title: "Warning",
                text: res.responseMsg,
                icon: "warning"
            })

        }
    });

}



$("#featuredFile").on('change', function (e) {

    if (e.target.files[0] != undefined) {
        FeaturedFileUpload = e.target.files[0];
    }

    $('#FeaturedImageAppend').empty();
    $("#FeaturedImageAppend").append(`
                                         <div class="col-lg-4 mb-4">
                                         <div class="form_group file-input-one">
                                                <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                                                <img src="${URL.createObjectURL(FeaturedFileUpload)}" alt="Image" style="width: 200px; height: 200px;" class="img-thumbnail">
                                                    <div style="position:absolute;top:5px;right:5px;">
                                                      <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                                                  
                                                </div>
                                            </div>
                                        </div>
                                    </div>`);

});



$(document).on('click', '.Featured-remove-button', function () {
    $("#FeaturedImageAppend").children().remove();
    $("#FeaturedFile").val(null);
});


$("#Gallery_Files").on('change', function (e) {

    for (let i = 0; i < e.target.files.length; i++) {

        let myFile = e.target.files[i];
        let myFileID = "FID" + (1000 + Math.random() * 9000).toFixed(0);
        if (GalleryFilesUpload.length < 6) {

            GalleryFilesUpload.push({
                file: myFile,
                size: myFile.size,
                FID: myFileID,
                name: myFile.name
            });
        }
        else {
            Swal.fire({
                title: "Error",
                text: "Only 6 Files Can be Uploaded",
                icon: "error"
            })
        }

    }

    GalleryView();

    e.target.value = null;
});

$(document).on('click', '.gallery-remove-button', function () {
    var fidToRemove = $(this).data('fid');
    for (let i = 0; i < GalleryFilesUpload.length; i++) {
        if (GalleryFilesUpload[i].FID === fidToRemove) {
            GalleryFilesUpload.splice(i, 1);
            break;
        }
    }
    GalleryView();
});


const GalleryView = () => {

    $('#GalleryFilesAppend').empty();

    for (let i = 0; i < GalleryFilesUpload.length; i++) {

        $("#GalleryFilesAppend").append(`
                                     <div class="col-lg-4 mb-4">
                                 <div class="form_group file-input-one">
                                       
                                        <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                                 <img src="${URL.createObjectURL(GalleryFilesUpload[i].file)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                            <div style="position:absolute;top:5px;right:5px;">
                                             <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${GalleryFilesUpload[i].FID}"style="border-radius: 25px;padding-left: 10px;padding-right: 10px;padding-bottom: 5px;padding-top: 5px;">X</button>
                                           
                                        </div>
                                    </div>
                                </div>
                            </div>`);
    }

};
$("#Btn_BlogSubmit").click(function () {
    debugger;
    const title = $('#title').val().trim();
    if (title === '') {
        $('#titleError').show();
        return;
    }
    // Show spinner and disable button
    $("#BtnSpinner").removeClass("d-none");
    $("#BtnText").text("Submitting...");
    $(this).prop("disabled", true);

    let formData = new FormData();

    formData.append("ShowId", $("#ShowId").val());
    formData.append("Title", $("#title").val());

    formData.append("Content", $('#summernote').summernote("code"));
    formData.append("FeatureImage", FeaturedFileUpload);


    for (let i = 0; i < GalleryFilesUpload.length; i++) {

        formData.append("GallaryImage", GalleryFilesUpload[i].file);
    }

    FilePostRequest('/Dashboard/AddShow', formData, function (res) {
        // Hide spinner and enable button
        $("#BtnSpinner").addClass("d-none");
        $("#BtnText").text("Submit");
        $("#Btn_BlogSubmit").prop("disabled", false);

        if (res.status == 200) {

            if (res.data != null) {
                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });


            }

            window.location.href="/Dashboard/ShowList"

        } else {

            let errorMessage = res.responseMsg || "An error occurred.";
            let icon = "error";
            let title = "Error";

            if (res.status == 600) {
                icon = "warning";
                title = "Warning";
            }

            Swal.fire({
                title: title,
                text: errorMessage,
                icon: icon
            });
        }
    }).fail(function () {

        // In case of failure, hide spinner and enable button
        $("#BtnSpinner").addClass("d-none");
        $("#BtnText").text("Submit");
        $("#Btn_BlogSubmit").prop("disabled", false);

        Swal.fire({
            title: "Error",
            text: "An unexpected error occurred.",
            icon: "error"
        });
    });
});

