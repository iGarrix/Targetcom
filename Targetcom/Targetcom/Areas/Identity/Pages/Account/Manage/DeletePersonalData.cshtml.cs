﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Targetcom.Data;
using Targetcom.Models;

namespace Targetcom.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly TargetDbContext _db;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            TargetDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

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
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
