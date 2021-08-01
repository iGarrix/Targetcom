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
        public IActionResult Index()
        {
            IEnumerable<Profile> profiles = _db.Profiles;
            return View(profiles);
        }

        public IActionResult Newfriend()
        {
            IEnumerable<Profile> profiles = _db.Profiles;
            return View(profiles);
        }

        /* old variant (Not proffited) A lot if requests (Repeated Requests) */
        //public async Task<IActionResult> ViewProfile(string id)
        //{
        //    ViewProfileVM viewProfileVM = new ViewProfileVM()
        //    {
        //        FindedProfile = new Profile(),
        //        IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
        //    };
        //    IEnumerable<Profile> profiles = _db.Profiles;
        //    var profile = profiles.FirstOrDefault(i => i.Id == id);          
        //    if (profile == null)
        //    {
        //        return RedirectToAction(nameof(Newfriend));
        //    }
        //    var myprofile = await _userManager.GetUserAsync(User);
        //    viewProfileVM.Role = _userManager.GetRolesAsync(profile as IdentityUser).Result.ToList()[0];
        //    viewProfileVM.MyRole = _userManager.GetRolesAsync(myprofile as IdentityUser).Result.ToList()[0];

        //    var postage = _db.ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
        //    postage.ForEach(i =>
        //    {
        //        var list = _db.ProfilePostageComments.Where(s => s.PostageId == i.Id).ToList();
        //        list.ForEach(j =>
        //        {
        //            j.ProfileCommentator = _db.Profiles.FirstOrDefault(f => f.Id == j.ProfileCommentatorId);
        //        });
        //        i.ProfilePostageComments = list;
        //    });
        //    postage.ForEach(i =>
        //    {
        //        i.LikedProfiles = _db.LikedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
        //    });
        //    postage.ForEach(i =>
        //    {
        //        i.SharedProfiles = _db.SharedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
        //    });
        //    profile.ProfilePostages = postage;
        //    profile.LikedProfilePostages = _db.LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
        //    profile.SharedProfilePostages = _db.SharedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();

        //    viewProfileVM.FindedProfile = profile;

        //    return View(viewProfileVM);
        //}

        public async Task<IActionResult> ViewProfile(string id)
        {
            ViewProfileVM viewProfileVM = new ViewProfileVM()
            {
                FindedProfile = new Profile(),
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
            };

            var ProfilePostages = _db.ProfilePostages;
            var ProfilePostageComments = _db.ProfilePostageComments;
            var LikedProfilePostages = _db.LikedProfilePostages;
            var SharedProfilePostages = _db.SharedProfilePostages;
            var Profiles = _db.Profiles;

            IEnumerable<Profile> profiles = Profiles;
            var profile = profiles.FirstOrDefault(i => i.Id == id);
            if (profile == null)
            {
                return RedirectToAction(nameof(Newfriend));
            }
            var myprofile = await _userManager.GetUserAsync(User);
            viewProfileVM.Role = _userManager.GetRolesAsync(profile as IdentityUser).Result.ToList()[0];
            viewProfileVM.MyRole = _userManager.GetRolesAsync(myprofile as IdentityUser).Result.ToList()[0];

            var postage = ProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();
            postage.ForEach(i =>
            {
                var list = ProfilePostageComments.Where(s => s.PostageId == i.Id).ToList();
                list.ForEach(j =>
                {
                    j.ProfileCommentator = Profiles.FirstOrDefault(f => f.Id == j.ProfileCommentatorId);
                });
                i.ProfilePostageComments = list;
            });
            postage.ForEach(i =>
            {
                i.LikedProfiles = LikedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
            });
            postage.ForEach(i =>
            {
                i.SharedProfiles = SharedProfilePostages.Where(s => s.ProfilePostageId == i.Id).ToList();
            });
            profile.ProfilePostages = postage;
            profile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == profile.Id).ToList();

            var sharedpostage = SharedProfilePostages.ToList();
            sharedpostage.ForEach(i =>
            {
                i.Postage = ProfilePostages.FirstOrDefault(f => f.Id == i.ProfilePostageId);
                i.Postage.LikedProfiles = LikedProfilePostages.Where(w => w.ProfilePostageId == i.Postage.Id).ToList();
                i.Postage.ProfilePostageComments = ProfilePostageComments.Where(s => s.PostageId == i.Postage.Id).ToList();
            });
            sharedpostage.ForEach(i =>
            {
                i.Profile = Profiles.FirstOrDefault(f => f.Id == i.ProfileId);
            });
            profile.SharedProfilePostages = sharedpostage;

            viewProfileVM.FindedProfile = profile;

            return View(viewProfileVM);
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
        public IActionResult DeleteProfilePostageCommentPost(int? id)
        {
            if (id != null)
            {
                var findedPostage = _db.ProfilePostageComments.Find(id);
                findedPostage.Postage = _db.ProfilePostages.FirstOrDefault(i => i.ProfilePostageComments.Contains(findedPostage));
                _db.ProfilePostageComments.Remove(findedPostage);
                _db.SaveChanges();
                return RedirectToAction(nameof(ViewProfile), new { id = findedPostage.Postage.ProfileId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
