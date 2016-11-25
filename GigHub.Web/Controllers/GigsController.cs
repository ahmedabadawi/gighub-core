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
                .Where(g => g.ArtistId == currentUser.Id && 
                    g.DateTime > DateTime.Now && 
                    !g.IsCancelled)
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
                Heading = "Add a Gig",
                Genres = _context.Genres.ToList()
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == currentUser.Id);
            
            var viewModel = new GigFormViewModel() {
                Heading = "Edit a Gig",
                Genres = _context.Genres.ToList(),
                Id = gig.Id,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                viewModel.Heading = "Add a Gig";
                return View("GigForm", viewModel);
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                viewModel.Heading = "Edit a Gig";
                return View("GigForm", viewModel);
            }

            var artist = await _userManager.GetUserAsync(HttpContext.User);
            
            var gig = _context.Gigs
                .Include(g => g.Attendances)
                    .ThenInclude(a => a.Attendee)
                .Single(g => g.Id == viewModel.Id && g.ArtistId == artist.Id);
            
            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}