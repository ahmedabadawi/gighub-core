using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using GigHub.Web.Data;
using GigHub.Web.Models;
using GigHub.Web.ViewModels;

namespace GigHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController (
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var upcomingGigs = 
                _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now)
                .ToList();
            
            var viewModel = new GigsViewModel() {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
