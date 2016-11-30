using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using GigHub.Web.Core.Models;


namespace GigHub.Web.Persistence.EntityMappings
{
    public class GigMapping : IEntityMappingConfiguration
    {
        public void Map(ModelBuilder builder)
        {
            builder.Entity<Gig>()
                .Property(g => g.Id)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Entity<Gig>()
                .Property(g => g.GenreId)
                .IsRequired();
            
            builder.Entity<Gig>()
                .Property(g => g.Venue)
                .IsRequired()
                .HasMaxLength(255);
            
        }
    }
}