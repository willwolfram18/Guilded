class MarkdownEditor {
    private readonly headerMarkdown = "#";
    private readonly boldMarkdown = "__";
    private readonly italicMarkdown = "-";
    private readonly unorderedListMarkdown = "* ";
    private readonly quoteMarkdown = "> ";

    private readonly $markdown: JQuery;
    private readonly markdownActionFunctions: {[action: string]: Function } = {
        "bold": this.insertBold,
        "italic": this.insertItalic,
        "header": this.insertHeader,
        "quote": this.insertQuote,
        "ul": this.insertUnorderedList,
        "ruler": this.insertRuler,
        "link": this.insertLink,
        "image": this.insertImage,
        "preview": this.togglePreview,
        "guide": this.toggleHelpGuide
    };

    constructor(private $markdownEditor: JQuery) {
        this.$markdown = $markdownEditor.find(".markdown");
        this.$markdownEditor.find(".markdown-preview").hide();

        this.$markdownEditor
            .on("click", ".markdown-toolbar .button", (e) => {
                this.onMarkdownToolbarButtonClick(e);
            })
            .on("keypress", (e) => {
                this.onMarkdownKeyPress(e);
            });
    }

    private get textarea(): HTMLTextAreaElement {
        return this.$markdown[0] as HTMLTextAreaElement;
    }

    private get markdownText(): string {
        return this.textarea.value;
    }

    private get startOfLine(): number {
        let startOfLine = this.markdownText.lastIndexOf("\n", this.selectStart);

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
        this.insertWrappingMarkdown(this.boldMarkdown);
    }

    insertItalic() {
        this.insertWrappingMarkdown(this.italicMarkdown);
    }

    insertHeader() {
        let originalStart = this.selectStart;
        let originalEnd = this.selectEnd;

        this.selectStart = this.selectEnd = this.startOfLine;
        let textToInsert = this.headerMarkdown;
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

            this.deleteText();

            textToInsert = "";
        }

        this.selectStart = originalStart + textToInsert.length;
        this.selectEnd = originalEnd + textToInsert.length;
    }

    insertQuote() {
        this.toggleSubstringAtStartOfLine(this.quoteMarkdown);
    }

    insertUnorderedList() {
        this.toggleSubstringAtStartOfLine(this.unorderedListMarkdown);
    }

    insertRuler() {
        let originalStart = this.selectStart;
        let originalEnd = this.selectEnd;

        if (this.selectStart !== this.selectEnd) {
            this.selectStart = this.selectEnd;
        }

        this.insertText("\n\n-----\n");

        this.selectStart = originalStart;
        this.selectEnd = originalEnd;
    }

    insertLink() {
        this.insertLinkMarkdown(false);
    }

    insertImage() {
        this.insertLinkMarkdown(true);
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
        this.markdownActionFunctions[markdownAction].bind(this)();
    }

    private onMarkdownKeyPress(e: JQueryEventObject) {
        const enterKeyCode = 13;

        if (e.keyCode !== enterKeyCode || !this.markdownText.length) {
            return;
        }

        if (this.markdownText.substr(this.startOfLine, this.unorderedListMarkdown.length)) {
            
        }
    }

    private insertWrappingMarkdown(markdown: string) {
        if (!this.textarea) {
            return;
        }

        let start = this.selectStart;
        let end = this.selectEnd;

        if (start === end) {
            this.insertText(markdown + markdown);
            start = end = start + markdown.length;
        } else {
            let textToWrap = this.markdownText.substr(start, end - start);
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

    private deleteText() {
        document.execCommand("delete", false);
    }

    private insertLinkMarkdown(isImage: boolean) {
        let start = this.selectStart;
        let end = this.selectEnd;

        if (start === end) {
            let linkText = !isImage ? "link text" : "image description";

            this.insertText(`${isImage ? "!" : ""}[${linkText}](http://)`);

            end += linkText.length + 1;
        } else {
            this.insertText(`${isImage ? "!" : ""}[${this.markdownText.substr(start, end - start)}](http://)`);
            end++;
        }

        if (isImage) {
            start++;
            end++;
        }

        this.selectStart = start + 1;
        this.selectEnd = end;
    }

    private getCurrentHeaderLevel() {
        let headerLevel = 1;
        let position = this.selectStart;

        while (headerLevel <= 6 && position < this.markdownText.length &&
            this.markdownText[position] === this.headerMarkdown) {
            headerLevel++;
            position++;
        }

        return headerLevel;
    }

    private toggleSubstringAtStartOfLine(text: string) {
        if (!this.markdownText.length) {
            this.insertText(text);
            return;
        }

        let start = this.selectStart;
        let end = this.selectEnd;
        let startOfLineCursorPosition = this.startOfLine + (this.startOfLine === 0 ? 0 : 1);

        if (this.markdownText.substr(startOfLineCursorPosition, text.length) === text) {
            this.selectStart = startOfLineCursorPosition;
            this.selectEnd = startOfLineCursorPosition + text.length;
            this.deleteText();

            start -= text.length;
            end -= text.length;
        } else {
            this.selectStart = this.selectEnd = startOfLineCursorPosition;
            this.insertText(text);

            start += text.length;
            end += text.length;
        }

        this.selectStart = start;
        this.selectEnd = end;
    }
}


$(document).ready(() => {
    $(".markdown-editor").each((i, elem) => {
        new MarkdownEditor($(elem));
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