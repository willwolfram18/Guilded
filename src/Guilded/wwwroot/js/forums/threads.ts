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
    const verificationTokenName = "__RequestVerificationToken";

    let $reply = $(this).closest(".comment");
    let replyId = $reply.data("reply-id");
    let verificationToken = $(`input[name='${verificationTokenName}']`).val();

    let formData: any = {
        replyId: replyId,
    };
    formData[verificationTokenName] = verificationToken;

    $.ajax({
        url: $("input[type='hidden'].delete-reply").val(),
        type: "DELETE",
        data: formData,
        beforeSend: () => {
            hideErrorAndSuccessMessages();
            replyFormEntersLoading();
        },
        complete: replyFormExitsLoading,
        success: () => {
            $reply.remove();
            showSuccessMessage("Successfully removed reply.");
        },
        error: (response) => {
            showErrorMessage(response.responseText);
        }
    });
}

function replyFormEntersLoading() {
    $("#create-reply").addClass("loading");
}

function replyFormExitsLoading() {
    $("#create-reply").removeClass("loading");
}

function onQuoteClick() {
    alert("A thing");
}

$(document).ready(() => {
    $(".comment .actions")
        .on("click", ".quote", onQuoteClick)
        .on("click", ".delete", onDeleteClick);
});