interface ConfirmModalCallback {
    (this: JQuery, $element: JQuery): void | false;
}
const $confirmModal = $("#confirmModal");

function confirmAction(message: string, confirmCallback: ConfirmModalCallback, cancelCallback?: ConfirmModalCallback, title?: string) {
    title = title || "Confirm";
    cancelCallback = cancelCallback || (($elem: JQuery): void | false => {});

    $confirmModal.find(".content").html(message);
    $confirmModal.find(".header").html(title);

    $confirmModal.modal({
        onApprove: confirmCallback,
        onDeny: cancelCallback
    }).modal("show");
}