using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Targetcom.Models;

namespace Targetcom.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            if (result.Succeeded)
            {
                var myprofile = user as Profile;
                if (!myprofile.IsVerify)
                {
                    if (myprofile.Status != null && myprofile.JobGeoplace != null && myprofile.StudyGeoplace != null &&
                        myprofile.UrlAvatar != null)
                    {
                        if ((DateTime.Now.Year - myprofile.Age.Year) >= 18 &&
                            myprofile.Status.Length > 0 &&
                            myprofile.JobGeoplace.Length > 0 && myprofile.StudyGeoplace.Length > 0 &&
                            myprofile.UrlAvatar != Env.DefaultImageUrl && myprofile.EmailConfirmed)
                        {
                            myprofile.IsVerify = true;
                            await _userManager.UpdateAsync(myprofile);
                        }
                    }
                }
            }
            return Page();
        }
    }
}
