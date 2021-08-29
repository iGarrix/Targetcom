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
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;
        public ProfileController(UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index(string listitem = "All", int postpage = 0, int sharepage = 0)
        {
            ProfileVM profileVM = new ProfileVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                ListItem = listitem,
                Current_AllPost_Page = postpage,
                Current_SharedPost_Page = sharepage,
            };

            if (listitem == Env.AllPost)
            {
                profileVM.IsAllable = true;
            }
            else
            {
                profileVM.IsAllable = false;
            }

            if (profileVM.IdentityProfile.AfkStatus == Env.Offline)
            {
                profileVM.IdentityProfile.AfkStatus = Env.Online;
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

            if (!profileVM.IdentityProfile.IsVerify)
            {
                if (ProfileFriendship.Where(w => w.FriendId == profileVM.IdentityProfile.Id && w.FriendStatus == Env.Subscribe).Count() >= Env.VerifySubscribe)
                {
                    profileVM.IdentityProfile.IsVerify = true;
                    await _userManager.UpdateAsync(profileVM.IdentityProfile);
                }
            }

            var ProfileBanned = _db.BannedProfiles;
            ProfileBanned.ToList().ForEach(i =>
            {
                i.Profile = _db.Profiles.Find(i.ProfileId);
                i.Admin = _db.Profiles.Find(i.AdminId);
            });

            SharedProfilePostages.ToList().ForEach(i =>
            {
                i.Postage = ProfilePostages.FirstOrDefault(f => f.Id == i.ProfilePostageId);
                i.Profile = Profiles.FirstOrDefault(f => f.Id == i.ProfileId);
            });

            profileVM.Current_AllPost_Lenght = ProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).Count() - 1;
            profileVM.Current_SharedPost_Lenght = SharedProfilePostages.Count(i => i.ProfileId == profileVM.IdentityProfile.Id) - 1;

            if (listitem == Env.AllPost)
            {
                profileVM.IdentityProfile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).OrderByDescending(o => o.TimeStamp).ToList().Skip(postpage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();
            }
            else if (listitem == Env.SharedPost)
            {
                profileVM.IdentityProfile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).OrderByDescending(o => o.Postage.TimeStamp).ToList().Skip(sharepage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();
            }
            else
            {
                profileVM.IdentityProfile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).OrderByDescending(o => o.TimeStamp).ToList().Skip(postpage * Env.PROFILE_NEWS_LOADING_LIMIT).Take(Env.PROFILE_NEWS_LOADING_LIMIT).ToList();            
            }

            //profileVM.IdentityProfile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).ToList();
            profileVM.IdentityProfile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).ToList();
            //profileVM.IdentityProfile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == profileVM.IdentityProfile.Id).ToList();
            profileVM.IdentityProfile.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == profileVM.IdentityProfile.Id).ToList();
            profileVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == profileVM.IdentityProfile.Id || w.ProfileId == profileVM.IdentityProfile.Id).ToList();
            profileVM.IdentityProfile.BannedProfiles = ProfileBanned.Where(w => w.AdminId == profileVM.IdentityProfile.Id).ToList();
           
            profileVM.IdentityProfile.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == profileVM.IdentityProfile.Id);


            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> PublishPost(string content, string urlimg1, string urlimg2, string urlimg3)
        {
            string uploadfiles = $"{urlimg1} {urlimg2} {urlimg3}";

            bool IsValid = true;

            if (content is null)
            {
                if (urlimg1 is null && urlimg2 is null & urlimg3 is null)
                {
                    IsValid = false;
                }
            }
            if (IsValid)
            {
                var profile = await _userManager.GetUserAsync(User) as Profile;
                _db.ProfilePostages.Add(new ProfilePostage()
                {
                    TimeStamp = DateTime.Now,
                    Profile = profile,
                    Writter = profile,
                    IsObject = false,
                    Content = content,
                    UploadingUrlFiles = uploadfiles,
                });
                _db.SaveChanges();
                //await _userManager.UpdateAsync(profile);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> CommentPublishPost(string youcomment, int? id)
        {
            if (youcomment is null || id is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var finderPostage = _db.ProfilePostages.Find(id);
            _db.ProfilePostageComments.Add(new ProfilePostageComment()
            {
                Comment = youcomment,
                ProfileCommentator = await _userManager.GetUserAsync(User) as Profile,
                Postage = finderPostage,
                TimeStamp = DateTime.Now,
            });
            _db.SaveChanges();

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
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult AllowLikePostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage.IsNessessaredLikedPost == true)
                {
                    findedPostage.IsNessessaredLikedPost = false;
                }
                else
                {
                    findedPostage.IsNessessaredLikedPost = true;
                }
                _db.ProfilePostages.Update(findedPostage);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult AllowSharePostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage.IsNessessaredSharedPost == true)
                {
                    findedPostage.IsNessessaredSharedPost = false;
                }
                else
                {
                    findedPostage.IsNessessaredSharedPost = true;
                }
                _db.ProfilePostages.Update(findedPostage);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult AllowCommentPostagePost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostages.Find(id);
                if (findedPostage.IsNessessaredCommentedPost == true)
                {
                    findedPostage.IsNessessaredCommentedPost = false;
                }
                else
                {
                    findedPostage.IsNessessaredCommentedPost = true;
                }
                _db.ProfilePostages.Update(findedPostage);
                _db.SaveChanges();
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
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DislikePostagePost(string id)
        {
            if (id != null)
            {
                var finded = _db.LikedProfilePostages.FirstOrDefault(i => i.ProfileId == id);
                if (finded != null)
                {
                    _db.LikedProfilePostages.Remove(finded);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteProfilePostageCommentPost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostageComments.Find(id);
                _db.ProfilePostageComments.Remove(findedPostage);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult UnsharePostagePost(string id)
        {
            if (id != null)
            {
                var finder = _db.SharedProfilePostages.FirstOrDefault(f => f.ProfileId == id);
                if (finder is null)
                {
                    return NotFound();
                }
                _db.SharedProfilePostages.Remove(finder);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
