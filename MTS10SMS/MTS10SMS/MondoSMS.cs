using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace MTS10SMS
{
    public class MondoSMS
    {
        public static string result;
        public static string session;

        public static string SMSAsync(string prefix, string number)
        {
            return SMSPostAsync(prefix, number).Result.Content.ToString();
        }

        public static async Task<HttpResponseMessage> SMSPostAsync(string prefix, string number)
        {
            var httpClient = new HttpClient();

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "mobnum", "" },
                { "passs", "" },
                { "pozivni2", prefix },
                { "passnum", number },
                { "Send2", "Pošalji"}
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);

            var response = await httpClient.PostAsync("http://services.mondo.rs/v2/inc/sms-raz/password.php", urlEncodedContent);

            response.EnsureSuccessStatusCode();

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);
            }

            return response;
        }
        
        public static Task LoginAsync(string pass, string prefix, string number)
        {
            return LoginPostAsync(pass, prefix, number);
        }

        public static async Task<HttpResponseMessage> LoginPostAsync(string pass, string prefix, string number)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = false;
            var httpClient = new HttpClient(httpClientHandler);

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "passnum", "" },
                { "mobnum", number },
                { "passs", pass },
                { "pozivni2", prefix },
                { "Send3", "Ulogujte se" }
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);

            httpClient.DefaultRequestHeaders.Referrer = new Uri("http://services.mondo.rs/v2/inc/sms-raz/password.php");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            httpClient.DefaultRequestHeaders.Add("Cookie", "wssid=OMyY8DdOXdAj;");

            var response = await httpClient.PostAsync("http://services.mondo.rs/v2/inc/sms-raz/password.php", urlEncodedContent);

            IEnumerable<string> values;

            if (response.Headers.TryGetValues("Set-Cookie", out values))
            {
                session = values.First();

                httpClient.DefaultRequestHeaders.Referrer = new Uri("http://services.mondo.rs/v2/inc/sms-raz/password.php");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
                httpClient.DefaultRequestHeaders.Add("Cookie", session);

                response = await httpClient.PostAsync("http://services.mondo.rs/v2/inc/sms-raz/password.php", urlEncodedContent);

                response.EnsureSuccessStatusCode();
            }

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);
            }
            
            return response;

        }

        public static Task SendAsync(string sms, string prefix, string tonumber)
        {
            return SendPostAsync(sms, prefix, tonumber);
        }

        public static async Task<HttpResponseMessage> SendPostAsync(string sms, string prefix, string tonumber)
        {
            var httpClient = new HttpClient();

            var postData = new Dictionary<string, string>
            {
                { "dstnum", tonumber },
                { "textmsg", sms },
                { "pozivni3", prefix },
                { "Send3", "Pošalji"}
            };
            var urlEncodedContent = new FormUrlEncodedContent(postData);


            httpClient.DefaultRequestHeaders.Referrer = new Uri("http://services.mondo.rs/v2/inc/sms-raz/poruka.php");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
            httpClient.DefaultRequestHeaders.Add("Cookie", session);

            var response = await httpClient.PostAsync("http://services.mondo.rs/v2/inc/sms-raz/poruka.php", urlEncodedContent);

            response.EnsureSuccessStatusCode();

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                String sResponse = responseReader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sResponse);
            }

            return response;
        }
    }
}
