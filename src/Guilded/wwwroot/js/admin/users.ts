interface IDisableUserResponse {
    userId: string;
    message: string;
}

const $disableUserModal = $("#disableUserModal");

function onAjaxFormSubmitBegin() {
    $(this).addClass("loading");
    $(".ui.error.message, .ui.success.message").addClass("hidden")
}

function onAjaxFormSubmitComplete() {
    $(this).removeClass("loading");
}

function onDisableUserSuccess(response: IDisableUserResponse) {
    $(".ui.success.message").removeClass("hidden").html(response.message);

    const $userRow = $(`#usersList tr[data-id='${response.userId}']`);
    $userRow.addClass("disabled");
    $userRow.find(".options .ui.icon.button[data-enable-user]").removeClass("hidden");
    $userRow.find(".options .ui.icon.button[data-disable-user]").addClass("hidden");
}

function onDisableUserFailure(response: any) {
    throw new Error("not implemented");
}

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