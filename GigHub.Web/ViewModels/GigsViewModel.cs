using System.Collections.Generic;
using System.Linq;

using GigHub.Web.Models;

namespace GigHub.Web.ViewModels
{
    public class GigsViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public ILookup<string, Attendance> Attendances { get; set; }
        public ILookup<string, Following> Followings { get; set; }
    }
}