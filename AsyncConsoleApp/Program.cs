using AsyncConsoleApp.Async;
using static System.Console;

namespace AsyncConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //AsyncIntro.ShowWebPage();
            //WriteLine("Hello"); //Hello is written first while running Async method ShowWebPage
            //for (int i = 0; i < 50000; i++)
            //{
            //    Write($"{i}, ");
            //}
            //DumpWebPageExamples.ShowMultipleWebPagesContentAsynchronously();


            //AsyncIntro.DisplayMessage();
            //AsyncIntro.ShowNumbers();
            //AsyncIntro.DisplayComplexNumbers();

            //FaviconAsync.DisplayFavicons();
            //DumpWebPageExamples.DisplayWebPageCharCount();

            AsyncDelegatesAndLambdas.Example1();

            ReadKey();
        }
    }
}