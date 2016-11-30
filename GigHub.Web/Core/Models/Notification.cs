using System;

namespace GigHub.Web.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        public Gig Gig { get; private set; }    

        protected Notification() 
        { 
            
        }

        private Notification(NotificationType type, Gig gig)
        {
            if(gig == null) throw new ArgumentNullException("gig");
            this.DateTime = DateTime.Now;
            this.Type = type;
            this.Gig = gig;
        }   

        public static Notification GigCreated(Gig gig) {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue) {
            var notification = new Notification(NotificationType.GigUpdated, newGig);
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification GigCancelled(Gig gig) {
            return new Notification(NotificationType.GigCancelled, gig);
        }
    }
}