$(document).ready(() => {
    const redirectUrl: string = $("#viewLink").val();

    window.location.assign(redirectUrl);
});