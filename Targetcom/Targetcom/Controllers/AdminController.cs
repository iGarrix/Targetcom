using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;
using Targetcom.Models.ViewModels;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

namespace Targetcom.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(TargetDbContext db, UserManager<IdentityUser> userManager)
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
                    Profile = s,
                    
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

        [HttpPost]
        public IActionResult EditBalance(string id, string coin, string crypt)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    if (coin is not null && crypt is not null)
                    {
                        find.TargetCoins = int.Parse(coin);
                        find.CryptCoins = int.Parse(crypt);
                        _db.Profiles.Update(find);
                        _db.SaveChanges();
                        return RedirectToAction(nameof(Users), new { id = find.Id });
                    }
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

        public async Task<IActionResult> SetAdmin(string id, string admpassword)
        {
            if (id is not null)
            {
                if (admpassword is not null)
                {
                    if (admpassword == Env.ToggleAdminPassword)
                    {
                        var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                        if (find is not null)
                        {
                            if ((await _userManager.GetRolesAsync(find)).Contains(Env.AdminRole))
                            {
                                var result = await _userManager.RemoveFromRoleAsync(find, Env.AdminRole);
                                if (result.Succeeded)
                                {
                                    return RedirectToAction(nameof(Users), new { id = find.Id });
                                }
                            }
                            else
                            {
                                var result = await _userManager.AddToRoleAsync(find, Env.AdminRole);
                                if (result.Succeeded)
                                {
                                    return RedirectToAction(nameof(Users), new { id = find.Id });
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> SetModer(string id)
        {
            if (id is not null)
            {
                var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    if ((await _userManager.GetRolesAsync(find)).Contains(Env.ModerRole))
                    {
                        var result = await _userManager.RemoveFromRoleAsync(find, Env.ModerRole);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Users), new { id = find.Id });
                        }
                    }
                    else
                    {
                        var result = await _userManager.AddToRoleAsync(find, Env.ModerRole);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Users), new { id = find.Id });
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DeleteUser(string id, string delpassword)
        {
            if (id is not null)
            {
                if (delpassword is not null)
                {
                    if (delpassword == Env.DeletePersonalAccountPassword)
                    {
                        var user = _db.Profiles.FirstOrDefault(f => f.Id == id);
                        if (user is not null)
                        {
                            var myprofile = user as Profile;
                            var postages = _db.ProfilePostages.Where(i => i.ProfileId == myprofile.Id);
                            var rooms = _db.MessageGroups.Where(w => w.AdminId == myprofile.Id || w.FriendId == myprofile.Id);
                            var messages = _db.Messages.Where(w => w.ProfileId == myprofile.Id);
                            var friendships = _db.Friendships.Where(w => w.FriendId == myprofile.Id || w.ProfileId == myprofile.Id);
                            var cases = _db.Cases.Where(w => w.ProfileId == myprofile.Id);
                            var likes = _db.LikedProfilePostages.Where(w => w.ProfileId == myprofile.Id);
                            var comments = _db.ProfilePostageComments.Where(w => w.ProfileCommentatorId == myprofile.Id);
                            var banned = _db.BannedProfiles.Where(w => w.ProfileId == myprofile.Id);
                            var games = _db.ProfileGames.Where(w => w.ProfileId == myprofile.Id);
                            var shareds = _db.SharedProfilePostages.Where(w => w.ProfileId == myprofile.Id);
                            var paypalhistory = _db.PaypalHistories.Where(w => w.ProfileId == myprofile.Id);

                            _db.SharedProfilePostages.RemoveRange(shareds);
                            _db.ProfileGames.RemoveRange(games);
                            _db.BannedProfiles.RemoveRange(banned);
                            _db.ProfilePostageComments.RemoveRange(comments);
                            _db.LikedProfilePostages.RemoveRange(likes);
                            _db.Cases.RemoveRange(cases);
                            _db.Friendships.RemoveRange(friendships);
                            _db.Messages.RemoveRange(messages);
                            _db.MessageGroups.RemoveRange(rooms);
                            _db.ProfilePostageComments.RemoveRange(_db.ProfilePostageComments.Where(i => i.ProfileCommentatorId == myprofile.Id));
                            _db.ProfilePostages.RemoveRange(postages);
                            _db.PaypalHistories.RemoveRange(paypalhistory);
                            _db.SaveChanges();

                            var result = await _userManager.DeleteAsync(user);
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> SetBanned(string id, string reason, string reasondate, string ispermanent, string blcpassword)
        {
            var myprofile = await _userManager.GetUserAsync(User) as Profile;
            if (id is not null)
            {
                if (blcpassword is not null)
                {
                    if (blcpassword == Env.BannedPassword)
                    {
                        var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                        if (find is not null)
                        {
                            if (ispermanent is not null)
                            {
                                if (reason is null)
                                {
                                    reason = "Violation of site rights";
                                }
                                _db.BannedProfiles.Add(new BannedProfile()
                                {
                                    Admin = myprofile,
                                    Profile = find,
                                    TimeStamp = DateTime.Now,
                                    ReasonDate = DateTime.Now.AddHours(3),
                                    Reason = reason,
                                    IsPermanent = true,
                                });
                                _db.SaveChanges();
                            }
                            else
                            {
                                if (reasondate is not null)
                                {
                                    DateTime ReasonTime = DateTime.Parse(reasondate);
                                    if (reason is null)
                                    {
                                        reason = "Violation of site rights";
                                    }
                                    _db.BannedProfiles.Add(new BannedProfile()
                                    {
                                        Admin = myprofile,
                                        Profile = find,
                                        TimeStamp = DateTime.Now,
                                        ReasonDate = ReasonTime,
                                        Reason = reason,
                                        IsPermanent = false,
                                    });
                                    _db.SaveChanges();
                                }
                            }
                    
                            return RedirectToAction(nameof(Users), new { id = find.Id });
                            //return NotFound($"{reason} {ReasonTime.ToShortDateString()} {ispermanent} 11111111");
                        }
                    }
                }
            }
            return NoContent();
        }

        public IActionResult UnBanned(string id, string blcpassword)
        {
            if (id is not null)
            {
                if (blcpassword is not null)
                {
                    if (blcpassword == Env.BannedPassword)
                    {
                        var find = _db.Profiles.FirstOrDefault(f => f.Id == id);
                        if (find is not null)
                        {
                            var banned = _db.BannedProfiles.Where(w => w.ProfileId == find.Id);
                            _db.BannedProfiles.RemoveRange(banned);

                            _db.SaveChanges();
                            return RedirectToAction(nameof(Users), new { id = find.Id });
                        }
                    }
                }
            }
            return NoContent();
        }

        #endregion

        #region Crypto Rating

        public IActionResult Crypto(int ? id)
        {
            Cryptohistory select = null;
            if (id is not null)
            {
                var find = _db.Cryptohistories.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    select = find;
                }
            }
            ManagementCrypto managementCrypto = new ManagementCrypto()
            {
                Cryptohistories = _db.Cryptohistories.ToList(),
                SelectedCrypt = select,
            };
            return View(managementCrypto);
        }

        public IActionResult RemoveCrypt(int ? id)
        {
            if (id is not null)
            {
                var find = _db.Cryptohistories.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    _db.Cryptohistories.Remove(find);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Crypto));
        }
        public IActionResult EditCrypt(int? id, string rate, string singlecrypt)
        {
            if (id is not null)
            {
                var find = _db.Cryptohistories.FirstOrDefault(f => f.Id == id);
                if (find is not null)
                {
                    find.Rate = rate.ToString();
                    find.SingleCrypt = int.Parse(singlecrypt);
                    _db.Cryptohistories.Update(find);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Crypto));
        }

        public IActionResult AddCrypt(string newrate, string newsinglecrypt)
        {
            _db.Cryptohistories.Add(new Cryptohistory()
            {
                CreateTime = DateTime.Now,
                Rate = newrate,
                SingleCrypt = int.Parse(newsinglecrypt),
            });
            if (_db.Cryptohistories.Count() >= Env.CRYPTRATE_LIMIT)
            {
                var old = _db.Cryptohistories.OrderBy(o => o.CreateTime).FirstOrDefault();
                if (old is not null)
                {
                    _db.Cryptohistories.Remove(old);
                }
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Crypto));
        }
        public IActionResult GenerateCrypt()
        {
            int rate = new Random().Next(Env.LowRangeCryptoGenerated, Env.UpperRangeCryptoGenerated);
            int singe = new Random().Next(rate * 3000, rate * 6000);
            _db.Cryptohistories.Add(new Cryptohistory()
            {
                CreateTime = DateTime.Now,
                Rate = rate.ToString(),
                SingleCrypt = singe,
            });
            if (_db.Cryptohistories.Count() >= Env.CRYPTRATE_LIMIT)
            {
                var old = _db.Cryptohistories.OrderBy(o => o.CreateTime).FirstOrDefault();
                if (old is not null)
                {
                    _db.Cryptohistories.Remove(old);
                }
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Crypto));
        }
        #endregion

        #region Messanger
        public IActionResult Messenger(string user1id = null, string user2id = null, string state = null)
        {
            var Profiles = _db.Profiles;

            var ProfileGroups = _db.MessageGroups;
            ProfileGroups.ToList().ForEach(i =>
            {
                i.Admin = Profiles.Find(i.AdminId);
                i.Friend = Profiles.Find(i.FriendId);
            });

            var ProfileMessages = _db.Messages;
            ProfileMessages.ToList().ForEach(i =>
            {
                i.MessageGroup = ProfileGroups.Find(i.MessageGroupId);
                i.Profile = Profiles.Find(i.ProfileId);
            });

            ProfileGroups.ToList().ForEach(i =>
            {
                i.Messages = ProfileMessages.Where(w => w.MessageGroupId == i.Id).ToList();
            });

            Profiles.ToList().ForEach(f =>
            {
                f.Messages = ProfileMessages.Where(w => w.ProfileId == f.Id).ToList();
                f.ToMessageGroups = ProfileGroups.Where(w => w.AdminId == f.Id || w.FriendId == f.Id).ToList();
            });

            MessageGroup room = null;
            if (user1id is not null && user2id is not null)
            {
                var find = ProfileGroups.FirstOrDefault(f => (f.AdminId == user1id && f.FriendId == user2id) || (f.FriendId == user1id && f.AdminId == user2id));
                if (find is not null)
                {
                    room = find;
                }
            }


            var allowstate = "Select a room";
            if (state is not null)
            {
                allowstate = state;
            }
            ManagementMessenger managementMessenger = new ManagementMessenger()
            {
                Profiles = Profiles.ToList(),
                SelectRoom = room,
                State = allowstate,
            };
            return View(managementMessenger);
        }

        public IActionResult Savelog(int ? id)
        {
            if (id is not null)
            {
                var room = _db.MessageGroups.FirstOrDefault(f => f.Id == id);
                if (room is not null)
                {
                    room.Messages = _db.Messages.Where(w => w.MessageGroupId == room.Id).ToList();
                    room.Messages.ToList().ForEach(f =>
                    {
                        f.Profile = _db.Profiles.Find(f.ProfileId);
                    });

                    List<MessageDataJSON> messageJSON = new List<MessageDataJSON>();
                    room.Messages.ToList().ForEach(f =>
                    {
                        messageJSON.Add(new MessageDataJSON()
                        {
                            Sender = new Sender()
                            {
                                Name = f.Profile.Name,
                                Surname = f.Profile.Surname,
                                Email = f.Profile.Email,
                                HashedPassword = f.Profile.PasswordHash,
                            },
                            Message = f.Content,
                            SendedStamp = f.TimeStamp.ToString("ss:HH::dd.MM.yyyy"),
                        });
                    });

                    List<string> jsondata = new List<string>();

                    messageJSON.ForEach(f =>
                    {
                        jsondata.Add(JsonSerializer.Serialize(f));
                    });

                    string filename = $@"\data{DateTime.Now.ToString("ss.mm.hh.dd.MM.yyy")}.json";

                    var file = string.Join("\n", jsondata);

                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(file);

                    var content = new System.IO.MemoryStream(bytes);
                    return File(content, "application/json", filename);
                }
            }

            return RedirectToAction(nameof(Messenger), new { user1id = string.Empty, user2id = string.Empty, state = "Fail donwloading" });
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
        {;
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
        public IActionResult DeleteProfilePostageCommentPost(int? id, int ? post)
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
        public IActionResult Games(string alert = null)
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
                };
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
