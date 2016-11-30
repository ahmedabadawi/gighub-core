using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using GigHub.Web.Core.ViewModels;
using GigHub.Web.Data;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Controllers
{
    public class FolloweesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public FolloweesController (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _context = context;
            _logger = loggerFactory.CreateLogger<GigsController>();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var artists = _context.Followings
                .Where(f => f.FollowerId == currentUser.Id)
                .Select(f => f.Followee)
                .ToList();
            
            return View(artists);
        }
    }
}