using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace WebApplication.Services
{
    public interface ICheckinRetrievalService
    {
        Task<Dictionary<string, string>> CreateUpdatedCheckinList();
    }

    public class CheckinRetrievalService : ICheckinRetrievalService
    {
        private IMemoryCache cache;

        public CheckinRetrievalService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public async Task<Dictionary<string, string>> CreateUpdatedCheckinList()
        {
            var currentCheckins = cache.Get("checkins") as Dictionary<string, string> ?? new Dictionary<string, string>();
            var page = 1;
            Dictionary<string, string> newCheckins;

            do
            {
                newCheckins = GetCheckinsOnPage(page++).Result;
            } while(WasUnseenCheckinReceived(currentCheckins, newCheckins));
            cache.Set("checkins", newCheckins);

            return await Task.Run(() => newCheckins);
        }

        private bool WasUnseenCheckinReceived(Dictionary<string, string> current, Dictionary<string, string> newCheckins)
        {
            return false;
        }

        private async Task<Dictionary<string, string>> GetCheckinsOnPage(int page = 1)
        {
            var unixTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var url = "https://api.onthecity.org/checkins?page=" + page;
            var stringToSign = $"{unixTime}GET{url}";
            var secretKey = "";
            var token = "";

            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringToSign));
            var unencodedHmac = hmacSha256.ComputeHash(stream);
            var unescapedHmac = Convert.ToBase64String(unencodedHmac).TrimEnd();
            var hmacSignature = Uri.EscapeDataString(unescapedHmac);

            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("X-City-Sig", hmacSignature);
            requestMessage.Headers.Add("X-City-User-Token", token);
            requestMessage.Headers.Add("X-City-Time", unixTime.ToString());
            requestMessage.Headers.Add("Accept", "application/vnd.thecity.admin.v1+json");

            var response = await httpClient.SendAsync(requestMessage);
            var responseAsString = await response.Content.ReadAsStringAsync();
            
            var newCheckins = new Dictionary<string, string>();
            var today = DateTime.Today.ToString("MM/dd/yyyy");
            dynamic r = JObject.Parse(responseAsString);
            foreach(var c in r.checkins)
            {
                var checkin = c.checked_in_at.ToString().Substring(0, 10);
                if (checkin == "06/19/2016")
                {
                    newCheckins.Add(c.barcode.ToString(), c.group.id.ToString());
                }
            }


            return await Task.Run(() => newCheckins);


        }
    }
}