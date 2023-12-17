using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Day5
    {
        public static long RunSolution1()
        {
            long lowestLocationNumber = 0;
            AlmanacInfo almanac = ParseTextFileToGetAlmanacInfo();
            lowestLocationNumber = GetLowestLocation(almanac);


            Console.WriteLine(lowestLocationNumber);

            return lowestLocationNumber;
        }

        public static long RunSolution2()
        {
            long lowestLocationNumber = 0;
            AlmanacInfo almanac = ParseTextFileToGetAlmanacInfo();
            lowestLocationNumber = GetLowestLocationFromSeedRanges(almanac);

            Console.WriteLine(lowestLocationNumber);

            return lowestLocationNumber;
        }

        public static long RunFastSolution2()
        {
            long lowestLocationNumber = 0;
            long currentLowest;
            AlmanacInfo almanac = ParseTextFileToGetAlmanacInfo();
            lowestLocationNumber = GetLowestLocationFromSeedRanges(almanac);
            long seedStart;
            long seedEnd;
            for (int i = 0; i < almanac.Seeds.Count(); i = i + 2)
            {
                seedStart = long.Parse(almanac.Seeds[i]);
                seedEnd = long.Parse(almanac.Seeds[i + 1]);
                currentLowest = GetLowestLocationFromSeedRange(almanac, seedStart, seedEnd, 1);

                if (lowestLocationNumber == 0 || currentLowest < lowestLocationNumber)
                {
                    lowestLocationNumber = currentLowest;
                }
            }
            
            Console.WriteLine(lowestLocationNumber);

            return lowestLocationNumber;
        }

        public static long GetLowestLocationFromSeedRange(AlmanacInfo almanac, long seedStart, long seedEnd, int step)
        {
            long lowestLocation = 0;
            long currentMappedNumber;
            List<List<long>> currentMapping = new List<List<long>>();
            int[] startRange = new int[2];
            int[] inRange = new int[2];
            int[] endRange = new int[2];

            switch (step)
            {
                case 1:
                    currentMapping = almanac.SeedToSoilMapping;
                    break;
                case 2:
                    currentMapping = almanac.SoilToFertilizerMapping;
                    break;
                case 3:
                    currentMapping = almanac.FertilizerToWaterMapping;
                    break;
                case 4:
                    currentMapping = almanac.WaterToLightMapping;
                    break;
                case 5:
                    currentMapping = almanac.LightToTemperatureMapping;
                    break;
                case 6:
                    currentMapping = almanac.TemperatureToHumidityMapping;
                    break;
                case 7:
                    currentMapping = almanac.HumidityToLocationMapping;
                    break;
                default:
                    break;
            }
            step = step + 1;

            foreach (var mapper in currentMapping)
            {

            }


            return lowestLocation;
        }

        public static long GetLowestLocationFromSeedRanges(AlmanacInfo almanac)
        {
            //79004094
            long lowestLocation = 0;
            long currentMappedNumber;
            long seed1;
            long seed2;
            for (int i = 0; i < almanac.Seeds.Count(); i = i+2)
            {
                seed1 = long.Parse(almanac.Seeds[i]);
                seed2 = long.Parse(almanac.Seeds[i+1]);
                long range = seed1 + seed2;

                for (long j = seed1; j < range; j++)
                {
                    currentMappedNumber = GetMappedNumber(almanac.SeedToSoilMapping, j);
                    currentMappedNumber = GetMappedNumber(almanac.SoilToFertilizerMapping, currentMappedNumber);
                    currentMappedNumber = GetMappedNumber(almanac.FertilizerToWaterMapping, currentMappedNumber);
                    currentMappedNumber = GetMappedNumber(almanac.WaterToLightMapping, currentMappedNumber);
                    currentMappedNumber = GetMappedNumber(almanac.LightToTemperatureMapping, currentMappedNumber);
                    currentMappedNumber = GetMappedNumber(almanac.TemperatureToHumidityMapping, currentMappedNumber);
                    currentMappedNumber = GetMappedNumber(almanac.HumidityToLocationMapping, currentMappedNumber);

                    if (lowestLocation == 0 || currentMappedNumber < lowestLocation)
                    {
                        lowestLocation = currentMappedNumber;
                    }
                }
            }

            return lowestLocation;
        }

        public static long GetLowestMappedNumber(List<List<long>> mapping, long mapNumber)
        {
            long mappedNumber = 0;

            long range;
            long startAt;
            long endAt;
            long mapToStartAt;
            bool foundMatch = false;

            foreach (var line in mapping)
            {
                range = line[2];
                startAt = line[1];
                mapToStartAt = line[0];
                endAt = startAt + range;

                if (mapNumber >= startAt && mapNumber < endAt)
                {
                    mappedNumber = (mapNumber - startAt) + mapToStartAt;
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                mappedNumber = mapNumber;
            }

            return mappedNumber;
        }

        public static long GetLowestLocation(AlmanacInfo almanac)
        {
            long lowestLocation = 0;
            long currentMappedNumber;
            long seed;
            foreach (var s in almanac.Seeds)
            {
                seed = long.Parse(s);
                currentMappedNumber = GetMappedNumber(almanac.SeedToSoilMapping, seed);
                currentMappedNumber = GetMappedNumber(almanac.SoilToFertilizerMapping, currentMappedNumber);
                currentMappedNumber = GetMappedNumber(almanac.FertilizerToWaterMapping, currentMappedNumber);
                currentMappedNumber = GetMappedNumber(almanac.WaterToLightMapping, currentMappedNumber);
                currentMappedNumber = GetMappedNumber(almanac.LightToTemperatureMapping, currentMappedNumber);
                currentMappedNumber = GetMappedNumber(almanac.TemperatureToHumidityMapping, currentMappedNumber);
                currentMappedNumber = GetMappedNumber(almanac.HumidityToLocationMapping, currentMappedNumber);

                if(lowestLocation == 0 || currentMappedNumber < lowestLocation)
                {
                    lowestLocation = currentMappedNumber;
                }
            }

            return lowestLocation;
        }

        public static long GetMappedNumber(List<List<long>> mapping, long mapNumber)
        {
            long mappedNumber = 0;

            long range;
            long startAt;
            long endAt;
            long mapToStartAt;
            bool foundMatch = false;

            foreach (var line in mapping)
            {
                range = line[2];
                startAt = line[1];
                mapToStartAt = line[0];
                endAt = startAt + range; 

                if(mapNumber>= startAt && mapNumber < endAt)
                {
                    mappedNumber = (mapNumber-startAt) + mapToStartAt;
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                mappedNumber = mapNumber;
            }

            return mappedNumber;
        }
        public static AlmanacInfo ParseTextFileToGetAlmanacInfo()
        {
            const Int32 BufferSize = 128;
            AlmanacInfo almanac = new AlmanacInfo();
            List<string> mappingLines = new List<string>();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day5/TextFile1.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith("seeds"))
                    {
                      almanac.Seeds = GetSeedsFromLine(line);
                    }
                    else if (line.StartsWith("seed-"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.SeedToSoilMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("soil"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.SoilToFertilizerMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("fert"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.FertilizerToWaterMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("wate"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.WaterToLightMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("ligh"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.LightToTemperatureMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("temp"))
                    {
                        while ((line = streamReader.ReadLine().Trim()) != "")
                        {
                            mappingLines.Add(line);
                        }
                        almanac.TemperatureToHumidityMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                    else if (line.StartsWith("humi"))
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            mappingLines.Add(line);
                        }
                        almanac.HumidityToLocationMapping = ConvertStringToMapping(mappingLines);
                        mappingLines = new List<string>();
                    }
                }
            }

            return almanac;
        }

        public class AlmanacInfo
        {
            public AlmanacInfo()
            {
                Seeds = new List<string>();
                SeedToSoilMapping = new List<List<long>>();
                SoilToFertilizerMapping = new List<List<long>>();
                FertilizerToWaterMapping = new List<List<long>>();
                WaterToLightMapping = new List<List<long>>();
                LightToTemperatureMapping = new List<List<long>>();
                TemperatureToHumidityMapping = new List<List<long>>();
                HumidityToLocationMapping = new List<List<long>>();
            }

            public List<string> Seeds { get; set; }
            public List<List<long>> SeedToSoilMapping { get; set; }
            public List<List<long>> SoilToFertilizerMapping { get; set; }
            public List<List<long>> FertilizerToWaterMapping { get; set; }
            public List<List<long>> WaterToLightMapping { get; set; }
            public List<List<long>> LightToTemperatureMapping { get; set; }
            public List<List<long>> TemperatureToHumidityMapping { get; set; }
            public List<List<long>> HumidityToLocationMapping { get; set; }
        }

        public static List<List<long>> ConvertStringToMapping(List<string> mappingLines)
        {
            List<List<long>> mapping = new List<List<long>>();

            foreach (var line in mappingLines)
            {
                List<long> mappingLine = new List<long>();

                string[] mappingInfo = line.Split(" ");
                mappingLine.Add(long.Parse(mappingInfo[0]));
                mappingLine.Add(long.Parse(mappingInfo[1]));
                mappingLine.Add(long.Parse(mappingInfo[2]));

                mapping.Add(mappingLine);
            }

            return mapping;

        }

        public static List<string> GetSeedsFromLine(string seedLine)
        {
            List<string> seeds = new List<string>();
            string[] wholeLine = seedLine.Split(':');
            seeds = wholeLine[1].Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            return seeds;

        }

    }


}
