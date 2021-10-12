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
        public async Task<IActionResult> Index(string eventSelector = "friendpag", int allpage = 0, int friendpage = 0, int invitepage = 0, int subscribepage = 0, int blacklistpage = 0)
        {
            string myid = (await _userManager.GetUserAsync(User) as Profile).Id;
            FriendVM friendVM = new FriendVM()
            {
                IdentityUser = _db.Profiles.Find(myid),
                //AllUsers = _db.Profiles.Where(w => w.Id != IdentityUser.Id),
                EventSetter = eventSelector,
                Current_All_Page = allpage,
                Current_Blacklist_Page = blacklistpage,
                Current_Friend_Page = friendpage,
                Current_Subscribe_Page = subscribepage,
                Current_Invite_Page = invitepage,
            };
            friendVM.AllUsers = _db.Profiles.Where(w => w.Id != friendVM.IdentityUser.Id);
            var Profiles = _db.Profiles;
            var ProfileFriendship = _db.Friendships;
            ProfileFriendship.ToList().ForEach(i =>
            {
                i.Profile = Profiles.Find(i.ProfileId);
                i.Friend = Profiles.Find(i.FriendId);
            });
            var ProfileBanned = _db.BannedProfiles;
            ProfileBanned.ToList().ForEach(i =>
            {
                i.Profile = _db.Profiles.Find(i.ProfileId);
                i.Admin = _db.Profiles.Find(i.AdminId);
            });

            friendVM.IdentityUser.BannedProfiles = ProfileBanned.Where(w => w.AdminId == friendVM.IdentityUser.Id).ToList();
            friendVM.IdentityUser.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == friendVM.IdentityUser.Id);

            //friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList();
            friendVM.AllUsers.ToList().ForEach(i =>
            {
                i.Friendships = ProfileFriendship.Where(w => w.FriendId == i.Id || w.ProfileId == i.Id).ToList();
                i.BannedProfiles = ProfileBanned.Where(w => w.AdminId == i.Id).ToList();
                i.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == i.Id);
            });

            friendVM.Current_Friend_Lenght = ProfileFriendship.Count(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id && w.FriendStatus == Env.Friend) - 1;
            friendVM.Current_Invite_Lenght = ProfileFriendship.Count(w => w.FriendId == friendVM.IdentityUser.Id && w.FriendStatus == Env.Invite) - 1;
            friendVM.Current_All_Lenght = friendVM.AllUsers.Count() - 1;
            friendVM.Current_Subscribe_Lenght = ProfileFriendship.Count(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id && w.FriendStatus == Env.Subscribe) - 1;
            friendVM.Current_Blacklist_Lenght = ProfileFriendship.Count(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id && w.FriendStatus == Env.Blacklist) - 1;

            if (eventSelector == Env.FriendPag)
            {
                friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList().Skip(friendpage * Env.FRIENDPEOPLE_LOADING_LIMIT).Take(Env.FRIENDPEOPLE_LOADING_LIMIT).ToList();
            }
            else if (eventSelector == Env.Invitepag)
            {
                friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList().Skip(invitepage * Env.FRIENDPEOPLE_LOADING_LIMIT).Take(Env.FRIENDPEOPLE_LOADING_LIMIT).ToList();
            }
            else if (eventSelector == Env.Subscribepag)
            {
                friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList().Skip(subscribepage * Env.FRIENDPEOPLE_LOADING_LIMIT).Take(Env.FRIENDPEOPLE_LOADING_LIMIT).ToList();
            }
            else if (eventSelector == Env.Blacklistpag)
            {
                friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList().Skip(blacklistpage * Env.FRIENDPEOPLE_LOADING_LIMIT).Take(Env.FRIENDPEOPLE_LOADING_LIMIT).ToList();
            }
            else
            {
                friendVM.IdentityUser.Friendships = ProfileFriendship.Where(w => w.FriendId == friendVM.IdentityUser.Id || w.ProfileId == friendVM.IdentityUser.Id).ToList().Skip(friendpage * Env.FRIENDPEOPLE_LOADING_LIMIT).Take(Env.FRIENDPEOPLE_LOADING_LIMIT).ToList();
            }


            return View(friendVM);
        }
        public async Task<IActionResult> ViewProfile(string id, string listitem = "All", int postpage = 0, int sharepage = 0)
        {
            string myid = (await _userManager.GetUserAsync(User) as Profile).Id;
            if (myid == id)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewProfileVM viewProfileVM = new ViewProfileVM()
            {
                FindedProfile = new Profile(),
                IdentityProfile = _db.Profiles.Find(myid),
                ListItem = listitem,
                Current_AllPost_Page = postpage,
                Current_SharedPost_Page = sharepage,
            };

            if (listitem == Env.AllPost)
            {
                viewProfileVM.IsAllable = true;
            }
            else
            {
                viewProfileVM.IsAllable = false;
            }

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

            var ProfileBanned = _db.BannedProfiles;
            ProfileBanned.ToList().ForEach(i =>
            {
                i.Profile = _db.Profiles.Find(i.ProfileId);
                i.Admin = _db.Profiles.Find(i.AdminId);
            });

            viewProfileVM.Current_AllPost_Lenght = ProfilePostages.Where(i => i.ProfileId == profile.Id).Count() - 1;
            viewProfileVM.Current_SharedPost_Lenght = SharedProfilePostages.Count(i => i.ProfileId == profile.Id) - 1;

            if (listitem == Env.AllPost)
            {
                profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id).OrderByDescending(o => o.TimeStamp).ToList().Skip(postpage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();
            }
            else if (listitem == Env.SharedPost)
            {
                profile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profile.Id).OrderByDescending(o => o.Postage.TimeStamp).ToList().Skip(sharepage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();
            }
            else
            {
                profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id).OrderByDescending(o => o.TimeStamp).ToList().Skip(postpage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();
            }

            //profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            //profile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == profile.Id).ToList();

            viewProfileVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == viewProfileVM.IdentityProfile.Id || w.ProfileId == viewProfileVM.IdentityProfile.Id).ToList();
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();

            profile.BannedProfiles = ProfileBanned.Where(w => w.AdminId == profile.Id).ToList();
            profile.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == profile.Id);
            viewProfileVM.IdentityProfile.BannedProfiles = ProfileBanned.Where(w => w.AdminId == viewProfileVM.IdentityProfile.Id).ToList();
            viewProfileVM.IdentityProfile.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == viewProfileVM.IdentityProfile.Id);

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

        public async Task<IActionResult> Images(string id, int page = 0)
        {
            if (id is null)
            {
                return NotFound();
            }

            string myid = (await _userManager.GetUserAsync(User) as Profile).Id;
            ViewProfileVM viewProfileVM = new ViewProfileVM()
            {
                FindedProfile = new Profile(),
                IdentityProfile = _db.Profiles.Find(myid),
                Current_AllPost_Page = page,
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

            viewProfileVM.Current_AllPost_Lenght = ProfilePostages.Where(i => i.ProfileId == profile.Id && i.IsObject).Count() - 1;

            profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id && i.IsObject).OrderByDescending(o => o.TimeStamp).ToList().Skip(page * Env.IMAGE_LOADING_LIMIT).Take(Env.IMAGE_LOADING_LIMIT).ToList();

            //profile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            profile.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == profile.Id).ToList();

            viewProfileVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == viewProfileVM.IdentityProfile.Id || w.ProfileId == viewProfileVM.IdentityProfile.Id).ToList();
            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();

            viewProfileVM.FindedProfile = profile;

            return View(viewProfileVM);
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
        public async Task<IActionResult> CommentPublishPost(string youcomment, int? id, string viewprofileid)
        {
            if (youcomment is null || id is null)
            {
                return RedirectToAction(nameof(Index));
            }
            var finderPostage = _db.ProfilePostages.Find(id);
            string returnid = null;
            if (viewprofileid is not null)
            {
                var viewprofile = _db.Profiles.FirstOrDefault(f => f.Id == viewprofileid);
                if (viewprofile is not null)
                {
                    returnid = viewprofileid;
                }
            }
            else
            {
                returnid = finderPostage.ProfileId;
            }
            
            _db.ProfilePostageComments.Add(new ProfilePostageComment()
            {
                Comment = youcomment,
                ProfileCommentator = await _userManager.GetUserAsync(User) as Profile,
                Postage = finderPostage,
                TimeStamp = DateTime.Now,
            });
            _db.SaveChanges();

            return RedirectToAction(nameof(ViewProfile), new { id = returnid });
        }
        [HttpPost]
        public async Task<IActionResult> PublishPost(string content, string id, string urlimg1, string urlimg2, string urlimg3)
        {
            string uploadfiles = $"{urlimg1} {urlimg2} {urlimg3}";

            bool IsValid = true;
            if (id is null)
            {
                IsValid = false;
            }

            if (content is null)
            {
                if (urlimg1 is null && urlimg2 is null & urlimg3 is null)
                {
                    IsValid = false;
                }
            }
            if (IsValid)
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
                    UploadingUrlFiles = uploadfiles,
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
    
        public async Task<IActionResult> WriteMessage (string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var finder = await _userManager.FindByIdAsync(id) as Profile;
            if (finder is null)
            {
                return NotFound();
            }

            var identity = await _userManager.GetUserAsync(User) as Profile;
            var room = _db.MessageGroups.FirstOrDefault(w => (w.AdminId == identity.Id && w.FriendId == finder.Id) || (w.AdminId == finder.Id && w.FriendId == identity.Id));
            if (room is null)
            {
                _db.MessageGroups.Add(new MessageGroup()
                {
                    Admin = identity,
                    Friend = finder,
                    IsInvite = true,
                    TimeStamp = DateTime.Now,
                    Name = "Private",
                });
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), "Messanger");
            }
            if (room.IsInvite)
            {
                return RedirectToAction(nameof(Index), "Messanger");
            }
            return RedirectToAction(nameof(Index), "Messanger", new { id = id });
        }
    }
}
