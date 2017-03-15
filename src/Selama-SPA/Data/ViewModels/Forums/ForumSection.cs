using DataModel = Selama.Data.Models.Forums.ForumSection;

namespace Selama.Data.ViewModels.Forums
{
    public class ForumSection
    {
        #region Properties
        #region Public Properties
        public int Id { get; set; }

        public string Title { get; set; }
        #endregion
        #endregion

        public ForumSection(DataModel section)
        {
            Id = section.Id;
            Title = section.Title;
        }
    }
}