let baseApiUrl = "";
let pageNumber = 1;
const pageSize = 10;
let varTotalCount = 0;
let varCurrentCount = 0;

$(document).ready(function () {
    baseApiUrl = $("#baseApiUrl").val();
    (async function () {
        try {
            await GetAllBlogCategories();
            await GetAllDistinctTags();
        } catch (error) {
            console.error('Error:', error);
        }
    })();
    loadBlogs();
});

// Initialize baseApiUrl and load the first page of blogs



// Function to load blogs
async function loadBlogs(paramBlogCategoryId = "", paramKeywordFilter = "", tag = "") {

    let blogCategoryId = paramBlogCategoryId || null;
    let keywordFilter = paramKeywordFilter || null;

    let obj = {
        PageNumber: pageNumber,
        PageSize: pageSize,
        BlogCategoryId: blogCategoryId,
        Title: keywordFilter,
        Tags: tag
    };

    const response = await fetch('/Blog/GetAllBlogsPagination', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });

    const data = await response.json();

    if (data.data) {
        const { blogs, totalCount, currentCount } = data.data;
        varTotalCount = totalCount;
        varCurrentCount = currentCount;

        $.each(blogs, function (index, blog) {
            $("#appendBlogs").append(`<div class="blog-standard-wrapper pb-50">

                <div class="blog-post-item blog-post-item-four mb-50 wow fadeInUp">
                    <div class="post-thumbnail">
                        <a href="/Blog/BlogDetails?Id=${blog.blogID}"><img class="w-100" src="${baseApiUrl + blog.featureImagePath}" alt="Blog Image"></a>
                    </div>
                    <div class="entry-content">
                        <a href="javascript:void(0);" class="cat-btn">${moment(blog.createdOn).format("DD-MMMM-YYYY")}</a>
                        <div class="post-meta">
                            <ul>
                                <li><span><i class="ti-bookmark-alt"></i><a href="javascript:void(0);">${blog.blogCategoryName}</a></span></li>
                                <li><span><i class="ti-comments-smiley"></i><a href="javascript:void(0);">${blog.commentsCount} Comment</a></span></li>
                                <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">By admin</a></span></li>
                            </ul>
                        </div>
                        <h3 class="title"><a href="/Blog/BlogDetails?Id=${blog.blogID}">${blog.title}</a></h3>
                        <p>${blog.shortDescription ? blog.shortDescription : ''}</p>
                        <a href="/Blog/BlogDetails?Id=${blog.blogID}" class="btn-link">Continue Reading</a>
                    </div>
                </div>

            </div>`);
        });

        pageNumber++;
       //await GetAllBlogCategories();
        updateCountDisplay();


        if (varCurrentCount >= varTotalCount) {
            $('#load-more').hide();
        } else {
            $('#load-more').show();
        }
    } else {
        $('#appendBlogs').append('<h4>No blogs found.</h4>');
        $('#load-more').hide();
    }
}

// Function to handle filtering
async function filteringSearch(paramBlogCategoryId = "", paramKeywordFilter = "") {
    pageNumber = 1;
    $('#appendBlogs').empty(); // Clear previous blogs
    loadBlogs(); // Load filtered blogs
}

// Function to update count display
function updateCountDisplay() {
    $('#count-display').text(`${varCurrentCount}/${varTotalCount}`);
}


$("#formKeywordFilter").submit(function (event) {
    event.preventDefault();  // Prevents the form from submitting
    
    pageNumber = 1;
    $('#appendBlogs').empty(); // Clear previous blogs

    loadBlogs("", $(this).find("#keywordFilter").val(), "");
    
});

// Event listener for category filter

$(document).on('change', '#appendBlogCategories', function (event) {
    
    pageNumber = 1;
    $('#appendBlogs').empty(); // Clear previous blogs
    loadBlogs($(this).val(), $(this).find("#keywordFilter").val());
    
})

// Handle tag selection event
$('#tagSelect').on('change', function () {
    let selectedTags = $(this).val(); // Get selected tags
    pageNumber = 1;
    $('#appendBlogs').empty(); // Clear previous blogs
    loadBlogs("", "", selectedTags.join(','));
});




// Event listener for load more button
$('#load-more').click(function () {
    loadBlogs();
});

async function  GetAllBlogCategories() {
    postRequest('/Blog/GetAllBlogCategoriesAndLatestBlog', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                // Empty the select element
                $("#appendBlogCategories").empty();

                // Add the placeholder option
                $("#appendBlogCategories").append(`
                    <option value="" disabled selected>Select a category</option>
                `);

                // Append each category option
                $.each(res.data.blogCategories, function (i, v) {
                    $("#appendBlogCategories").append(`
                        <option value="${v.categoryId}">${v.categoryName} <span>(${v.blogsCount})</span></option>
                    `);
                });

                // Update the niceSelect plugin
                $("#appendBlogCategories").niceSelect('update');

                $.each(res.data.latestblog, function (i, v) {
               
                    $("#ulrecenposts").append(`

                             <li class="mt-2">
                             <a href="/Blog/BlogDetails?Id=${v.blogID}" class="blog_links"> <i class="fa-solid fa-file mr-1"></i>   ${v.title}</a>
                            </li>
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


function GetAllDistinctTags() {

    postRequest('/Blog/GetAllDistinctTags', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {


                let tags = res.data.map(tag => `<option value="${tag.tags}">${tag.tags}</option>`);
                $("#tagSelect").append(tags);


                $("#tagSelect").niceSelect('update');
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