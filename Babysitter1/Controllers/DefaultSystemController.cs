using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Babysitter1.Data;
using Babysitter1.Models;

namespace Babysitter1.Controllers
{
    public class DefaultSystemController : Controller
    {
        private readonly BabysitterContext _context;

        public DefaultSystemController(BabysitterContext context)
        {
            _context = context;
        }

        // GET: DefaultSystem
        public async Task<IActionResult> Index()
        {
            return View(await _context.DefaultSystem.ToListAsync());
        }

        // GET: DefaultSystem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultSystem = await _context.DefaultSystem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defaultSystem == null)
            {
                return NotFound();
            }

            return View(defaultSystem);
        }

        // GET: DefaultSystem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DefaultSystem/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MinTime,MaxTime,Standard,Evening,LateNite")] DefaultSystem defaultSystem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(defaultSystem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(defaultSystem);
        }

        // GET: DefaultSystem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultSystem = await _context.DefaultSystem.FindAsync(id);
            if (defaultSystem == null)
            {
                return NotFound();
            }
            return View(defaultSystem);
        }

        // POST: DefaultSystem/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MinTime,MaxTime,Standard,Evening,LateNite")] DefaultSystem defaultSystem)
        {
            if (id != defaultSystem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(defaultSystem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefaultSystemExists(defaultSystem.Id))
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
            return View(defaultSystem);
        }

        // GET: DefaultSystem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defaultSystem = await _context.DefaultSystem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defaultSystem == null)
            {
                return NotFound();
            }

            return View(defaultSystem);
        }

        // POST: DefaultSystem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var defaultSystem = await _context.DefaultSystem.FindAsync(id);
            _context.DefaultSystem.Remove(defaultSystem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefaultSystemExists(int id)
        {
            return _context.DefaultSystem.Any(e => e.Id == id);
        }
    }
}
