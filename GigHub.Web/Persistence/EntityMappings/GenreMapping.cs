using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using GigHub.Web.Core.Models;


namespace GigHub.Web.Persistence.EntityMappings
{
    public class GenreMapping : IEntityMappingConfiguration
    {
        public void Map(ModelBuilder builder)
        {
            builder.Entity<Genre>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(255);
            
        }
    }
}