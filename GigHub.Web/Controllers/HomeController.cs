using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using GigHub.Web.Core;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.ViewModels;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IUnitOfWork _unitOfWork;

        public HomeController (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string query = null)
        {
            var upcomingGigs = _unitOfWork.Gigs.GetUpcomingGigs();
            
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
                attendances = _unitOfWork.Attendances.GetFutureAttendances(currentUser.Id).ToLookup(a => a.GigId);
                
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
