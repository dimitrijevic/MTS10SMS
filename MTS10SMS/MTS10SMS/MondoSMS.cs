using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Acr.UserDialogs;
using HtmlAgilityPack;
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
            var httpClient = new HttpClient();

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "mobnum", "" },
                { "passs", "" },
                { "pozivni2", prefix },
                { "passnum", number },
                { "Send2.x", "35"},
                { "Send2.y", "17" }
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
        /// TODO: CHECK PROBLEM WITH ONE TIME LOGIN, RESUMING FROM PREVIOUSLY RECIVED SMS-CODE, BUG?!?
        public static async Task<HttpResponseMessage> LoginPostAsync(string pass, string prefix, string number)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = false;
            var httpClient = new HttpClient(httpClientHandler);

            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped;

            var postData = new Dictionary<string, string>
            {
                { "pozivni", prefix },
                { "passnum", "" },
                { "mobnum", number },
                { "passs", pass },
                { "pozivni2", prefix },
                { "Send3.x", "48" },
                { "Send3.y", "12"}
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
                HttpClientHandler httpClientHandler2 = new HttpClientHandler();
                httpClientHandler2.AllowAutoRedirect = true;
                var httpClient2 = new HttpClient(httpClientHandler2);
                session = values.First();
                var urlEncodedContent2 = new FormUrlEncodedContent(postData);
                httpClient.DefaultRequestHeaders.Referrer = new Uri("http://mondo.rs/sms/password.php");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0; ARM; Touch; WPDesktop)");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
                httpClient.DefaultRequestHeaders.Add("Cookie", session);
                
                var response2 = await httpClient2.PostAsync("http://mondo.rs/sms/password.php", urlEncodedContent2);
                //{StatusCode: 302, ReasonPhrase: 'Moved Temporarily', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:{Server: nginx/1.4.4Date: Fri, 13 Nov 2015 17:35:11 GMTSet-Cookie: wssid=VtwTualqA7Py; expires=Fri, 13-Nov-2015 17:36:36 GMTLocation: poruka.phpX-Cache: MISS from wccp-proxy.arm.uns.ac.rsX-Cache-Lookup: MISS from wccp-proxy.arm.uns.ac.rs:8080Via: 1.1 proxy.uns.ac.rs (squid/3.3.13), 1.1 wccp-proxy.arm.uns.ac.rs (squid/3.3.13)Connection: closeContent-Type: text/html; charset=UTF-8Content-Length: 5417}}
                /*
                FATAL EXCEPTION: main
11-13 18:08:37.981 E/AndroidRuntime( 2143): java.lang.RuntimeException: java.lang.reflect.InvocationTargetException
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at com.android.internal.os.ZygoteInit.main(ZygoteInit.java:760)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at dalvik.system.NativeStart.main(Native Method)
11-13 18:08:37.981 E/AndroidRuntime( 2143): Caused by: java.lang.reflect.InvocationTargetException
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at java.lang.reflect.Method.invokeNative(Native Method)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at java.lang.reflect.Method.invoke(Method.java:511)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at com.android.internal.os.ZygoteInit$MethodAndArgsCaller.run(ZygoteInit.java:993)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	... 2 more
11-13 18:08:37.981 E/AndroidRuntime( 2143): Caused by: md52ce486a14f4bcd95899665e9d932190b.JavaProxyThrowable: System.Net.Http.HttpRequestException: 411 (Length Required)
11-13 18:08:37.981 E/AndroidRuntime( 2143): at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000b] in /Users/builder/data/lanes/2185/53fce373/source/mono/mcs/class/corlib/System.Runtime.ExceptionServices/ExceptionDispatchInfo.cs:61
11-13 18:08:37.981 E/AndroidRuntime( 2143): at System.Runtime.CompilerServices.AsyncMethodBuilderCore.<ThrowAsync>m__0 (object) [0x00000] in /Users/builder/data/lanes/2185/53fce373/source/mono/external/referencesource/mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs:1006
11-13 18:08:37.981 E/AndroidRuntime( 2143): at Android.App.SyncContext/<Post>c__AnonStorey0.<>m__0 () [0x00000] in /Users/builder/data/lanes/2185/53fce373/source/monodroid/src/Mono.Android/src/Android.App/SyncContext.cs:18
11-13 18:08:37.981 E/AndroidRuntime( 2143): at Java.Lang.Thread/RunnableImplementor.Run () [0x0000b] in /Users/builder/data/lanes/2185/53fce373/source/monodroid/src/Mono.Android/src/Java.Lang/Thread.cs:36
11-13 18:08:37.981 E/AndroidRuntime( 2143): at Java.Lang.IRunnableInvoker.n_Run (intptr,intptr) [0x00009] in /Users/builder/data/lanes/2185/53fce373/source/monodroid/src/Mono.Android/platforms/android-23/src/generated/Java.Lang.IRunnable.cs:71
11-13 18:08:37.981 E/AndroidRuntime( 2143): at (wrapper dynamic-method) object.22eeb877-0be7-40fd-b1d0-47ed6a14d1ad (intptr,intptr) <IL 0x00011, 0x0003b>
11-13 18:08:37.981 E/AndroidRuntime( 2143): 
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at mono.java.lang.RunnableImplementor.n_run(Native Method)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at mono.java.lang.RunnableImplementor.run(RunnableImplementor.java:29)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at android.os.Handler.handleCallback(Handler.java:605)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at android.os.Handler.dispatchMessage(Handler.java:92)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at android.os.Looper.loop(Looper.java:137)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	at android.app.ActivityThread.main(ActivityThread.java:4517)
11-13 18:08:37.981 E/AndroidRuntime( 2143): 	... 5 more

                */
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
            var httpClient = new HttpClient();

            var postData = new Dictionary<string, string>
            {
                { "dstnum", tonumber },
                { "textmsg", sms },
                { "pozivni3", prefix },
                { "Send3.x", "54"},
                { "Send3.y", "12" }
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

                UserDialogs.Instance.Alert("Server response: " + doc.DocumentNode.Descendants("div").Where(div => div.GetAttributeValue("class", string.Empty).Contains("notice")).Select(div => div.InnerText).First());

            }

            return response;
        }
    }
}
