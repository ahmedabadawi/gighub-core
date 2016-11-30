using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using GigHub.Web.Core.Models;


namespace GigHub.Web.Persistence.EntityMappings
{
    public class AttendanceMapping : IEntityMappingConfiguration
    {
        public void Map(ModelBuilder builder)
        {
            builder.Entity<Attendance>()
                .HasKey(a => new { a.GigId, a.AttendeeId });            
        }
    }
}