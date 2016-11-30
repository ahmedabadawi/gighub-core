using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using GigHub.Web.Core.Models;


namespace GigHub.Web.Persistence.EntityMappings
{
    public class NotificationMapping : IEntityMappingConfiguration
    {
        public void Map(ModelBuilder builder)
        {
            builder.Entity<Notification>()
                .Property(n => n.Gig)
                .IsRequired();
        }
    }
}