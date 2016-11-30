using System;
using System.Collections.Generic;
using System.Linq;

using GigHub.Web.Data;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList();
        }

        public Attendance GetAttendance(string gigId, string userId)
        {
            return _context.Attendances
                    .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == userId);
        }
    }
}