
namespace GigHub.Web.Core.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }
        public string GigId { get; set; }
        public string AttendeeId { get; set; }
    }

}