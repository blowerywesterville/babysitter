using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Babysitter1.Data;
using Babysitter1.Models;

namespace Babysitter1.Controllers
{
    public class KidController : Controller
    {
        private readonly BabysitterContext _context;

        public KidController(BabysitterContext context)
        {
            _context = context;
        }

        // GET: Kid
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kid.ToListAsync());
        }

        // GET: Kid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var kid = await _context.Kid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
                return NotFound();

            ViewBag.FamilyId = FamilySelections();
            kid.Family = _context.Family.First(r => r.Id == kid.FamilyId);
            return View(kid);
        }

        // GET: Kid/Create
        public IActionResult Create()
        {
            ViewBag.FamilyId = FamilySelections();
            return View();
        }

        // POST: Kid/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Bedtime,Age,Note,FamilyId")] Kid kid)
        {
            ValidateKidBedtimeTime(kid);
            kid.Family = _context.Family.First(r => r.Id == kid.FamilyId);
            if (ModelState.IsValid)
            {
                _context.Add(kid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kid);
        }

        // GET: Kid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var kid = await _context.Kid.FindAsync(id);
            if (kid == null)
                return NotFound();

            kid.Family = _context.Family.First(r => r.Id == kid.FamilyId);
            return View(kid);
        }

        // POST: Kid/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bedtime,Age,Note,FamilyId")] Kid kid)
        {
            if (id != kid.Id)
                return NotFound();

            kid.Family = _context.Family.First(r => r.Id == kid.FamilyId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KidExists(kid.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kid);
        }

        // GET: Kid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // POST: Kid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kid = await _context.Kid.FindAsync(id);
            _context.Kid.Remove(kid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KidExists(int id)
        {
            return _context.Kid.Any(e => e.Id == id);
        }

        private List<SelectListItem> FamilySelections()
        {
            var selections = new List<SelectListItem>();
            foreach (var family in _context.Family.Select(r => r))
            {
                selections.Add(new SelectListItem(family.Name, family.Id.ToString()));
            }

            return selections;
        }

        private void ValidateKidBedtimeTime(Kid kid)
        {
            var systemDefault = _context.DefaultSystem.Select(r => r).First();
            var mint = Convert.ToDateTime(systemDefault.MinTime);
            var maxt = Convert.ToDateTime(systemDefault.MaxTime);
            var bedtime = Convert.ToDateTime(kid.Bedtime);

            if (bedtime < mint & bedtime > maxt)
                ModelState.AddModelError("Bedtime", "Time cannot start before 5pm or after 4am");

            if (kid.Bedtime == null)
                ModelState.AddModelError("Bedtime", "Time required between 5 pm and 4 am");        }
    }
}
