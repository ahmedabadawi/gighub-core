using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GigHub.Web.Core.ViewModels;
using GigHub.Web.Persistence;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Dtos;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class FollowingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public FollowingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<FollowingsController>();
        }

        [HttpPost]
        public async Task<IActionResult> Follow(FollowingDto dto)
        {
            if(string.IsNullOrEmpty(dto.ArtistId)) {
                return BadRequest();
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _logger.LogInformation("Current User: " + currentUser.Id);

            var exists = 
                _context.Followings.Any(
                    f => f.FollowerId == currentUser.Id && f.FolloweeId == dto.ArtistId);
            
            if(exists)
            {
                return BadRequest("You are already following this artist");
            }

            var following = new Following() {
                FolloweeId = dto.ArtistId,
                FollowerId = currentUser.Id
            };

            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var following = _context.Followings
                .SingleOrDefault(f => f.FollowerId == currentUser.Id && f.FolloweeId == id);
            
            if (following == null)
            {
                return NotFound();
            }

            _context.Followings.Remove(following);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}