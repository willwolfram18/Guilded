using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guilded.Areas.Admin.ViewModels.Roles
{
    public class PaginatedRolesViewModel
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<ApplicationRole> Roles { get; set; }
    }
}
