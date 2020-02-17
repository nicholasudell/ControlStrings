using ControlStrings;
using System;
using System.Threading;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine(new ControlStrings.Example.Example().ToString());
                Console.WriteLine("================");
            }
        }
    }
}
