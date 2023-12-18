using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Day6
    {
        //Solution 1 & 2
        public double FindProductOfAllWaysToWinRace()
        {
            Game raceInfo = ParseTextFileToGetGameInfo();
            double waysToWinProduct = 1;
            double waysToWin = 0;
            double numToTie ;
            for (int i = 0; i < raceInfo.Times.Count(); i++)
            {

                double time = double.Parse(raceInfo.Times[i]);
                double distance = double.Parse(raceInfo.Distances[i]);

                // solving for the fking quadratic formula, OMG
                //time - x + (x)(time - x) = 9
                // (x)(7-x) = 9
                // 7x -x^2 = 9
                // 7x -x^2 -9 =0
                //x^2 - 7x + 9 = 0

                int a = 1;
                double b = -(time);
                double c =  distance;
                double a1 = 0;
                double a2 = 0;

                a1 = (-b + Math.Sqrt((b * b) - (4 * a * c))) / 2;
                a2 = (-b - Math.Sqrt((b * b) - (4 * a * c))) / 2;

                double lowerBound = a1 > a2 ? a2 : a1;
                double upperBound = a1 > a2 ? a1 : a2;

                Console.WriteLine(upperBound + " " + lowerBound);
                waysToWin = Math.Floor(upperBound) - Math.Ceiling(lowerBound) + 1;
                Console.WriteLine(waysToWin);
                //don't include ties
                if (Math.Floor(upperBound) == upperBound) { waysToWin--; }
                if(Math.Ceiling(lowerBound) == lowerBound) { waysToWin--; }

                waysToWinProduct = waysToWinProduct * waysToWin;
            }

            return waysToWinProduct;
        }
        public Game ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            Game game = new Game();
            //Solution1: use textfile2.txt 
            //Solution2: use textfile3.txt 
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day6/TextFile3.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    game.Times = ExtractGameInfo(line);

                    line = streamReader.ReadLine();
                    game.Distances = ExtractGameInfo(line);
                }
            }

            return game;
        }

        public class Game
        {
            public Game()
            {
                Times = new List<string>();
                Distances = new List<string>();
            }

            public List<string> Times { get; set; }
            public List<string> Distances { get; set; }
        }

        public List<string> ExtractGameInfo(string gameLine)
        {
            List<string> gameInfo = new List<string>();

            string[] wholeGameLine = gameLine.Split(':');
            gameInfo = wholeGameLine[1].Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            return gameInfo;

        }
    }
}
