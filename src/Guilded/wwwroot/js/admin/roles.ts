function cancelFormClick(e: JQueryEventObject) {
    const $elem = $(e.target);

    if ($elem.data("href") && confirm("Are you sure? All unsaved changes will be lost.")) {
        window.location.href = $elem.data("href");
    }
}

$(document).ready(function () {
    $("#roles-list").accordion({
        exclusive: false,
        selector: {
            trigger: ".title .name"
        }
    });
    $("form .ui.cancel.button").on("click", cancelFormClick);
})