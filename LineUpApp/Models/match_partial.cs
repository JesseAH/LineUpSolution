using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpApp.Models
{
    using System;
    using System.Collections.Generic;

    public partial class match
    {
        public string _match_descriptor { get { return round.name + ": " + team.name + " vs. " + team1.name; } }
    }

}

