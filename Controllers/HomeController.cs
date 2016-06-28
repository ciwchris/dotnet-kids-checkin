using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var checkins = new List<CheckinProperties>();
            checkins.Add(new CheckinProperties { Id = 108117, Color = "red", Count = 0, Max = 12 });
            checkins.Add(new CheckinProperties { Id = 108119, Color = "orange", Count = 0, Max = 12 });
            checkins.Add(new CheckinProperties { Id = 108120, Color = "yellow", Count = 0, Max = 12 });
            checkins.Add(new CheckinProperties { Id = 144673, Color = "green", Count = 0, Max = 12 });
            checkins.Add(new CheckinProperties { Id = 108123, Color = "blue", Count = 0, Max = 16 });
            checkins.Add(new CheckinProperties { Id = 89515, Color = "purple", Count = 0, Max = 16 });
            return View(checkins);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
