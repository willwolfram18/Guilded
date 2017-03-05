using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama_SPA.Data.Models.Forums;
using Selama_SPA.Data.ViewModels.Forums;

using DataModel = Selama_SPA.Data.Models.Forums.ForumSection;

namespace Selama_SPA.Data.DAL.Forums
{
    public interface IForumSectionReadWriteRepository : IReadWriteRepository<DataModel>
    {
        void CreateForumSection(EditForumSection sectionToCreate);
        void UpdateForumSection(EditForumSection sectionToUpdate);
    }
}
