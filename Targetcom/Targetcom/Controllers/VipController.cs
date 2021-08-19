using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class VipController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;
        public VipController(UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index(string error_message)
        {
            if (error_message is null)
            {
                error_message = string.Empty;
            }
            var profile = await _userManager.GetUserAsync(User) as Profile;
            if (profile.DroppedAvatar is not null)
            {
                if (!profile.DroppedAvatar.Split().ToList().Contains(Env.GridExclusive))
                {
                    if (profile.DroppedAvatar.Split().ToList().Skip(1).ToList().Count == 8)
                    {
                        var list = profile.DroppedAvatar.Split().ToList();
                        list.Add(Env.GridExclusive);
                        profile.DroppedAvatar = string.Join(" ", list);
                        await _userManager.UpdateAsync(profile);
                    }
                }
            }
            _db.Cases.RemoveRange(_db.Cases.Where(w => w.ProfileId == profile.Id && w.CaseCount == 0));
            _db.SaveChanges(); 
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();
            profile.Cases = _db.Cases.Where(w => w.ProfileId == profile.Id).ToList();
            return View(new Tuple<Profile, string>(profile, error_message));
        }

        public async Task<IActionResult> Coins()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();
            return View(profile);
        }

        public async Task<IActionResult> Opencase(string type)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;          
            profile.Cases = _db.Cases.Where(w => w.ProfileId == profile.Id).ToList();
            CaseVM defVM = new CaseVM()
            {
                IdentityProfile = profile,
                CaseType = type,
                Prize = new Tuple<string, int>("Empty", 0),
                Attempts = "0 attempts",
            };
            if (type == Env.DefaultCase)
            {
                if (profile.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase) is not null)
                {
                    if (profile.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase).CaseCount >= 1)
                    {                                             
                        var prize = GetSilverPrize();
                        if (prize.Item1 == Env.GridMolecular || prize.Item1 == Env.GridGhost ||
                            prize.Item1 == Env.GridSlime || prize.Item1 == Env.GridBlick ||
                            prize.Item1 == Env.GridCogti || prize.Item1 == Env.GridShark ||
                            prize.Item1 == Env.GridScan || prize.Item1 == Env.GridHeart)
                        {
                            var myavatar = profile.DroppedAvatar;
                            if (myavatar is null)
                            {
                                myavatar = "";
                            }
                            List<string> avatars = myavatar.Split().ToList();
                            if (avatars.Contains(prize.Item1))
                            {
                                CaseVM caseVM = new CaseVM()
                                {
                                    IdentityProfile = profile,
                                    CaseType = type,
                                    Prize = new Tuple<string, int>("coins", 40),
                                    Attempts = "success",
                                };
                                profile.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase).CaseCount -= 1;
                                await _userManager.UpdateAsync(profile);
                                return View(caseVM);
                            }
                            else
                            {
                                CaseVM caseVM = new CaseVM()
                                {
                                    IdentityProfile = profile,
                                    CaseType = type,
                                    Prize = prize,
                                    Attempts = "success",
                                };
                                avatars.Add(prize.Item1);
                                profile.DroppedAvatar = string.Join(" ", avatars);
                                profile.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase).CaseCount -= 1;
                                await _userManager.UpdateAsync(profile);
                                return View(caseVM);
                            }
                        }
                        else
                        {
                            CaseVM caseVM = new CaseVM()
                            {
                                IdentityProfile = profile,
                                CaseType = type,
                                Prize = prize,
                                Attempts = "success",
                            };
                            if (prize.Item1 == "coins" || prize.Item1 == "Jackpot")
                            {
                                profile.TargetCoins += prize.Item2;
                            }
                            else
                            {
                                var game = _db.Games.FirstOrDefault(f => f.Name == prize.Item1);
                                if (game is not null)
                                {
                                    await GiveGame(game.Id);
                                }
                            }
                            profile.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase).CaseCount -= 1;
                            await _userManager.UpdateAsync(profile);
                            return View(caseVM);
                        }                        
                    }              
                }
            }
            else if (type == Env.PremiumCase)
            {
                if (profile.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase) is not null)
                {
                    if (profile.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase).CaseCount >= 1)
                    {
                        var prize = GetPremiumPrize();
                        if (prize.Item1 == Env.GridMolecular || prize.Item1 == Env.GridGhost ||
                            prize.Item1 == Env.GridSlime || prize.Item1 == Env.GridBlick ||
                            prize.Item1 == Env.GridCogti || prize.Item1 == Env.GridShark ||
                            prize.Item1 == Env.GridScan || prize.Item1 == Env.GridHeart)
                        {
                            var myavatar = profile.DroppedAvatar;
                            if (myavatar is null)
                            {
                                myavatar = "";
                            }
                            List<string> avatars = myavatar.Split().ToList();
                            if (avatars.Contains(prize.Item1))
                            {
                                CaseVM caseVM = new CaseVM()
                                {
                                    IdentityProfile = profile,
                                    CaseType = type,
                                    Prize = new Tuple<string, int>("coins", 150),
                                    Attempts = "success",
                                };
                                profile.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase).CaseCount -= 1;
                                await _userManager.UpdateAsync(profile);
                                return View(caseVM);
                            }
                            else
                            {
                                CaseVM caseVM = new CaseVM()
                                {
                                    IdentityProfile = profile,
                                    CaseType = type,
                                    Prize = prize,
                                    Attempts = "success",
                                };
                                avatars.Add(prize.Item1);
                                profile.DroppedAvatar = string.Join(" ", avatars);
                                profile.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase).CaseCount -= 1;
                                await _userManager.UpdateAsync(profile);
                                return View(caseVM);
                            }
                        }
                        else
                        {
                            CaseVM caseVM = new CaseVM()
                            {
                                IdentityProfile = profile,
                                CaseType = type,
                                Prize = prize,
                                Attempts = "success",
                            };
                            if (prize.Item1 == "coins")
                            {
                                profile.TargetCoins += prize.Item2;
                            }
                            else if (prize.Item1 == "Fragment")
                            {
                                if (profile.Fragment < 7)
                                {
                                    profile.Fragment += 1;
                                }
                            }
                            else if (prize.Item1 == "Rainbow")
                            {
                                profile.RainbowMode = Env.RainbowDropped;
                            }
                            else if (!profile.ImageVidget)
                            {
                                if (prize.Item1 == "Widget")
                                {
                                    profile.ImageVidget = true;
                                }
                            }
                            else
                            {
                                var game = _db.Games.FirstOrDefault(f => f.Name == prize.Item1);
                                if (game is not null)
                                {
                                    await GiveGame(game.Id);
                                }
                            }
                            profile.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase).CaseCount -= 1;
                            await _userManager.UpdateAsync(profile);
                            return View(caseVM);
                        }
                    }
                }
            }

            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();
            return View(defVM);
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
            return RedirectToAction(nameof(Coins));
        }
        [HttpPost]
        public async Task<IActionResult> GetPremiumPost()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            if (!profile.IsPremium)
            {
                profile.IsPremium = true;
                await _userManager.UpdateAsync(profile);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BuypremiumPost()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            if (profile.TargetCoins >= Env.PremiumPrice)
            {
                profile.TargetCoins -= Env.PremiumPrice;
                profile.IsPremium = true;
                await _userManager.UpdateAsync(profile);
                return RedirectToAction(nameof(Index));
            }         
            return RedirectToAction(nameof(Index), new { error_message = "You don't have enough coins on your balance to buy premium plan" });
        }
        [HttpPost]
        public async Task<IActionResult> BuyDefCasePost(int dcasecounter)
        {
            var prof = await _userManager.GetUserAsync(User) as Profile;
            if (prof.TargetCoins >= (dcasecounter * 60))
            {
                prof.TargetCoins -= (dcasecounter * 60);
                await _userManager.UpdateAsync(prof);
            }
            else 
            {
                return RedirectToAction(nameof(Index));
            }

            if (prof.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase) is not null)
            {
                var findcase = prof.Cases.FirstOrDefault(f => f.CaseType == Env.DefaultCase);
                findcase.CaseCount += dcasecounter;
            }
            else
            {
                _db.Cases.Add(new Case()
                {
                    CaseType = Env.DefaultCase,
                    Profile = prof,
                    CaseCount = dcasecounter,
                });
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Opencase), new { type = Env.DefaultCase });
        }
        [HttpPost]
        public async Task<IActionResult> BuyPremCasePost(int pcasecounter)
        {
            var prof = await _userManager.GetUserAsync(User) as Profile;
            if (prof.TargetCoins >= (pcasecounter * 140))
            {
                prof.TargetCoins -= (pcasecounter * 140);
                await _userManager.UpdateAsync(prof);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            if (prof.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase) is not null)
            {
                var findcase = prof.Cases.FirstOrDefault(f => f.CaseType == Env.PremiumCase);
                findcase.CaseCount += pcasecounter;
            }
            else
            {
                _db.Cases.Add(new Case()
                {
                    CaseType = Env.PremiumCase,
                    Profile = prof,
                    CaseCount = pcasecounter,
                });
            }
            _db.SaveChanges();

            return RedirectToAction(nameof(Opencase), new { type = Env.PremiumCase });
        }

        [HttpPost]
        public async Task<IActionResult> SetGridAvatar(string name)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            profile.AvatarGridURL = name;
            await _userManager.UpdateAsync(profile); 
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UnsetGridAvatar()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            profile.AvatarGridURL = null;
            await _userManager.UpdateAsync(profile);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SetRainbow()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            if (profile.RainbowMode == Env.RainbowOff || profile.RainbowMode == Env.RainbowDropped)
            {
                profile.RainbowMode = Env.RainbowOn;
            }
            else
            {
                profile.RainbowMode = Env.RainbowOff;
            }
            await _userManager.UpdateAsync(profile);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SetColorTheme()
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            if (profile.IsDark)
            {
                profile.IsDark = false;
            }
            else
            {
                profile.IsDark = true;
            }
            await _userManager.UpdateAsync(profile);
            return RedirectToAction(nameof(Index));
        }
        
        public async Task GiveGame(int? id)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;
            var game = _db.Games.Find(id);

            if (game == null)
            {
                return;
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
                    profile.ProfileGames.Add(new ProfileGame()
                    {
                        Game = game,
                        Profile = profile,
                        Status = Env.GameStatusBuyed,
                    });

                }
                await _userManager.UpdateAsync(profile);
            }
        }

        private Tuple<string, int> GetSilverPrize()
        {
            Random row = new Random();
            Random col = new Random();

            var games = _db.Games.Where(w => w.TargetPrice < 1000).ToList();

            int x = col.Next(1, 102);

            if (x < 11)
            {
                Console.WriteLine("4 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridCogti, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 100));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 70);
                }
            }
            else if (x < 31)
            {
                Console.WriteLine("3 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridSlime, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 100));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 60);
                }
            }
            else if (x < 61)
            {
                Console.WriteLine("2 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridBlick, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 100));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 50);
                }
            }
            else if (x < 101)
            {
                Console.WriteLine("1 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridMolecular, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 100));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 45);
                }
            }
            if (x == 101)
            {
                return new Tuple<string, int>("Jackpot", 140);
            }
            return new Tuple<string, int>("Empty", 0);
        }
        private Tuple<string, int> GetPremiumPrize()
        {
            Random row = new Random();
            Random col = new Random();

            var games = _db.Games.Where(w => w.TargetPrice > 1000).ToList();

            int x = col.Next(1, 102);
            if (x < 5)
            {
                Console.WriteLine("5 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>("Fragment", 1);                   
                }
                else if (j < 41)
                {
                    return new Tuple<string, int>("Rainbow", 1);
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("Widget", 1);
                }
            }
            else if (x < 11)
            {
                Console.WriteLine("4 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridShark, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 200));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 160);
                }
            }
            else if (x < 31)
            {
                Console.WriteLine("3 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridScan, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 200));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 140);
                }
            }
            else if (x < 61)
            {
                Console.WriteLine("2 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridGhost, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 200));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 110);
                }
            }
            else if (x < 101)
            {
                Console.WriteLine("1 row");
                int j = row.Next(1, 101);
                if (j < 11)
                {
                    return new Tuple<string, int>(Env.GridHeart, 1);
                }
                else if (j < 41)
                {
                    if (games.Count > 0)
                    {
                        return new Tuple<string, int>(games[new Random().Next(0, games.Count)].Name, 1);
                    }
                    return new Tuple<string, int>("coins", new Random().Next(10, 200));
                }
                else if (j < 101)
                {
                    return new Tuple<string, int>("coins", 80);
                }
            }
            return new Tuple<string, int>("Empty", 0);
        }
    }
}
