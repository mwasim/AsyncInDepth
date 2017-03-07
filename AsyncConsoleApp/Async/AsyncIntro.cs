using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncConsoleApp.Async
{
    public class AsyncIntro
    {        
        public static void DisplayMessage()
        {
            var message = GetMessage(5000, "Hello Sync World!");
            WriteLine(message);
            WriteLine();

            var task = GetMessageConvertedToAsync(5000, "Hello Async World!");
            Action<Task<string>> action = async t =>
            {
                var msg = await t;
                WriteLine(msg);
            };

            task.ContinueWith(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay">milliseconds</param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string GetMessage(int delay, string message)
        {
            Task.Delay(delay);

            //this string is return even the delay above completes because its not awaited
            return $"Message: {message} with delay: {delay}";
        }

        #region "Method converted to Async"
        private static Func<int, string, string> _getMessageFunc = GetMessage;

        private static IAsyncResult BeginGetMessage(int delay, string message, AsyncCallback callback, object state)
        {
            return _getMessageFunc.BeginInvoke(delay, message, callback, state);
        }

        private static string OnMessageReceivedCallback(IAsyncResult ar)
        {
            return _getMessageFunc.EndInvoke(ar);
        }

        public static async Task<string> GetMessageConvertedToAsync(int delay, string message)
        {
            var result = await Task<string>.Factory.FromAsync<int, string>(BeginGetMessage, OnMessageReceivedCallback, delay, message, null);
            return result;
        }
        #endregion

        #region "Method Converted to Async 2"

        public static void ShowNumbers()
        {
            PrintNumbersAsync().Wait();
        }

        private static void PrintNumbers()
        {
            for (int i = 0; i < 25; i++)
            {
                Write($"{i} ");
            }
        }

        private static Action _printNumbers = PrintNumbers;

        private static async Task PrintNumbersAsync()
        {
            await Task.Factory.FromAsync(BeginPrintNumbers, EndPrintNumbers, null);
        }

        private static void EndPrintNumbers(IAsyncResult result)
        {
            _printNumbers.EndInvoke(result);
        }

        private static IAsyncResult BeginPrintNumbers(AsyncCallback callback, object state)
        {
            return _printNumbers.BeginInvoke(callback, state);
        }
        #endregion

        #region "Method converted to Async 3"

        public static void DisplayComplexNumbers()
        {
            const int count = 10;
            WriteLine("Synchronous Call");
            var list = GetComplexNumberList(count);
            WriteNumbers(list);

            WriteLine("Asynchronous Call");
            var taskGetComplexNumberList = GetComplexNumberListAsync(count);
            taskGetComplexNumberList.ContinueWith(task =>
                {
                    var items = task.Result;
                    WriteNumbers(items);
                }
            );
        }

        private static void WriteNumbers(List<AsyncIntro.ComplexNumber> list)
        {
            foreach (var complexNumber in list)
            {
                WriteLine(complexNumber);
            }
        }

        private static List<AsyncIntro.ComplexNumber> GetComplexNumberList(int count)
        {
            var list = new List<AsyncIntro.ComplexNumber>();
            for (int i = 0; i < count; i++)
            {
                var number = i + 1;
                list.Add(new AsyncIntro.ComplexNumber(number, number * Math.PI));
            }

            return list;
        }

        private static async Task<List<AsyncIntro.ComplexNumber>> GetComplexNumberListAsync(int count)
        {
            var task = await Task<List<AsyncIntro.ComplexNumber>>.Factory.FromAsync<int>(BeginGetComplexNumberList,
                EndGetComplexNumberList, count, null);

            return task;
        }

        private static Func<int, List<AsyncIntro.ComplexNumber>> _getComplexNumberListInvoker = GetComplexNumberList;
        private static List<AsyncIntro.ComplexNumber> EndGetComplexNumberList(IAsyncResult asyncResult)
        {
            return _getComplexNumberListInvoker.EndInvoke(asyncResult);
        }

        private static IAsyncResult BeginGetComplexNumberList(int count, AsyncCallback asyncCallback, object state)
        {
            return _getComplexNumberListInvoker.BeginInvoke(count, asyncCallback, state);
        }


        private struct ComplexNumber
        {
            private int _i;
            private double _imaginary;

            public ComplexNumber(int i, double imaginary)
            {
                _i = i;
                _imaginary = imaginary;
            }

            public override string ToString()
            {
                return $"{_i}+{Math.Round(_imaginary, 2)}";
            }
        }
        #endregion
    }
}