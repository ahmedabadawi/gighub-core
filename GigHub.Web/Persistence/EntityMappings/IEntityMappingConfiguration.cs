using Microsoft.EntityFrameworkCore;

namespace GigHub.Web.Persistence.EntityMappings
{
    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder builder);
    }
}