﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingCapacityManagement.Data;
using TrainingCapacityManagement.Models;
using System.Security.Claims;

namespace TrainingCapacityManagement.Controllers
{
    public class TrainingsRegistrationsController : Controller
    {
        private readonly TrainingCapacityDefaultContext _context;

        public TrainingsRegistrationsController(TrainingCapacityDefaultContext context)
        {
            _context = context;
        }

        // GET: TrainingsRegistrations
        public async Task<IActionResult> Index()
        {
            var UserID = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(await _context.TrainingsRegistration.Where(tr => tr.UserId == UserID).Include(tr => tr.Training).Include(tr => tr.Training.Sport).ToListAsync());
        }

        public async Task<IActionResult> TrainerView(int tid)
        {
            var registrations = await _context.TrainingsRegistration.Include(tr => tr.Training).Include(tr => tr.Training.Sport).Include(tr => tr.Training.Gym).Where(tr => tr.Training.Id == tid).ToListAsync();
            var users = await _context.Users.ToListAsync();
            ViewBag.Users = users;
            return View(registrations);
        }

        // GET: TrainingsRegistrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingsRegistration = await _context.TrainingsRegistration
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingsRegistration == null)
            {
                return NotFound();
            }

            return View(trainingsRegistration);
        }

        //// GET: TrainingsRegistrations/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // GET: TrainingsRegistrations/Create
        public async Task<IActionResult> CreateAsync(int? tid)
        {
            if (tid != null)
            {
                ViewBag.Id = tid;

                var training = await _context.Training.Where(t => t.Id == tid).Include(t => t.Sport).Include(t => t.Gym).FirstOrDefaultAsync();
                ViewBag.Training = training;
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: TrainingsRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tid")] TrainingsRegistration trainingsRegistration)
        {

            if (ModelState.IsValid)
            {
                trainingsRegistration.Training = await _context.Training.Where(t => t.Id == trainingsRegistration.tid).FirstOrDefaultAsync();
                trainingsRegistration.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Add(trainingsRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingsRegistration);
        }

        // GET: TrainingsRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingsRegistration = await _context.TrainingsRegistration.FindAsync(id);
            if (trainingsRegistration == null)
            {
                return NotFound();
            }
            return View(trainingsRegistration);
        }

        // POST: TrainingsRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId")] TrainingsRegistration trainingsRegistration)
        {
            if (id != trainingsRegistration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingsRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingsRegistrationExists(trainingsRegistration.Id))
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
            return View(trainingsRegistration);
        }

        // GET: TrainingsRegistrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingsRegistration = await _context.TrainingsRegistration
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingsRegistration == null)
            {
                return NotFound();
            }

            return View(trainingsRegistration);
        }

        // POST: TrainingsRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingsRegistration = await _context.TrainingsRegistration.FindAsync(id);
            _context.TrainingsRegistration.Remove(trainingsRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingsRegistrationExists(int id)
        {
            return _context.TrainingsRegistration.Any(e => e.Id == id);
        }
    }
}
