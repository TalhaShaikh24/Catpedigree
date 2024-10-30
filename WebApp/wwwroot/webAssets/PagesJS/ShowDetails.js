
let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();

    var urlParams = new URLSearchParams(window.location.search);

    var blogId = urlParams.get("Id");

    if (blogId) {
        ShowDetails(blogId);

    }

});


function ShowDetails(Id) {

    postRequest('/Show/GetAllShowDetails/' + Id, null, function (res) {

        if (res.status == 200) {
            if (res.data != null) {
                debugger;
                $("#BlogDetails_Append").append(`
                    
                    <div class="post-thumbnail mb-90">
                        <img class="w-100" src="${baseApiUrl + res.data.featureImagePath}" alt="Blog Image">
                         <h1 class="my-0" style="font-weight:900">${res.data.title}</h1>
                    </div>
                   
                    <div class="entry-content">
                        <div class="post-meta d-none">
                            <ul>
                                <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">${res.data.username}</a></span></li>
                                <li><span><i class="ti-calendar"></i><a href="javascript:void(0);">${moment(res.data.createdOn).format("DD MMMM - YYYY")}</a></span></li>
                            </ul>
                        </div>
                        ${res.data.content}
                    </div>
                `);

                // Split the comma-separated string into an array
                const galleryImages = res.data.gallaryImagePath.split(',');

                if (galleryImages.length>0) {
                    // Clear previous gallery items
                    $("#showMoments").show();
                    $("#Gallery").empty();

                    // Iterate through each image path
                    $.each(galleryImages, function (index, imagePath) {
                        const trimmedPath = imagePath.trim(); // Trim any extra spaces

                        // Create the <a> tag with appropriate attributes
                        const $a = $('<a></a>')
                            .attr('href', baseApiUrl + trimmedPath)
                            .attr('data-fancybox', 'gallery')
                            .attr('data-caption', `Image ${index + 1}`);

                        // Create the <img> tag
                        const $img = $('<img />')
                            .attr('src', baseApiUrl + trimmedPath)
                            .attr('alt', `Gallery Image ${index + 1}`);

                        // Append the <img> tag to the <a> tag
                        $a.append($img);

                        // Create the grid item and append the <a> tag inside it
                        const $gridItem = $('<div class="grid-item"></div>').append($a);

                        // Append the grid item to the gallery
                        $("#grid-wrapper").append($gridItem);
                    });

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

                

                $("#HDBLOGID").val(res.data.showId);
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




function postRequest(url, requestData, handledata) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        dataType: "json",
        url: url,
        data: JSON.stringify(requestData),
        success: function (data, textStatus, xhr) {

            handledata(data);
        },
        error: function (xhr, textStatus, errorThrown) {
            Swal.fire({
                title: "Error",
                text: "Something Went Wrong!",
                icon: "error",
                dangerMode: true,
            })
        }
    });
}