using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Acr.UserDialogs;
using HtmlAgilityPack;
using ModernHttpClient;
using Toasts.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace MTS10SMS
{
    public class MondoSMS
    {
        public static string result;
        public static string session;

        /// <summary>
        /// Test method only
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string SMSAsync(string prefix, string number)
        {
            return SMSPostAsync(prefix, number).Result.Content.ToString();
        }

        /// <summary>
        /// Initial SMS method
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SMSPostAsync(string prefix, string number)
        {
            var httpClient = new HttpClient(new NativeMessageHandler { AllowAutoRedirect = false });

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "mobnum", "" },
                { "passs", "" },
                { "pozivni2", prefix },
                { "passnum", number },
                { "Send2.x", "5"},
                { "Send2.y", "7" }
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);

            var response = await httpClient.PostAsync("http://mondo.rs/sms/password.php", urlEncodedContent);

            response.EnsureSuccessStatusCode();

            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Success,
                "OK", "SMS number sent, awaiting response, checking...", TimeSpan.FromSeconds(1));


            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);

                UserDialogs.Instance.Alert("Server response: " + doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).First());

            }

            return response;
        }
        

        /// <summary>
        /// Test Login method
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="prefix"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Task LoginAsync(string pass, string prefix, string number)
        {
            return LoginPostAsync(pass, prefix, number);
        }

        /// <summary>
        /// Login using SMS-code method 
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="prefix"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> LoginPostAsync(string pass, string prefix, string number)
        {

            // TODO: CHECK WHY ANDROID WON'T ALLOW COOKIE SETTING WITH NATIVEMESSAGEHANDLER?
            HttpClient httpClient;

            if (Device.OS == TargetPlatform.iOS)
                httpClient = new HttpClient(new NativeMessageHandler {AllowAutoRedirect = false});
            else
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                httpClient = new HttpClient(httpClientHandler);
            }

            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped;

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "passnum", "" },
                { "mobnum", number },
                { "passs", pass },
                { "pozivni2", prefix },
                { "Send3.x", "4" },
                { "Send3.y", "2"}
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);

            httpClient.DefaultRequestHeaders.Referrer = new Uri("http://mondo.rs/sms/password.php");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            httpClient.DefaultRequestHeaders.Add("Cookie", "wssid=O2i2N98m1uIw;");

            var response = await httpClient.PostAsync("http://mondo.rs/sms/password.php", urlEncodedContent);

            IEnumerable<string> values;

            if (response.Headers.TryGetValues("Set-Cookie", out values))
            {
                //HttpClientHandler httpClientHandler2 = new HttpClientHandler();
                //httpClientHandler2.AllowAutoRedirect = true;
                var httpClient2 = new HttpClient(new NativeMessageHandler { AllowAutoRedirect = true });
                session = values.First();
                var urlEncodedContent2 = new FormUrlEncodedContent(postData);
                httpClient2.DefaultRequestHeaders.Referrer = new Uri("http://mondo.rs/sms/password.php");
                httpClient2.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                httpClient2.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                httpClient2.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
                httpClient2.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
                httpClient2.DefaultRequestHeaders.Add("Cookie", session);
                
                var response2 = await httpClient2.PostAsync("http://mondo.rs/sms/password.php", urlEncodedContent2);
                //response2.EnsureSuccessStatusCode();

                tapped = await notificator.Notify(ToastNotificationType.Success,
                    "OK", "Login code sent, awaiting response, checking...", TimeSpan.FromSeconds(1));

                Stream responseStream2 = await response2.Content.ReadAsStreamAsync();
                using (StreamReader responseReader = new StreamReader(responseStream2))
                {
                    String sResponse = responseReader.ReadToEnd();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(sResponse);
                    if(doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).Any())
                        UserDialogs.Instance.Alert("Server response: " + doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).First());

                }

                return response2;

            }
            
            tapped = await notificator.Notify(ToastNotificationType.Success,
                "OK", "Login code sent, awaiting response, checking...", TimeSpan.FromSeconds(1));

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);
                if (doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).Any())
                    UserDialogs.Instance.Alert("Server response: " + doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).First());

            }

            return response;

        }

        /// <summary>
        /// Test method Send
        /// </summary>
        /// <param name="sms"></param>
        /// <param name="prefix"></param>
        /// <param name="tonumber"></param>
        /// <returns></returns>
        public static Task SendAsync(string sms, string prefix, string tonumber)
        {
            return SendPostAsync(sms, prefix, tonumber);
        }

        /// <summary>
        /// Send SMS method using Mondo-portal
        /// </summary>
        /// <param name="sms"></param>
        /// <param name="prefix"></param>
        /// <param name="tonumber"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendPostAsync(string sms, string prefix, string tonumber)
        {
            var httpClient = new HttpClient(new NativeMessageHandler { AllowAutoRedirect = true });

            var postData = new Dictionary<string, string>
            {
                { "dstnum", tonumber },
                { "textmsg", sms },
                { "pozivni3", prefix },
                { "Send3.x", "5"},
                { "Send3.y", "1" }
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);


            httpClient.DefaultRequestHeaders.Referrer = new Uri("http://mondo.rs/sms/poruka.php");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            httpClient.DefaultRequestHeaders.Add("Cookie", session);

            var response = await httpClient.PostAsync("http://mondo.rs/sms/poruka.php", urlEncodedContent);

            response.EnsureSuccessStatusCode();


            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Success,
                "OK", "Message text sent, awaiting response, checking...", TimeSpan.FromSeconds(1));

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);
                if (doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).Any())
                    UserDialogs.Instance.Alert("Server response: " + doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).First());
            }

            return response;
        }
    }
}
