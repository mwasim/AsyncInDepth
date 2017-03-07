using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncConsoleApp.Async
{
    public class FaviconAsync
    {

        public async static void DisplayFavicons()
        {
            var urls = new List<string>
            {
                "https://www.tutorialspoint.com/favicon.ico",
                "https://www.xamarin.com/favicon.ico",
                "https://github.com/favicon.ico"
            };

            var tasks = new List<Task<byte[]>>();
            foreach (var url in urls)
            {
                tasks.Add(GetFavicon(url));
            }

            var result = await Task.WhenAll(tasks);

            foreach (var item in result)
            {
                //Build an image from the byte array item
                //See the FaviconApp WPF Application Example in the same solution
               WriteLine(item);
            }
        }

        private static async Task<byte[]> GetFavicon(string url)
        {
            using (var client = new WebClient())
            {
                var task = await client.DownloadDataTaskAsync(url);

                return task;
            }
        }

        
    }
}
