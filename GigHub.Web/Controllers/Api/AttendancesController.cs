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
    public class AttendancesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public AttendancesController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AttendancesController>();
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
                _unitOfWork.Attendances.GetAttendance(dto.GigId, currentUser.Id) != null; 
            
            if(exists)
            {
                return BadRequest("The attendance already exists");
            }
            var attendance = new Attendance() {
                GigId = dto.GigId,
                AttendeeId = currentUser.Id
            };

            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(string id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var attendance = _unitOfWork.Attendances.GetAttendance(id, currentUser.Id);

            if(attendance == null)
            {
                return NotFound();
            }

            _unitOfWork.Attendances.Remove(attendance);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}