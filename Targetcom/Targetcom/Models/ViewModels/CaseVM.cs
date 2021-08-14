using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class CaseVM
    {
        public Profile IdentityProfile { get; set; }
        public string CaseType { get; set; }
        public string Attempts { get; set; }
        public Tuple<string, int> Prize { get; set; }
    }
}
