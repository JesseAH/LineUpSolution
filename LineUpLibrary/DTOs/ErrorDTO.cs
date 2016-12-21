using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class ErrorDTO
    {
        public int user_id { get; set; }
        public string controller { get; set; }
        public string method { get; set; }
        public string source { get; set; }
    }
}
