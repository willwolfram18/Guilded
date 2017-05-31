interface IUserEnabledStateChangeResponse {
    userId: string;
    message: string;
}

const $disableUserModal = $("#disableUserModal");

function onAjaxFormSubmitBegin() {
    $(this).addClass("loading");
    hideErrorAndSuccessMessages();
}

function onAjaxFormSubmitComplete() {
    $(this).removeClass("loading");
}

function onDisableUserSuccess(response: IUserEnabledStateChangeResponse) {
    $(".ui.success.message").removeClass("hidden").html(response.message);

    const $userRow = $(`#usersList tr[data-id='${response.userId}']`);
    $userRow.addClass("disabled");
    $userRow.find(".options .ui.icon.button[data-enable-user]").removeClass("hidden");
    $userRow.find(".options .ui.icon.button[data-disable-user]").addClass("hidden");
}

function onUserEnabledStateChangeFailure(jqxhr: JQueryXHR) {
    const response = jqxhr.response as IUserEnabledStateChangeResponse;

    $(".ui.error.message").removeClass("hidden").html(response.message);
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

function onEnableUserSuccess(response: IUserEnabledStateChangeResponse): void {
    $(".ui.success.message").removeClass("hidden").html(response.message);

    const $userRow = $(`#usersList tr[data-id='${response.userId}']`);
    $userRow.removeClass("disabled");
    $userRow.find(".options .ui.icon.button[data-enable-user]").addClass("hidden");
    $userRow.find(".options .ui.icon.button[data-disable-user]").removeClass("hidden");
}

function enableUserClick(e: JQueryEventObject): void {
    const $userRow = $(e.target).closest("tr[data-id]");

    confirmAction("Are you sure you want to enable this user?",
        () => {
            let enableUserUrl: string = $userRow.closest("#usersList").data("enable-url");
            enableUserUrl = enableUserUrl.replace(/userId/ig, $userRow.data("id"));

            $.ajax({
                method: "POST",
                url: enableUserUrl,
                beforeSend: hideErrorAndSuccessMessages,
                success: onEnableUserSuccess,
                error: onUserEnabledStateChangeFailure
            });
        }
    );
}

$(document).ready(() => {
    $(".ui.button[data-disable-user]").on("click", displayDisableUserModal);
});