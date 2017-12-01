function insertText(text: string, textToInsert: string, insertAt: number): string {
    return `${text.substr(0, insertAt)}${textToInsert}${text.substr(insertAt)}`;
}

function onMarkdownToolbarButtonClick(e: JQueryEventObject) {
    let $target = $(e.target);
    if (!$target.is(".button")) {
        $target = $target.closest(".button");
    }

    const $markdownEditor = $target.closest(".markdown-editor");
    const markdownAction = $target.data("markdown-action");
    const $markdown = $markdownEditor.find(".markdown");

    switch (markdownAction) {
        case "bold":
            insertBold($markdownEditor, $markdown);
            break;
        case "preview":
            togglePreview($markdownEditor, $markdown);
            break;
        case "guide":
            $("#markdown-help").modal("toggle");
            break;
    }
}

function insertBold($markdownEditor: JQuery, $markdown: JQuery) {
    let textarea: HTMLTextAreaElement = $markdown[0] as HTMLTextAreaElement;

    if (!textarea) {
        return;
    }

    let start = textarea.selectionStart;
    let end = textarea.selectionEnd;

    if (start === end) {
        document.execCommand("insertText", false, "____")
    } else {
        let textToWrap = textarea.value.substr(start, end - start);
        document.execCommand("insertText", false, `__${textToWrap}__`)
    }
    
    textarea.focus();
    textarea.selectionStart = textarea.selectionEnd = start + 2;
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