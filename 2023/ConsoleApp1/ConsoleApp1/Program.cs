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
            Day9 game = new Day9();
            answer = game.RunSolution1();

            Console.WriteLine(answer);
        }
    }

}
