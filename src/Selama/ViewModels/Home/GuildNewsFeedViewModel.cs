using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel
    {
        #region Properties
        #region Public properties
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime Timestamp { get; private set; }

        public HtmlString Content { get; private set; }
        #endregion
        #endregion

        #region Constructors
        public GuildNewsFeedViewModel(DateTime timestamp, string content)
        {
            Timestamp = timestamp;
            Content = new HtmlString(content);
        }
        #endregion
    }
}
