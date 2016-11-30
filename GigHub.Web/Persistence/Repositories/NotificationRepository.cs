using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using GigHub.Web.Persistence;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetNewNotifications(string userId)
        {
            var userNotifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Include(un => un.Notification)
                    .ThenInclude(n => n.Gig.Artist)
                .ToList();
            // TODO: Remove the re-projection
            var notifications = userNotifications.Select(un => un.Notification);

            return notifications;
        }

        public IEnumerable<UserNotification> GetUnreadUserNotifications(string userId)
        {
             return _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
        }
    }
}