using Guilded.ViewModels.Forums;

using DataModel = Guilded.Data.Forums.ForumSection;

namespace Guilded.Data.DAL.Forums
{
    public interface IForumSectionReadWriteRepository : IReadWriteRepository<DataModel>
    {
        void CreateForumSection(EditForumSection sectionToCreate);
        void UpdateForumSection(EditForumSection sectionToUpdate);
    }
}
