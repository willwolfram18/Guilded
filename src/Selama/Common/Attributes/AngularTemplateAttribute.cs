using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Common.Attributes
{
    public class AngularTemplateAttribute : RouteAttribute
    {
        public AngularTemplateAttribute(string template, bool includeArea = false, bool includeController = false) : base(
            (includeArea ? "[area]/" : "") +
            (includeController ? "[controller]/" : "" ) +
            "template/" + template
        )
        {
        }
    }
}
