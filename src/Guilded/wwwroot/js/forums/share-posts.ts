$(document).ready(() => {
    $(".share.button").popup({
        position: "bottom center",
        content: "Copied!",
        on: "click",
        onShow: (popup) => {
            const $popup = $(popup);

            setTimeout(() => $popup.popup("hide"), 1500);
        },
        className: {
            popup: "ui tiny inverted popup"
        }
    });

    let clipboard = new Clipboard(".share.button",
        {
            text: (trigger: Element) => $(trigger).data("share-link"),
        });
});