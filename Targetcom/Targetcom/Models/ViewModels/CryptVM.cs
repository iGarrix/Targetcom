using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class CryptVM
    {
        public Profile IdentityProfile { get; set; }
        public List<Cryptohistory> Cryptohistories { get; set; }
    }
}
