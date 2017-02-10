using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DTOs
{
    public class PaymentDTO
    {
        public int id { get; set; }
        public string braintree_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> league_team_id { get; set; }
        public Nullable<decimal> payment_total { get; set; }
        public bool paypal { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
        public string braintree_payment_date { get; set; }
        public string league_team_name { get; set; }

        public virtual League_TeamDTO league_team { get; set; }
        public virtual userDTO user { get; set; }
    }
}
