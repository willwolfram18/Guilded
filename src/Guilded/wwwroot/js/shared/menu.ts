function showSidebar() {
    $(".sidebar").sidebar("show");
}

function documentReady() {
    $(".ui.dropdown").dropdown();
    $(".sidebar").sidebar();
    $(".ui.accordion").accordion();

    $(".sidebar-toggle").on("click", showSidebar);
}

$(document).ready(documentReady);