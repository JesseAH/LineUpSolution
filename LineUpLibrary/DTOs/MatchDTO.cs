using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class MatchDTO
    {
        public MatchDTO()
        {
            this.picks = new List<PickDTO>();

        }

        [Required]
        public int id { get; set; }
        [Required]
        public Nullable<int> match_type_id { get; set; }
        [Required]
        public int round_id { get; set; }
        [Required]
        public int team1_id { get; set; }
        [Required]
        public int team2_id { get; set; }
        public Nullable<int> winning_team_id { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> team1_start_date { get; set; }
        public Nullable<System.DateTime> team2_start_date { get; set; }
        public string match_outcome { get; set; }

        public string match_type_name { get; set; }
        public string team1_name { get; set; }
        public string team2_name { get; set; }
        public string winning_team_name { get; set; }
        public string round_name { get; set; }

        public virtual IList<PickDTO> picks { get; set; }
    }
}
