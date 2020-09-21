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
    [Authorize(Roles = "Admin,Trainer")]
    public class TrainingsController : Controller
    {
        private readonly TrainingCapacityDefaultContext _context;

        public TrainingsController(TrainingCapacityDefaultContext context)
        {
            _context = context;
        }

        // GET: Trainings
        
        public async Task<IActionResult> Index()
        {
            var trainings = _context.Training.Include(t => t.Sport).Include(t => t.Gym).Where(t => DateTime.Compare(t.StartTime,DateTime.Now.AddDays(1)) == 1);
            return View(await trainings.ToListAsync());
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        public async Task<IActionResult> CreateAsync()
        {
            var sports = await _context.Sport.ToListAsync();
            ViewBag.Sports = sports;
            var gyms = await _context.Gym.ToListAsync();
            ViewBag.Gyms = gyms;
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,EndTime,Capacity,HouseholdsSharePlace,SignupMessage,SafetyConceptURL,SportsSelection,GymSelection,PublishingDate")] Training training)
        {
            if (ModelState.IsValid)
            {
                var sport = _context.Sport.Where(s => s.Id == training.SportsSelection).FirstOrDefault();
                training.Sport = sport;
                var gym = _context.Gym.Where(g => g.Id == training.GymSelection).FirstOrDefault();
                training.Gym = gym;
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,Capacity,HouseholdsSharePlace,SignupMessage,SafetyConceptURL")] Training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
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
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .FirstOrDefaultAsync(m => m.Id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Training.FindAsync(id);

            var trainingsRegistrations = await _context.TrainingsRegistration.Include(tr => tr.Training).Where(tr => tr.Training.Id == training.Id).ToListAsync();
            foreach(var tr in trainingsRegistrations)
            {
                _context.TrainingsRegistration.Remove(tr);
            }



            _context.Training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.Id == id);
        }
    }
}
