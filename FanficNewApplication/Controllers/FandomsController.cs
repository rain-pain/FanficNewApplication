using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanficNewApplication.Data;
using FanficNewApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using FanficNewApplication.ViewModels;

namespace FanficNewApplication.Controllers
{
    public class FandomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FandomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fandoms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fandom.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fandom = await _context.Fandom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fandom == null)
            {
                return NotFound();
            }

            return View(fandom);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fandoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FandomName,ShortDescription")] Fandom fandom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fandom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fandom);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fandom = await _context.Fandom.FindAsync(id);
            if (fandom == null)
            {
                return NotFound();
            }
            return View(fandom);
        }

        // POST: Fandoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FandomName,ShortDescription")] Fandom fandom)
        {
            if (id != fandom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fandom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FandomExists(fandom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fandom);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fandom = await _context.Fandom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fandom == null)
            {
                return NotFound();
            }

            return View(fandom);
        }

        // POST: Fandoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fandom = await _context.Fandom.FindAsync(id);
            _context.Fandom.Remove(fandom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ShowFandomFanfics(string fandomName)
        {
            var fanfics = _context.Fanfic.ToList();
            var users = _context.Users.ToList();
            var fandom = _context.Fandom.Where(c => c.FandomName == fandomName).ToList().First();

            var model = new FanficsOfFandomViewModel
            {
                Fandom = fandom
            };
            if (fandom.Fanfic != null)
            {
                model.Fanfics = fandom.Fanfic.ToList();
            }
            else {
                model.Fanfics = null;
            }
            return View(model);
        }

        private bool FandomExists(int id)
        {
            return _context.Fandom.Any(e => e.Id == id);
        }
    }
}
