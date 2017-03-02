
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.General
{
    public class Passwords
    {

        public string GitHub_clientID { get; set; }
        public string GitHub_client_secret { get; set; }
        public string GitHub_access_token { get; set; }

        public string Instaper_ID { get; set; }
        public string Instaper_secret { get; set; }

        public string OutlookClientSecret { get; set; }
        public string OutlookClientID { get; set; }
        public string OutlookAccessToken { get; set; }
        public string OutlookEmail { get; set; }
        public string OutlookRefreshToken { get; set; }

        public string PocketKey { get; set; }
        public string Pocket_access_token { get; set; }

        public string RedditClient { get; set; }
        public string RedditSecret { get; set; }
        public string RedditToken { get; set; }
        public string RedditRefreshToken { get; set; }

        public string Telegram_token { get; set; }

        public string TodoistKey { get; set; }
        public string TodoistUserAgent { get; set; }

        public string Travis_Token { get; set; }

        public string GeneralRedirectUrl { get; set; }
        public string WunderlistToken { get; set; }
        public string WunderlistId { get; set; }
        public string WunderlistSecret { get; set; }

        internal Memory mem;

        public void WriteToFile(string baseUrl)
        {
            WritePasswords(this, baseUrl);
        }

        public static Passwords ReadPasswords(string baseUrl)
        {
            Memory mem = new Memory(baseUrl);
            return mem.ReadFile<Passwords>("KeyFile.pass");
        }

        
        public static void WritePasswords(Passwords pass, string baseUrl)
        {
            Memory mem = new Memory(baseUrl);
            mem.WriteFile("KeyFile.pass", pass);
        }
        
    }
}
