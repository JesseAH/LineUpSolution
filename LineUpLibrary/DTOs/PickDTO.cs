using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class PickDTO
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int match_id { get; set; }
        [Required]
        public int league_team_id { get; set; }
        [Required]
        public int picked_team_id { get; set; }
        [Required]
        public Nullable<int> confidence_value { get; set; }

        public string league_team_name { get; set; }
        public string picked_team_name { get; set; }
        public bool is_winner { get; set; }
        public int round_id { get; set; }
        public string round_name { get; set; }
        public int match_teams { get; set; }
        public int pick_points {
            get {
                if (is_winner && confidence_value != null)
                    return (int)confidence_value;
                else
                    return 0; }
        }


    }
}
