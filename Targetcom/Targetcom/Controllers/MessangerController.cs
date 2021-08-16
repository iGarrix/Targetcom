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
    public class MessangerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TargetDbContext _db;
        public MessangerController(UserManager<IdentityUser> userManager, TargetDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index(string id = null)
        {
            var profile = await _userManager.GetUserAsync(User) as Profile;

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


            profile.Friendships = ProfileFriendship.Where(w => w.FriendId == profile.Id || w.ProfileId == profile.Id).ToList();
            profile.BannedProfiles = ProfileBanned.Where(w => w.AdminId == profile.Id).ToList();
            profile.Banned = ProfileBanned.FirstOrDefault(f => f.ProfileId == profile.Id);

            profile.ToMessageGroups = ProfileGroups.Where(w => w.AdminId == profile.Id || w.FriendId == profile.Id).ToList();
            profile.WithMessageGroups = ProfileGroups.Where(w => w.FriendId == profile.Id).ToList();
            profile.Messages = ProfileMessages.Where(w => w.ProfileId == profile.Id).ToList();
       
            MessageVM messageVM = new MessageVM()
            {
                IdentityProfile = profile,
                Profiles = Profiles.ToList(),
            };

            if (id is not null)
            {
                var selectprofile = Profiles.Find(id);
                if (selectprofile is not null)
                {
                    selectprofile.ToMessageGroups = ProfileGroups.Where(w => w.AdminId == selectprofile.Id || w.FriendId == selectprofile.Id).ToList();
                    selectprofile.WithMessageGroups = ProfileGroups.Where(w => w.FriendId == selectprofile.Id).ToList();
                    selectprofile.Messages = ProfileMessages.Where(w => w.ProfileId == selectprofile.Id).ToList();
                    messageVM.SelectedProfile = selectprofile;
                }
            }

            return View(messageVM);
        }
        public async Task<IActionResult> RemoveRoom (int? id)
        {
            var myprofile = await _userManager.GetUserAsync(User) as Profile;
            if (id is not null)
            {
                var room = _db.MessageGroups.Find(id);
                if (room is not null)
                {
                    var messages = _db.Messages.Where(w => w.MessageGroupId == room.Id);
                    if (messages is not null)
                    {
                        _db.Messages.RemoveRange(messages);
                        _db.MessageGroups.Remove(room);
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AcceptInvite(int? id)
        {
            var myprofile = await _userManager.GetUserAsync(User) as Profile;
            if (id is not null)
            {
                var room = _db.MessageGroups.Find(id);
                if (room is not null)
                {
                    room.IsInvite = false;
                    _db.MessageGroups.Update(room);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
