using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class LeagueDTO
    {

        public LeagueDTO()
        {
            this.league_teams = new List<League_TeamDTO>();
        }

        [Required]
        public int id { get; set; }
        [Required]
        public int game_type_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public int manager_id { get; set; }
        public bool is_private { get; set; }
        public Nullable<int> max_players { get; set; }
        public Nullable<decimal> price { get; set; }
        public string password { get; set; }
        public bool is_completed { get; set; }
        public string description { get; set; }
        public string game_type_name { get; set; }
        public Nullable<System.DateTime> lock_date { get; set; }
        public bool has_round_payouts { get; set; }
        public int round_winnings_percentage { get; set; }
        public int number_of_rounds { get; set; }



        public int team_count { get; set; }
        public bool is_full { get { return league_teams.Count() >= max_players; } }

        public bool is_manager { get; set; }
        public decimal? total_pot { get { return league_teams.Count() * price; } }


        public virtual IList<League_TeamDTO> league_teams { get; set; }
    }
}
