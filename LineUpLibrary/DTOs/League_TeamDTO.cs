using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class League_TeamDTO
    {
        public League_TeamDTO()
        {
            //this.picks = new List<PickDTO>();
            this.rounds = new List<RoundDTO>();
        }

        [Required]
        public int id { get; set; }
        [Required]
        public int user_id { get; set; }
        [Required]
        public int league_id { get; set; }
        [Required]
        public string name { get; set; }
        public string user_name { get; set; }
        public bool is_paid_up { get; set; }
        public string league_name { get; set; }
        public Nullable<int> league_team_points_sum { get; set; }
        public decimal? total_winnings { get; set; }

        //public virtual IList<PickDTO> picks { get; set; }
        public virtual IList<RoundDTO> rounds { get; set; }
    }

    public class User_League_TeamDTO
    {
        public User_League_TeamDTO()
        {
            //this.picks = new List<PickDTO>();
            this.rounds = new List<RoundDTO>();
        }

        [Required]
        public int id { get; set; }
        [Required]
        public int user_id { get; set; }
        [Required]
        public int league_id { get; set; }
        [Required]
        public string name { get; set; }
        public string user_name { get; set; }
        public bool is_paid_up { get; set; }
        public string league_name { get; set; }
        public Nullable<int> league_team_points_sum { get; set; }
        public int ranking { get; set; }
        public string league_team_name { get; set; }
        public int league_team_id { get; set; }
        public bool is_completed { get; set; }

        //TODO
        public int league_ranking { get { return 2; } }
        public decimal? total_winnings { get { return 100; } }

        //public virtual IList<PickDTO> picks { get; set; }
        public virtual IList<RoundDTO> rounds { get; set; }
    }
}
