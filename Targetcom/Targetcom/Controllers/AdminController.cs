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

namespace Targetcom.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly TargetDbContext _db;
        public AdminController(TargetDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

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

                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string path = Path.Combine(desktop + $@"\data{DateTime.Now.ToString("ss.mm.hh.dd.MM.yyy")}.json");

                    using (StreamWriter fs = new StreamWriter(path))
                    {
                        jsondata.ForEach(f =>
                        {
                            fs.WriteLine(JObject.Parse(f));
                        });
                    }
                }
            }

            return RedirectToAction(nameof(Messenger), new { user1id = string.Empty, user2id = string.Empty, state = "Messages downloaded" });
        }
        #endregion
        
        #region News
        public IActionResult News(int? id)
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
            };

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
