class MarkdownEditor {
    private readonly HeaderMarkdown = "#";
    private readonly BoldMarkdown = "__";
    private readonly ItalicMarkdown = "-";

    private readonly $markdown: JQuery;

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

    private get startOfLine(): number {
        let startOfLine = this.textarea.value.lastIndexOf("\n", this.selectStart);

        return startOfLine === -1 ? 0 : startOfLine;
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
        this.insertWrappingMarkdown(this.BoldMarkdown);
    }

    insertItalic() {
        this.insertWrappingMarkdown(this.ItalicMarkdown);
    }

    insertHeader() {
        let originalStart = this.selectStart;
        let originalEnd = this.selectEnd;

        this.selectStart = this.selectEnd = this.startOfLine;
        let textToInsert = this.HeaderMarkdown;
        let headerLevel = this.getCurrentHeaderLevel();

        if (headerLevel === 1) {
            textToInsert += " ";
        }

        if (headerLevel <= 6) {
            this.insertText(textToInsert);
        } else {
            // We need to select the header characaters.
            this.selectStart = this.startOfLine;
            this.selectEnd = this.startOfLine + headerLevel;

            originalStart -= headerLevel;
            originalEnd -= headerLevel;

            document.execCommand("delete", false);

            textToInsert = "";
        }

        this.selectStart = originalStart + textToInsert.length;
        this.selectEnd = originalEnd + textToInsert.length;
    }

    togglePreview() {
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

    insertRuler() {
        let originalStart = this.selectStart;
        let originalEnd = this.selectEnd;

        if (this.selectStart !== this.selectEnd) {
            this.selectStart = this.selectEnd
        }

        this.insertText("\n\n-----\n")

        this.selectStart = originalStart;
        this.selectEnd = originalEnd;
    }

    insertLink() {
        let start = this.selectStart;
        let end = this.selectEnd;

        if (start === end) {
            this.insertText("[link text](http://)");
            end += "link text".length + 1;
        } else {
            this.insertText(`[${this.textarea.value.substr(start, end - start)}](http://)`);
            end++;
        }

        this.selectStart = start + 1;
        this.selectEnd = end;
    }

    toggleHelpGuide() {
        $("#markdown-help").modal("toggle");
    }

    focus() {
        this.textarea.focus();
    }
    
    private onMarkdownToolbarButtonClick(e: JQueryEventObject) {
        let $target = $(e.target);
        if (!$target.is(".button")) {
            $target = $target.closest(".button");
        }

        let markdownAction = $target.data("markdown-action");

        this.focus();

        switch (markdownAction) {
            case "bold":
                this.insertBold();
                break;
            case "italic":
                this.insertItalic();
                break;
            case "header":
                this.insertHeader();
                break;
            case "ruler":
                this.insertRuler();
                break;
            case "link":
                this.insertLink();
                break;
            case "preview":
                this.togglePreview();
                break;
            case "guide":
                this.toggleHelpGuide();
                break;
        }
    }

    private insertWrappingMarkdown(markdown: string) {
        if (!this.textarea) {
            return;
        }

        let start = this.selectStart;
        let end = this.selectEnd;

        if (start === end) {
            this.insertText(markdown + markdown)
            start = end = start + markdown.length;
        } else {
            let textToWrap = this.textarea.value.substr(start, end - start);
            this.insertText(`${markdown}${textToWrap}${markdown}`);
            end = end + markdown.length * 2;
        }

        this.focus();
        this.selectStart = start;
        this.selectEnd = end;
    }

    private insertText(text: string) {
        document.execCommand("insertText", false, text);
    }

    private getCurrentHeaderLevel() {
        let headerLevel = 1;
        let position = this.selectStart;

        while (headerLevel <= 6 && position < this.textarea.value.length &&
            this.textarea.value[position] === this.HeaderMarkdown) {
            headerLevel++;
            position++;
        }

        return headerLevel;
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