using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using GigHub.Web.Data;
using GigHub.Web.Models;
using GigHub.Web.ViewModels;

namespace GigHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string query = null)
        {
            var upcomingGigs = 
                _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCancelled);
            
            if(!String.IsNullOrWhiteSpace(query)) {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                        g.Artist.Name.Contains(query) ||
                        g.Genre.Name.Contains(query) ||
                        g.Venue.Contains(query));
            }
            
            ILookup<string, Attendance> attendances = null;

            if(_signInManager.IsSignedIn(HttpContext.User)) 
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                attendances = _context.Attendances
                    .Where(a => a.AttendeeId == currentUser.Id && a.Gig.DateTime > DateTime.Now)
                    .ToList()
                    .ToLookup(a => a.GigId);
                
            }
            else
            {
                attendances = Enumerable.Empty<Attendance>().ToLookup(a => a.GigId);
            }

            var viewModel = new GigsViewModel() {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
