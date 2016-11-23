using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GigHub.Web.ViewModels;
using GigHub.Web.Data;
using GigHub.Web.Models;
using GigHub.Web.Dtos;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class AttendancesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public AttendancesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AttendancesController>();
        }

        public IEnumerable<Attendance> GetAll() {
            var attendances = _context.Attendances.ToList();

            return attendances;
        }

        [HttpPost]
        public async Task<IActionResult> Attend(AttendanceDto dto)
        {
            if(string.IsNullOrEmpty(dto.GigId)) {
                return BadRequest();
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _logger.LogInformation("Current User: " + currentUser.Id);

            var exists = 
                _context.Attendances.Any(
                    a => a.AttendeeId == currentUser.Id && a.GigId == dto.GigId);
            
            if(exists)
            {
                return BadRequest("The attendance already exists");
            }
            var attendance = new Attendance() {
                GigId = dto.GigId,
                AttendeeId = currentUser.Id
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}