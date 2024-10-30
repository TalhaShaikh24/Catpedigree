let baseApiUrl = "";
let pageNumber = 1;
const pageSize = 10;
let varTotalCount = 0;
let varCurrentCount = 0;

$(document).ready(function () {
    baseApiUrl = $("#baseApiUrl").val();

    loadBlogs();
});

// Initialize baseApiUrl and load the first page of blogs



// Function to load blogs
async function loadBlogs() {
    let obj = {
        PageNumber: pageNumber,
        PageSize: pageSize,
    };

    const response = await fetch('/Show/GetAllShowsPagination', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });

    const data = await response.json();

    if (data.data) {
        const { shows, totalCount, currentCount } = data.data;
        varTotalCount = totalCount;
        varCurrentCount = currentCount;

        // Create a new row for each two blogs
        for (let i = 0; i < shows.length; i += 2) {
            for (let j = i; j < i + 2 && j < shows.length; j++) {
                const blog = shows[j];
                $("#appendBlogs").append(`
                    <div class="col-lg-6 mb-30">
                        <div class="blog-standard-wrapper pb-50">
                            <div class="blog-post-item blog-post-item-four mb-50 wow fadeInUp">
                                <div class="post-thumbnail">
                                    <a href="/Show/ShowDetails?Id=${blog.showId}"><img class="w-100" src="${baseApiUrl + blog.featureImagePath}" alt="Show Image"></a>
                                </div>
                                <div class="entry-content">
                                    <a href="javascript:void(0);" class="cat-btn">${moment(blog.createdOn).format("DD-MMMM-YYYY")}</a>
                                    <h3 class="title"><a href="/Show/ShowDetails?Id=${blog.showId}">${blog.title}</a></h3>
                                    <a href="/Show/ShowDetails?Id=${blog.showId}" class="btn-link">Continue Reading</a>
                                </div>
                            </div>
                        </div>
                    </div>`);
            }
        }

        pageNumber++;
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




// Function to update count display
function updateCountDisplay() {
    $('#count-display').text(`${varCurrentCount}/${varTotalCount}`);
}






// Event listener for load more button
$('#load-more').click(function () {
    loadBlogs();
});



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