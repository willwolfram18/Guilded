using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataModel = Guilded.Data.Models.Forums.ForumSection;

namespace Guilded.ViewModels.Forums
{
    public class EditForumSection
    {
        #region Properties
        #region Public Properties
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = DataModel.REQUIRED_ERROR_MSG)]
        [StringLength(DataModel.TITLE_MAX_LENGTH, MinimumLength = DataModel.TITLE_MIN_LENGTH, ErrorMessage = DataModel.LENGTH_ERROR_MSG)]
        public string Title { get; set; }

        [Required]
        public bool IsActive { get; set; }
        #endregion
        #endregion

        public EditForumSection()
        {
        }
    }
}
