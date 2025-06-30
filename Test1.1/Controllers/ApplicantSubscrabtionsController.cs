using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;

namespace Test1._1.Controllers
{
    public class ApplicantSubscrabtionsController : Controller
    {
        private readonly AppDBContext _context;

        public ApplicantSubscrabtionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: ApplicantSubscrabtions
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicantSubscrabtions.ToListAsync());
        }

        // GET: ApplicantSubscrabtions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSubscrabtion = await _context.ApplicantSubscrabtions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantSubscrabtion == null)
            {
                return NotFound();
            }

            return View(applicantSubscrabtion);
        }

        // GET: ApplicantSubscrabtions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicantSubscrabtions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubType,Price,NumAllowed")] ApplicantSubscrabtion applicantSubscrabtion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicantSubscrabtion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicantSubscrabtion);
        }

        // GET: ApplicantSubscrabtions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSubscrabtion = await _context.ApplicantSubscrabtions.FindAsync(id);
            if (applicantSubscrabtion == null)
            {
                return NotFound();
            }
            return View(applicantSubscrabtion);
        }

        // POST: ApplicantSubscrabtions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubType,Price,NumAllowed")] ApplicantSubscrabtion applicantSubscrabtion)
        {
            if (id != applicantSubscrabtion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantSubscrabtion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantSubscrabtionExists(applicantSubscrabtion.Id))
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
            return View(applicantSubscrabtion);
        }

        // GET: ApplicantSubscrabtions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantSubscrabtion = await _context.ApplicantSubscrabtions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantSubscrabtion == null)
            {
                return NotFound();
            }

            return View(applicantSubscrabtion);
        }

        // POST: ApplicantSubscrabtions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicantSubscrabtion = await _context.ApplicantSubscrabtions.FindAsync(id);
            if (applicantSubscrabtion != null)
            {
                _context.ApplicantSubscrabtions.Remove(applicantSubscrabtion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantSubscrabtionExists(int id)
        {
            return _context.ApplicantSubscrabtions.Any(e => e.Id == id);
        }
    }
}
