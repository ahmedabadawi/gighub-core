using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using GigHub.Web.Data;
using GigHub.Web.Core.Models;
using GigHub.Web.Core.Repositories;

namespace GigHub.Web.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGig(string gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public Gig GetGigWithAttendees(string gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances)
                    .ThenInclude(a => a.Attendee)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetUpcomingGigs()
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCancelled);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Gigs
                .Where(g => g.Attendances.Any(a => a.AttendeeId == userId))
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId)
        {
            return _context.Gigs
                .Where(g => g.ArtistId == artistId && 
                    g.DateTime > DateTime.Now && 
                    !g.IsCancelled)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}