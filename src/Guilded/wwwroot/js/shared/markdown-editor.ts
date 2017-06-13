let editor: MarkdownEditor;

class MarkdownEditor {
    private readonly unorderedListMarkdown: string = "* ";
    private readonly boldMarkdown = "**";

    private readonly $editorTextArea = $(".markdown-editor #Content");

    private get editorValue(): string {
        return this.$editorTextArea.val();
    }

    private set editorValue(value: string) {
        this.$editorTextArea.val(value).trigger("change");
    }

    private get selectionStart(): number {
        return this.$editorTextArea.prop("selectionStart");
    }

    private get selectionEnd(): number {
        return this.$editorTextArea.prop("selectionEnd");
    }

    private get isRangeSelected(): boolean {
        return this.selectionStart !== this.selectionEnd;
    }

    constructor() {
        
    }

    public insertUnorderedList(): void {
        const startOfLine = this.findStartOfLine();
        const valueBeforeList = this.editorValue.substring(0, startOfLine);

        if (!this.isRangeSelected) {
            const valueToConvertToList = this.editorValue.substring(startOfLine);

            this.insertText(this.unorderedListMarkdown, startOfLine, startOfLine);
        }
        else {
            const valueToConvertToList = this.editorValue.substring(startOfLine, this.selectionEnd);
            let listItems = valueToConvertToList.split("\n");

            this.editorValue = valueBeforeList + this.unorderedListMarkdown + listItems.join(`\n${this.unorderedListMarkdown}`);
        }
    }

    public insertBoldText(): void {
        if (this.isRangeSelected) {
            let textToBold = this.editorValue.substring(this.selectionStart, this.selectionEnd).trim()
                .replace(/\n/g, " ");

            this.insertText(this.boldMarkdown + textToBold + this.boldMarkdown, this.selectionStart, this.selectionEnd);
        }
        else {
            this.insertText(`${this.boldMarkdown}Bold text${this.boldMarkdown}`, this.selectionStart, this.selectionEnd);
        }
    }

    private insertText(message: string, startPosition: number, endPosition?: number): void {
        if (endPosition !== 0 && !endPosition) {
            endPosition = startPosition;
        }

        this.editorValue = this.editorValue.substring(0, startPosition) +
            message +
            this.editorValue.substring(endPosition);
    }

    private findStartOfLine(): number {
        let cursorPosition = this.selectionStart;

        if (this.isRangeSelected && this.editorValue[cursorPosition] === "\n") {
            cursorPosition--;
        }

        return this.editorValue.lastIndexOf("\n", cursorPosition) + 1;
    }
}

$(document).ready(() => {
    editor = new MarkdownEditor();

    $(".markdown-editor")
        .on("click", ".ui.button[data-action='unordered list']", () => editor.insertUnorderedList())
        .on("click", ".ui.button[data-action='bold']", () => editor.insertBoldText());
})