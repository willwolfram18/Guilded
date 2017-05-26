const $disableUserModal = $("#disableUserModal");

function displayDisableUserModal(e: JQueryEventObject): void {
    $disableUserModal.modal("show");

    const $userRow = $(e.target).closest("tr.user");
    const userId: string = $userRow.data("id");
    const userDisableUrl: string = $userRow.data("disable-url");
    const $form = $disableUserModal.find("form");

    let disableUrl = $form.data("disable-url");
    disableUrl = disableUrl.replace(/userId/gi, userId);

    $form.attr("action", disableUrl);
    $form.find("input[name='Id']").val(userId).trigger("change");

    $disableUserModal.find(".ui.calendar").calendar({
        type: "date",
        popupOptions: {
            position: "bottom left"
        }
    });
}

$(document).ready(() => {
    $(".ui.button[data-disable-user]").on("click", displayDisableUserModal);
});