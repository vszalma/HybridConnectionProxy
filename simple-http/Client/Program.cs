using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Relay;
using System.Net.Http;
using System.Net;

namespace Client
{
    class Program
    {
        public static System.Net.IWebProxy DefaultWebProxy { get; set; }

        static void Main(string[] args)
        {
            RunAsync(args).GetAwaiter().GetResult();
        }

        static async Task RunAsync(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("dotnet client [ns] [hc] [keyname] [key]");
                return;
            }
            
            var ns = args[0];
            var hc = args[1];
            var keyname = args[2];
            var key = args[3];

            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyname, key);
            var uri = new Uri(string.Format("https://{0}/{1}", ns, hc));
            var token = (await tokenProvider.GetTokenAsync(uri.AbsoluteUri, TimeSpan.FromHours(1))).TokenString;

            // TODO:
            // use this code (line 39) to hard code a specific proxy URL.
            //var webProxy = new WebProxy(new Uri("http://192.168.1.140:808"));

            // TODO:
            // use this code (line 43) to use the system settings for the user running the code. Lines 43-51 were added or modified from original sample.
            var webProxy = HttpWebRequest.DefaultWebProxy;
            //var proxysetting = Program.DefaultWebProxy;

            var proxyHttpClientHandler = new HttpClientHandler
            {
                Proxy = webProxy,
                UseProxy = true,
            };
            var client = new HttpClient(proxyHttpClientHandler);
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = HttpMethod.Get,
            };
            request.Headers.Add("ServiceBusAuthorization", token);
            var response = await client.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}