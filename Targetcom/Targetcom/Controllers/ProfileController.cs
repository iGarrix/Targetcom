using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Models;
using Targetcom.Models.ViewModels;

namespace Targetcom.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ProfileVM profileVM = new ProfileVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
            };
            if (profileVM.IdentityProfile.AfkStatus == Env.Offline)
            {
                profileVM.IdentityProfile.AfkStatus = Env.Online;
            }
            return View(profileVM);
        }
    }
}
