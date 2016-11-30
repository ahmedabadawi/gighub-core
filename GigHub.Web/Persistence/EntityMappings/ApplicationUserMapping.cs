using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using GigHub.Web.Core.Models;


namespace GigHub.Web.Persistence.EntityMappings
{
    public class ApplicationUserMapping : IEntityMappingConfiguration
    {
        public void Map(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}