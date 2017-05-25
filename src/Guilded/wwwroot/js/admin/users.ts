const $disableUserModal = $("#disableUserModal");

function displayDisableUserModal(e: JQueryEventObject): void {
    $disableUserModal.modal("show");

    const userId = $(e.target).closest("tr.user").data("id");
    $disableUserModal.find("form input[name='Id']").val(userId).trigger("change");
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