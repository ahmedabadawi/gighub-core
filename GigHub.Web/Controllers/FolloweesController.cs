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

namespace GigHub.Web.Controllers
{
    public class FolloweesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public FolloweesController (
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger<GigsController>();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var artists = 
                _unitOfWork.Followings.GetFollowingsByUser(currentUser.Id);
            
            return View(artists);
        }
    }
}