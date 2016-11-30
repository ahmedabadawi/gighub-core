using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using GigHub.Web.Core;
using GigHub.Web.Core.ViewModels;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Controllers
{
    public class GigsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        private readonly IUnitOfWork _unitOfWork;

        public GigsController (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<GigsController>();
        
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public async Task<IActionResult> Mine() 
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(currentUser.Id);
            
            return View(gigs);
        }

        [Authorize]
        public async Task<IActionResult> Attending()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var viewModel = new GigsViewModel() {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(currentUser.Id),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(currentUser.Id).ToLookup(a => a.GigId)
            };
            return View("Gigs", viewModel);
        }

        [HttpPost]
        public IActionResult Search(GigsViewModel viewModel) 
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        public async Task<IActionResult> Details(string id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);
            
            if(gig == null)
                return RedirectToAction("Index", "Home");
            
            var viewModel = new GigDetailsViewModel(){
                Gig = gig
            };

            if(_signInManager.IsSignedIn(HttpContext.User))
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                viewModel.IsAttending = 
                    _unitOfWork.Attendances.GetAttendance(gig.Id, currentUser.Id) != null;

                viewModel.IsFollowing = 
                    _unitOfWork.Followings.GetFollowing(currentUser.Id, gig.Artist.Id) != null;
            }

            return View("Details", viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel() {
                Heading = "Add a Gig",
                Genres = _unitOfWork.Genres.GetGenres()
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {            
            var gig = _unitOfWork.Gigs.GetGig(id);
            
            if (gig == null)
                return NotFound();
         
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (gig.Artist.Id != currentUser.Id)
                return Unauthorized();

            var viewModel = new GigFormViewModel() {
                Heading = "Edit a Gig",
                Genres = _unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
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

            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(GigFormViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                viewModel.Heading = "Edit a Gig";
                return View("GigForm", viewModel);
            }

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);
            
            if (gig == null)
                return NotFound();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (gig.Artist.Id != currentUser.Id)
                return Unauthorized();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}