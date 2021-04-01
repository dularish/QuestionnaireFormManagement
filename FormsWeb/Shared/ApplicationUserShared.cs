using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsWeb.Shared
{
    public class ApplicationUserShared
    {
        public string EmailId { get; init; }

        public IList<string> Roles { get; init; } = new List<string>();
    }
}
