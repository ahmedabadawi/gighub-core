using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GigHub.Web.Models
{
    public class Gig
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        
        public bool IsCancelled { get; set; }

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

        public IEnumerable<Attendance> Attendances { get; set; }
    }
}