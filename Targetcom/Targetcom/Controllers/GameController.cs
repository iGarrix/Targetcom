using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Targetcom.Data;

namespace Targetcom.Controllers
{
    public class GameController : Controller
    {
        private readonly TargetDbContext _db;
        public GameController(TargetDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
