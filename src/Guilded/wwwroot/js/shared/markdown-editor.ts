let simpleMde: SimpleMDE;

function convertMarkdown(content: string, previewElement?: HTMLElement): string {
    const $markdownContainer = $(".markdown-editor");

    $.ajax({
        method: "POST",
        url: $markdownContainer.data("markdown-action"),
        data: {
            content: content
        },
        error: () => {
            previewElement.innerHTML = "An error occurred. Please try again."
        },
        success: (response) => {
            previewElement.innerHTML = response;
        }
    });

    return "Loading...";
}

$(document).ready(() => {

    simpleMde = new SimpleMDE({
        autoDownloadFontAwesome: false,
        element: $(".markdown-editor #Content")[0],
        indentWithTabs: false,
        parsingConfig: {
            strikethrough: false
        },
        placeholder: "Content...",
        previewRender: convertMarkdown,
        tabSize: 4,
        toolbar: [
            "bold",
            "italic",
            "heading",
            "|",
            "quote",
            "unordered-list",
            "ordered-list",
            "horizontal-rule",
            "|",
            "link",
            "image",
            "|",
            "preview",
            "|",
            "guide"
        ]
    });
});