interface IHandleRoleDelete {
    onRoleDeleteSuccess(response: IRoleDeleteResponse): void;
    onRoleDeleteError(response: IRoleDeleteResponse): void;
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
    onRoleDeleteError: (response: IRoleDeleteResponse): void => {
        $(".ui.error.message").html(response.message).removeClass("hidden");
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