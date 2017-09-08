function onPostReplySuccess(data: any)
{
    let pageUrl = $("input[type='hidden'].last-page").val();
    window.location.href = pageUrl;
}

function onPostReplyError(response: JQueryXHR)
{
    let $currentReplyForm = $("#CreateReply");
    let $updatedForm = $(response.responseText);
    $updatedForm.insertBefore($currentReplyForm);
    $currentReplyForm.remove();
}