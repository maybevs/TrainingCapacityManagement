using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingCapacityManagement.Data;
using TrainingCapacityManagement.Models;

namespace TrainingCapacityManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GymSegmentsController : Controller
    {
        private readonly TrainingCapacityDefaultContext _context;

        public GymSegmentsController(TrainingCapacityDefaultContext context)
        {
            _context = context;
        }

        // GET: GymSegments
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymSegment.ToListAsync());
        }

        // GET: GymSegments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSegment = await _context.GymSegment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymSegment == null)
            {
                return NotFound();
            }

            return View(gymSegment);
        }

        // GET: GymSegments/Create
        public IActionResult Create()
        {
            var gyms = _context.Gym.ToList();

            ViewBag.Gyms = gyms;
            return View();
        }

        // POST: GymSegments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GymSelection")] GymSegment gymSegment)
        {
            if (ModelState.IsValid)
            {
                var gym = _context.Gym.Where(g => g.Id == Convert.ToInt32(gymSegment.GymSelection)).First();
                gymSegment.Gym = gym;

                _context.Add(gymSegment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymSegment);
        }

        // GET: GymSegments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSegment = await _context.GymSegment.FindAsync(id);
            if (gymSegment == null)
            {
                return NotFound();
            }
            return View(gymSegment);
        }

        // POST: GymSegments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GymSegment gymSegment)
        {
            if (id != gymSegment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymSegment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymSegmentExists(gymSegment.Id))
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
            return View(gymSegment);
        }

        // GET: GymSegments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymSegment = await _context.GymSegment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymSegment == null)
            {
                return NotFound();
            }

            return View(gymSegment);
        }

        // POST: GymSegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymSegment = await _context.GymSegment.FindAsync(id);
            _context.GymSegment.Remove(gymSegment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymSegmentExists(int id)
        {
            return _context.GymSegment.Any(e => e.Id == id);
        }
    }
}
