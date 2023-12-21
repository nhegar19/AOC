using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Day8
    {
        public string RunSolution1()
        {
            long answer = 0;
            GameInfo game = ParseTextFileToGetGameInfo();
            answer = GetNumberOfStepsToReachEnd(ref game, answer);
            return answer.ToString();
        }

        public long RunSolution2()
        {
            long answer = 0;
            GameInfo game = ParseTextFileToGetGameInfo();
            answer = GetNumberOfStepsToReachEndSimultaneously(ref game, answer);
            return answer;
        }

        public long GetNumberOfStepsToReachEndSimultaneously(ref GameInfo game, long stepAmount)
        {
            List<GamePattern> gamePatterns = new List<GamePattern>();

            int directionIterator;
            bool foundPattern;
            long currentStep;
            string currentLocation;
            long stepsAfterRepeating;
            GamePattern newPattern;
            for (int spi = 0; spi < game.StartingPoints.Count(); spi++)
            {
                foundPattern = false;
                currentStep = 1;
                stepsAfterRepeating = 1;
                currentLocation = game.StartingPoints[spi];
                newPattern = new GamePattern();
                directionIterator = 0;
                while (foundPattern != true)
                {
                    if (directionIterator >= game.Directions.Length)
                    {
                        directionIterator = 0;
                    }

                    if (newPattern.Matches.ContainsKey(currentLocation))
                    {
                        var match = newPattern.Matches[currentLocation].Where(m => m.DirectionIterator == directionIterator).ToList();
                        //found a pattern if ran into the same location at the same point in the directions list
                        if(match.Count() > 0)
                        {
                            foundPattern = true;
                            newPattern.StepsBeforePatternStarts = currentStep;
                            //newPattern.Pattern.RemoveAt(newPattern.Pattern.Count() - 1);
                            newPattern.MatchFoundAt.DirectionIterator = directionIterator;
                            newPattern.MatchFoundAt.Index = newPattern.Pattern.Count() - 1;
                            newPattern.MatchFoundAt.Location = currentLocation;
                            gamePatterns.Add(newPattern);
                            break;
                        }
                        else
                        {
                            newPattern.Matches[currentLocation].Add(new Match() { DirectionIterator = directionIterator, Index= newPattern.Pattern.Count() -1 });
                        }

                    } else
                    {
                        Match newMatch = new Match() { DirectionIterator = directionIterator, Index = newPattern.Pattern.Count() - 1 };
                        newPattern.Matches.Add(currentLocation, new List<Match>() { newMatch });
                    }

                    if (game.Directions[directionIterator] == 'R')
                    {
                        currentLocation = game.Map[currentLocation][1];
                    }
                    else // else take the left
                    {
                        currentLocation = game.Map[currentLocation][0];
                    }
                    newPattern.Pattern.Add(currentLocation);
                    if (currentLocation.EndsWith("Z"))
                    {
                        newPattern.StepThatEndsInZForPattern.Add(currentStep);
                    }
                    currentStep++;
                    directionIterator++;
                }
            }
            stepAmount = 1;
            // need to calculate LCD for correct answer
            List<long> indexesEndingInZ = new List<long>();
            foreach (var p in gamePatterns)
            {
                indexesEndingInZ.Add(p.StepThatEndsInZForPattern[0]);
                stepAmount = stepAmount * p.StepThatEndsInZForPattern[0];
            }
            Console.WriteLine(stepAmount);//gcd
            stepAmount = getLowestCommonDenominator(indexesEndingInZ.ToArray());
            //after we get teh game Patterns, we need to use them to determine lowest possible amount of steps
            return stepAmount;
        }

        public long gcd(long n1, long n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return gcd(n2, n1 % n2);
            }
        }

        //I googled this code becuase I forgot how to Math
        public long getLowestCommonDenominator(long[] numbers)
        {
            return numbers.Aggregate((S, val) => S * val / gcd(S, val));
        }


        public long GetNumberOfStepsToReachEnd(ref GameInfo game, long stepAmount)
        {
            string currentLocation = "AAA";
            while (currentLocation != "ZZZ")
            {
                for (int i = 0; i < game.Directions.Length; i++)
                {
                    if (game.Directions[i] == 'R')
                    {
                        currentLocation = game.Map[currentLocation][1];
                    }
                    else // else take the left
                    {
                        currentLocation = game.Map[currentLocation][0];
                    }

                    stepAmount++;
                    if (currentLocation == "ZZZ")
                    {
                        break;
                    }
                }
            }

            return stepAmount;
        }

        public class Match {
            public long DirectionIterator {get;set;}
            public long Index { get; set; }
            public string Location { get; set; }
        }

        public class GamePattern
        {
            public GamePattern()
            {
                Pattern = new List<string>();
                StepThatEndsInZForPattern = new List<long>();
                DirectionIterator = new List<long>();
                StepsBeforePatternStarts = 0;
                Matches = new Dictionary<string, List<Match>>();
                MatchFoundAt = new Match();
            }

            public List<string> Pattern { get; set; }
            public List<long> StepThatEndsInZForPattern { get; set; }
            public Dictionary<string, List<Match>> Matches { get; set; }
            public List<long> DirectionIterator { get; set; }
            public long StepsBeforePatternStarts { get; set; }
            public Match MatchFoundAt { get; set; }

        }

        public GameInfo ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            GameInfo game = new GameInfo();
            string mapKey = "";
            List<string> map = new List<string>();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day8/TextFile3.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine().Trim()) != "")
                {
                    game.Directions += line;
                }

                //streamReader.ReadLine();//skip next blank line

                while ((line = streamReader.ReadLine()) != null)
                {
                    ExtractGameInfo(line, out mapKey, out map);
                    if (mapKey.EndsWith('A'))
                    {
                        game.StartingPoints.Add(mapKey);
                    }
                    game.Map.Add(mapKey, map);
                }
            }

            return game;
        }

        public class GameInfo
        {
            public GameInfo()
            {
                Map = new Dictionary<string, List<string>>();
                StartingPoints = new List<string>();
            }

            public string Directions { get; set; }
            public Dictionary<string, List<string>> Map { get; set; }
            public List<string> StartingPoints { get; set; }
        }

        public GameInfo ExtractGameInfo(string gameLine, out string key, out List<string> mapping)
        {
            GameInfo gameInfo = new GameInfo();
            key = "";

            var info = gameLine.Split('=').ToList();
            key = info[0].Trim();
            string dirtyList = info[1].Replace(")", "").Replace("(", "");
            mapping = dirtyList.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            return gameInfo;
        }
    }
}
