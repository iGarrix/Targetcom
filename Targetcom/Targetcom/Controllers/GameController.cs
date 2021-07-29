using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class GameController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public GameController(TargetDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            GameVM gameVM = new GameVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                Games = _db.Games,
            };
            gameVM.IdentityProfile.ProfileGames = _db.ProfileGames.Where(i => i.ProfileId == gameVM.IdentityProfile.Id).ToList();
            return View(gameVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> IndexPost(int? id)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            var game = _db.Games.Find(id);

            if (game == null)
            {
                return NotFound();
            }
            if (profile != null)
            {
                var finded = _db.ProfileGames.FirstOrDefault(i => i.GameId == game.Id && i.ProfileId == profile.Id);
                if (finded != null)
                {
                    profile.ProfileGames.Remove(finded);
                    if (finded.Status == Env.GameStatusBuyed)
                    {
                        finded.Status = Env.GameStatusFollowed;
                    }
                    else
                    {
                        finded.Status = Env.GameStatusBuyed;
                    }
                    profile.ProfileGames.Add(finded);
                }
                else
                {
                    if (profile.TargetCoins >= game.TargetPrice || profile.IsPremium)
                    {
                        profile.ProfileGames.Add(new ProfileGame()
                        {
                            Game = game,
                            Profile = profile,
                            Status = Env.GameStatusBuyed,
                        });
                        if (!profile.IsPremium)
                        {
                            profile.TargetCoins -= game.TargetPrice;
                        }
                    }

                }
                await _userManager.UpdateAsync(profile);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult StartGame(string url, string name)
        {
            return View(new Game() { GameUrl = url, Name = name });
        }
    }
}
