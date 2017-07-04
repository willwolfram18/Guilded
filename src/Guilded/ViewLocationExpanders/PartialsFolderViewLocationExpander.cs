using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Guilded.ViewLocationExpanders
{
    public class PartialsFolderViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var currentLocations = viewLocations.ToList();

            currentLocations.Add("~/Areas/{2}/Views/{1}/Partials/{0}.cshtml");
            currentLocations.Add("~/Areas/{2}/Views/Shared/Partials/{0}.cshtml");
            currentLocations.Add("~/Views/{1}/Partials/{0}.cshtml");
            currentLocations.Add("~/Views/Shared/Partials/{0}.cshtml");

            return currentLocations;
        }
    }
}
