using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using GigHub.Web.ViewModels;
using GigHub.Web.Data;

namespace GigHub.Web.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel() {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }
    }
}