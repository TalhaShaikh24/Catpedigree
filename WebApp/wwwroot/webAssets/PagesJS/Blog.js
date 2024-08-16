let baseApiUrl = "";
let pageNumber = 1;
const pageSize = 10;
let varTotalCount = 0;
let varCurrentCount = 0;

//$(document).ready(function () {
//    loadBlogs();
//});

// Initialize baseApiUrl and load the first page of blogs
document.addEventListener('DOMContentLoaded', () => {
    baseApiUrl = $("#baseApiUrl").val();
    
    loadBlogs();
});


// Function to load blogs
async function loadBlogs(paramBlogCategoryId = "", paramKeywordFilter = "") {

    let blogCategoryId = paramBlogCategoryId || null;
    let keywordFilter = paramKeywordFilter || null;

    let obj = {
        PageNumber: pageNumber,
        PageSize: pageSize,
        BlogCategoryId: blogCategoryId,
        Title: keywordFilter
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
                        <a href="javascript:void(0);" class="cat-btn">${moment(blog.createdOn).format("DD MMMM - YYYY")}</a>
                        <div class="post-meta">
                            <ul>
                                <li><span><i class="ti-bookmark-alt"></i><a href="javascript:void(0);">${blog.blogCategoryName}</a></span></li>
                                <li><span><i class="ti-comments-smiley"></i><a href="javascript:void(0);">${blog.commentsCount} Comment</a></span></li>
                                <li><span><i class="ti-id-badge"></i><a href="javascript:void(0);">By admin</a></span></li>
                            </ul>
                        </div>
                        <h3 class="title"><a href="/Blog/BlogDetails?Id=${blog.blogID}">${blog.title}</a></h3>
                        <p>${blog.shortDescription}</p>
                        <a href="/Blog/BlogDetails?Id=${blog.blogID}" class="btn-link">Continue Reading</a>
                    </div>
                </div>

            </div>`);
        });

        pageNumber++;
       await GetAllBlogCategories();
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

    loadBlogs("", $(this).find("#keywordFilter").val());
    
});

// Event listener for category filter

$(document).on('click', '.categoryId', function (event) {
    event.preventDefault();
    pageNumber = 1;
    $('#appendBlogs').empty(); // Clear previous blogs
    loadBlogs($(this).data('id'), $(this).find("#keywordFilter").val());
    
})

// Event listener for load more button
$('#load-more').click(function () {
    loadBlogs();
});

async function  GetAllBlogCategories() {
    postRequest('/Blog/GetAllBlogCategories', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {
                $("#appendBlogCategories").empty();
                $.each(res.data, function (i, v) {

                    $("#appendBlogCategories").append(`
                      <li><a href="#" class="categoryId" data-id="${v.categoryId}">${v.categoryName} <span>(${v.blogsCount})</span></a></li>
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