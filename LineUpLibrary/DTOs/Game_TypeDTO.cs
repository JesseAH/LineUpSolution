using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class Game_TypeDTO
    {
        public int id { get; set; }
        public int adminUserId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime? lock_date { get; set; }
        public bool? completed { get; set; }

        public int number_of_rounds { get; set; }
        public string _lookup_admin_name { get; set; }

        public virtual ICollection<RoundDTO> rounds { get; set; }
        public virtual ICollection<TeamDTO> teams { get; set; }
    }
}
