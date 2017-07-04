using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.ViewLocationExpanders
{
    public class DisplayTemplateFolderViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var currentLocations = viewLocations.ToList();

            currentLocations.Add("~/Areas/{2}/Views/{1}/DisplayTemplates/{0}.cshtml");
            currentLocations.Add("~/Views/{1}/DisplayTemplates/{0}.cshtml");

            return currentLocations;
        }
    }
}
