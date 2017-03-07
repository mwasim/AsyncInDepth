using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FaviconApp.Core
{
    public class FaviconService
    {
        static List<string> _DomainList = new List<string>
        {
            "www.tutorialspoint.com",
            "www.xamarin.com",
            "www.github.com",
            "www.bitbucket.org",
            "www.google.com",
            "www.bing.com",
            "www.oreilly.com",
            "www.microsoft.com",
            "www.facebook.com",
            "www.twitter.com",
        };

        public async static Task<List<Image>> GetFaviconImages()
        {
            var tasks = new List<Task<byte[]>>();
            foreach (var domainName in _DomainList)
            {
                tasks.Add(GetFavicon(GetFaviconUrl(domainName)));
            }

            var byteArray = await Task.WhenAll(tasks);

            return BuildImageList(byteArray);
        }

        private static string GetFaviconUrl(string domainName)
        {
            return $"https://{domainName}/favicon.ico";
        }

        private static List<Image> BuildImageList(byte[][] byteArray)
        {
            var imageList = new List<Image>();
            foreach (var bytes in byteArray)
            {
                //Build an image from the byte array item
                imageList.Add(BuildImage(bytes));
            }

            return imageList;
        }

        private static async Task<byte[]> GetFavicon(string url)
        {
            using (var client = new WebClient())
            {
                var task = await client.DownloadDataTaskAsync(url);

                return task;
            }
        }


        private static Image BuildImage(byte[] bytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();

            return new Image
            {
                Source = bitmapImage,
                Width = 32,
                Height = 32,
            };
        }
    }
}
