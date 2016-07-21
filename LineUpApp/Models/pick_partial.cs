using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpApp.Models
{
    public partial class pick
    {
        public string _winner_name { get {
                if (match.team2 == null)
                    return null;

                return match.team2.name;
            } }

    }

    public partial class league_team
    {
        public string _league_team_descriptor
        {
            get
            {
                if (league == null)
                    return null;

                return league.name + ": " + name;
            }
        }
    }
}
