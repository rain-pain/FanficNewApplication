using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanficNewApplication.Data;
using FanficNewApplication.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FanficNewApplication.Controllers
{
    public class FanficsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FanficsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        { 
            _context = context;
            _userManager = userManager;
        }

        // GET: Fanfics
        // GET: Fanfics
        public IActionResult Index()
        {
            var fandoms = _context.Fandom.ToList();
            var users = _context.Users.ToList();
            var fanfics = _context.Fanfic.ToList();

            return View(fanfics);
        }

        // GET: Fanfics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fanfic = await _context.Fanfic
                .Include(f => f.Author)
                .Include(f => f.Fandom)
                .FirstOrDefaultAsync(m => m.FanficId == id);
            if (fanfic == null)
            {
                return NotFound();
            }

            return View(fanfic);
        }

        // GET: Fanfics/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["FandomId"] = new SelectList(_context.Fandom, "Id", "FandomName");
            return View();
        }

        // POST: Fanfics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FanficId,FanficName,ShortDescription,FandomId,AuthorId")] Fanfic fanfic)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(fanfic.AuthorId);
                fanfic.AuthorId = user.Id;

                _context.Add(fanfic);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserFanfics");
            }
            ViewData["FandomId"] = new SelectList(_context.Set<Fandom>(), "Id", "Id", fanfic.FandomId);
            return View(fanfic);
        }

        // GET: Fanfics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fanfic = await _context.Fanfic.FindAsync(id);
            if (fanfic == null)
            {
                return NotFound();
            }
            ViewData["FandomId"] = new SelectList(_context.Set<Fandom>(), "Id", "FandomName", fanfic.FandomId);
            return View(fanfic);
        }

        // POST: Fanfics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FanficId,FanficName,ShortDescription,FandomId,AuthorId")] Fanfic fanfic)
        {
            if (id != fanfic.FanficId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(fanfic.AuthorId);
                    fanfic.AuthorId = user.Id;

                    _context.Update(fanfic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanficExists(fanfic.FanficId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UserFanfics");
            }
            ViewData["FandomId"] = new SelectList(_context.Set<Fandom>(), "Id", "FandomName", fanfic.FandomId);
            return View(fanfic);
        }

        // GET: Fanfics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fanfic = await _context.Fanfic
                .Include(f => f.Author)
                .Include(f => f.Fandom)
                .FirstOrDefaultAsync(m => m.FanficId == id);
            if (fanfic == null)
            {
                return NotFound();
            }

            return View(fanfic);
        }

        // POST: Fanfics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fanfic = await _context.Fanfic.FindAsync(id);
            _context.Fanfic.Remove(fanfic);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserFanfics");
        }

        [Authorize]
        public IActionResult UserFanfics()
        {
            var fandoms = _context.Fandom.ToList();
            var users = _context.Users.ToList();
            var fanfics = _context.Fanfic.ToList();

            var userFanfics = _context.Fanfic.Where(c => c.Author.UserName == User.Identity.Name).ToList();
            return View(userFanfics);
        }

        public IActionResult ShowFanfic(int fanficId)
        {
            var chapters = _context.Chapter.ToList();

            var fanfic = _context.Fanfic.Where(c => c.FanficId == fanficId).ToList().First();
            return View(fanfic);
        }

        private bool FanficExists(int id)
        {
            return _context.Fanfic.Any(e => e.FanficId == id);
        }
    }
}
