﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ManagementGames
    {
        public IEnumerable<Game> Games { get; set; }
        public string Alert { get; set; } = "Select a game";
    }
}
