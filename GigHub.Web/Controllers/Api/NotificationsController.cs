using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using GigHub.Web.Core;
using GigHub.Web.Core.ViewModels;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Dtos;

using GigHub.Web.Helpers.Extensions;

namespace GigHub.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public NotificationsController(
            IUnitOfWork  unitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<NotificationsController>();
        }

        [HttpGet]
        public async Task<IEnumerable<NotificationDto>> GetNewNotifications()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var notifications = _unitOfWork.Notifications.GetNewNotifications(currentUser.Id);

            return notifications.Select(_mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userNotifications = _unitOfWork.Notifications.GetUnreadUserNotifications(currentUser.Id);

            userNotifications.ForEach(n => n.Read());

            _unitOfWork.Complete();

            return Ok();
        }
    }
}