using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

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
/*
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
  */              
            builder.Entity<Following>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId });
/*
            builder.Entity<Following>()            
                .HasOne(f => f.Follower)
                .WithMany()
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.Entity<Following>()            
                .HasOne(f => f.Followee)
                .WithMany()
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.SetNull);
*/

            builder.Entity<UserNotification>()
                .HasKey(n => new { n.UserId, n.NotificationId });
        }
    }
}
