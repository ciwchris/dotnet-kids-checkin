using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            new Services.CheckinRetrievalService().Test();
            var checkins = new List<CheckinProperties>();
            checkins.Add(new CheckinProperties { Id = 108117, Color = "red", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108119, Color = "orange", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108120, Color = "yellow", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 144673, Color = "green", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108123, Color = "blue", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 89515, Color = "purple", Count = 1, Max = 3 });
            return View(checkins);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
