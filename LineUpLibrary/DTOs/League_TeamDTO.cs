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
        public bool is_logged_in_users_team { get; set; }
        public string league_name { get; set; }
        public string game_name { get; set; }
        public Nullable<int> league_team_points_sum { get; set; }
        public Nullable<int> leagues_league_team_count { get; set; }
        public Nullable<long> league_ranking { get; set; }
        public bool league_is_completed { get; set; }
        public decimal? total_winnings { get; set; }
        public decimal? league_points_per_pick { get; set; }
        public decimal? league_total_pot { get; set; }


        //public virtual IList<PickDTO> picks { get; set; }
        public virtual IList<RoundDTO> rounds { get; set; }
    }

    //public class User_League_TeamDTO
    //{
    //    public User_League_TeamDTO()
    //    {
    //        //this.picks = new List<PickDTO>();
    //        this.rounds = new List<RoundDTO>();
    //    }

    //    [Required]
    //    public int id { get; set; }
    //    [Required]
    //    public int user_id { get; set; }
    //    [Required]
    //    public int league_id { get; set; }
    //    [Required]
    //    public string name { get; set; }
    //    public string user_name { get; set; }
    //    public bool is_paid_up { get; set; }
    //    public string league_name { get; set; }
    //    public Nullable<int> league_team_points_sum { get; set; }
    //    public string league_team_name { get; set; }
    //    public int league_team_id { get; set; }
    //    public bool league_is_completed { get; set; }
    //    public decimal? league_total_pot { get; set; }
    //    public Nullable<int> leagues_league_team_count { get; set; }
    //    public Nullable<long> league_ranking { get; set; }
    //    public decimal? total_winnings { get; set; }
    //    public decimal? league_points_per_pick { get; set; }

    //    //public virtual IList<PickDTO> picks { get; set; }
    //    public virtual IList<RoundDTO> rounds { get; set; }
    //}
}
