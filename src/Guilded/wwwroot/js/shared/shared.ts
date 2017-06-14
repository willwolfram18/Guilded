function setUpModalToggles(index: number, elem: Element) {
    const $toggle = $(elem);

    const targetModalSelector = $toggle.data("target");
    if (!targetModalSelector) {
        return;
    }

    $toggle.on("click",
        () => {
            $(targetModalSelector).modal("show");
        });
}

function hideErrorAndSuccessMessages() {
    $(".ui.error.message, .ui.success.message").addClass("hidden");
}

function showSuccessMessage(message: string): void {
    $(".ui.success.message").removeClass("hidden").html(message);
}

function showErrorMessage(message: string): void {
    $(".ui.error.message").removeClass("hidden").html(message);
}

$(document).ready(() => {
    $("[data-toggle='modal']").each(setUpModalToggles);
    $(".ui.calendar").calendar();
    $(".ui.dropdown").dropdown();
    $(".ui.tabular.menu .item").tab();
});