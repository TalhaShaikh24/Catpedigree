let baseApiUrl = "";
$(document).ready(function () {
    baseApiUrl = $("#baseApiUrl").val();
    (async function () {
        try {
            await GetAllUsefulLinksForGuest();
        } catch (error) {
            console.error('Error:', error);
        }
    })();
})

function GetAllUsefulLinksForGuest() {
    postRequest('/UsefulLinks/GetAllUsefulLinksForGuest', null, function (res) {
        if (res.status == 200) {
            if (res.data != null && res.data.length > 0) {
                const usefulLinksContainer = document.getElementById('useful-links');
                usefulLinksContainer.innerHTML = ''; // Clear existing content

                res.data.forEach(link => {
                    const col = document.createElement('div');
                    col.className = 'col-4'; // Bootstrap column for three in a row
                    col.innerHTML = `
                        <a href="${baseApiUrl+link.usefulLinkFilePath}" data-fancybox="gallery">
                            <img src="${baseApiUrl +link.usefulLinkFilePath}" class="img-fluid" alt="Useful Link">
                        </a>
                    `;
                    usefulLinksContainer.appendChild(col);
                });

                // Initialize Fancybox after adding new elements
                $.fancybox.bind();
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

