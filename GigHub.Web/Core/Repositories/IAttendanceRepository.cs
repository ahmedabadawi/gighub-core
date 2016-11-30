using System.Collections.Generic;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        Attendance GetAttendance(string gigId, string userId);
    }
}