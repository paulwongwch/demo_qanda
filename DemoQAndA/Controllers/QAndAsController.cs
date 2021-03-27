using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoQAndA.Data;
using DemoQAndA.Models;
using Microsoft.AspNetCore.Authorization;

namespace DemoQAndA.Controllers
{
    public class QAndAsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QAndAsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QAndAs
        public async Task<IActionResult> Index()
        {
            return View(await _context.QAndA.ToListAsync());
        }

        // GET: QAndAs/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: QAndAs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.QAndA.Where(j => j.Question.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: QAndAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qAndA = await _context.QAndA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qAndA == null)
            {
                return NotFound();
            }

            return View(qAndA);
        }

        // GET: QAndAs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: QAndAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] QAndA qAndA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qAndA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qAndA);
        }

        // GET: QAndAs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qAndA = await _context.QAndA.FindAsync(id);
            if (qAndA == null)
            {
                return NotFound();
            }
            return View(qAndA);
        }

        // POST: QAndAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] QAndA qAndA)
        {
            if (id != qAndA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qAndA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QAndAExists(qAndA.Id))
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
            return View(qAndA);
        }

        // GET: QAndAs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qAndA = await _context.QAndA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qAndA == null)
            {
                return NotFound();
            }

            return View(qAndA);
        }

        // POST: QAndAs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qAndA = await _context.QAndA.FindAsync(id);
            _context.QAndA.Remove(qAndA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QAndAExists(int id)
        {
            return _context.QAndA.Any(e => e.Id == id);
        }
    }
}
