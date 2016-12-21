﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<game_type> game_type { get; set; }
        public virtual DbSet<league> leagues { get; set; }
        public virtual DbSet<league_team> league_team { get; set; }
        public virtual DbSet<match> matches { get; set; }
        public virtual DbSet<match_type> match_type { get; set; }
        public virtual DbSet<pick> picks { get; set; }
        public virtual DbSet<round> rounds { get; set; }
        public virtual DbSet<team> teams { get; set; }
        public virtual DbSet<round_calculations> round_calculations { get; set; }
        public virtual DbSet<league_calculations> league_calculations { get; set; }
        public virtual DbSet<round_winnings_calculations> round_winnings_calculations { get; set; }
        public virtual DbSet<objects_with_open_rounds> objects_with_open_rounds { get; set; }
        public virtual DbSet<round_summary> round_summary { get; set; }
        public virtual DbSet<league_summary> league_summary { get; set; }
        public virtual DbSet<Error> Errors { get; set; }
        public virtual DbSet<league_calculations_team_count> league_calculations_team_count { get; set; }
        public virtual DbSet<league_rank> league_rank { get; set; }
    }
}
