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
    public class FamilyController : Controller
    {
        private readonly BabysitterContext _context;

        public FamilyController(BabysitterContext context)
        {
            _context = context;
        }

        // GET: Family
        public async Task<IActionResult> Index()
        {
            return View(await _context.Family.ToListAsync());
        }

        // GET: Family/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Family
                .FirstOrDefaultAsync(m => m.Id == id);
            if (family == null)
            {
                return NotFound();
            }
            family.Kids = _context.Kid.Where(r => r.FamilyId == family.Id).ToList();
            return View(family);
        }

        // GET: Family/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Family/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,HomePhone,ContactPhone,ContactEmail,Bedtime,PartialPay,Cash")] Family family)
        {
            ValidateFamilyBedtimeTime(family);

            if (ModelState.IsValid && !_context.Family.Any(r => r.Name == family.Name))
            {
                _context.Add(family);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            family.Kids = _context.Kid.Where(r => r.FamilyId == family.Id).ToList();
            return View(family);
        }

        // GET: Family/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var familyRec = await _context.Family.FindAsync(id);
            if (familyRec == null)
            {
                return NotFound();
            }

            familyRec.Kids = _context.Kid.Where(r => r.FamilyId == familyRec.Id).ToList();
            ViewBag.FamilyId = FamilySelections();
            return View(familyRec);
        }

        // POST: Family/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HomePhone,ContactPhone,ContactEmail,Bedtime,PartialPay,Cash")] Family family)
        {
            if (id != family.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    family.Kids = _context.Kid.Where(r => r.FamilyId == family.Id).ToList();
                    _context.Update(family);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyExists(family.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                ViewBag.FamilyId = FamilySelections();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.FamilyId = FamilySelections();
            return View(family);
        }

        // GET: Family/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Family
                .FirstOrDefaultAsync(m => m.Id == id);
            if (family == null)
            {
                return NotFound();
            }

            return View(family);
        }

        // POST: Family/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var family = await _context.Family.FindAsync(id);
            family.Kids = _context.Kid.Where(r => r.FamilyId == family.Id).ToList();
            foreach (var familyKid in family.Kids)
            {
                _context.Kid.Remove(familyKid);
            }
            _context.Family.Remove(family);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyExists(int id)
        {
            return _context.Family.Any(e => e.Id == id);
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

        private void ValidateFamilyBedtimeTime(Family family)
        {
            var systemDefault = _context.DefaultSystem.Select(r => r).First();
            var mint = Convert.ToDateTime(systemDefault.MinTime);
            var maxt = Convert.ToDateTime(systemDefault.MaxTime);
            var bedtime = Convert.ToDateTime(family.Bedtime);

            if (bedtime < mint & bedtime > maxt)
                ModelState.AddModelError("Bedtime", "Time cannot start before 5pm or after 4am");

            if (family.Bedtime == null)
                ModelState.AddModelError("Bedtime", "Time required between 5 pm and 4 am");
        }
    }
}
