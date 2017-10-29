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

function insertRequestVerificationTokenIntoData(formData: any) {
    const verificationTokenName = "__RequestVerificationToken";

    let verificationToken = $(`input[name='${verificationTokenName}']`).val();

    formData[verificationTokenName] = verificationToken;
}

function replyFormEntersLoading() {
    $("#create-reply").addClass("loading");
}

function replyFormExitsLoading() {
    $("#create-reply").removeClass("loading");
}

function onBeforeDeleteSend() {
    hideErrorAndSuccessMessages();
    replyFormEntersLoading();
}

function onDeleteError(response: JQueryXHR) {
    showErrorMessage(response.responseText || response.statusText);
}

function onReplyDeleteClick() {
    let $reply = $(this).closest(".comment");
    let replyId = $reply.data("reply-id");
    let formData: any = {
        replyId: replyId,
    };

    insertRequestVerificationTokenIntoData(formData);

    $.ajax({
        url: $("input[type='hidden'].delete-reply-url").val(),
        type: "DELETE",
        data: formData,
        beforeSend: onBeforeDeleteSend,
        complete: replyFormExitsLoading,
        success: () => {
            $reply.remove();
            showSuccessMessage("Successfully removed reply.");
        },
        error: onDeleteError
    });
}

function onThreadDeleteClick() {
    let formData: any = {
    };

    insertRequestVerificationTokenIntoData(formData);

    $.ajax({
        url: $("input[type='hidden'].delete-thread-url").val(),
        type: "DELETE",
        data: formData,
        beforeSend: onBeforeDeleteSend,
        complete: replyFormExitsLoading,
        success: () => {
            showSuccessMessage("Successfully removed the thread. You will be redirected to the forums in a moment.");

            setTimeout(() => {
                let forumUrl: string = $("input[type='hidden'].forum-url").val();

                window.location.href = forumUrl;
            }, 2500);
        },
        error: onDeleteError
    });
}

function onQuoteClick() {
    alert("A thing");
}

$(document).ready(() => {
    $(".comment .actions")
        .on("click", ".quote", onQuoteClick);

    $(".comment[data-reply-id] .actions")
        .on("click", ".delete", onReplyDeleteClick);

    $(".comment[data-thread-id]")
        .on("click", ".delete", onThreadDeleteClick);
});