using FanficNewApplication.Data;
using FanficNewApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FanficNewApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var authors = _context.Users.ToList();

            return View(authors);
        }

        public IActionResult AuthorFanfics(string author)
        {

            var authorFanfics = _context.Fanfic.Include(x => x.Fandom).Include(x => x.Author).Where(c => c.Author.UserName == author).ToList();
            return View(authorFanfics);
        }
    }
}
