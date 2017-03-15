using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SelamaApi.Data.Models.Forums;
using SelamaApi.Data.ViewModels.Forums;

using DataModel = SelamaApi.Data.Models.Forums.ForumSection;

namespace SelamaApi.Data.DAL.Forums
{
    public interface IForumSectionReadWriteRepository : IReadWriteRepository<DataModel>
    {
        void CreateForumSection(EditForumSection sectionToCreate);
        void UpdateForumSection(EditForumSection sectionToUpdate);
    }
}
