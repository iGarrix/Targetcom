using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class GameVM
    {
        public Profile IdentityProfile { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }
}
