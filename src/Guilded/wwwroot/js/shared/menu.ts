function showSidebar() {
    $(".sidebar").sidebar("show");
}

function documentReady() {
    $(".main.menu .ui.dropdown").dropdown({
        action: "nothing"
    });
    $(".sidebar").sidebar();
    $(".ui.accordion").accordion();

    $(".sidebar-toggle").on("click", showSidebar);
}

$(document).ready(documentReady);