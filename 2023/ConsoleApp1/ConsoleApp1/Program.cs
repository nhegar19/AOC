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
            long answer = 0;
            Day8 game = new Day8();
            answer = game.RunSolution2();

            Console.WriteLine(answer);
        }

      


    }

}
