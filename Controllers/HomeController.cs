using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var checkins = new List<CheckinProperties>();
            checkins.Add(new CheckinProperties
            {
                Id = 108117,
                Color = "red",
                Count = 1,
                Max = 3
            });
            return View(checkins);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
