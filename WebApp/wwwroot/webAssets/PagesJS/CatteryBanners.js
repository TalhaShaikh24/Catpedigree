let baseApiUrl = "";
$(document).ready(function () {
    baseApiUrl = $("#baseApiUrl").val();
    (async function () {
        try {
            await GetBannerAdvertisments();
        } catch (error) {
            console.error('Error:', error);
        }
    })();
})

function GetBannerAdvertisments() {
    postRequest('/Advertisement/GetBannerAdvertisments', null, function (res) {
        if (res.status == 200) {
            if (res.data != null && res.data.length > 0) {
                // Assuming you want to append to the .row div
                var rowContainer = $('#appendBanners');

                // Clear existing banners if needed
                rowContainer.empty();

                $.each(res.data, function (index, item) {
                    var html = `
                        <div class="col-md-6 mb-4">
                            <div class="banner-item">
                                <img src="${baseApiUrl.value == undefined ? baseApiUrl + item.paidAdvertisments : baseApiUrl.value + item.paidAdvertisments}" alt="Banner ${index + 1}">
                                <div class="banner-overlay">Banner ${index + 1}</div>
                            </div>
                        </div>
                    `;
                    rowContainer.append(html);
                });
            }
        } else {
            Swal.fire({
                title: "Error",
                text: res.responseMsg,
                icon: res.status == 600 ? "warning" : "error"
            });
        }
    });
}

