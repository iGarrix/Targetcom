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
    public class HelpController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;
        public HelpController(UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            var profile = await _userManager.GetUserAsync(User) as Profile;
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();
            return View(profile);
        }
    }
}
