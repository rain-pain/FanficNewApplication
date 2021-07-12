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
using FanficNewApplication.ViewModels;

namespace FanficNewApplication.Controllers
{
    [Authorize]
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChaptersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chapters
        public IActionResult Index(int fanficId)
        {
            var chapters = _context.Chapter.ToList();
            var fanfic = _context.Fanfic.Where(c => c.FanficId == fanficId).ToList().First();

            return View(fanfic);
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapter
                .Include(c => c.Fanfic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // GET: Chapters/Create
        public IActionResult Create(int idFanfic)
        {
            var model = new ChapterIdFanficViewModel();
            model.chapter = new Chapter();
            model.idFanfic = idFanfic;
            return View(model);
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,FanficId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowFanfic", "Fanfics", new { fanficId = chapter.FanficId });
            }
            ViewData["FanficId"] = new SelectList(_context.Fanfic, "FanficId", "FanficId", chapter.FanficId);
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public IActionResult Edit(int id, int chapterId)
        {

            var model = new ChapterIdFanficViewModel();
            var chapters = _context.Chapter.Where(c => c.FanficId == id).ToList();
            
            model.idFanfic = id;
            model.chapter = chapters[chapterId];

            return View(model);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,FanficId")] Chapter chapter)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ShowFanfic", "Fanfics", new { fanficId = id });
            }
            ViewData["FanficId"] = new SelectList(_context.Fanfic, "FanficId", "FanficId", chapter.FanficId);
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapter
                .Include(c => c.Fanfic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapter.FindAsync(id);
            _context.Chapter.Remove(chapter);
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowFanfic", "Fanfics", new { fanficId = id });
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapter.Any(e => e.Id == id);
        }
    }
}
