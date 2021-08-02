using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;
using Targetcom.Models.ViewModels;

namespace Targetcom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            NewsVM newsVM = new NewsVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                AllUsers = _db.Profiles,
            };
            return View(newsVM);
        }

        [AllowAnonymous]
        public IActionResult Rules()
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
