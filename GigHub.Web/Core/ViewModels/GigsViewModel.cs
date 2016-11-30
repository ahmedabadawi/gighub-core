using System.Collections.Generic;
using System.Linq;

using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.ViewModels
{
    public class GigsViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public ILookup<string, Attendance> Attendances { get; set; }
    }
}