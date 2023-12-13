using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Day4
    {
        public static int RunSolution1()
        {
            int totalGamePoints = 0;
            int gamePoints = 0;
            List<Game> allGames = ParseTextFileToGetGameInfo();
            foreach (var game in allGames)
            {
                gamePoints = GetGamePoints(game);
                totalGamePoints += gamePoints;
            }
            Console.WriteLine(totalGamePoints);

            return totalGamePoints;
        }

        public static int RunSolution2()
        {
            int totalCards = 0;
            int gamePoints = 0;
            List<Game> allGames = ParseTextFileToGetGameInfo();
            int[] copyCollector = new int[allGames.Count()];
            for (int i = 0; i < allGames.Count(); i++)
            {
                gamePoints = GetPart2GamePoints(allGames[i]);

                for (int j = i + 1; j <= i + gamePoints; j++) // always run card once
                {
                    if (j < allGames.Count())
                    {
                        copyCollector[j] += 1 + copyCollector[i];
                    }
                }
                totalCards += 1 + copyCollector[i];
            }
            Console.WriteLine(totalCards);

            return totalCards;
        }

        public static int GetPart2GamePoints(Game game)
        {
            int points = 0;
            foreach (var cardNum in game.PlayerNumbers)
            {
                if (game.CardNumbers.Contains(cardNum))
                {
                    points++;
                }
                //Console.WriteLine(points);
            }

            return points;
        }

        public static int GetGamePoints(Game game)
        {
            int points = 0;
            foreach (var cardNum in game.PlayerNumbers)
            {
                if (game.CardNumbers.Contains(cardNum))
                {
                    if (points == 0)
                    {
                        points = 1;
                    }
                    else
                    {
                        points = points * 2;
                    }
                }
            }

            return points;
        }

        public static List<Game> ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            List<Game> allGames = new List<Game>();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day4/TextFile2.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Game game = ExtractGameInfo(line);
                    allGames.Add(game);
                }
            }

            return allGames;
        }

        public class Game
        {
            public Game()
            {
                CardNumbers = new List<string>();
                PlayerNumbers = new List<string>();
            }

            public List<string> CardNumbers { get; set; }
            public List<string> PlayerNumbers { get; set; }
        }

        public static Game ExtractGameInfo(string gameText)
        {
            Game game = new Game();
            string[] wholeGameLine = gameText.Split(':');
            string[] cards = wholeGameLine[1].Split('|');
            game.CardNumbers = cards[0].Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            game.PlayerNumbers = cards[1].Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            return game;

        }

    }
}
