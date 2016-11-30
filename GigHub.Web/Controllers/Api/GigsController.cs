using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GigHub.Web.Core.Models;
using GigHub.Web.Persistence;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class GigsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public GigsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<GigsController>();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var gig = _context.Gigs
                .Include(g => g.Attendances)
                    .ThenInclude(a => a.Attendee)
                .Single(g => g.Id == id && g.ArtistId == currentUser.Id);
            
            if(gig.IsCancelled) return NotFound();    

            gig.Cancel();

            _context.SaveChanges();

            return Ok();
        }
    }
}