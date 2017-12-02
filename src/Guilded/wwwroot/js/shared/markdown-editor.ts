class MarkdownEditor {
    private $markdown: JQuery;

    constructor(private $markdownEditor: JQuery) {
        this.$markdown = $markdownEditor.find(".markdown");
        this.$markdownEditor.find(".markdown-preview").hide();

        this.$markdownEditor
            .on("click", ".markdown-toolbar .button", (e) => {
                this.onMarkdownToolbarButtonClick(e);
            });
    }

    private get textarea(): HTMLTextAreaElement {
        return this.$markdown[0] as HTMLTextAreaElement;
    }

    private get selectStart(): number {
        return this.textarea.selectionStart;
    }
    private set selectStart(position: number) {
        this.textarea.selectionStart = position;
    }

    private get selectEnd(): number {
        return this.textarea.selectionEnd;
    }
    private set selectEnd(position: number) {
        this.textarea.selectionEnd = position;
    }

    insertBold() {
        this.insertWrappingMarkdown("__");
    }

    insertItalic() {
        this.insertWrappingMarkdown("_");
    }

    togglePreview() {
        console.log("preview toggle")
        let $markdownPreview = this.$markdownEditor.find(".markdown-preview");

        this.$markdown.toggle();
        $markdownPreview.toggle()
            .text("Loading...");
        
        $.ajax({
            method: "POST",
            url: this.$markdownEditor.data("markdown-action"),
            data: {
                content: this.$markdown.val()
            },
            error: () => $markdownPreview.html("An error occurred. Please try again."),
            success: (response) => $markdownPreview.html(response)
        });
    }

    toggleHelpGuide() {
        $("#markdown-help").modal("toggle");
    }

    private insertWrappingMarkdown(markdown: string) {
        if (!this.textarea) {
            return;
        }

        let start = this.selectStart;
        let end = this.selectEnd;

        if (start === end) {
            document.execCommand("insertText", false, markdown + markdown)
            start = end = start + markdown.length;
        } else {
            let textToWrap = this.textarea.value.substr(start, end - start);
            document.execCommand("insertText", false, `${markdown}${textToWrap}${markdown}`)
            end = end + markdown.length * 2;
        }

        this.textarea.focus();
        this.selectStart = start;
        this.selectEnd = end;
    }

    private onMarkdownToolbarButtonClick(e: JQueryEventObject) {
        let $target = $(e.target);
        if (!$target.is(".button")) {
            $target = $target.closest(".button");
        }

        let markdownAction = $target.data("markdown-action");

        this.textarea.focus();

        switch (markdownAction) {
            case "bold":
                this.insertBold();
                break;
            case "italic":
                this.insertItalic();
                break;
            case "preview":
                this.togglePreview();
                break;
            case "guide":
                this.toggleHelpGuide();
                break;
        }
    }
}


$(document).ready(() => {
    $(".markdown-editor").each((i, elem) => {
        new MarkdownEditor($(elem))
    });
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