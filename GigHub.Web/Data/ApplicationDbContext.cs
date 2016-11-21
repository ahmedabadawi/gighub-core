using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using GigHub.Web.Models;

namespace GigHub.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("uuid-ossp");
            builder.Entity<Attendance>()
                .HasKey(a => new { a.GigId, a.AttendeeId });
            builder.Entity<Attendance>()            
                .HasOne(a => a.Gig)
                .WithMany()
                .HasForeignKey(a => a.GigId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.Entity<Attendance>()            
                .HasOne(a => a.Attendee)
                .WithMany()
                .HasForeignKey(a => a.AttendeeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
