using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingCapacityManagement.Models;
using TrainingCapacityManagement.Data;
using Message = TrainingCapacityManagement.Models.Message;
using Microsoft.AspNetCore.Authorization;

namespace TrainingCapacityManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MessagesController : Controller
    {
        private readonly TrainingCapacityDefaultContext _context;

        public MessagesController(TrainingCapacityDefaultContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Message.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] Message msg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(msg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(msg);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var msg = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null)
            {
                return NotFound();
            }

            return View(msg);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var msg = await _context.Message.FindAsync(id);
            _context.Message.Remove(msg);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
