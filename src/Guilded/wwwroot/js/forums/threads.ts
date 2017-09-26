function onPostReplySuccess(data: any) {
    let pageUrl = $("input[type='hidden'].last-page").val();

    window.location.href = pageUrl;
}

function onPostReplyError(response: JQueryXHR) {
    let $currentReplyForm = $("#create-reply");
    let $updatedForm = $(response.responseText);

    $updatedForm.insertBefore($currentReplyForm);
    $currentReplyForm.remove();
}

function onPostReplyBegin() {
    $(this).addClass("loading");
}

function onDeleteClick() {
    let replyId = $(this).closest(".comment").data("reply-id");

    $.ajax({
        url: $("input[type='hidden'].delete-reply").val(),
        type: "DELETE",
        data: {
            replyId: replyId,
        }
    });
}

function onQuoteClick() {
    alert("A thing");
}

$(document).ready(() => {
    $(".comment .actions")
        .on("click", ".quote", onQuoteClick)
        .on("click", ".delete", onDeleteClick);
});