//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LineUpLibrary.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class payment
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> league_team_id { get; set; }
        public Nullable<decimal> payment_total { get; set; }
        public bool paypal { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
        public string braintree_id { get; set; }
        public string braintree_payment_date { get; set; }
    
        public virtual league_team league_team { get; set; }
        public virtual user user { get; set; }
    }
}