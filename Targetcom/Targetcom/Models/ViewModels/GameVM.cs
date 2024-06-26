﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class GameVM
    {
        public Profile IdentityProfile { get; set; }
        public IEnumerable<Game> Games { get; set; }
        public int CurrentPage { get; set; }
        public int GameLenght { get; set; }
    }
}
