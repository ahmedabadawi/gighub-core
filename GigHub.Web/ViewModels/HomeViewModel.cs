using System.Collections.Generic;
using GigHub.Web.Models;

namespace GigHub.Web.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public bool ShowActions { get; set; }
    }
}