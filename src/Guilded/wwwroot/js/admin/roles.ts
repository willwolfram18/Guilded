interface IHandleRoleDelete {
    onRoleDeleteSuccess(response: IRoleDeleteResponse): void;
    onRoleDeleteError(response: JQueryXHR): void;
    onRoleDeleteBegin(): void;
}

interface IRoleDeleteResponse {
    message: string;
    roleId: string;
}

function cancelFormClick(e: JQueryEventObject) {
    const $elem = $(e.target);

    if (!$elem.data("href")) {
        return;
    }

    confirmAction("Are you sure? All unsaved changes will be lost.", () => {
        window.location.href = $elem.data("href");
    }, null);
}

const index: IHandleRoleDelete = {
    onRoleDeleteSuccess: (response: IRoleDeleteResponse): void => {
        $(".ui.success.message").html(response.message).removeClass("hidden");
        $(`[data-role="${response.roleId}"]`).remove();
    },
    onRoleDeleteError: (jqxhr: JQueryXHR): void => {
        const response = jqxhr.responseJSON as IUserEnabledStateChangeResponse;

        if (response === null) {
            $(".ui.error.message").removeClass("hidden")
                .html("An error occurred while trying to enable the user. Please try again.");
        } else {
            $(".ui.error.message").removeClass("hidden").html(response.message);
        }
    },
    onRoleDeleteBegin: (): void => {
        $(".ui.error.message, .ui.sucess.message").addClass("hidden");
    }
};

$(document).ready(function () {
    $("#roles-list").accordion({
        exclusive: false,
        selector: {
            trigger: ".title .name"
        }
    });
    $("form .ui.cancel.button").on("click", cancelFormClick);
})