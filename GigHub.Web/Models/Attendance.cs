using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Web.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }
        
        //[ForeignKey("Gig")]
        //[Column(Order = 1)]
        public string GigId { get; set; }
        
        //[ForeignKey("Attendee")]
        //[Column(Order = 2)]
        public string AttendeeId { get; set; }
    }

}