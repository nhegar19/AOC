using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Day2
    {
        public static int RunSolution1()
        {
            int total;
            Dictionary<int, Game[]> allGames = ParseTextFileToGetGameInfo();
            total = DetermineIndexSumOfGameIsPlayable(12, 14, 13, allGames);
            return total;
        }

        public static int RunSolution2()
        {
            int total;
            Dictionary<int, Game[]> allGames = ParseTextFileToGetGameInfo();
            total = DeterminePowerOfAllGames(allGames);
            return total;
        }

        public static Dictionary<int, Game[]> ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            int gameNumber = 1;
            Dictionary<int, Game[]> allGames = new Dictionary<int, Game[]>();
            using (var fileStream = File.OpenRead("C:/Users/hegarmai/source/repos/ConsoleApp1/ConsoleApp1/Day2/TextFile1.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Game[] game = ExtractGamesInfo(line);
                    allGames.Add(gameNumber, game);
                    gameNumber++;
                }
            }

            return allGames;
        }

        public static int DeterminePowerOfAllGames(Dictionary<int, Game[]> allGames)
        {
            int totalPower = 0;
            foreach (var game in allGames)
            {
                totalPower += DeterminePowerOfGame(game.Value);
            }

            return totalPower;
        }


        public static int DeterminePowerOfGame(Game[] game)
        {
            int power = 0;
            int bigRed = 1;
            int bigBlue = 1;
            int bigGreen = 1;
            for (int i = 0; i < game.Length; i++)
            {
                if (game[i].Red > bigRed) bigRed = game[i].Red;

                if (game[i].Blue > bigBlue) bigBlue = game[i].Blue;

                if (game[i].Green > bigGreen) bigGreen = game[i].Green;
            }

            power = bigRed * bigBlue * bigGreen;

            return power;
        }

        public class Game
        {
            public int Red { get; set; }
            public int Blue { get; set; }
            public int Green { get; set; }
        }

        public static Game[] ExtractGamesInfo(string gameText)
        {
            List<Game> game = new List<Game>();
            string[] wholeGame = gameText.Split(':');
            string[] games = wholeGame[1].Split(';');
            int num; //initialize once
            string stringNum;
            for (int i = 0; i < games.Length; i++)
            {
                Game gameRound = new Game();

                string[] info = games[i].Split(',');
                for (int j = 0; j < info.Length; j++)
                {
                    num = 0;
                    if (info[j].Contains("blue"))
                    {
                        stringNum = info[j].Replace(" blue", "");
                        int.TryParse(stringNum, out num);
                        gameRound.Blue = num;
                    }
                    else if (info[j].Contains("red"))
                    {
                        stringNum = info[j].Replace(" red", "");
                        int.TryParse(stringNum, out num);
                        gameRound.Red = num;
                    }
                    else if (info[j].Contains("green"))
                    {
                        stringNum = info[j].Replace(" green", "");
                        int.TryParse(stringNum, out num);
                        gameRound.Green = num;
                    }
                }
                Console.Write("RED: " + gameRound.Red + ", BLUE: " + gameRound.Blue + ", GREEN: " + gameRound.Green);
                Console.WriteLine();
                game.Add(gameRound);
            }



            return game.ToArray();

        }

        private static int DetermineIndexSumOfGameIsPlayable(int red, int blue, int green, Dictionary<int, Game[]> allGames)
        {
            int indexTotal = 0;
            bool isPlayable = true;

            foreach (var game in allGames)
            {
                isPlayable = true;
                for (int i = 0; i < game.Value.Length; i++)
                {
                    var round = game.Value[i];
                    if (round.Red > red || round.Blue > blue || round.Green > green)
                    {
                        isPlayable = false;
                    }
                }
                if (isPlayable)
                {
                    int gameNumber = game.Key;
                    indexTotal += gameNumber;
                }
            }

            return indexTotal;
        }


    }
}
