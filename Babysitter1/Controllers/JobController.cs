using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Babysitter1.Data;
using Babysitter1.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Babysitter1.Controllers
{
    public class JobController : Controller
    {
        private readonly BabysitterContext _context;

        public JobController(BabysitterContext context)
        {
            _context = context;
        }

        // GET: Job
        public async Task<IActionResult> Index()
        {
            var systemDefault = _context.DefaultSystem.Select(r => r).FirstOrDefault();
            if (systemDefault == null)
            {
                systemDefault = new DefaultSystem
                {
                    Evening = 8.0m,
                    LateNite = 16.0m,
                    Standard = 12.0m,
                    MaxTime = "4 am",
                    MinTime = "5 pm"
                };
                _context.Add(systemDefault);
                await _context.SaveChangesAsync();
            }
            return View(await _context.Job.Include("Family").ToListAsync());
        }

        // GET: Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            job.Family = _context.Family.FirstOrDefault(r => r.Id == job.FamilyId);
            return View(job);
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            ViewBag.FamilyId = FamilySelections();
            return View();
        }

        // POST: Job/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FamilyId,JobDate,StartTime,EndTime,Paid")] Job job)
        {
            ValidateJobStartAndEndTimes(job);
            job.Family = _context.Family.FirstOrDefault(r => r.Id == job.FamilyId);
            if (ModelState.IsValid)
            {
                if (job.Paid && job.Family != null)
                    AddOrUpdateTransaction(job);

                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.FamilyId = FamilySelections();
            return View(job);

        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            job.Family = _context.Family.FirstOrDefault(r => r.Id == job.FamilyId);
            ViewBag.FamilyId = FamilySelections();
            return View(job);
        }

        // POST: Job/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FamilyId,JobDate,StartTime,EndTime,Paid")] Job job)
        {
            if (id != job.Id)
                return NotFound();

            ValidateJobStartAndEndTimes(job);
            job.Family = _context.Family.FirstOrDefault(r => r.Id == job.FamilyId);
            if (ModelState.IsValid)
            {
                try
                {
                    if (job.Paid && job.Family != null)
                        AddOrUpdateTransaction(job);

                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.FamilyId = FamilySelections();
            return View(job);
        }

        private void AddOrUpdateTransaction(Job job)
        {
            var systemDefault = _context.DefaultSystem.Select(r => r).First();
            var standardHours = 0;
            standardHours = Convert.ToDateTime(job.EndTime).TimeOfDay >= Convert.ToDateTime(job.Family.Bedtime).TimeOfDay 
                ? (Convert.ToDateTime(job.Family.Bedtime) - Convert.ToDateTime(job.StartTime)).Hours 
                : (Convert.ToDateTime(job.EndTime) - Convert.ToDateTime(job.StartTime)).Hours;

            var eveningHours = 0;
            if (Convert.ToDateTime(job.EndTime).TimeOfDay > Convert.ToDateTime(job.Family.Bedtime).TimeOfDay)
                eveningHours = (job.JobDate.Date.AddDays(1).AddSeconds(-1) -
                                Convert.ToDateTime(job.Family.Bedtime)).Hours + 1;
            var overnightHours = 0;
            if ((Convert.ToDateTime(job.EndTime).TimeOfDay - job.JobDate.Date.TimeOfDay).Hours <= 0)
                overnightHours = Convert.ToDateTime(job.EndTime).TimeOfDay
                    .Subtract(job.JobDate.Date.TimeOfDay).Hours;
            var totalAmount = standardHours * systemDefault.Standard
                              + eveningHours * systemDefault.Evening
                              + overnightHours * systemDefault.LateNite;
            var totalHours = standardHours + eveningHours + overnightHours;

            var trans = new Transaction
            {
                Amount = totalAmount,
                EarlyHours = standardHours,
                EveningHours = eveningHours,
                LateHours = overnightHours,
                JobDate = job.JobDate,
                TotalHours = totalHours,
                FamilyName = job.Family.Name
            };
            var tran = _context.Transaction.FirstOrDefault(r => r.JobDate == job.JobDate
                                                                && r.FamilyName == job.Family.Name);
            if (tran != null)
            {
                trans.Id = tran.Id;
                _context.Transaction.Remove(tran);
            }

            _context.Update(trans);
        }

        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.Job.FirstOrDefaultAsync(m => m.Id == id);

            if (job == null)
                return NotFound();

            job.Family = _context.Family.FirstOrDefault(r => r.Id == job.FamilyId);
            ViewBag.FamilyId = FamilySelections();
            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Job.FindAsync(id);
            _context.Job.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
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

        private void ValidateJobStartAndEndTimes(Job job)
        {
            var systemDefault = _context.DefaultSystem.Select(r => r).First();
            var mint = Convert.ToDateTime(systemDefault.MinTime);
            var maxt = Convert.ToDateTime(systemDefault.MaxTime);
            var jobStart = Convert.ToDateTime(job.StartTime);
            var jobEnd = Convert.ToDateTime(job.EndTime);

            if (jobStart < mint & jobStart > maxt) 
                ModelState.AddModelError("StartTime", "Cannot start work after 4 am or before 5pm");

            if (jobEnd < mint & jobEnd > maxt)
                ModelState.AddModelError("EndTime", "Cannot end work after 4 am or before 5pm");

            if (jobStart == jobEnd)
                ModelState.AddModelError("EndTime", "Cannot be identical as the start time");

            if(job.StartTime == null) 
                ModelState.AddModelError("StartTime", "Start time required between 5 pm and 4 am");

            if(job.EndTime == null) 
                ModelState.AddModelError("EndTime", "End time required between 5 pm and 4 am");
        }
    }
}
