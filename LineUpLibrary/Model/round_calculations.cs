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
    
    public partial class round_calculations
    {
        public int round_id { get; set; }
        public string round_name { get; set; }
        public int game_type_id { get; set; }
        public Nullable<int> league_team_id { get; set; }
        public Nullable<int> correct_pick_count { get; set; }
        public Nullable<int> round_sum { get; set; }
        public Nullable<int> round_number { get; set; }
        public Nullable<int> league_id { get; set; }
    }
}
