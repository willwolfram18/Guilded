$(document).ready(function() {
    $("#roles-list").accordion({
        exclusive: false,
        selector: {
            trigger: ".title .name"
        }
    });
})