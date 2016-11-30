using System.Collections.Generic;
using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IGigRepository
    {
        Gig GetGig(string gigId);
        Gig GetGigWithAttendees(string gigId);
        IEnumerable<Gig> GetUpcomingGigs();
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId);
        void Add(Gig gig);
    }
}