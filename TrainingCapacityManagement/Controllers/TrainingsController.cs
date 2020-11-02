using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
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

        private readonly IEmailSender _emailSender;

        public TrainingsController(TrainingCapacityDefaultContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Trainings
        
        public async Task<IActionResult> Index()
        {
            var trainings = _context.Training.Include(t => t.Sport).Include(t => t.Gym).Where(t => DateTime.Compare(t.StartTime.AddDays(1), DateTime.Now) == 1).OrderBy(t => t.StartTime);
            var test = await trainings.ToListAsync();
            var history = await _context.Training.Include(t => t.Sport).Include(t => t.Gym).Where(t => DateTime.Compare(t.StartTime.AddDays(1), DateTime.Now) == -1).Where(t => DateTime.Compare(t.StartTime.AddMonths(1), DateTime.Now) == 1).OrderByDescending(t => t.StartTime).ToListAsync();
            ViewBag.History = history;
            var registrations = await _context.TrainingsRegistration.Include(r => r.Training).ToListAsync();
            ViewBag.Registrations = registrations;
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
                training.EndTime = new DateTime(training.StartTime.Year, training.StartTime.Month, training.StartTime.Day, training.EndTime.Hour, training.EndTime.Minute, 0);
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        //public async Task<IActionResult> CreateRepeatingAsync()
        //{
        //    var sports = await _context.Sport.ToListAsync();
        //    ViewBag.Sports = sports;
        //    var gyms = await _context.Gym.ToListAsync();
        //    ViewBag.Gyms = gyms;
        //    var rTraining = new RepeatingTraining
        //    {
        //        BaseTraining = new Training()
        //    };

        //    return View(rTraining);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRepeatingAsync([Bind("BaseTraining.Name,BaseTraining.StartTime,BaseTraining.EndTime,BaseTraining.Capacity,BaseTraining.HouseholdsSharePlace,BaseTraining.SignupMessage,BaseTraining.SafetyConceptURL,BaseTraining.SportsSelection,BaseTraining.GymSelection,BaseTraining.PublishingDate,Cadence")] RepeatingTraining rtraining)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var sport = _context.Sport.Where(s => s.Id == rtraining.BaseTraining.SportsSelection).FirstOrDefault();
        //        rtraining.BaseTraining.Sport = sport;
        //        var gym = _context.Gym.Where(g => g.Id == rtraining.BaseTraining.GymSelection).FirstOrDefault();
        //        rtraining.BaseTraining.Gym = gym;
        //        rtraining.BaseTraining.EndTime = new DateTime(rtraining.BaseTraining.StartTime.Year, rtraining.BaseTraining.StartTime.Month, rtraining.BaseTraining.StartTime.Day, rtraining.BaseTraining.EndTime.Hour, rtraining.BaseTraining.EndTime.Minute, 0);
        //        //_context.Add(training);
        //        //await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(rtraining);
        //}


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
            var Trainer = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _emailSender.SendEmailAsync("bernd.mayer@outlook.com", "Trainingsabsage", $"Der Trainer {Trainer} hat das Training {training.Name} am {training.StartTime} abgesagt");


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
