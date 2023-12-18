using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Day5FastSolution
    {
        public long RunSolution2()
        {
            long lowestLocationNumber = 0;
            long currentLowest;
            AlmanacInfo almanac = ParseTextFileToGetAlmanacInfo();
            long seedStart;
            long seedEnd;
            for (int i = 0; i < almanac.Seeds.Count(); i = i + 2)
            {
                seedStart = long.Parse(almanac.Seeds[i]);
                seedEnd = long.Parse(almanac.Seeds[i + 1]) + seedStart;
                currentLowest = GetLowestLocationFromSeedRange(ref almanac, seedStart, seedEnd, 1);

                if (lowestLocationNumber == 0 || currentLowest < lowestLocationNumber)
                {
                    lowestLocationNumber = currentLowest;
                }
            }
           

            return lowestLocationNumber;
        }

        public long GetLowestLocationFromSeedRange(ref AlmanacInfo almanac, long seedStart, long seedEnd, int step)
        {
            long lowestLocation = 0;
            long currentMappedNumber = lowestLocation;
            List<List<long>> currentMapping = new List<List<long>>();

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

            Ranges newSeedMappings = new Ranges();
            List<List<long>> outerSeedMappings = new List<List<long>>();

            newSeedMappings = GetAllMappingRanges(ref currentMapping, newSeedMappings, seedStart, seedEnd);
            foreach (var newSeedRange in newSeedMappings.MappedRanges)
            {
                if (step > 7)
                {
                    currentMappedNumber = newSeedRange.First();//the first number in sequence will be the lowest number in the range
                }
                else
                {
                    currentMappedNumber = GetLowestLocationFromSeedRange(ref almanac, newSeedRange.First(), newSeedRange.Last(), step);
                }

                if (lowestLocation == 0 || currentMappedNumber < lowestLocation)
                {
                    lowestLocation = currentMappedNumber;
                }

            }

            return lowestLocation;
        }

        public Ranges GetAllMappingRanges(ref List<List<long>> mappingRows, Ranges currentMappings, long seedStart, long seedEnd)
        {
            Ranges newSeedMappings = new Ranges();
            Ranges newRange = new Ranges();

            List<List<long>> currentUnMappedRanges = new List<List<long>>();
            List<List<long>> previousUnMappedRanges = new List<List<long>>();

            previousUnMappedRanges.Add(new List<long>{ seedStart, seedEnd });

            for (int i = 0; i < mappingRows.Count(); i++)
            {
                for (int ui = 0; ui < previousUnMappedRanges.Count(); ui++)
                {
                    newRange = new Ranges();
                    currentUnMappedRanges = new List<List<long>>();

                    newRange = GetNewSeedMappingRanges(mappingRows[i], previousUnMappedRanges[ui].First(), previousUnMappedRanges[ui].Last());

                    if (newRange.MappedRanges.Count() > 0)
                    {
                        newSeedMappings.MappedRanges.AddRange(newRange.MappedRanges);
                    }

                    if(newRange.OuterRanges.Count() > 0)
                    {
                        currentUnMappedRanges.AddRange(newRange.OuterRanges);
                    }
                }

                previousUnMappedRanges = currentUnMappedRanges;
            }

            newSeedMappings.MappedRanges.AddRange(currentUnMappedRanges);//any outerRange after going through all mappings, becomes a mapped Range


            return newSeedMappings;
        }

        public Ranges GetNewSeedMappingRanges(List<long> mapping, long seedStart, long seedEnd)
        {
            long mappedNumber = 0;
            Ranges newMappedNumberRanges = new Ranges();

                long range = mapping[2];
                long mapStart = mapping[1];
                long mapEnd = mapStart + range - 1; // -1 to account for starting number being a part of the range
                long mapTo_StartAt = mapping[0];

                long midRangeSeedStart = seedStart;
                long endRangeSeedStart = seedStart;

                List<long> newMapRange = new List<long>();

                if (seedStart < mapStart) //start range
                {
                    newMapRange.Add(seedStart);
                    if (seedEnd < mapStart)
                    {
                        newMapRange.Add(seedEnd);
                    }
                    else
                    {
                        newMapRange.Add((mapStart - seedStart) + seedStart - 1);// end one before the next mapper begins
                        midRangeSeedStart = (mapStart - seedStart) + seedStart;
                    }

                newMappedNumberRanges.OuterRanges.Add(newMapRange);
                }

                if ((midRangeSeedStart >= mapStart) && (midRangeSeedStart <= mapEnd)) // mid range (actual new mapping)
                {
                    mappedNumber = (midRangeSeedStart - mapStart) + mapTo_StartAt;
                    newMapRange = new List<long>();
                    newMapRange.Add(mappedNumber);
                    if (seedEnd < mapEnd)
                    {
                        mappedNumber = (seedEnd - mapStart) + mapTo_StartAt;
                        newMapRange.Add(mappedNumber);
                    }
                    else
                    {
                        newMapRange.Add(mapEnd);
                        endRangeSeedStart = mapEnd + 1;
                    }
                    newMappedNumberRanges.MappedRanges.Add(newMapRange);
                }


                if (endRangeSeedStart > mapEnd)//end Range
                {
                    newMapRange = new List<long>();
                    newMapRange.Add(endRangeSeedStart);
                    newMapRange.Add(seedEnd);

                newMappedNumberRanges.OuterRanges.Add(newMapRange);
            }

            return newMappedNumberRanges;
        }


        public AlmanacInfo ParseTextFileToGetAlmanacInfo()
        {
            const Int32 BufferSize = 128;
            AlmanacInfo almanac = new AlmanacInfo();
            List<string> mappingLines = new List<string>();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day5/TextFile2.txt"))
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

        public List<List<long>> ConvertStringToMapping(List<string> mappingLines)
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

        public List<string> GetSeedsFromLine(string seedLine)
        {
            List<string> seeds = new List<string>();
            string[] wholeLine = seedLine.Split(':');
            seeds = wholeLine[1].Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            return seeds;
        }

        public class Ranges
        {
            public Ranges()
            {
                MappedRanges = new List<List<long>>();
                OuterRanges = new List<List<long>>();
            }
            public List<List<long>> MappedRanges { get; set; }
            public List<List<long>> OuterRanges { get; set; }
        }
    }


}
