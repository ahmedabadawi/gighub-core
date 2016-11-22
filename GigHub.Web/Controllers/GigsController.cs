using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using GigHub.Web.ViewModels;
using GigHub.Web.Data;
using GigHub.Web.Models;

namespace GigHub.Web.Controllers
{
    public class GigsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public GigsController (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _context = context;
            _logger = loggerFactory.CreateLogger<GigsController>();
        }

        [Authorize]
        public async Task<IActionResult> Mine() 
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var gigs = _context.Gigs
                .Where(g => g.ArtistId == currentUser.Id && g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();

            
            return View(gigs);
        }

        [Authorize]
        public async Task<IActionResult> Attending()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var gigs = _context.Gigs
                .Where(g => g.Attendances.Any(a => a.AttendeeId == currentUser.Id))
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
            

            foreach (var gig in gigs)
            {
                _logger.LogDebug(string.Format("Date:{0}, Venue: {1}, Artist:{2}, Genre:{3}",
                    gig.DateTime, gig.Venue, gig.Artist.Name, gig.Genre.Name));
            }

            var viewModel = new GigsViewModel() {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };
            return View("Gigs", viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel() {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var artist = await _userManager.GetUserAsync(HttpContext.User);
            
            var gig = new Gig() {
                Id = Guid.NewGuid().ToString(),
                ArtistId = artist.Id,
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue                
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}