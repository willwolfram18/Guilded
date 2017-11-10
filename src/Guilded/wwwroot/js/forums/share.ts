$(document).ready(() => {
    const redirectUrl: string = $("#shareLink").val();

    window.location.assign(redirectUrl);
});