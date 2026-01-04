
var datatable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDatatable("approved");
    }
    else if (url.includes("readyforpickup")) {
        loadDatatable("readyforpickup");
    }
    else if (url.includes("cancelled")) {
        loadDatatable("cancelled");
    }
    else {
        loadDatatable("all");
    }
});
function loadDatatable(status) {
    datatable = $("#tblData").DataTable({
        "ajax": {
            "url": `/order/getall?status=${status}`
        },
        columns: [

            { data: "orderHeaderId", width: "7%" },
            { data: "email", width: "18%" },
            { data: "name", width: "20%" },
            { data: "phone", width: "15%" },
            { data: "status", width: "15%" },
            {
                data: "orderTotal", width: "15%", className: "text-end",
                render: function (data) {
                    // format as currency if needed
                    return typeof data === "number" ? data.toFixed(2) : data;
                }
            },
            {
                data: "orderHeaderId",width: "10%",
                render: function (data) {
                    return `<div class="btn-group" role="group"><a href="/Order/OrderDetail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a></div>`
                }
            }

        ]
    });
}