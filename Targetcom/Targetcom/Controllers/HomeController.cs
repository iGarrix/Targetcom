using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Data;
using Targetcom.Models;
using Targetcom.Models.ViewModels;

namespace Targetcom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            NewsVM newsVM = new NewsVM()
            {
                IdentityProfile = await _userManager.GetUserAsync(User) as Profile,
                AllUsers = _db.Profiles,
                ProfilePostages = new List<ProfilePostage>(),
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
            ProfilePostages.ToList().ForEach(i =>
            {
                i.LikedProfiles.ToList().ForEach(j =>
                {
                    j.Profile = _db.Profiles.Find(j.ProfileId);
                    j.Postage = _db.ProfilePostages.Find(j.ProfilePostageId);
                });
                i.SharedProfiles.ToList().ForEach(j =>
                {
                    j.Profile = _db.Profiles.Find(j.ProfileId);
                    j.Postage = _db.ProfilePostages.Find(j.ProfilePostageId);
                });
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

            var ProfileBanned = _db.BannedProfiles;
            ProfileBanned.ToList().ForEach(i =>
            {
                i.Profile = _db.Profiles.Find(i.ProfileId);
                i.Admin = _db.Profiles.Find(i.AdminId);
            });

            newsVM.IdentityProfile.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == newsVM.IdentityProfile.Id).ToList();
            newsVM.IdentityProfile.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == newsVM.IdentityProfile.Id).ToList();
            newsVM.IdentityProfile.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == newsVM.IdentityProfile.Id).ToList();
            newsVM.IdentityProfile.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == newsVM.IdentityProfile.Id).ToList();
            newsVM.IdentityProfile.Friendships = ProfileFriendship.Where(w => w.FriendId == newsVM.IdentityProfile.Id || w.ProfileId == newsVM.IdentityProfile.Id).ToList();
            newsVM.IdentityProfile.BannedProfiles = ProfileBanned.Where(w => w.AdminId == newsVM.IdentityProfile.Id).ToList();

            newsVM.IdentityProfile.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == newsVM.IdentityProfile.Id);

            newsVM.AllUsers.ToList().ForEach(f =>
            {
                f.ProfilePostages = ProfilePostages.Where(i => i.ProfileId == f.Id).ToList();
                f.LikedProfilePostages = LikedProfilePostages.Where(i => i.ProfileId == f.Id).ToList();
                f.SharedProfilePostages = SharedProfilePostages.Where(i => i.ProfileId == f.Id).ToList();
                f.ProfilePostageComments = ProfilePostageComments.Where(i => i.Postage.ProfileId == f.Id).ToList();
                f.Friendships = ProfileFriendship.Where(w => w.FriendId == newsVM.IdentityProfile.Id || w.ProfileId == f.Id).ToList();
                f.BannedProfiles = ProfileBanned.Where(w => w.AdminId == newsVM.IdentityProfile.Id).ToList();

                f.Banned = ProfileBanned.FirstOrDefault(g => g.ProfileId == f.Id);
            });

            newsVM.ProfilePostages = ProfilePostages;

            return View(newsVM);
        }

        [AllowAnonymous]
        public IActionResult Rules()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> PublishPost(string content)
        {
            if (content.Length > 0)
            {
                var profile = await _userManager.GetUserAsync(User) as Profile;
                _db.ProfilePostages.Add(new ProfilePostage()
                {
                    TimeStamp = DateTime.Now,
                    Profile = profile,
                    Writter = profile,
                    IsObject = false,
                    Content = content,
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
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
