using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ApiLibs.General
{
    public class XAuth
    {
        public static string GetAccessToken(string url, string username, string password, string clientId, string clientSecret)
        {
            List<Param> paras = GenerateParams(username, password, clientId);
            string signature = GenerateSignature(clientSecret, paras);
            paras.Add(new Param("oauth_signature", signature));
            return "OAuth " + string.Join(", ", paras.OrderBy(p => p.Name).ToList().ConvertAll(p => p.Name + "=\"" + p.Value + "\""));
        }

        public static string GenerateSignature(string clientSecret, List<Param> paras)
        {
            HMACSHA1 crypto = new HMACSHA1();
            string[] paramser = paras.ConvertAll<string>(p => p.Name + "=" + p.Value).ToArray();
            string signatureBefore = string.Join(",", paramser);

            crypto.Key = Encoding.UTF8.GetBytes(clientSecret);
            var signatureBytes = crypto.ComputeHash(Encoding.UTF8.GetBytes( signatureBefore));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in signatureBytes)
                sb.Append(b.ToString("X2"));

            string signature = sb.ToString();
            return signature;
        }

        public static List<Param> GenerateParams(string username, string password, string clientId)
        {
            return new List<Param>
            {
                new Param("oauth_consumer_key", clientId),
                new Param("oauth_timestamp", GetTimestamp()),
                new Param("oauth_nonce", GetNonce()),
                new Param("oauth_signature_method", "HMAC-SHA1"),
                new Param("oauth_callback", ""),
                new Param("x_auth_mode", "client_auth"),
                new Param("x_auth_username", HttpUtility.UrlEncode(username)),
                new Param("x_auth_password", password),

            };
        }

        private const string DIGIT = "1234567890";

        private const string LOWER = "abcdefghijklmnopqrstuvwxyz";

        public static string GetNonce()
        {
            const string chars = (LOWER + DIGIT);

            Random random = new Random();

            char[] nonce = new char[16];

            for (int i = 0; i < nonce.Length; i++)
                {
                    nonce[i] = chars[random.Next(0, chars.Length)];
            }

            return new string(nonce);
        }

        /// <summary>
        /// Generates a timestamp based on the current elapsed seconds since '01/01/1970 0000 GMT"
        /// </summary>
        /// <seealso cref="http://oauth.net/core/1.0#nonce"/>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            return GetTimestamp(DateTime.UtcNow);
        }

        /// <summary>
        /// Generates a timestamp based on the elapsed seconds of a given time since '01/01/1970 0000 GMT"
        /// </summary>
        /// <seealso cref="http://oauth.net/core/1.0#nonce"/>
        /// <param name="dateTime">A specified point in time.</param>
        /// <returns></returns>
        public static string GetTimestamp(DateTime dateTime)
        {
            return (dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
        }

    }

}