$(document).ready(() => {
    debugger;
    const redirectUrl: string = $("#shareLink").val();

    window.location.assign(redirectUrl);
});