using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ManagementCrypto
    {
        public List<Cryptohistory> Cryptohistories { get; set; }
        public Cryptohistory SelectedCrypt { get; set; }
    }
}
