using System;

namespace GigHub.Web.Models
{
    public class UserNotification
    {
        public string UserId { get; private set; }
        
        public int NotificationId { get; private set; }

        public ApplicationUser User { get; private set; }

        public Notification Notification { get; private set; }

        public bool IsRead { get; set; }

        protected UserNotification() { }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if(user == null) throw new ArgumentNullException("user");
            if(notification == null) throw new ArgumentNullException("notification");
            this.User = user;
            this.Notification = notification;
        }
    }
}