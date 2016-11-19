using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GigHub.Web.ViewModels;
using GigHub.Web.Data;
using GigHub.Web.Models;

namespace GigHub.Web.Controllers
{
    public class GigsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public GigsController (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
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

            return RedirectToAction("Index", "Home");
        }
    }
}