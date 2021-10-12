using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;
using Targetcom.Models.ViewModels;

namespace Targetcom.Controllers
{
    [Authorize(Roles = "Moderator,Administrator")]
    public class ModerController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ModerController(TargetDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Profile

        public async Task<IActionResult> Users(string id, int page = 0)
        {
            Profile select = null;
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    select = find;
                }
            }
            UserManagement userManagement = new UserManagement()
            {
                ProfileManages = _db.Profiles.Select(s => new ProfileManage()
                {
                    Profile = s
                }).ToList().Skip(page * Env.MANAGEPANEL_PEOPLES_LOADING_LIMIT).Take(Env.MANAGEPANEL_PEOPLES_LOADING_LIMIT).ToList(),
                SelectUser = select,
                Current_User_Page = page,
            };
            userManagement.ProfileManages.ToList().ForEach(i =>
            {
                i.Roles = _userManager.GetRolesAsync(i.Profile).Result as List<string>;
            });
            if (select is not null)
            {
                userManagement.SelectRoles = await _userManager.GetRolesAsync(select) as List<string>;
                userManagement.SelectUser.PaypalHistories = _db.PaypalHistories.Where(w => w.ProfileId == userManagement.SelectUser.Id).ToList();
                userManagement.SelectUser.Banned = _db.BannedProfiles.FirstOrDefault(f => f.ProfileId == userManagement.SelectUser.Id);
            }
            userManagement.Current_User_Lenght = _db.Profiles.Count() - 1;
            return View(userManagement);
        }

        public IActionResult SelectUser(string id)
        {
            if (id is not null)
            {
                var user = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (user is not null)
                {
                    return RedirectToAction(nameof(Users), new { id = id });
                }
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult EditGeneral(string id, string email, string name, string surname, string age, string telephone, string gender)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    if (email != null && name != null && surname != null && age != null && gender != null)
                    {
                        find.Name = name;
                        find.Surname = surname;
                        find.Age = DateTime.Parse(age);
                        find.PhoneNumber = telephone;
                        find.Gender = gender;
                        _db.Profiles.Update(find);
                        _db.SaveChanges();
                        return RedirectToAction(nameof(Users), new { id = find.Id });
                    }
                }
            }
            return RedirectToAction(nameof(Users));
        }
        [HttpPost]
        public IActionResult EditAbout(string id, string qouote, string image, string job, string hobby, string study, string status)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    find.UrlAvatar = image;
                    find.JobGeoplace = job;
                    find.StudyGeoplace = study;
                    find.Status = status;
                    find.Quote = qouote;
                    find.Hobbies = hobby;
                    _db.Profiles.Update(find);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Users), new { id = find.Id });
                }
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult EditPrivacy(string id, string privacy)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    find.Privacy = privacy;
                    _db.Profiles.Update(find);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Users), new { id = find.Id });
                }
            }
            return RedirectToAction(nameof(Users));
        }
      
        public IActionResult SetPremium(string id)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    if (!find.IsPremium)
                    {
                        find.IsPremium = true;
                        _db.Profiles.Update(find);
                        _db.SaveChanges();
                        return RedirectToAction(nameof(Users), new { id = find.Id });
                    }
                    else
                    {
                        find.IsPremium = false;
                        _db.Profiles.Update(find);
                        _db.SaveChanges();
                        return RedirectToAction(nameof(Users), new { id = find.Id });
                    }
                }
            }
            return RedirectToAction(nameof(Users));
        }

        #endregion

        #region News
        public IActionResult News(int? id, int page = 0)
        {

            var Profiles = _db.Profiles;
            var SharedProfilePostages = _db.SharedProfilePostages;
            var LikedProfilePostages = _db.LikedProfilePostages;

            var ProfilePostages = _db.ProfilePostages;
            ProfilePostages.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Writter = Profiles.Find(i.WritterId);
                i.LikedProfiles = LikedProfilePostages.Where(l => l.ProfilePostageId == i.Id).ToList();
                i.SharedProfiles = SharedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
            });

            var ProfilePostageComments = _db.ProfilePostageComments;
            ProfilePostageComments.ToList().ForEach(i =>
            {
                i.Postage = ProfilePostages.FirstOrDefault(f => f.Id == i.PostageId);
                i.ProfileCommentator = Profiles.Find(i.ProfileCommentatorId);
            });

            ProfilePostages.ToList().ForEach(i =>
            {
                i.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == i.ProfileCommentatorId).ToList();
            });

            ProfilePostage select = null;
            if (id is not null)
            {
                var findselect = ProfilePostages.Find(id);
                if (findselect is not null)
                {
                    select = findselect;
                    select.ProfilePostageComments = ProfilePostageComments.Where(w => w.PostageId == select.Id).ToList();
                }
            }

            ManagementNews managementNews = new ManagementNews()
            {
                ProfilePostages = ProfilePostages.ToList(),
                SelectedProfilePostage = select,
                Current_News_Page = page
            };

            managementNews.Current_News_Lenght = _db.ProfilePostages.Count() - 1;
            managementNews.ProfilePostages = ProfilePostages.OrderByDescending(o => o.TimeStamp).ToList().Skip(page * Env.MANAGEPANEL_NEWS_LOADING_LIMIT).Take(Env.MANAGEPANEL_NEWS_LOADING_LIMIT);

            return View(managementNews);
        }

        [HttpPost]
        public IActionResult DeletePostagePost(int? id)
        {
            ;
            if (id is not null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage is not null)
                {
                    _db.ProfilePostages.Remove(findedPostage);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(News));
                }
            }
            return NoContent();
        }

        public IActionResult SelectPost(int? id)
        {
            if (id is not null)
            {
                var find = _db.ProfilePostages.Find(id);
                if (find is not null)
                {
                    return RedirectToAction(nameof(News), new { id = id });
                }
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult DeleteProfilePostageCommentPost(int? id, int? post)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostageComments.Find(id);
                _db.ProfilePostageComments.Remove(findedPostage);
                _db.SaveChanges();
            }
            if (post is not null)
            {
                return RedirectToAction(nameof(News), new { id = post });
            }
            return RedirectToAction(nameof(News));
        }
        #endregion

        #region Games
        public IActionResult Games(string alert = null, int page = 0)
        {
            var games = _db.Games.ToList();
            string tryalert = "Select a game";
            if (alert is not null)
            {
                tryalert = alert;
            }

            ManagementGames managementGames = new ManagementGames()
            {
                Games = _db.Games.ToList(),
                Alert = tryalert,
                Current_Games_Page = page
            };

            managementGames.Current_Games_Lenght = _db.Games.Count() - 1;
            managementGames.Games = _db.Games.ToList().Skip(page * Env.MANAGEPANEL_GAMES_LOADING_LIMIT).Take(Env.MANAGEPANEL_GAMES_LOADING_LIMIT);

            return View(managementGames);
        }

        [HttpPost]
        public IActionResult SaveSelectedGame(string gameid, string gameurlimage, string gameurlgame, string gamename, string gamedesc, string gameprice)
        {
            if (gameid is not null)
            {
                var findgame = _db.Games.Find(int.Parse(gameid));
                bool ischange = false;
                if (findgame is not null)
                {
                    if (gameurlgame is not null && gameurlimage is not null && gamename is not null && gamedesc is not null && gameprice is not null)
                    {
                        if (gameurlgame != findgame.GameUrl)
                        {
                            findgame.GameUrl = gameurlgame;
                            ischange = true;
                        }
                        if (gameurlimage != findgame.ImageUrl)
                        {
                            findgame.ImageUrl = gameurlimage;
                            ischange = true;
                        }
                        if (gamename != findgame.Name)
                        {
                            findgame.Name = gamename;
                            ischange = true;
                        }
                        if (gamedesc != findgame.Description)
                        {
                            findgame.Description = gamedesc;
                            ischange = true;
                        }
                        if (gameprice != findgame.TargetPrice.ToString())
                        {
                            findgame.TargetPrice = int.Parse(gameprice);
                            ischange = true;
                        }
                        if (ischange)
                        {
                            _db.Games.Update(findgame);
                            _db.SaveChanges();
                            return RedirectToAction(nameof(Games), new { alert = "Game is changed" });
                        }
                    }
                }
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult RemoveSelectedGame(string gameid)
        {
            if (gameid is not null)
            {
                var findgame = _db.Games.Find(int.Parse(gameid));
                _db.Games.Remove(findgame);
                _db.SaveChanges();
                return RedirectToAction(nameof(Games), new { alert = "Game is deleted" });
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult Addnewgame(string newurlimage, string newurlgame, string newname, string newdesc, string newprice)
        {
            if (newurlimage is not null && newurlgame is not null && newname is not null && newdesc is not null && newprice is not null)
            {
                int price = 0;
                int.TryParse(newprice, out price);
                _db.Games.Add(new Game()
                {
                    Name = newname,
                    Description = newdesc,
                    TargetPrice = price,
                    GameUrl = newurlgame,
                    ImageUrl = newurlimage,
                });
                _db.SaveChanges();
                return RedirectToAction(nameof(Games), new { alert = "Game is added" });
            }
            return NoContent();
        }

        #endregion
    }
}
