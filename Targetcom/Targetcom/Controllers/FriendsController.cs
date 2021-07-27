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
    public class FriendsController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public FriendsController(TargetDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<Profile> profiles = _db.Profiles;
            return View(profiles);
        }

        public IActionResult Newfriend()
        {
            IEnumerable<Profile> profiles = _db.Profiles;
            return View(profiles);
        }

        public async Task<IActionResult> ViewProfile(string id)
        {
            ViewProfileVM viewProfileVM = new ViewProfileVM()
            {
                FindedProfile = new Profile(),
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
            };
            IEnumerable<Profile> profiles = _db.Profiles;
            var profile = profiles.FirstOrDefault(i => i.Id == id);          
            if (profile == null)
            {
                return RedirectToAction(nameof(Newfriend));
            }
            var myprofile = await _userManager.GetUserAsync(User);
            viewProfileVM.Role = _userManager.GetRolesAsync(profile as IdentityUser).Result.ToList()[0];
            viewProfileVM.MyRole = _userManager.GetRolesAsync(myprofile as IdentityUser).Result.ToList()[0];
            viewProfileVM.FindedProfile = profile;

            return View(viewProfileVM);
        }
    }
}
