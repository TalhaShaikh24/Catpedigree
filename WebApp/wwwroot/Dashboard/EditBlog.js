let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    $('#blogTags').select2({
        tags: true,
        tokenSeparators: [',', ' ']
    });

    $('#summernote').summernote({
        height:650
    });

    var urlParams = new URLSearchParams(window.location.search);

    GetAllBlogCategories();

    var Id = urlParams.get("Id");

    if (Id) {

      EditBlogById(Id);

    }
})

function GetAllBlogCategories() {
    postRequest('/Dashboard/GetAllBlogCategories', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                $("#AppendCategories").empty();
                $.each(res.data, function (i, v) {

                    debugger
                    $("#blogCategory").append(`
                      <option value="${v.id}">${v.categoryName}</option>
                      `);

                });

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

function EditBlogById(Id) {
    postRequest('/Dashboard/BlogEditById?Id='+Id, null, function (res) {

        if (res.status == 200) {

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
                $('#blogCategory').val(res.data.blogCategoryId); // Assuming this is the category field
                $('#featureImage').attr("src", baseApiUrl + res.data.featureImagePath);

                // Populate Summernote editor
                $('#summernote').summernote("code", res.data.content);
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


$("#Btn_BlogUpdate").click(function () {


    let formData = new FormData(); 

    formData.append("BlogID", $("#HDID").val());
    formData.append("Title", $("#title").val());
    formData.append("ShortDescription", $("#shortdescription").val());
    formData.append("BlogCategoryId", $("#blogCategory").val());
    formData.append("Tags", String($("#blogTags").val()));
    formData.append("Content", $('#summernote').summernote("code"));
    formData.append("FeatureImage", $("#featuredFile")[0].files[0]);

    FilePostRequest('/Dashboard/UpdateBlog',formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

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


});

