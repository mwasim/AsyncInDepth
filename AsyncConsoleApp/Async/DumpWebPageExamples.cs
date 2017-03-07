using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncConsoleApp.Async
{
    public class DumpWebPageExamples
    {
        private static string _WebPageUrl = "https://httpbin.org/get?color=red&shape=oval";
        public static void ShowWebPage()
        {
            //var url = "https://httpbin.org/get?color=red&shape=oval";
            DumpWebPage(_WebPageUrl);
            //DumpleWebPageAsync(url);
            //DumpleWebPage2Async(url);
            //DumpWebPageContinueWith(url);
        }

        private static void DumpWebPage(string url)
        {
            var client = new WebClient();
            var page = client.DownloadString(url);

            //loop executes synchronously
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                WriteLine($"Counter = {i}");
                WriteLine("---------------");
                WriteLine(client.DownloadString(url));
            }

            WriteLine(page);

            sw.Stop();

            WriteLine("--------------------------------\n\n\n");
            WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds} milliseconds = {sw.Elapsed.Seconds} seconds");
        }

        private static async void DumpleWebPageAsync(string url)
        {
            var client = new WebClient();
            //var page = await client.DownloadStringTaskAsync(url);

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                WriteLine($"Counter = {i}");
                WriteLine("---------------");
                WriteLine(await client.DownloadStringTaskAsync(url));
            }
            sw.Stop();

            WriteLine("--------------------------------\n\n\n");
            WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds} milliseconds = {sw.Elapsed.Seconds} seconds");

            //WriteLine(page);
        }

        public async static void ShowMultipleWebPagesContentAsynchronously()
        {
            List<Task<string>> list = new List<Task<string>>();
            //var url = "https://httpbin.org/get?color=red&shape=oval";

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                list.Add(GetWebPageContentAsync(_WebPageUrl));
            }

            var result = await Task.WhenAll(list);
            int j = 1;
            foreach (var output in result)
            {
                WriteLine($"Counter = {j}");
                WriteLine("---------------");
                WriteLine(output);
                j++;
            }

            //list.ForEach(t => t.ContinueWith(async task =>
            //  {
            //      var result = await task;
            //      WriteLine(result);
            //  }));

            sw.Stop();

            WriteLine("--------------------------------\n\n\n");
            WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds} milliseconds = {sw.Elapsed.Seconds} seconds");
        }

        private static async Task<string> GetWebPageContentAsync(string url)
        {
            var task = await new WebClient().DownloadStringTaskAsync(url);
            return task;
        }

        private static void DumpWebPageContinueWith(string url)
        {
            var client = new WebClient();
            var downloadStringTask = client.DownloadStringTaskAsync(url);

            downloadStringTask.ContinueWith(async task =>
            {
                //WriteLine(task);
                var result = await task;
                WriteLine(result);
            });
        }

        public static void DisplayWebPageCharCount()
        {
            var task = GetWebPageCharCountAsync();

            task.ContinueWith(async t =>
            {
                var result = await t;

                WriteLine($"Character Count: {result}");
            });
        }

        private static async Task<int> GetWebPageCharCountAsync()
        {
            using (var client = new WebClient())
            {
                var page = await client.DownloadStringTaskAsync(_WebPageUrl);

                return page.Length;
            }
        }

        //Following multiple concurrent I/O (Network) are not supported asynchronously
        //The right solution is above in ShowMultipleWebPagesContentAsynchronously()
        /*
        private static async void DumpleWebPage2Async(string url)
        {
            var client = new WebClient();
            //var page = await client.DownloadStringTaskAsync(url);

            var t1 = client.DownloadStringTaskAsync(url);
            var t2 = client.DownloadStringTaskAsync(url);
            var t3 = client.DownloadStringTaskAsync(url);
            var t4 = client.DownloadStringTaskAsync(url);
            var t5 = client.DownloadStringTaskAsync(url);
            var t6 = client.DownloadStringTaskAsync(url);
            var t7 = client.DownloadStringTaskAsync(url);
            var t8 = client.DownloadStringTaskAsync(url);
            var t9 = client.DownloadStringTaskAsync(url);
            var t10 = client.DownloadStringTaskAsync(url);

            var sw = new Stopwatch();
            sw.Start();
            //for (int i = 0; i < 10; i++)
            //{
            //    WriteLine($"Counter = {i}");
            //    WriteLine("---------------");
            //    WriteLine(await client.DownloadStringTaskAsync(url));
            //}
            var resultList = await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

            foreach (var result in resultList)
            {
                WriteLine(result);
            }
            sw.Stop();

            WriteLine("--------------------------------\n\n\n");
            WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds} milliseconds = {sw.Elapsed.Seconds} seconds");

            //WriteLine(page);
        }*/
    }
}