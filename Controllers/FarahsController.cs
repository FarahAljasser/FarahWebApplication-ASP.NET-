using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarahWebApplication.Data;
using FarahWebApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace FarahWebApplication.Controllers
{
    public class FarahsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarahsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Farahs
        public async Task<IActionResult> Index()
        {
              return _context.Farah != null ? 
                          View(await _context.Farah.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Farah'  is null.");
        }

        // GET: Farahs //ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Farahs //ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Farah.Where(j => j.FarahQuestion.Contains
            (SearchPhrase)).ToListAsync());
        }


        // GET: Farahs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Farah == null)
            {
                return NotFound();
            }

            var farah = await _context.Farah
                .FirstOrDefaultAsync(m => m.id == id);
            if (farah == null)
            {
                return NotFound();
            }

            return View(farah);
        }

        // GET: Farahs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farahs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,FarahQuestion,FarahAnswer")] Farah farah)
        {
            if (ModelState.IsValid)
            {
                _context.Add(farah);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farah);
        }

        // GET: Farahs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Farah == null)
            {
                return NotFound();
            }

            var farah = await _context.Farah.FindAsync(id);
            if (farah == null)
            {
                return NotFound();
            }
            return View(farah);
        }

        // POST: Farahs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,FarahQuestion,FarahAnswer")] Farah farah)
        {
            if (id != farah.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farah);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarahExists(farah.id))
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
            return View(farah);
        }

        // GET: Farahs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Farah == null)
            {
                return NotFound();
            }

            var farah = await _context.Farah
                .FirstOrDefaultAsync(m => m.id == id);
            if (farah == null)
            {
                return NotFound();
            }

            return View(farah);
        }

        // POST: Farahs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Farah == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Farah'  is null.");
            }
            var farah = await _context.Farah.FindAsync(id);
            if (farah != null)
            {
                _context.Farah.Remove(farah);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarahExists(int id)
        {
          return (_context.Farah?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
