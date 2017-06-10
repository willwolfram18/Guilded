interface IUserAjaxResponse {
    userId: string;
    message: string;
}

interface IUserEnabledStateChangeResponse extends  IUserAjaxResponse {
}

interface IUserRoleChangeResponse extends IUserAjaxResponse {
    roleInfo: {
        roleId: string,
        roleName: string;
    }
}

interface IUserEmailUpdateResponse extends IUserAjaxResponse {
    email: string;
}

const $disableUserModal = $("#disableUserModal");
const $changeRoleModal = $("#changeRoleModal");
const $changeEmailModal = $("#changeEmailModal");

function getUserRow(userId: string): JQuery {
    return $(`#usersList tr[data-id='${userId}']`);
}

function onAjaxFormSubmitBegin() {
    $(this).addClass("loading");
    hideErrorAndSuccessMessages();
}

function onAjaxFormSubmitComplete() {
    $(this).removeClass("loading");
    $disableUserModal.modal("hide");
    $changeRoleModal.modal("hide");
    $changeEmailModal.modal("hide");
}

function onUserEnabledStateChangeFailure(jqxhr: JQueryXHR) {
    const response = jqxhr.responseJSON as IUserAjaxResponse;
    let errorMessage: string = "An error occurred while trying to update the user. Please try again.";

    if (response && response.message) {
        errorMessage = response.message;
    }

    showErrorMessage(errorMessage);
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
                data: {
                    __RequestVerificationToken: $("input[type='hidden'][name='__RequestVerificationToken']").val(),
                },
                beforeSend: hideErrorAndSuccessMessages,
                success: onEnableUserSuccess,
                error: onUserEnabledStateChangeFailure
            });
        }
    );
}

function onEnableUserSuccess(response: IUserEnabledStateChangeResponse): void {
    showSuccessMessage(response.message);

    const $userRow = getUserRow(response.userId);
    $userRow.removeClass("disabled");
    $userRow.find(".options [data-enable-user]").addClass("hidden");
    $userRow.find(".options [data-disable-user]").removeClass("hidden");
}


function displayDisableUserModal(e: JQueryEventObject): void {
    $disableUserModal.modal("show");

    const $userRow = $(e.target).closest("tr.user");
    const userId: string = $userRow.data("id");
    const $form = $disableUserModal.find("form");
    const disableUrl: string = $form.data("action").replace(/userId/gi, userId);

    $form.attr("action", disableUrl);
    $form.find("input[name='Id']").val(userId).trigger("change");

    $disableUserModal.find(".ui.calendar").calendar({
        type: "date",
        popupOptions: {
            position: "bottom left"
        }
    });
}

function onDisableUserSuccess(response: IUserEnabledStateChangeResponse) {
    showSuccessMessage(response.message);

    const $userRow = getUserRow(response.userId);
    $userRow.addClass("disabled");
    $userRow.find(".options [data-enable-user]").removeClass("hidden");
    $userRow.find(".options [data-disable-user]").addClass("hidden");
}


function displayRoleChangeModal(e: JQueryEventObject): void {
    const $userRow = $(e.target).closest("tr");
    const userId: string = $userRow.data("id");
    const userRoleId: string = $userRow.data("role-id");
    const userRoleName: string = $userRow.find("td.user-role").text().trim();
    

    const $changeRoleForm = $changeRoleModal.find("form");
    const targetUrl: string = $changeRoleForm.data("action").replace(/userId/gi, userId);
    $changeRoleForm.find("input[type='hidden'][name='UserId']").val(userId).trigger("change");
    $changeRoleForm.find("select[name='NewRoleId']").val(userRoleId).trigger("change");
    $changeRoleForm.find("#oldRoleName").text(userRoleName);
    $changeRoleForm.attr("action", targetUrl);

    $changeRoleModal.modal("show");
}

function onChangeUserRoleSuccess(response: IUserRoleChangeResponse): void {
    showSuccessMessage(response.message);

    const $userRole = getUserRow(response.userId);

    $userRole.data("role-id", response.roleInfo.roleId)
        .find("td.user-role").text(response.roleInfo.roleName);
}


function displayEmailChangeModal(e: JQueryEventObject): void {
    const $userRow = $(e.target).closest("tr");
    const userId: string = $userRow.data("id");
    const userEmail: string = $userRow.find("td.user-email a").text().trim();

    const $changeEmailForm = $changeEmailModal.find("form");
    const targetUrl: string = $changeEmailForm.data("action").replace(/userId/gi, userId);
    $changeEmailForm.find("input[type='hidden'][name='UserId']").val(userId).trigger("change");
    $changeEmailForm.find("input[type='email']").val(userEmail).trigger("change");
    $changeEmailForm.attr("action", targetUrl);

    $changeEmailModal.modal("show");
}

function onChangeUserEmailSuccess(response: IUserEmailUpdateResponse): void {
    const $userRow = getUserRow(response.userId);

    $userRow.find("td.user-email a").text(response.email).attr("href", `mailto:${response.email}`);
    showSuccessMessage(response.message);
}


function onSendConfirmationEmailClick(e: JQueryEventObject): void {
    const $userRow = $(e.target).closest("tr");
    const userId: string = $userRow.data("id");
    const targetUrl: string = $userRow.closest("#usersList").data("confirmation-email-url").replace(/userId/ig, userId);
    const userEmail = $userRow.find("td.user-email a").text().trim();

    confirmAction(`A new confirmation email will be sent to ${userEmail}. Is this correct?`,
        () => {
            $.ajax({
                method: "POST",
                url: targetUrl,
                data: {
                    __RequestVerificationToken: $("input[type='hidden'][name='__RequestVerificationToken']").val(),
                },
                beforeSend: hideErrorAndSuccessMessages,
                success: () => showSuccessMessage("Email sent!"),
                error: onUserEnabledStateChangeFailure
            })
        }
    );
}

$(document).ready(() => {
    $(".options .ui.dropdown.button").dropdown({
        action: "nothing"
    });

    $("#usersList .options .menu").on("click", "[data-disable-user]", displayDisableUserModal)
        .on("click", "[data-enable-user]", enableUserClick)
        .on("click", "[data-change-role]", displayRoleChangeModal)
        .on("click", "[data-change-email]", displayEmailChangeModal)
        .on("click", "[data-send-confirmation]", onSendConfirmationEmailClick);
});