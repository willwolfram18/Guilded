class MarkdownEditor {
    private readonly headerMarkdown = "#";
    private readonly boldMarkdown = "__";
    private readonly italicMarkdown = "_";
    private readonly unorderedListMarkdown = "* ";
    private readonly orderedListMarkdown = "1. ";
    private readonly quoteMarkdown = "> ";

    private readonly $markdown: JQuery;
    private readonly markdownActionFunctions: {[action: string]: Function } = {
        "bold": this.insertBold,
        "italic": this.insertItalic,
        "header": this.insertHeader,
        "quote": this.insertQuote,
        "ul": this.insertUnorderedList,
        "ol": this.insertOrderedList,
        "ruler": this.insertRuler,
        "link": this.insertLink,
        "image": this.insertImage,
        "preview": this.togglePreview,
        "guide": this.toggleHelpGuide
    };

    private static Editors: [JQuery, MarkdownEditor][] = [];

    constructor(private $markdownEditor: JQuery) {
        this.$markdown = $markdownEditor.find(".markdown");
        this.$markdownEditor.find(".markdown-preview").hide();

        this.$markdownEditor
            .on("click", ".markdown-toolbar .button", (e) => {
                this.onMarkdownToolbarButtonClick(e);
            })
            .on("keydown", (e) => {
                this.onMarkdownKeyPress(e);
            });

        MarkdownEditor.Editors.push([$markdownEditor, this]);
    }

    private get textarea(): HTMLTextAreaElement {
        return this.$markdown[0] as HTMLTextAreaElement;
    }

    private get markdownText(): string {
        return this.textarea.value;
    }

    private get startOfLine(): number {
        for (let i = this.selectStart; i > 0; i--) {
            if (this.markdownText[i - 1] === "\n") {
                return i;
            }
        }

        return 0;
    }

    private get endOfLine(): number {
        const endOfLine = this.markdownText
            .indexOf("\n", this.startOfLine);

        return endOfLine === -1 ? this.markdownText.length : endOfLine;
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

    static getEditor($markdownEditor: JQuery): MarkdownEditor {
        for (let pair of MarkdownEditor.Editors) {
            if (pair[0][0] == $markdownEditor[0]) {
                return pair[1];
            }
        }

        return null;
    }

    get text(): string {
        return this.markdownText;
    }
    set text(value: string) {
        this.$markdown.val(value).trigger("change");
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

    insertOrderedList() {
        this.toggleSubstringAtStartOfLine(this.orderedListMarkdown);
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
        const tabKeyCode = 9;
        const enterKeyCode = 13;

        if (e.keyCode === tabKeyCode) {
            e.preventDefault();

            if (e.shiftKey) {
                this.unindentLine();
            } else {
                this.indentLine();
            }
            return;
        }

        if (e.keyCode !== enterKeyCode || !this.markdownText.length) {
            return;
        }

        if (this.doesLineStartWithText("\\s{0,}" + this.unorderedListMarkdown)) {
            e.preventDefault();
            this.continueOrRemoveLeadingMarkdown(this.unorderedListMarkdown);
            return;
        }
        if (this.doesLineStartWithText("\\s{0,}" + this.quoteMarkdown)) {
            e.preventDefault();
            this.continueOrRemoveLeadingMarkdown(this.quoteMarkdown);
            return;
        }
        if (this.doesLineStartWithText("\\s{0,}" + this.orderedListMarkdown)) {
            e.preventDefault();
            this.continueOrRemoveLeadingMarkdown(this.orderedListMarkdown);
            return;
        }
    }

    private unindentLine() {
        if (this.markdownText[this.startOfLine] !== " ") {
            return;
        }

        let start = this.selectStart;
        let end = this.selectEnd;

        let spacesToRemove = 1;
        while (spacesToRemove < 4 &&
            this.startOfLine + spacesToRemove < this.markdownText.length &&
            this.markdownText[this.startOfLine + spacesToRemove] === " ") {
            spacesToRemove++;
        }

        this.selectStart = this.startOfLine;
        this.selectEnd = this.startOfLine + spacesToRemove;
        this.deleteText();

        this.selectStart = start - spacesToRemove;
        this.selectEnd = end - spacesToRemove;
    }

    private indentLine() {
        let start = this.selectStart;
        let end = this.selectEnd;

        this.selectStart = this.selectEnd = this.startOfLine;

        this.insertText("    ");

        this.selectStart = start + 4;
        this.selectEnd = end + 4;
    }

    private continueOrRemoveLeadingMarkdown(leadingMarkdown: string) {
        if (this.markdownText
            .substr(this.startOfLine, this.endOfLine - this.startOfLine)
            .replace(leadingMarkdown, "")) {
            this.insertText(`\n${leadingMarkdown}`);
        } else {
            this.selectStart = this.startOfLine;
            this.selectEnd = this.endOfLine;

            this.deleteText();
            this.selectStart = this.selectEnd = this.startOfLine;
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
            this.insertText(`${isImage ? "!" : ""}[${this.markdownText.substr(start, end - start).trim()}](http://)`);
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

        if (this.doesLineStartWithText(text)) {
            this.selectStart = this.startOfLine;
            this.selectEnd = this.startOfLine + text.length;
            this.deleteText();

            start -= text.length;
            end -= text.length;
        } else {
            this.selectStart = this.selectEnd = this.startOfLine;
            this.insertText(text);

            start += text.length;
            end += text.length;
        }

        this.selectStart = start;
        this.selectEnd = end;
    }

    private doesLineStartWithText(text: string): boolean {
        return new RegExp(`^${text.replace(/\*/g, "\\*")}`).test(this.markdownText.substr(this.startOfLine))
    }
}

let m: MarkdownEditor;

$(document).ready(() => {
    $(".markdown-editor").each((i, elem) => {
        m = new MarkdownEditor($(elem));
    });
});