using System.Collections.Generic;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        Attendance GetAttendance(string gigId, string userId);
        void Add(Attendance attendance);
        void Remove(Attendance attendance);
    }
}