
$(document).ready(function () {

    var selectedCurrency = localStorage.getItem('cur')
    debugger;
    SinglePriceListing(selectedCurrency);


})

$(".btnChoosePkg").click(function () {
    var packageId = $(this).data("id");

    Swal.fire({
        title: 'Are you sure?',
        text: 'This action will detuct 1 listing form the package. Continue?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, select it!'
    }).then((result) => {
        if (result.isConfirmed) {
            postRequest('/Listing/SelectPackageListingShowValidation/' + packageId, null, function (res) {

                if (res.status == 200) {


                    window.location.reload();


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
    });
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


function initMap() {
    var latitude = parseFloat($("#lat").val());  // Latitude
    var longitude = parseFloat($("#long").val());  // Longitude

    // The location of the place
    var location = { lat: latitude, lng: longitude };

    // Create a map centered on the location
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 13,
        center: location
    });

    // Add a marker with a custom label
    var marker = new google.maps.Marker({
        position: location,
        map: map,
        //label: 'NYC'  // Custom label
    });
}