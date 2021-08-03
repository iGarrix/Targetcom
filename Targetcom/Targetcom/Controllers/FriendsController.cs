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
    public class FriendsController : Controller
    {
        private readonly TargetDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public FriendsController(TargetDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string eventSelector = "None")
        {
            string myid = (await _userManager.GetUserAsync(User) as Profile).Id;
            FriendVM friendVM = new FriendVM()
            {
                IdentityUser = _db.Profiles.Find(myid),
                AllUsers = _db.Profiles,
                EventSetter = eventSelector,
            };
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });

            friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList();
            friendVM.AllUsers.ToList().ForEach(i =>
            {
                i.Friendships = ProfileFriendship.Where(w => w.FriendId == i.Id || w.ProfileId == i.Id).ToList();
            });
            return View(friendVM);
        }
        //public async Task<IActionResult> ViewProfile(string id)
        //{
        //    ViewProfileVM viewProfileVM = new ViewProfileVM()
        //    {
        //        FindedProfile = new Profile(),
        //        IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
        //    };

        //    var ProfilePostages = _db.ProfilePostages;
        //    var ProfilePostageComments = _db.ProfilePostageComments;
        //    var LikedProfilePostages = _db.LikedProfilePostages;
        //    var SharedProfilePostages = _db.SharedProfilePostages;
        //    var Profiles = _db.Profiles;

        //    IEnumerable<Profile> profiles = Profiles;
        //    var profile = profiles.FirstOrDefault(i => i.Id == id);
        //    if (profile == null)
        //    {
        //        return RedirectToAction(nameof(Newfriend));
        //    }
        //    var myprofile = await _userManager.GetUserAsync(User);
        //    viewProfileVM.Role = _userManager.GetRolesAsync(profile as IdentityUser).Result.ToList()[0];
        //    viewProfileVM.MyRole = _userManager.GetRolesAsync(myprofile as IdentityUser).Result.ToList()[0];

        //    var postage = ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
        //    postage.ForEach(i =>
        //    {
        //        var list = ProfilePostageComments.Where(s => s.PostageId == i.Id).ToList();
        //        list.ForEach(j =>
        //        {
        //            j.ProfileCommentator = Profiles.FirstOrDefault(f => f.Id == j.ProfileCommentatorId);
        //        });
        //        i.ProfilePostageComments = list;
        //    });
        //    postage.ForEach(i =>
        //    {
        //        i.LikedProfiles = LikedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
        //    });
        //    postage.ForEach(i =>
        //    {
        //        i.SharedProfiles = SharedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
        //    });
        //    profile.ProfilePostages = postage;
        //    profile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();

        //    var sharedpostage = SharedProfilePostages.ToList();
        //    sharedpostage.ForEach(i =>
        //    {
        //        i.Postage = ProfilePostages.FirstOrDefault(f => f.Id == i.ProfilePostageId);
        //        i.Postage.LikedProfiles = LikedProfilePostages.Where(w => w.ProfilePostageId == i.Postage.Id).ToList();
        //        i.Postage.ProfilePostageComments = ProfilePostageComments.Where(s => s.PostageId == i.Postage.Id).ToList();
        //    });
        //    sharedpostage.ForEach(i =>
        //    {
        //        i.Profile = Profiles.FirstOrDefault(f => f.Id == i.ProfileId);
        //    });
        //    profile.SharedProfilePostages = sharedpostage;

        //    viewProfileVM.FindedProfile = profile;

        //    return View(viewProfileVM);
        //}
        public async Task<IActionResult> ViewProfile(string id)
        {
            string myid = (await _userManager.GetUserAsync(User) as Profile).Id;
            ViewProfileVM viewProfileVM = new ViewProfileVM()
            {
                FindedProfile = new Profile(),
                IdentityProfile = _db.Profiles.Find(myid),
            };

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

            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });

            var profile = await _userManager.FindByIdAsync(id) as Profile;
            if (profile == null)
            {
                return RedirectToAction(nameof(Index));
            }
            viewProfileVM.Role = _userManager.GetRolesAsync(profile as IdentityUser).Result.ToList()[0];
            viewProfileVM.MyRole = _userManager.GetRolesAsync(viewProfileVM.IdentityProfile as IdentityUser).Result.ToList()[0];

            profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == profile.Id).ToList();

            viewProfileVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == viewProfileVM.IdentityProfile.Id || w.ProfileId == viewProfileVM.IdentityProfile.Id).ToList();
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();

            viewProfileVM.FindedProfile = profile;

            return View(viewProfileVM);
        }

        private async Task AddFriendMethod(string id)
        {
            if (id != null)
            {
                var findedFriend = _db.Profiles.Find(id);
                var identity = await _userManager.GetUserAsync(User) as Profile;
                if (findedFriend != null)
                {
                    identity.Friendships.Add(new Friendship()
                    {
                        Profile = identity,
                        Friend = findedFriend,
                        FriendStatus = Env.Invite,
                    });
                    await _userManager.UpdateAsync(identity);
                }
            }            
        }
        private Friendship RemoveFriendMethod(int? id)
        {
            if (id is not null)
            {
                var findedFriendship = _db.Friendships.FirstOrDefault(f => f.Id == id);
                if (findedFriendship is not null)
                {
                    _db.Friendships.Remove(findedFriendship);
                    _db.SaveChanges();
                    return findedFriendship;
                }
            }
            return null;
        }
        private Friendship AddInviteFriendMethod(int? id)
        {
            if (id is not null)
            {
                var findedFriendship = _db.Friendships.FirstOrDefault(f => f.Id == id);
                if (findedFriendship is not null)
                {
                    findedFriendship.FriendStatus = Env.Friend;
                    _db.Friendships.Update(findedFriendship);
                    _db.SaveChanges();
                    return findedFriendship;
                }
            }
            return null;
        }
        private Friendship LeaveSubscribersMethod(int? id)
        {
            if (id is not null)
            {
                var findedFriendship = _db.Friendships.FirstOrDefault(f => f.Id == id);
                if (findedFriendship is not null)
                {
                    findedFriendship.FriendStatus = Env.Subscribe;
                    _db.Friendships.Update(findedFriendship);
                    _db.SaveChanges();
                    return findedFriendship;
                }
            }
            return null;
        }
        private Friendship AddBlacklistFriendMethod(int? id)
        {
            if (id is not null)
            {
                var findedFriendship = _db.Friendships.FirstOrDefault(f => f.Id == id);
                if (findedFriendship is not null)
                {
                    findedFriendship.FriendStatus = Env.Blacklist;
                    _db.Friendships.Update(findedFriendship);
                    _db.SaveChanges();
                    return findedFriendship;
                }
            }
            return null;
        }
        private Friendship ChangeFriendMethod(int? id)
        {
            if (id is not null)
            {
                var findedFriendship = _db.Friendships.FirstOrDefault(f => f.Id == id);
                if (findedFriendship is not null)
                {
                    findedFriendship.FriendStatus = Env.Friend;
                    _db.Friendships.Update(findedFriendship);
                    _db.SaveChanges();
                    return findedFriendship;
                }
            }
            return null;
        }

        public async Task<IActionResult> AddFriend(string id)
        {
            await AddFriendMethod(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFriend(int? id)
        {
            RemoveFriendMethod(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddInviteFriend(int? id)
        {
            AddInviteFriendMethod(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult LeaveSubscribers(int? id)
        {
            LeaveSubscribersMethod(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddBlacklistFriend(int? id)
        {
            AddBlacklistFriendMethod(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ChangeFriend(int? id)
        {
            ChangeFriendMethod(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewAddFriend(string id)
        {
            await AddFriendMethod(id);
            return RedirectToAction(nameof(ViewProfile), new { id = id });
        }
        public async Task<IActionResult> ViewAddInviteFriend(int? id)
        {
            var ship = AddInviteFriendMethod(id);
            if (ship is not null)
            {
                var identity = await _userManager.GetUserAsync(User) as Profile;
                string finderid = "";
                if (ship.FriendId != identity.Id)
                {
                    finderid = ship.FriendId;
                }
                else
                {
                    finderid = ship.ProfileId;
                }
                return RedirectToAction(nameof(ViewProfile), new { id = finderid });
            }
            return View();
        }
        public async Task<IActionResult> ViewRemoveFriend(int? id)
        {
            var ship = RemoveFriendMethod(id);
            if (ship is not null)
            {
                var identity = await _userManager.GetUserAsync(User) as Profile;
                string finderid = "";
                if (ship.FriendId != identity.Id)
                {
                    finderid = ship.FriendId;
                }
                else
                {
                    finderid = ship.ProfileId;
                }
                return RedirectToAction(nameof(ViewProfile), new { id = finderid });
            }
            return View();
        }
        public async Task<IActionResult> ViewLeaveSubscribers(int? id)
        {
            LeaveSubscribersMethod(id);
            var identity = await _userManager.GetUserAsync(User) as Profile;
            string finderid = "";
            var ship = _db.Friendships.Find(id);
            if (ship.FriendId != identity.Id)
            {
                finderid = ship.FriendId;
            }
            else
            {
                finderid = ship.ProfileId;
            }
            return RedirectToAction(nameof(ViewProfile), new { id = finderid });
        }
        public async Task<IActionResult> ViewAddBlacklistFriend(int? id)
        {
            AddBlacklistFriendMethod(id);
            var identity = await _userManager.GetUserAsync(User) as Profile;
            string finderid = "";
            var ship = _db.Friendships.Find(id);
            if (ship.FriendId != identity.Id)
            {
                finderid = ship.FriendId;
            }
            else
            {
                finderid = ship.ProfileId;
            }
            return RedirectToAction(nameof(ViewProfile), new { id = finderid });
        }
        public async Task<IActionResult> ViewChangeFriend(int? id)
        {
            ChangeFriendMethod(id);
            var identity = await _userManager.GetUserAsync(User) as Profile;
            string finderid = "";
            var ship = _db.Friendships.Find(id);
            if (ship.FriendId != identity.Id)
            {
                finderid = ship.FriendId;
            }
            else
            {
                finderid = ship.ProfileId;
            }
            return RedirectToAction(nameof(ViewProfile), new { id = finderid });
        }
        public async Task<IActionResult> ViewCreateBlacklistFriend(string id)
        {
            if (id != null)
            {
                var findedFriend = _db.Profiles.Find(id);
                var identity = await _userManager.GetUserAsync(User) as Profile;
                if (findedFriend != null)
                {
                    findedFriend.Friendships.Add(new Friendship()
                    {
                        Profile = findedFriend,
                        Friend = identity,
                        FriendStatus = Env.Blacklist,
                    });
                    await _userManager.UpdateAsync(findedFriend);
                }
                return RedirectToAction(nameof(ViewProfile), new { id = findedFriend.Id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LikePostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage != null)
                {
                    findedPostage.LikedProfiles.Add(new LikedProfilePostage()
                    {
                        Profile = await _userManager.GetUserAsync(User) as Profile,
                        Postage = findedPostage,
                    });
                }
                else
                {
                    return NotFound();
                }
                _db.ProfilePostages.Update(findedPostage);
                _db.SaveChanges();
                return RedirectToAction(nameof(ViewProfile), new { id = findedPostage.ProfileId });
            }
            return RedirectToAction(nameof(ViewProfile));
        }
        [HttpPost]
        public IActionResult DislikePostagePost(string id)
        {
            if (id != null)
            {
                var finded = _db.LikedProfilePostages.FirstOrDefault(i => i.ProfileId == id);
                finded.Postage = _db.ProfilePostages.Find(finded.ProfilePostageId);
                if (finded != null)
                {
                    _db.LikedProfilePostages.Remove(finded);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ViewProfile), new { id = finded.Postage.ProfileId });
            }
            return RedirectToAction(nameof(ViewProfile));
        }

        [HttpPost]
        public async Task<IActionResult> SharePostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage != null)
                {
                    findedPostage.SharedProfiles.Add(new SharedProfilePostage()
                    {
                        Profile = await _userManager.GetUserAsync(User) as Profile,
                        Postage = findedPostage,
                    });
                }
                else
                {
                    return NotFound();
                }
                _db.ProfilePostages.Update(findedPostage);
                _db.SaveChanges();
                return RedirectToAction(nameof(ViewProfile), new { id = findedPostage.ProfileId });
            }
            return RedirectToAction(nameof(ViewProfile));
        }
        [HttpPost]
        public IActionResult DissharePostagePost(string id)
        {
            if (id != null)
            {
                var finded = _db.SharedProfilePostages.FirstOrDefault(i => i.ProfileId == id);
                finded.Postage = _db.ProfilePostages.Find(finded.ProfilePostageId);
                if (finded != null)
                {
                    _db.SharedProfilePostages.Remove(finded);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ViewProfile), new { id = finded.Postage.ProfileId });
            }
            return RedirectToAction(nameof(ViewProfile));
        }

        [HttpPost]
        public async Task<IActionResult> CommentPublishPost(string youcomment, int? id)
        {
            if (youcomment is null || id is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var finderPostage = _db.ProfilePostages.Find(id);
            //finderPostage.LikedProfiles = _db.LikedProfilePostages.Where(i => i.ProfilePostageId == finderPostage.Id).ToList();
            _db.ProfilePostageComments.Add(new ProfilePostageComment()
            {
                Comment = youcomment,
                ProfileCommentator = await _userManager.GetUserAsync(User) as Profile,
                Postage = finderPostage,
                TimeStamp = DateTime.Now,
            });
            _db.SaveChanges();

            return RedirectToAction(nameof(ViewProfile), new { id = finderPostage.ProfileId });
        }
        [HttpPost]
        public async Task<IActionResult> PublishPost(string content, string id)
        {
            if (content is null)
            {
                return NotFound("Content null");
            }
            if (id is null)
            {
                return NotFound("Id null");
            }
            if (content.Length > 0)
            {
                var myprofile = await _userManager.GetUserAsync(User) as Profile;
                var profile = await _userManager.FindByIdAsync(id) as Profile;
                profile.ProfilePostages.Add(new ProfilePostage()
                {
                    TimeStamp = DateTime.Now,
                    Profile = profile,
                    Writter = myprofile,
                    IsObject = false,
                    Content = content,
                });
                await _userManager.UpdateAsync(profile);
                return RedirectToAction(nameof(ViewProfile), new { id = profile.Id });
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteProfilePostageCommentPost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostageComments.Find(id);
                findedPostage.Postage = _db.ProfilePostages.FirstOrDefault(i => i.ProfilePostageComments.Contains(findedPostage));
                _db.ProfilePostageComments.Remove(findedPostage);
                _db.SaveChanges();
                return RedirectToAction(nameof(ViewProfile), new { id = _db.SharedProfilePostages.FirstOrDefault(f => f.ProfilePostageId == findedPostage.PostageId).ProfileId });
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeletePostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                _db.ProfilePostages.Remove(findedPostage);
                _db.SaveChanges();
                return RedirectToAction(nameof(ViewProfile), new { id = findedPostage.ProfileId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
