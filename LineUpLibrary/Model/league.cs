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
    
    public partial class league
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public league()
        {
            this.league_team = new HashSet<league_team>();
        }
    
        public int id { get; set; }
        public int game_type_id { get; set; }
        public string name { get; set; }
        public Nullable<int> max_players { get; set; }
        public Nullable<decimal> price { get; set; }
        public bool is_private { get; set; }
        public string password { get; set; }
        public bool is_completed { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
        public Nullable<System.DateTime> lock_date { get; set; }
    
        public virtual game_type game_type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<league_team> league_team { get; set; }
    }
}