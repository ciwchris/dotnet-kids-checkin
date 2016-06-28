using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication.Services
{
    public class CheckinRetrievalService
    {
        public async Task<string> Test()
        {
            var unixTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var url = "https://api.onthecity.org/checkins?page=1";
            var stringToSign = $"{unixTime}GET{url}";
            var secretKey = "";
            var token = "";
            // Construct HMAC signature
            var encoding = Encoding.UTF8;
            var hmacSha256 = new HMACSHA256(encoding.GetBytes(secretKey));
            var stream = new MemoryStream(encoding.GetBytes(stringToSign));
            var unencodedHmac = hmacSha256.ComputeHash(stream);

            // Base64 encode the HMAC output
            var unescapedHmac = Convert.ToBase64String(unencodedHmac).TrimEnd();

            //URL-encode the Base64-encoded HMAC code
            var hmacSignature = Uri.EscapeDataString(unescapedHmac);


            // {"X-City-Sig", hmacSignature}
            // {"X-City-User-Token", token}
            // {"X-City-Time", unixTime}
            // {"Accept", "application/vnd.thecity.admin.v1+json"}
            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("X-City-Sig", hmacSignature);
            requestMessage.Headers.Add("X-City-User-Token", token);
            requestMessage.Headers.Add("X-City-Time", unixTime.ToString());
            requestMessage.Headers.Add("Accept", "application/vnd.thecity.admin.v1+json");

            var response = await httpClient.SendAsync(requestMessage);
            var responseAsString = await response.Content.ReadAsStringAsync();
            dynamic r = JObject.Parse(responseAsString);
            Console.WriteLine(r.checkins[0]);

            //return await httpClient.GetStringAsync(url);
           return await Task.Run(() => "test");//JsonConvert.Deserialize(response));


        }
    }
}