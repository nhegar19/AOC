using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Day3
    {
        public static int RunSolution1()
        {
            int total;
            List<string> data = ParseTextFile();
            total = GetSumOfSchematic(data.ToArray());

            return total;
        }

        public static int RunSolution2()
        {
            int total;
            List<string> data = ParseTextFile();
            total = GetSumOfGearRatios(data.ToArray());

            return total;
        }
        public static List<string> ParseTextFile()
        {
            const Int32 BufferSize = 128;
            List<string> data = new List<string>();

            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day3/TextFile2.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    data.Add(line);
                    Console.WriteLine(line);
                }
            }
            return data;
        }

        //Run this for Solution 2
        public static int GetSumOfGearRatios(string[] data)
        {
            int sum = 0;
            char[] specialChars = { '*', '#', '+', '$' };

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (IsSpecialChar(data[i][j]))
                    {
                        sum += GetGearRatios(data, i, j);
                    }
                }
            }

            return sum;
        }

        public static int GetGearRatios(string[] data, int i1, int i2)
        {
            int sum = 0;
            int currentNum = 0;
            string currentChar = "";
            string currentIndex = "";
            List<string> currentRow;
            List<List<string>> duplicates = new List<List<string>>();
            bool isDuplicate = false;

            List<int> gears = new List<int>();
            int gearRatio = 0;
            for (int i = i1 - 1; i <= i1 + 1; i++)
            {
                currentNum = 0;
                currentRow = new List<string>();
                //duplicates = new List<List<string>>();
                if (i >= 0 && i < data.Length)
                {
                    for (int j = i2 - 1; j <= i2 + 1; j++)
                    {
                        currentNum = 0;
                        if (j >= 0 && j < data[i].Length)
                        {
                            currentChar = data[i][j].ToString();
                            if (int.TryParse(currentChar, out currentNum))
                            {
                                isDuplicate = false;
                                currentIndex = i + "" + j;
                                foreach (var row in duplicates)// check for duplicates
                                {
                                    if (row.Contains(currentIndex))
                                    {
                                        isDuplicate = true;
                                    }
                                }

                                if (!isDuplicate)
                                {
                                    currentNum = CreateNumFromAdjacents(data[i].ToCharArray(), j, i, out currentRow);
                                    gears.Add(currentNum);
                                    sum += currentNum;
                                }

                                // update duplicate Checker
                                if (duplicates.Count() > 3) //only need 
                                {
                                    duplicates.RemoveAt(0);
                                }
                                duplicates.Add(currentRow);
                            }
                        }

                    }
                }
            }

            if (gears.Count() > 1)
            {
                //var product = vals.Aggregate(1, (acc, val) => acc * val);
                gearRatio = gears.Aggregate(1, (acc, val) => acc * val);
            }

            return gearRatio;
        }

        //Run this for Solution 1
        public static int GetSumOfSchematic(string[] data)
        {
            int sum = 0;
            char[] specialChars = { '*', '#', '+', '$' };

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (IsSpecialChar(data[i][j]))
                    {
                        sum += GetSumOfSurroundingNums(data, i, j);
                    }
                }
            }

            return sum;
        }

        public static bool IsSpecialChar(char c)
        {
            bool isSpecialChar = true;
            int num = 0;
            if (c == '.' || int.TryParse(c.ToString(), out num))
            {
                isSpecialChar = false;
            }
            return isSpecialChar;
        }

        public static int GetSumOfSurroundingNums(string[] data, int i1, int i2)
        {
            int sum = 0;
            int currentNum = 0;
            string currentChar = "";
            string currentIndex = "";
            List<string> currentRow;
            List<List<string>> duplicates = new List<List<string>>();
            bool isDuplicate = false;
            for (int i = i1 - 1; i <= i1 + 1; i++)
            {
                currentNum = 0;
                currentRow = new List<string>();
                if (i >= 0 && i < data.Length)
                {
                    for (int j = i2 - 1; j <= i2 + 1; j++)
                    {
                        currentNum = 0;
                        if (j >= 0 && j < data[i].Length)
                        {
                            currentChar = data[i][j].ToString();
                            if (int.TryParse(currentChar, out currentNum))
                            {
                                isDuplicate = false;
                                currentIndex = i + "" + j;
                                foreach (var row in duplicates)// check for duplicates
                                {
                                    if (row.Contains(currentIndex))
                                    {
                                        isDuplicate = true;
                                    }
                                }
                                if (!isDuplicate)
                                {
                                    currentNum = CreateNumFromAdjacents(data[i].ToCharArray(), j, i, out currentRow);

                                    sum += currentNum;
                                }

                                // update duplicate Checker
                                if (duplicates.Count() > 3) //only need 
                                {
                                    duplicates.RemoveAt(0);
                                }
                                duplicates.Add(currentRow);
                            }
                        }

                    }
                }
            }

            return sum;
        }

        public static int CreateNumFromAdjacents(char[] charArray, int startingIndex, int rowIndex, out List<string> foundIndexes)
        {
            int num = 0;
            int finalNum = 0;
            string currentIndex = "";
            List<char> numArray = new List<char>();
            foundIndexes = new List<string>();
            for (int i = startingIndex; i >= 0; i--)//leftSide
            {
                if (int.TryParse(charArray[i].ToString(), out num))
                {
                    currentIndex = rowIndex + "" + i;
                    foundIndexes.Add(currentIndex);
                    numArray.Insert(0, charArray[i]);

                }
                else
                {
                    break;//stop when get to a non number
                }
            }

            for (int i = startingIndex + 1; i < charArray.Length; i++) //mid to rightSide
            {
                if (int.TryParse(charArray[i].ToString(), out num))
                {
                    currentIndex = rowIndex + "" + i;
                    foundIndexes.Add(currentIndex);
                    numArray.Add(charArray[i]);
                }
                else
                {
                    break;//stop when get to a non number
                }
            }

            string n = new string(numArray.ToArray());
            Console.WriteLine(n);
            int.TryParse(n, out finalNum);

            return finalNum;
        }


    }
}
