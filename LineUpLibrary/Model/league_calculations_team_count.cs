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
    
    public partial class league_calculations_team_count
    {
        public Nullable<long> id { get; set; }
        public int league_id { get; set; }
        public string league_name { get; set; }
        public Nullable<int> league_team_id { get; set; }
        public string league_team_name { get; set; }
        public Nullable<int> game_type_id { get; set; }
        public Nullable<int> league_team_sum { get; set; }
        public Nullable<int> correct_pick_count { get; set; }
        public Nullable<decimal> points_per_pick { get; set; }
        public bool is_completed { get; set; }
        public Nullable<decimal> total_winnings { get; set; }
        public Nullable<int> league_team_count { get; set; }
    }
}
