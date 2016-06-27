using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using  Newtonsoft.Json.Serialization;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class WebHookController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            var checkins = new List<CheckinProperties>();
            checkins.Add(new CheckinProperties { Id = 108117, Color = "red", Count = 3, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108119, Color = "orange", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108120, Color = "yellow", Count = 10, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 144673, Color = "green", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 108123, Color = "blue", Count = 1, Max = 3 });
            checkins.Add(new CheckinProperties { Id = 89515, Color = "purple", Count = 1, Max = 3 });
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(checkins, Formatting.None, jsonSerializerSettings);
 
            Startup.WebSockets.ForEach(async (socket) =>
            {
                if (socket != null && socket.State == WebSocketState.Open)
                {
                    var type = WebSocketMessageType.Text;
                    var data = Encoding.UTF8.GetBytes(json);
                    var buffer = new ArraySegment<Byte>(data);
                    await socket.SendAsync(buffer, type, true, CancellationToken.None);
                }
            });
            
            return Ok("result");
        }

        [HttpPost]
        public IActionResult Create([FromBody] string item)
        {
            System.Console.WriteLine("testing");
            return Ok("result");
        }
    }
}