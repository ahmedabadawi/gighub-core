using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GigHub.Web.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        //public ICollection<ApplicationUser> Followers { get; set; }
        //public ICollection<ApplicationUser> Followees { get; set; }
        
        public ICollection<UserNotification> UserNotifications { get; set; }

        public ApplicationUser()
        {
            //Followers = new List<ApplicationUser>();
            //Followees = new List<ApplicationUser>();

            UserNotifications = new List<UserNotification>();
        }

        public void Notify(Notification notification)
        {
            UserNotifications.Add(new UserNotification(this,notification));
        }
    }
}
