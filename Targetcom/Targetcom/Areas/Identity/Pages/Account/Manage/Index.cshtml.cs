using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Targetcom.Models;

namespace Targetcom.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
        public string Role { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public Profile profile { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Profile myprofile = user as Profile;

            Username = userName;
            Role = (await _userManager.GetRolesAsync(myprofile)).ToList()[0];
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                profile = myprofile,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var usermanagement = await _userManager.GetUserAsync(User) as Profile;
            if (usermanagement != null)
            {
                Profile myprofile = user as Profile;
                bool ischanged = false;
                if (usermanagement.Name != Input.profile.Name)
                {
                    myprofile.Name = Input.profile.Name;
                    ischanged = true;
                }
                if (usermanagement.Surname != Input.profile.Surname)
                {
                    myprofile.Surname = Input.profile.Surname;
                    ischanged = true;
                }
                if (usermanagement.Age != Input.profile.Age)
                {
                    myprofile.Age = Input.profile.Age;
                    ischanged = true;
                }
                if (usermanagement.Gender != Input.profile.Gender)
                {
                    myprofile.Gender = Input.profile.Gender;
                    ischanged = true;
                }

                if (usermanagement.Quote != Input.profile.Quote)
                {
                    myprofile.Quote = Input.profile.Quote;
                    ischanged = true;
                }
                if (usermanagement.Status != Input.profile.Status)
                {
                    myprofile.Status = Input.profile.Status;
                    ischanged = true;
                }
                if (usermanagement.JobGeoplace != Input.profile.JobGeoplace)
                {
                    myprofile.JobGeoplace = Input.profile.JobGeoplace;
                    ischanged = true;
                }
                if (usermanagement.StudyGeoplace != Input.profile.StudyGeoplace)
                {
                    myprofile.StudyGeoplace = Input.profile.StudyGeoplace;
                    ischanged = true;
                }
                if (usermanagement.Hobbies != Input.profile.Hobbies)
                {
                    myprofile.Hobbies = Input.profile.Hobbies;
                    ischanged = true;
                }
                if (usermanagement.UrlAvatar != Input.profile.UrlAvatar)
                {
                    myprofile.UrlAvatar = Input.profile.UrlAvatar;
                    var content = myprofile.UrlAvatar;
                    if (content != null || content.Length > 0)
                    {
                        myprofile.ProfilePostages.Add(new ProfilePostage()
                        {
                            TimeStamp = DateTime.Now,
                            Profile = myprofile,
                            Writter = myprofile,
                            IsObject = true,
                            Content = content,
                            Alert = "Updated photo",
                        });
                    }


                    ischanged = true;
                }


                if (usermanagement.Privacy != Input.profile.Privacy)
                {
                    myprofile.Privacy = Input.profile.Privacy;
                    ischanged = true;
                }

                if (usermanagement.IsShortDate != Input.profile.IsShortDate)
                {
                    myprofile.IsShortDate = Input.profile.IsShortDate;
                    ischanged = true;
                }
                if (usermanagement.VisibilityQuote != Input.profile.VisibilityQuote)
                {
                    myprofile.VisibilityQuote = Input.profile.VisibilityQuote;
                    ischanged = true;
                }
                if (usermanagement.VisibilityAboutMe != Input.profile.VisibilityAboutMe)
                {
                    myprofile.VisibilityAboutMe = Input.profile.VisibilityAboutMe;
                    ischanged = true;
                }
                if (usermanagement.VisibilityPostage != Input.profile.VisibilityPostage)
                {
                    myprofile.VisibilityPostage = Input.profile.VisibilityPostage;
                    ischanged = true;
                }
                if (usermanagement.VisibilityPlaylist != Input.profile.VisibilityPlaylist)
                {
                    myprofile.VisibilityPlaylist = Input.profile.VisibilityPlaylist;
                    ischanged = true;
                }
                if (usermanagement.VisibilityFriends != Input.profile.VisibilityFriends)
                {
                    myprofile.VisibilityFriends = Input.profile.VisibilityFriends;
                    ischanged = true;
                }
                if (usermanagement.VisibilitySubscribers != Input.profile.VisibilitySubscribers)
                {
                    myprofile.VisibilitySubscribers = Input.profile.VisibilitySubscribers;
                    ischanged = true;
                }
                if (usermanagement.VisibilityImages != Input.profile.VisibilityImages)
                {
                    myprofile.VisibilityImages = Input.profile.VisibilityImages;
                    ischanged = true;
                }
                if (usermanagement.VisibilityCommunity != Input.profile.VisibilityCommunity)
                {
                    myprofile.VisibilityCommunity = Input.profile.VisibilityCommunity;
                    ischanged = true;
                }

                if (usermanagement.VisibilityCommerceData != Input.profile.VisibilityCommerceData)
                {
                    myprofile.VisibilityCommerceData = Input.profile.VisibilityCommerceData;
                    ischanged = true;
                }
                if (usermanagement.VisibilityRole != Input.profile.VisibilityRole)
                {
                    myprofile.VisibilityRole = Input.profile.VisibilityRole;
                    ischanged = true;
                }

                if (usermanagement.IsNessessaredLikedPost != Input.profile.IsNessessaredLikedPost)
                {
                    myprofile.IsNessessaredLikedPost = Input.profile.IsNessessaredLikedPost;
                    ischanged = true;
                }
                if (usermanagement.IsNessessaredSharedPost != Input.profile.IsNessessaredSharedPost)
                {
                    myprofile.IsNessessaredSharedPost = Input.profile.IsNessessaredSharedPost;
                    ischanged = true;
                }
                if (usermanagement.IsNessessaredPublishPost != Input.profile.IsNessessaredPublishPost)
                {
                    myprofile.IsNessessaredPublishPost = Input.profile.IsNessessaredPublishPost;
                    ischanged = true;
                }

                if (myprofile.Status != null &&  myprofile.JobGeoplace != null && myprofile.StudyGeoplace != null &&
                    myprofile.UrlAvatar != null)
                {
                    if ((DateTime.Now.Year - myprofile.Age.Year) >= 18 && 
                        myprofile.Status.Length > 0 &&
                        myprofile.JobGeoplace.Length > 0 && myprofile.StudyGeoplace.Length > 0 &&
                        myprofile.UrlAvatar != Env.DefaultImageUrl && myprofile.EmailConfirmed)
                    {
                        myprofile.IsVerify = true;
                        ischanged = true;
                    }
                }
                if (ischanged)
                {
                    await _userManager.UpdateAsync(myprofile);
                }
            }


            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
