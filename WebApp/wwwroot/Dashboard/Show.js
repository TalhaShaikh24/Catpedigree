var filesToUpload = [];
var FeatureImageToUpload = [];

let baseApiUrl = "";
$(document).ready(function () {


  
    $('#title').on('blur keyup', function () {
        const title = $(this).val().trim();
        if (title === '') {
            $('#titleError').show();
        } else {
            $('#titleError').hide();
        }
    });

    // File validation
    $('#featuredFile').on('change', function () {
        if (this.files.length === 0) {
            $('#fileError').show();
        } else {
            $('#fileError').hide();
        }
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

        fetch(`/Dashboard/UploadShowImage`, {  // Adjust the URL to match the API endpoint
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
    baseApiUrl = $("#baseApiUrl").val();

});


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
                // Set the value for the Select2 element
                $('#featureImage').attr("src", baseApiUrl + res.data.featureImagePath);
                $('#Gallery_Files').attr("src", baseApiUrl + res.data.gallaryImagePath);

                $('#FeaturedImageAppend').empty();
                $("#FeaturedImageAppend").append(`
                                  <div class="col-lg-4 mb-4">
                                     <div class="form_group file-input-one">

                                            <div class="img-thumbnail" style="width:50%!important">
                                            <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                            <img src="${baseApiUrl + res.data.featureImagePath}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                                <div style="position:absolute;top:0px;right:0px;">
                                                  <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;">X</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`);

                // Populate Summernote editor
                $('#summernote').summernote("code", res.data.content);

                var gallaryimages = res.data.gallaryImagePath.split(',');

                debugger;
                $('#GalleryFilesAppend').empty();
                for (var i = 0; i < gallaryimages.length; i++) {
                    let myFileID = "FID" + (1000 + Math.random() * 9000).toFixed(0);
                    debugger;
                    $("#GalleryFilesAppend").append(`
                                 <div class="col-lg-4 mb-4">
                             <div class="form_group file-input-one">
                                    <div class="img-thumbnail" style="width:50%!important">
                                    <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                             <img src="${baseApiUrl + gallaryimages[i]}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                        <div style="position:absolute;top:0px;right:0px;">
                                              <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${myFileID}" style="border-radius: 25px;">X</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`);

                }



            }

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

    debugger;
    let FeaturedFile = e.target.files[0];
    FeatureImageToUpload = FeaturedFile;

    $('#FeaturedImageAppend').empty();
    $("#FeaturedImageAppend").append(`
                                  <div class="col-lg-4 mb-4">
                                     <div class="form_group file-input-one">

                                            <div class="img-thumbnail" style="width:50%!important">
                                            <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                            <img src="${URL.createObjectURL(FeaturedFile)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                                <div style="position:absolute;top:0px;right:0px;">
                                                  <button class="btn btn-danger btn-xs Featured-remove-button" style="border-radius: 25px;">X</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`);

});


$(document).on('click', '.Featured-remove-button', function () {
    $("#FeaturedImageAppend").children().remove();
    $("#featuredFile").val(null);
});

$("#Gallery_Files").on('change', function (e) {



    for (let i = 0; i < e.target.files.length; i++) {
        let myFile = e.target.files[i];
        let myFileID = "FID" + (1000 + Math.random() * 9000).toFixed(0);
        if (filesToUpload.length < 6) {


            filesToUpload.push({
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
    debugger;
    for (let i = 0; i < filesToUpload.length; i++) {
        if (filesToUpload[i].FID === fidToRemove) {
            filesToUpload.splice(i, 1);
            break;
        }
    }
    GalleryView();
});

const GalleryView = () => {

    $('#GalleryFilesAppend').empty();
    for (let i = 0; i < filesToUpload.length; i++) {
        $("#GalleryFilesAppend").append(`
                                 <div class="col-lg-4 mb-4">
                             <div class="form_group file-input-one">
                                    <div class="img-thumbnail" style="width:50%!important">
                                    <div class="upload-title-icon d-flex align-items-center justify-content-center" style="position:relative;">
                                             <img src="${URL.createObjectURL(filesToUpload[i].file)}" alt="Image" style="width:200px;height:200px;" class="img-thumbnail">
                                        <div style="position:absolute;top:0px;right:0px;">
                                              <button class="btn btn-danger btn-xs gallery-remove-button" data-fid="${filesToUpload[i].FID}" style="border-radius: 25px;">X</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>`);
    }

};




$("#Btn_BlogSubmit").click(function () {

    const title = $('#title').val().trim();
    if (title === '') {
        $('#titleError').show();
        return;
    }
    const fileSelected = $("#featuredFile")[0].files.length > 0;


    debugger;

    if (!fileSelected) {
        $('#fileError').show();
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
        formData.append("FeatureImage", $("#featuredFile")[0].files[0]);

        for (let i = 0; i < filesToUpload.length; i++) {

            formData.append("GallaryImage", filesToUpload[i].file);
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


                window.location.href = "/Dashboard/ShowList"
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
