using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AsyncConsoleApp.Async
{
    public class AsyncDelegatesAndLambdas
    {
        public static void Example1()
        {
            Func<Task<int>> getNumberFunc = async delegate { return 3; }; //delegate syntax

            //lambda expression syntax
            Func<int, int, Task<int>> addNumberFunc = async (number1, number2) => number1 + number2;

            var task = addNumberFunc(5, 7);
            task.ContinueWith(async t =>
            {
                var result = await t;
                WriteLine(result);
            });

            task = getNumberFunc();
            task.ContinueWith(async t =>
            {
                var result = await t;
                WriteLine(result);
            });
        }
    }
}
