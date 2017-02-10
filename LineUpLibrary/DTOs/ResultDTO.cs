using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class ResultDTO
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int league_id { get; set; }
        public int rank { get; set; }
        public Nullable<double> winnings { get; set; }
        public Nullable<bool> payment_sent { get; set; }
        public Nullable<int> modified_by { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
        public string league_name { get; set; }
        public string league_team_name { get; set; }

        public virtual LeagueDTO league { get; set; }
    }
}
