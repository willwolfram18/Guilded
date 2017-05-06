function showSidebar() {
    $(".sidebar").sidebar("show");
}

function documentReady() {
    $(".sidebar-toggle").on("click", showSidebar);
}

$(document).ready(documentReady);