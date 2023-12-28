using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Day9
    {

        public long RunSolution1()
        {
            long answer = 0;
            GameInfo gInfo = new GameInfo();
            gInfo = ParseTextFileToGetGameInfo();
            answer = GetSumOfExtrapolatedValues(gInfo);

            return answer;
        }

        public long RunSolution2()
        {
            long answer = 0;
            GameInfo gInfo = new GameInfo();
            gInfo = ParseTextFileToGetGameInfo();
            answer = GetSumOfPreviousValues(gInfo);

            return answer;
        }

        public long GetSumOfExtrapolatedValues(GameInfo gInfo)
        {
            long sum = 0;

            foreach (var valList in gInfo.ValueHistories)
            {
                sum += GetNextValueOfSequence(valList);
            }

            return sum;
        }

        public long GetSumOfPreviousValues(GameInfo gInfo)
        {
            long sum = 0;

            foreach (var valList in gInfo.ValueHistories)
            {
                sum += GetPreviousValueOfSequence(valList);
            }

            return sum;
        }

        public long GetPreviousValueOfSequence(List<long> valSequence)
        {
            long nextVal = 0;
            List<List<long>> LastNums = new List<List<long>>();

            List<long> currentSequence = valSequence;
            List<long> newSequence = valSequence;
            List<long> lastSequence = valSequence;
            long lastDigit = valSequence[valSequence.Count() - 1];
            long diff = 0;
            bool canEnd = false;
            LastNums.Add(new List<long>() { currentSequence.First(), 0 });
            while (canEnd == false)
            {
                currentSequence = newSequence;
                newSequence = new List<long>();

                for (int i = 0; i < currentSequence.Count() - 1; i++)
                {
                    diff = currentSequence[i] - currentSequence[i + 1];
                    newSequence.Add(diff);
                }

                if (newSequence.Count() == 0 || newSequence.All((i) => i == 0))
                {
                    canEnd = true;
                }

                lastDigit = newSequence.Count() > 0 ? newSequence.First() : 0;
                LastNums.Add(new List<long>() { lastDigit, 0 });
            }

            //ableToCalculate because found a 0 sequence
            if ((newSequence.Count() > 0 && newSequence.Last() == 0))
            {
                for (int i = LastNums.Count() - 1; i > 0; i--)
                {
                    LastNums[i - 1][1] = LastNums[i][0] + LastNums[i][1];
                    nextVal = LastNums[i - 1][1];

                }
                nextVal = LastNums[0][0] + LastNums[0][1];
            }
            else
            {
                nextVal = 0;
            }
            return nextVal;
        }

        public long GetNextValueOfSequence(List<long> valSequence)
        {
            long nextVal = 0;
            List<List<long>> LastNums = new List<List<long>>();

            List<long> currentSequence = valSequence;
            List<long> newSequence = valSequence;
            List<long> lastSequence = valSequence;
            long lastDigit = valSequence[valSequence.Count() - 1];
            long diff = 0;
            bool canEnd = false;
            LastNums.Add(new List<long>() {valSequence.Last(), 0 });
            while (canEnd == false)
            {
                currentSequence = newSequence;
                newSequence = new List<long>();

                for (int i = currentSequence.Count() -1; i > 0; i--)
                {
                    diff = currentSequence[i] - currentSequence[i - 1];
                    newSequence.Add(diff);
                }

                if (newSequence.Count() == 0 || newSequence.All((i)=> i == 0))
                {
                    canEnd = true;
                    
                }

                lastDigit = newSequence.Count() > 0 ? newSequence.First() : 0;
                LastNums.Add(new List<long>() { lastDigit, 0 });
                newSequence.Reverse();
            }

            //ableToCalculate because found a 0 sequence
            if ((newSequence.Count() > 0 && newSequence.Last() == 0))
            {
                for (int i = LastNums.Count() - 1; i > 0; i--)
                {
                    LastNums[i - 1][1] = LastNums[i][0] + LastNums[i][1];
                    nextVal = LastNums[i - 1][1];
                    
                }
                nextVal = LastNums[0][0] + LastNums[0][1];
            }
            else
            {
                nextVal = 0;
            }
            return nextVal;
        }

        public GameInfo ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            GameInfo game = new GameInfo();
            string mapKey = "";
            List<long> valueHistory = new List<long>();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day9/TextFile2.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    valueHistory = ExtractGameInfo(line);
                    game.ValueHistories.Add(valueHistory);
                }
            }

            return game;
        }

        public class GameInfo
        {
            public GameInfo()
            {
                ValueHistories = new List<List<long>>();
            }

            public List<List<long>> ValueHistories { get; set; }
        }

        public List<long> ExtractGameInfo(string gameLine)
        {
            List<long> valHistory = new List<long>();

           var valHistoryS = gameLine.Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            valHistory = valHistoryS.Select(v => long.Parse(v)).ToList();

            return valHistory;
        }
    }
}
