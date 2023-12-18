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
            double answer = 0;
            Day6 day6 = new Day6();
            answer = day6.FindProductOfAllWaysToWinRace();

            Console.WriteLine(answer);
        }

      


    }

}
