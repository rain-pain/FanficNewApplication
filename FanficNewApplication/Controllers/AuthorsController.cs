using FanficNewApplication.Data;
using FanficNewApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var authors = _context.Users.ToList();
            var fanfics = _context.Fanfic.ToList();

            return View(authors);
        }

        public IActionResult AuthorFanfics(string author)
        {
            var fandoms = _context.Fandom.ToList();
            var users = _context.Users.ToList();
            var fanfics = _context.Fanfic.ToList();

            var authorFanfics = _context.Fanfic.Where(c => c.Author.UserName == author).ToList();
            return View(authorFanfics);
        }
    }
}
