using System.Collections.Generic;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNewNotifications(string userId);
        IEnumerable<UserNotification> GetUnreadUserNotifications(string userId);
    }
}