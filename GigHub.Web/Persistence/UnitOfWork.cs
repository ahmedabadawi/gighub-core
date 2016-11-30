using Microsoft.EntityFrameworkCore;

using GigHub.Web.Persistence;
using GigHub.Web.Core;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IGenreRepository Genres { get; private set; }

        public UnitOfWork(ApplicationDbContext context,
            IGigRepository gigRepository,
            IAttendanceRepository attendanceRepository,
            IFollowingRepository followingRepository,
            IGenreRepository genreRepository)
        {
            _context = context;

            Gigs = gigRepository;
            Attendances = attendanceRepository;
            Followings = followingRepository;
            Genres = genreRepository;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}