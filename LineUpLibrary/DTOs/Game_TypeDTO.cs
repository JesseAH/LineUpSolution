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
        public string name { get; set; }
        public string description { get; set; }
        public DateTime? lock_date { get; set; }
    }
}
