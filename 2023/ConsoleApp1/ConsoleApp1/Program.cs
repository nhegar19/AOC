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
            Day7 game = new Day7();
            answer = game.RunSolution2();

            Console.WriteLine(answer);
        }

      


    }

}
