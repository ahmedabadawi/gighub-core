using Microsoft.EntityFrameworkCore;

using GigHub.Web.Persistence;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Core
{
    public interface IUnitOfWork
    {
        IGigRepository Gigs { get; }
        IAttendanceRepository Attendances { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }

        void Complete();
    }
}