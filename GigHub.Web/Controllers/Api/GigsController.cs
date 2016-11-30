using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GigHub.Web.Core;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class GigsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GigsController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<GigsController>();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var gig = _unitOfWork.Gigs.GetGigWithAttendees(id);
            
            if(gig.IsCancelled) return NotFound();    

            gig.Cancel();

            _unitOfWork.Complete();

            return Ok();
        }
    }
}