
$(document).ready(function () {

   
    GetAllGellary();

});




function GetAllGellary() {

    postRequest("/Dashboard/GetAllVideosGallery", null, function (res) {

        if (res.status == 200) {

            $(".gallery").html("");

            $.each(res.data, function (i, v) {

                $(".gallery").append(`<div class="col-lg-3 col-md-4 col-xs-6 thumb">

                                                        <div class="CheckBoxSelection shadow-sm">
                                                           <input type="checkbox" value="${v.fileName}"/>
                                                        </div>
            <video width="320" height="240" controls>
                <source src="${v.filePath}" type="video/mp4">
                Your browser does not support the video tag.
            </video>
                                                </div>`);

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

$("#masterCheckbox").change(function () {
    if ($(this).is(":checked")) {
        $(".CheckBoxSelection input[type='checkbox']").prop("checked", true);
    } else {
        $(".CheckBoxSelection input[type='checkbox']").prop("checked", false);
    }
});


$("#btn-deleteGallery").click(function () {
    debugger;
    var checkedValues = [];
    $(".CheckBoxSelection input[type='checkbox']:checked").each(function () {
        checkedValues.push($(this).val());
    });

    if (checkedValues.length > 0) {
        postRequest("/Dashboard/DeleteSelectedVideoGalleryPath/" + checkedValues.join(","), null, function (res) {

            if (res.status == 200) {


                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                });

                GetAllGellary();

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
    else {

        Swal.fire({
            title: "Warning",
            text: "Please Select Gallery Images",
            icon: "warning"
        })

    }

});
