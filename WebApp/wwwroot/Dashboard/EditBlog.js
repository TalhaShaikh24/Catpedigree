let baseApiUrl = "";
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
    (async function () {
        try {
            await GetAllBlogCategories();
            await EditBlogById(Id);
            
            
        } catch (error) {
            console.error('Error:', error);
        }
    })();

    

    

    
})

async function GetAllBlogCategories() {
    postRequest('/Dashboard/GetAllBlogCategories', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

              
                $.each(res.data, function (i, v) {

                    $("#blogCategory").append(`
                      <option value="${v.categoryId}">${v.categoryName}</option>
                      `);

                });

            }
            $('#blogCategory').select2({
                placeholder: "Select Category"
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

async function EditBlogById(Id) {
    postRequest('/Dashboard/BlogEditById?Id='+Id, null, function (res) {
        ShowPreloader();
        if (res.status == 200) {
            HidePreloader();
            if (res.data != null) {
                // Populate Select2 with tags
                if (res.data.tags) {
                    var tagsArray = res.data.tags.split(',').map(function (tag) {
                        return tag.trim();
                    });

                    var $select = $('#blogTags'); // Assuming your select element has id="blogTags"
                    $.each(tagsArray, function (index, tag) {
                        var option = new Option(tag, tag, true, true);
                        $select.append(option).trigger('change');
                    });
                }

                // Populate other fields
                $('#HDID').val(res.data.blogID);
                $('#title').val(res.data.title);
                $('#shortdescription').val(res.data.shortDescription);
                // Set the value for the Select2 element
                $('#blogCategory').val(res.data.blogCategoryId).trigger('change'); // Assuming this is the category field
                $('#featureImage').attr("src", baseApiUrl + res.data.featureImagePath);

                // Populate Summernote editor
                $('#summernote').summernote("code", res.data.content);

                // Set the value for the Select2 element
                $('#blogCategory').val(res.data.blogCategoryId).trigger('change');
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


$("#Btn_BlogUpdate").click(function () {
  
    // Show spinner and disable button
    $("#BtnSpinner").removeClass("d-none");
    $("#BtnText").text("Submitting...");
    $(this).prop("disabled", true);

    let formData = new FormData();

    formData.append("BlogID", $("#HDID").val());
    formData.append("Title", $("#title").val());
    formData.append("ShortDescription", $("#shortdescription").val());
    formData.append("BlogCategoryId", $("#blogCategory").val());
    formData.append("Tags", String($("#blogTags").val()));
    formData.append("Content", $('#summernote').summernote("code"));
    formData.append("FeatureImage", $("#featuredFile")[0].files[0]);

    FilePostRequest('/Dashboard/UpdateBlog', formData, function (res) {
        // Hide spinner and enable button
        $("#BtnSpinner").addClass("d-none");
        $("#BtnText").text("Submit");
        $("#Btn_BlogUpdate").prop("disabled", false);

        if (res.status == 200) {
          
            if (res.data != null) {
                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });
            }
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
        $("#Btn_BlogUpdate").prop("disabled", false);

        Swal.fire({
            title: "Error",
            text: "An unexpected error occurred.",
            icon: "error"
        });
    });
});


