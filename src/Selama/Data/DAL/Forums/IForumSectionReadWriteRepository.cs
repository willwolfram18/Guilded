using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Selama.Data.Models.Forums;
using Selama.Data.ViewModels.Forums;

using DataModel = Selama.Data.Models.Forums.ForumSection;

namespace Selama.Data.DAL.Forums
{
    public interface IForumSectionReadWriteRepository : IReadWriteRepository<DataModel>
    {
        void CreateForumSection(EditForumSection sectionToCreate);
        void UpdateForumSection(EditForumSection sectionToUpdate);
    }
}
