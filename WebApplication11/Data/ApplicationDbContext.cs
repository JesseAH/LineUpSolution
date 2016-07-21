using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication11.Models;

namespace WebApplication11.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    var table = entityType.Relational().TableName;
            //    if (table.StartsWith("AspNet"))
            //        entityType.Relational().TableName = $"Identity{table.Substring(6)}"; // this should be the default.
            //}
        }
    }
}
