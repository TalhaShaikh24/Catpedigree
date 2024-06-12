var filesToUpload = [];

let baseApiUrl = "";
$(document).ready(function () {

    baseApiUrl = $("#baseApiUrl").val();
    getAll()
})



function getAll() {
    postRequest('/Dashboard/UserAdvertisementPackages', null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {


                $("#AdvertisementPackages").empty();

                if (res.data.dropdown.length > 0) {
                    $.each(res.data.dropdown, function (i, v) {
                        $("#AdvertisementPackages").append(`<option value="${v.userAdvertisementPackageID}">${v.advertisementPackageName}</option>`);
                    });
                }
                else {
                    $("#AdvertisementPackages").append(`<option value="-1" disabled>You have no  Package</option>`);
                }

                $("#AppendApprovalListing").empty();
                $.each(res.data.userAdvertisementPackages, function (i, v) {
                    debugger;
                    $("#AppendApprovalListing").append(`
                                                <tr>

                                                   <td>${v.userAdvertisementPackageID}</td>
                                                   <td>${v.advertisementPackageName}</td>
                                                   <td>${v.advertisementPackageCost}</td>
                                                   <td>${v.advertisementPackageType}</td>
                                                   <td>${v.numberOfAdvertisement}</td>
                                                   <td>${v.status}</td>
                                                 
                                                   
                                                   <td>${v.createdOn}</td>
                                                   <td> <img src="${baseApiUrl + v.filePath}" style="

    height: 51px;
    width: 51px;
" />   </td>
                                                   

                                                </tr>`);

                });

                $('#TableApprovalListing').DataTable();
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




$("#save").click(function () {


    let formData = new FormData();

    if ($("#file")[0].files[0] == undefined) {
        Swal.fire({
            title: "Error",
            text:"Select any file",
            icon: "error"
        })

        return;

    }

    formData.append("AddFile", $("#file")[0].files[0]);
    formData.append("UserAdvertisementPackageID", $('#AdvertisementPackages').val());

    FilePostRequest('/Dashboard/UtilizePurchasedAdvertisementPackage', formData, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Congrats",
                    text: res.responseMsg,
                    icon: "success"
                });

                $(document).find("input").val(null);
                getAll();
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

})




