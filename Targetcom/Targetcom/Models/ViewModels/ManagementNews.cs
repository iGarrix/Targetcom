using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ManagementNews
    {
        public IEnumerable<ProfilePostage> ProfilePostages { get; set; }
        public ProfilePostage SelectedProfilePostage { get; set; }

        public int Current_News_Page { get; set; }
        public int Current_News_Lenght { get; set; }
    }
}
