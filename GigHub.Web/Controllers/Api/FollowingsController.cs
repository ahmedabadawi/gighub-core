using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GigHub.Web.Core;
using GigHub.Web.Core.ViewModels;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Dtos;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class FollowingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public FollowingsController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
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
                _unitOfWork.Followings.GetFollowing(currentUser.Id, dto.ArtistId) != null;
            
            if(exists)
            {
                return BadRequest("You are already following this artist");
            }

            var following = new Following() {
                FolloweeId = dto.ArtistId,
                FollowerId = currentUser.Id
            };

            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var following = _unitOfWork.Followings.GetFollowing(currentUser.Id, id);
            
            if (following == null)
            {
                return NotFound();
            }

            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}