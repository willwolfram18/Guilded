using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Guilded.Areas.Forums.DAL;
using Guilded.Areas.Forums.ViewModels;
using Guilded.Data.Forums;
using Guilded.Extensions;
using Guilded.Migrations.SqlServer.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Guilded.Areas.Forums.Controllers
{
    [Route("[area]/[controller]")]
    public class RepliesController : ForumsController
    {
        public RepliesController(IForumsDataContext dataContext, ILoggerFactory loggerFactory) : base(dataContext, loggerFactory)
        {
        }

        [HttpPost("{threadId}/new")]
        public async Task<IActionResult> PostNewReplyToThread(CreateReplyViewModel reply)
        {
            var thread = await DataContext.GetThreadByIdAsync(reply.ThreadId);

            if (thread == null)
            {
                ModelState.AddModelError("", "That thread does not exist.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var createdReply = await DataContext.CreateReplyAsync(reply.ToReply(User.UserId()));

                    return PartialView("DisplayTemplates/ReplyViewModel", new ReplyViewModel(createdReply));
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message, e);

                    ModelState.AddModelError("", "An error occurred while creating the reply.");
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(reply);
        }
    }
}
