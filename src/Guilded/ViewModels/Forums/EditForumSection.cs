using System.ComponentModel.DataAnnotations;
using DataModel = Guilded.Data.Forums.ForumSection;

namespace Guilded.ViewModels.Forums
{
    public class EditForumSection
    {
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = DataModel.TitleRequiredErrorMessage)]
        [StringLength(DataModel.TitleMaxLength, MinimumLength = DataModel.TitleMinLength, ErrorMessage = DataModel.TitleLengthErrorMessage)]
        public string Title { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public EditForumSection()
        {
        }
    }
}
