using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using GigHub.Web.ViewModels;
using GigHub.Web.Data;
using GigHub.Web.Models;
using GigHub.Web.Dtos;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public NotificationsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<NotificationsController>();
        }

        [HttpGet]
        public async Task<IEnumerable<NotificationDto>> GetNewNotifications()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var userNotifications = 
                _context.UserNotifications
                .Where(un => un.UserId == currentUser.Id && !un.IsRead)
                .Include(un => un.Notification)
                    .ThenInclude(n => n.Gig.Artist)
                .ToList();
            // TODO: Remove the re-projection
            var notifications = userNotifications.Select(un => un.Notification);

            return notifications.Select(_mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userNotifications = 
                _context.UserNotifications
                .Where(un => un.UserId == currentUser.Id && !un.IsRead)
                .ToList();

            userNotifications.ForEach(n => n.Read());

            _context.SaveChanges();

            return Ok();
        }
    }
}