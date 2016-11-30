using System.Collections.Generic;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}