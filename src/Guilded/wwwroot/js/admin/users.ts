$(document).ready(() => {
    $(".ui.button[data-disable-user]").on("click", (e: JQueryEventObject) => {
        $("#disableUserModal").modal("show");
        $("#disableUserModal .ui.calendar").calendar();
    });
});