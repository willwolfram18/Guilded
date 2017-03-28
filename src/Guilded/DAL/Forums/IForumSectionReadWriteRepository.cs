using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guilded.Data.Models.Forums;
using Guilded.ViewModels.Forums;

using DataModel = Guilded.Data.Models.Forums.ForumSection;

namespace Guilded.Data.DAL.Forums
{
    public interface IForumSectionReadWriteRepository : IReadWriteRepository<DataModel>
    {
        void CreateForumSection(EditForumSection sectionToCreate);
        void UpdateForumSection(EditForumSection sectionToUpdate);
    }
}
