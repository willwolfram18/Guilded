function showSidebar() {
    $(".sidebar").sidebar("show");
}

function documentReady() {
    $(".ui.dropdown").dropdown();
    $(".sidebar").sidebar();

    $(".sidebar-toggle").on("click", showSidebar);
}

$(document).ready(documentReady);