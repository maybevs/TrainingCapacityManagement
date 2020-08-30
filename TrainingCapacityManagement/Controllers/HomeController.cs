using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingCapacityManagement.Data;
using TrainingCapacityManagement.Models;

namespace TrainingCapacityManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TrainingCapacityDefaultContext _context;

        public HomeController(TrainingCapacityDefaultContext context,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.User = UserId;
                return RedirectToAction("Start");
            }
            catch (Exception ex)
            {
                ViewBag.User = "No User found";
            }
            //var UserRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            //ViewBag.Role = UserRole;
            return View();
        }

        public async Task<IActionResult> StartAsync()
        {
            var uId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var myTrainings = await _context.TrainingsRegistration.Where(tr => tr.UserId == uId).Include(tr => tr.Training).Include(tr => tr.Training.Sport).Include(tr => tr.Training.Gym).OrderBy(tr => tr.Training.StartTime).ToListAsync();
            ViewBag.MyTrainings = myTrainings;


            var trainings = _context.Training.Include(s => s.Sport).Include(s => s.Gym).OrderBy(s => s.StartTime);
            ViewBag.Trainings = await trainings.ToListAsync();

            try
            {
                var UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.User = UserId;
            }
            catch (Exception ex)
            {
                ViewBag.User = "No User found";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
