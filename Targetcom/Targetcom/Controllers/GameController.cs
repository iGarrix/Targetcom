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
using System.Text.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Index(int page = 0)
        {
            int pageSize = _db.Games.Count();
            GameVM gameVM = new GameVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                Games = _db.Games.ToList().Skip(page * Env.GAME_LOADING_LIMIT).Take(Env.GAME_LOADING_LIMIT),
                CurrentPage = page,
                GameLenght = _db.Games.Count(),
            };
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            var ProfGames = _db.ProfileGames;
            ProfGames.ToList().ForEach(i => 
            {
                i.Profile = Profiles.FirstOrDefault(f => f.Id == i.ProfileId);
                i.Game = _db.Games.FirstOrDefault(f => f.Id == i.GameId);
            });
            gameVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == gameVM.IdentityProfile.Id || w.ProfileId == gameVM.IdentityProfile.Id).ToList();
            gameVM.IdentityProfile.ProfileGames = ProfGames.Where(i => i.ProfileId == gameVM.IdentityProfile.Id).ToList();
            return View(gameVM);
        }

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
