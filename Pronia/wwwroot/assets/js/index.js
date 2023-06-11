$(document).on("click", ".modal-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (response.ok) {
                return response.text()
            }
            else {
                alert("Xeta bas verdi")
            }
        })
        .then(data => {
            $("#quickModal .modal-dialog").html(data)
            $("#quickModal").modal('show');
        })
})
$(document).on("click", ".addtobasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("xeta bas verdi");
            }
            return response.text();
        })
        .then(data => {
            $(".offcanvas-minicart_wrapper").html(data)
        })
})
$(document).on("click", ".removebasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("xeta bas verdi");
            }
            return response.text();
        })
        .then(data => {
            $(".offcanvas-minicart_wrapper").html(data)
        })
})