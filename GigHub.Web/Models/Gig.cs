using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GigHub.Web.Models
{
    public class Gig
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        
        public bool IsCancelled { get; private set; }

        public ApplicationUser Artist { get; set; }
        
        [Required]
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Venue { get; set; }
        
        public Genre Genre { get; set; }
    
        [Required]
        public byte GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new List<Attendance>();
        }

        public void Cancel()
        {
            IsCancelled = true;

            // Create Notifications
            var notification = new Notification(NotificationType.GigCancelled, this);

            foreach(var attendee in Attendances.Select(a => a.Attendee)) 
            {
                attendee.Notify(notification);
            }

        }
    }
}