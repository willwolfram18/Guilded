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

function convertMarkdown(e: JQueryEventObject) {
    const $markdownContainer = $(e.target).closest(".markdown-editor");
    const editorContent: string = $markdownContainer.find("textarea[name='Content']").val();
    const $previewSegment = $markdownContainer.find(".segment[data-tab='preview'] .container .segment")
    const $previewContent = $previewSegment.find(".content");
    const $previewLoader = $previewSegment.find(".ui.dimmer");

    $.ajax({
        method: "POST",
        url: $markdownContainer.data("markdown-action"),
        data: {
            content: editorContent
        },
        beforeSend: () => {
            $previewContent.addClass("hidden");
            $previewLoader.addClass("active");
        },
        success: (response) => {
            $previewContent.html(response).removeClass("hidden");
            $previewLoader.removeClass("active")
        }
    });
}

$(document).ready(() => {
    $("[data-toggle='modal']").each(setUpModalToggles);
    $(".ui.calendar").calendar();
    $(".ui.tabular.menu .item").tab();
    $(".ui.tabular.menu").on("click", ".item[data-tab='preview']", convertMarkdown);
});