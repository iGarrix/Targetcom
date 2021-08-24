using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;

namespace Targetcom.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public AchievementsController(TargetDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var identity = await _userManager.GetUserAsync(User) as Profile;
            return View(identity);
        }
    }
}
