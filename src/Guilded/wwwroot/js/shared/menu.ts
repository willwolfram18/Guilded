function showSidebar() {
    $(".sidebar").sidebar("show");
}

function documentReady() {
    $(".ui.dropdown").dropdown();
    $(".sidebar-toggle").on("click", showSidebar);
}

$(document).ready(documentReady);