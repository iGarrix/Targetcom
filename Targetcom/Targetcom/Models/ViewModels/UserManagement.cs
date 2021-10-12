using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ProfileManage
    {
        public Profile Profile { get; set; }
        public List<string> Roles { get; set; }

    }
    public class UserManagement
    {
        public List<ProfileManage> ProfileManages { get; set; }
        public Profile SelectUser { get; set; }
        public List<string> SelectRoles { get; set; }

        public int Current_User_Page { get; set; }
        public int Current_User_Lenght { get; set; }
    }
}
