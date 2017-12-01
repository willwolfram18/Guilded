function onMarkdownToolbarButtonClick(e: JQueryEventObject) {
    let $target = $(e.target);
    if (!$target.is(".button")) {
        $target = $target.closest(".button");
    }

    const $markdownEditor = $target.closest(".markdown-editor");
    const markdownAction = $target.data("markdown-action");
    const $markdown = $markdownEditor.find(".markdown");

    switch (markdownAction) {
        case "preview":
            togglePreview($markdownEditor, $markdown);
            break;
        case "guide":
            $("#markdown-help").modal("toggle");
            break;
    }
}

function togglePreview($markdownEditor: JQuery, $markdown: JQuery) {
    let $markdownPreview = $(".markdown-editor .markdown-preview");

    $markdown.toggle();
    $markdownPreview.toggle()
        .text("Loading...");

    $.ajax({
        method: "POST",
        url: $markdownEditor.data("markdown-action"),
        data: {
            content: $markdown.val()
        },
        error: () => $markdownPreview.html("An error occurred. Please try again."),
        success: (response) => $markdownPreview.html(response)
    });
}

$(document).ready(() => {
    $(".markdown-editor")
        .on("click", ".markdown-toolbar .button", onMarkdownToolbarButtonClick)

    $(".markdown-editor .markdown-preview").hide();
    //simpleMde = new SimpleMDE({
    //    autoDownloadFontAwesome: false,
    //    blockStyles: {
    //        bold: "__",
    //        italic: "_"
    //    },
    //    element: $(".markdown-editor #Content")[0],
    //    indentWithTabs: false,
    //    parsingConfig: {
    //        strikethrough: false
    //    },
    //    placeholder: "Content...",
    //    previewRender: convertMarkdown,
    //    tabSize: 4,
    //    toolbar: [
    //        "bold",
    //        "italic",
    //        "heading",
    //        "|",
    //        "quote",
    //        "unordered-list",
    //        "ordered-list",
    //        "horizontal-rule",
    //        "|",
    //        "link",
    //        "image",
    //        "|",
    //        "preview",
    //        "|",
    //        {
    //            name: "guide",
    //            action: () => $("#markdown-help").modal("toggle"),
    //            className: "fa fa-question-circle",
    //            title: "View markdown guide"
    //        }
    //    ]
    //});
});