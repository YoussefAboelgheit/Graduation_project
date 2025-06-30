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
    public class CompanySubscrabtionsController : Controller
    {
        private readonly AppDBContext _context;

        public CompanySubscrabtionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: CompanySubscrabtions
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanySubscraptions.ToListAsync());
        }

        // GET: CompanySubscrabtions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySubscrabtion = await _context.CompanySubscraptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companySubscrabtion == null)
            {
                return NotFound();
            }

            return View(companySubscrabtion);
        }

        // GET: CompanySubscrabtions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanySubscrabtions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubType,Price,NumAllowed")] CompanySubscrabtion companySubscrabtion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companySubscrabtion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companySubscrabtion);
        }

        // GET: CompanySubscrabtions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySubscrabtion = await _context.CompanySubscraptions.FindAsync(id);
            if (companySubscrabtion == null)
            {
                return NotFound();
            }
            return View(companySubscrabtion);
        }

        // POST: CompanySubscrabtions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubType,Price,NumAllowed")] CompanySubscrabtion companySubscrabtion)
        {
            if (id != companySubscrabtion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companySubscrabtion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanySubscrabtionExists(companySubscrabtion.Id))
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
            return View(companySubscrabtion);
        }

        // GET: CompanySubscrabtions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySubscrabtion = await _context.CompanySubscraptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companySubscrabtion == null)
            {
                return NotFound();
            }

            return View(companySubscrabtion);
        }

        // POST: CompanySubscrabtions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companySubscrabtion = await _context.CompanySubscraptions.FindAsync(id);
            if (companySubscrabtion != null)
            {
                _context.CompanySubscraptions.Remove(companySubscrabtion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySubscrabtionExists(int id)
        {
            return _context.CompanySubscraptions.Any(e => e.Id == id);
        }
    }
}
