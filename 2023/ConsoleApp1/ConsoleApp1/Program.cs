using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleApp1;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            long lowest = 0;
            Day5FastSolution day5 = new Day5FastSolution();
            lowest = day5.RunSolution2();

            Console.WriteLine(lowest);
        }

      


    }

}
