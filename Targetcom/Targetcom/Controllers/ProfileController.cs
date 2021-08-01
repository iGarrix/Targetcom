﻿using Microsoft.AspNetCore.Authorization;
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
        /* old variant (Not proffited) A lot if requests (Repeated Requests) */
        //public async Task<IActionResult> Index()
        //{
        //    ProfileVM profileVM = new ProfileVM()
        //    {
        //        IdentityProfile = new Profile(),
        //    };

        //    IEnumerable<Profile> profiles = _db.Profiles;
        //    var user = await _userManager.GetUserAsync(User);
        //    var profile = profiles.FirstOrDefault(i => i.Id == user.Id);

        //    if (profile.AfkStatus == Env.Offline)
        //    {
        //        profile.AfkStatus = Env.Online;
        //    }

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

        //    var sharedpostage = _db.SharedProfilePostages.ToList();
        //    sharedpostage.ForEach(i =>
        //    {
        //        i.Postage = _db.ProfilePostages.FirstOrDefault(f => f.Id == i.ProfilePostageId);
        //        i.Postage.LikedProfiles = _db.LikedProfilePostages.Where(w => w.ProfilePostageId == i.Postage.Id).ToList();
        //        i.Postage.ProfilePostageComments = _db.ProfilePostageComments.Where(s => s.PostageId == i.Postage.Id).ToList();
        //    });
        //    sharedpostage.ForEach(i =>
        //    {
        //        i.Profile = _db.Profiles.FirstOrDefault(f => f.Id == i.ProfileId);
        //    });
        //    profile.SharedProfilePostages = sharedpostage;

        //    profileVM.IdentityProfile = profile;
        //    return View(profileVM);
        //}
        public async Task<IActionResult> Index()
        {
            ProfileVM profileVM = new ProfileVM()
            {
                IdentityProfile = new Profile(),
            };

            IEnumerable<Profile> profiles = _db.Profiles;
            var user = await _userManager.GetUserAsync(User);
            var profile = profiles.FirstOrDefault(i => i.Id == user.Id);

            if (profile.AfkStatus == Env.Offline)
            {
                profile.AfkStatus = Env.Online;
            }
            var ProfilePostages = _db.ProfilePostages;
            var ProfilePostageComments = _db.ProfilePostageComments;
            var LikedProfilePostages = _db.LikedProfilePostages;
            var SharedProfilePostages = _db.SharedProfilePostages;
            var Profiles = _db.Profiles;

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

            profileVM.IdentityProfile = profile;
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> PublishPost(string content)
        {
            if (content != null || content.Length > 0)
            {
                var profile = await _userManager.GetUserAsync(User) as Profile;
                profile.ProfilePostages.Add(new ProfilePostage()
                {
                    TimeStamp = DateTime.Now,
                    Profile = profile,
                    Writter = profile,
                    IsObject = false,
                    Content = content,
                });
                await _userManager.UpdateAsync(profile);
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
