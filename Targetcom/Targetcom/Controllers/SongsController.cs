using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;
using Targetcom.Models.ViewModels;

namespace Targetcom.Controllers
{
    public class SongsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;
        public SongsController(UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            SongVM songVM = new SongVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                Friends = new List<Profile>(),
            };
            return View(songVM);
        }
    }
}
