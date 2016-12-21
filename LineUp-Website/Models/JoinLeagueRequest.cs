using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp_Website.Models
{
    public class JoinLeagueRequest
    {
        [Required]
        public int league_id { get; set; }
        [Required]
        public string league_team_name { get; set; }
        [Required]
        public string password { get; set; }
    }

    public class InviteRequest
    {
        [Required]
        public string emails { get; set; }
        [Required]
        public int league_id { get; set; }
    }

    public class EmailRequest
    {
        [Required]
        public string subject { get; set; }
        [Required]
        public string body { get; set; }
        [Required]
        public int league_id { get; set; }
    }
}
