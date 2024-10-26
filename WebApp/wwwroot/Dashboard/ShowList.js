let baseApiUrl = "";
$(document).ready(function () {
    const baseApiUrl = $("#baseApiUrl").val();


    GetAllAdminShow();
});




function GetAllAdminShow() {
    debugger;
    $("#TableShows").DataTable().destroy();
    $("#TableShows").DataTable({
        "responsive": true,
        "lengthChange": true,
        "processing": true, // for show progress bar
        "serverSide": false, // for process server side
        "searching": true, // Enable searching (filter)
        "orderMulti": false, // Disable multiple column ordering
        "pageLength": 10,
        "orderClasses": false,
        "language": {
            "search": "Search:",
            "processing": "Processing...",
            "lengthMenu": "Display _MENU_ records",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries"
        },
        "ajax": {
            "url": "/Dashboard/GetAllAdminShow",
            "type": "POST",
            "dataType": "json",
            "dataSrc": function (data) {
                if (data.status === 200) {
                    return data.data;
                }

                let title, icon;
                switch (data.status) {
                    case 304:
                    case 305:
                    case 401:
                    case 403:
                    case 320:
                    case 500:
                        title = "Error";
                        icon = "error";
                        break;
                    case 600:
                        title = "Warning";
                        icon = "warning";
                        break;
                    default:
                        title = "Error";
                        icon = "error";
                        break;
                }

                Swal.fire({
                    title: title,
                    text: data.responseMsg,
                    icon: icon
                });

                return []; // Return an empty array if there is an error
            }
        },
        "columns": [
            {
                "data": "title",
                "name": "title",
                "autoWidth": true
            },
          
            {
                "data": "username",
                "name": "username",
                "autoWidth": true
            },
            {
                "data": "createdOn",
                "name": "createdOn",
                "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return `<span>${moment(full.createdOn).format("DD - MMMM - YYYY")}</span>`;
                }
            },
            {
                "data": "showId",
                "name": "showId",
                "autoWidth": true,
                "render": function (data, type, row) {
            
                    // Build the HTML string with conditional rendering
                    return `
                    <div >
              

                        
                        <a class="btn btn-info btn-md" title="Edit" href="/Dashboard/EditShow?Id=${data}">
                            <i class="fa fa-edit"></i>
                        </a>

                        <button type="button" class="btn btn-danger btn-md" title="Delete" onclick="BlogDeleteById(${data})">
                            <i class="fa fa-trash"></i>
                        </button>
                    </div>`;
                }

            }
        ],
        "order": [[2, 'desc']]
    });
}

function BlogDeleteById(Id) {
    postRequest('/Dashboard/ShowDelete?Id=' + Id, null, function (res) {

        if (res.status == 200) {

            if (res.data != null) {

                Swal.fire({
                    title: "Success",
                    text: res.responseMsg,
                    icon: "success"
                })

                GetAllAdminShow();


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


