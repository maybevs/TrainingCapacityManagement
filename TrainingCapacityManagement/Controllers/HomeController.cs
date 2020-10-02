using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize]
        public async Task<IActionResult> StartAsync()
        {
            var uId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var allRegs = await _context.TrainingsRegistration.Include(tr => tr.Training).Include(tr => tr.Training.Sport).Include(tr => tr.Training.Gym).OrderBy(tr => tr.Training.StartTime).ToListAsync();
            var myTrainings = allRegs.Where(tr => tr.UserId == uId).ToList();
            var myInterests = await _context.Interest.Include(i => i.Sport).Include(i => i.User).Where(i => i.User.Id == uId).Select(i => i.Sport).ToListAsync();

            if (myInterests.Count == 0)
            {
                return RedirectToAction("Create", "Interests");
            }


            ViewBag.MyTrainings = myTrainings;


            var trainings = await _context.Training.Include(s => s.Sport).Include(s => s.Gym).OrderBy(s => s.StartTime).Where(s => myInterests.Contains(s.Sport)).ToListAsync();
            //var cutoffDate = DateTime.Now.AddDays(1).AddHours(5);
            //var dateTest = DateTime.Compare(new DateTime(2020, 10, 01, 20, 00,00), cutoffDate);
            trainings = trainings.Where(s => DateTime.Compare(s.PublishingDate,DateTime.Now.AddHours(2)) == -1).Where(s => DateTime.Compare(s.StartTime.AddHours(6), DateTime.Now) >= 0).ToList();


            Dictionary<Training, int> trainingsWithRemainingCapa = new Dictionary<Training, int>();

            foreach (var training in trainings)
            {
                var regs = allRegs.Where(reg => reg.Training == training).ToList();
                if(regs.Count >= training.Capacity)
                {
                    trainingsWithRemainingCapa.Add(training, 0);
                }
                else
                {
                    trainingsWithRemainingCapa.Add(training, training.Capacity - regs.Count);
                }
            }

            ViewBag.Trainings = trainingsWithRemainingCapa;
            

            try
            {
                var UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.User = UserId;
            }
            catch (Exception ex)
            {
                ViewBag.User = "No User found";
            }

            var msgs = await _context.Message.ToListAsync();

            ViewBag.Messages = msgs;


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Impressum()
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
