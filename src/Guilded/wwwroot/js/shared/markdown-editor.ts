let editor: MarkdownEditor;

class MarkdownEditor {
    private readonly unorderedListCharacter: string = "* ";

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

            this.editorValue = valueBeforeList +
                this.unorderedListCharacter +
                valueToConvertToList;
        }
        else {
            const valueToConvertToList = this.editorValue.substring(startOfLine, this.selectionEnd);
            let listItems = valueToConvertToList.split("\n");

            this.editorValue = valueBeforeList + this.unorderedListCharacter + listItems.join(`\n${this.unorderedListCharacter}`);
        }
    }

    private findStartOfLine(): number {
        let cursorPosition = this.editorValue.lastIndexOf("\n", this.selectionStart) + 1;

        if (this.editorValue[cursorPosition] === "\n") {
            cursorPosition--;
        }

        return cursorPosition;
    }
}

$(document).ready(() => {
    editor = new MarkdownEditor();

    $(".markdown-editor")
        .on("click", ".ui.button[data-action='unordered list']", () => editor.insertUnorderedList());
})