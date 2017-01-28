using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Data.Models.Home
{
    [Table("GuildActivities")]
    public class GuildActivity
    {
        #region Properties
        #region Public properties
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Content { get; set; }
        #endregion
        #endregion
    }
}
