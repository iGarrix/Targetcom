using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Models;

namespace Targetcom.Controllers
{
    [Authorize]
    public class VipController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public VipController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Coins()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Coins(string package)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;

            if (package == Env.LitePackage)
            {
                profile.TargetCoins += Env.LitePackageCoin;
            }
            else if (package == Env.MiddlePackage)
            {
                profile.TargetCoins += Env.MiddlePackageCoin;
            }
            else if (package == Env.SuperPackage)
            {
                profile.TargetCoins += Env.SuperPackageCoin;
            }
            else if (package == Env.RichPackage)
            {
                profile.TargetCoins += Env.RichPackageCoin;
            }
            await _userManager.UpdateAsync(profile);
            return View();
        }
    }
}
