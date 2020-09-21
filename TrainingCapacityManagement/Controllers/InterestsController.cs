using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrainingCapacityManagement.Data;
using TrainingCapacityManagement.Models;

namespace TrainingCapacityManagement.Controllers
{
    [Authorize]
    public class InterestsController : Controller
    {
        private readonly TrainingCapacityDefaultContext _context;

        public InterestsController(TrainingCapacityDefaultContext context)
        {
            _context = context;
        }


        //Get: MyInterests Interests
        public async Task<IActionResult> Index()
        {
            var uId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var User = _context.Users.Where(u => u.Id == uId).FirstOrDefault();
            var myInterests = await _context.Interest.Where(i => i.User == User).Include(i => i.Sport).ToListAsync();
            return View(myInterests);
        }

        //Get Interests/Create
        public async Task<IActionResult> Create()
        {
            var sports = _context.Sport;
            List<SportsSelector> SportsSelectors = new List<SportsSelector>();
            var uId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var User = _context.Users.Where(u => u.Id == uId).FirstOrDefault();

            var myInterests = await _context.Interest.Where(i => i.User == User).ToListAsync();

            foreach (var item in sports)
            {

                SportsSelector _selector = new SportsSelector
                {
                    Sport = item,
                    IsChecked = false
                };

                if (myInterests.Where(i => i.Sport == item).Count() > 0)
                {
                    _selector.IsChecked = true;
                }


                SportsSelectors.Add(_selector);

            }


            return View(SportsSelectors);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<SportsSelector> sportsSelectors)
        {
            if (ModelState.IsValid)
            {
                var sports = _context.Sport;
                List<SportsSelector> SportsSelectors = new List<SportsSelector>();
                var uId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _context.Users.Where(u => u.Id == uId).FirstOrDefault();
                var myInterests = await _context.Interest.Where(i => i.User == user).ToListAsync();


                foreach (var sportSelected in sportsSelectors)
                {
                    if(!myInterests.Any(i => i.Sport == sportSelected.Sport) && sportSelected.IsChecked)
                    {
                        Interest interest = new Interest
                        {
                            User = user,
                            Sport = _context.Sport.Where(s => s.Name == sportSelected.Sport.Name).FirstOrDefault()
                        };

                        _context.Add(interest);
                    }
                }

                await _context.SaveChangesAsync();


            }


            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interest = await _context.Interest.Include(i => i.Sport).FirstOrDefaultAsync(i => i.Id == id);
            if (interest == null)
            {
                return NotFound();
            }

            return View(interest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var interest = await _context.Interest.FindAsync(id);
            _context.Interest.Remove(interest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
