using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Day7
    {
        private Dictionary<char, char> _cardValues = new Dictionary<char, char>() {
            {'2', 'a'}, {'3', 'b'},{'4', 'c'}, {'5', 'd'}, { '6','e'}, {'7','f' },{'8','g' },{'9','h' },{'T','i' }, {'J','j' }, {'Q', 'k' }, {'K','l' },{ 'A', 'm'}
        };
        private Dictionary<char, char> _cardValuesWithJWild = new Dictionary<char, char>() {
           {'J','a' }, {'2', 'b'}, {'3', 'c'},{'4', 'd'}, {'5', 'e'}, { '6','f'}, {'7','g' },{'8','h' },{'9','i' },{'T','j' }, {'Q', 'k' }, {'K','l' },{ 'A', 'm'}
        };

        public Dictionary<char, char> CardValues { get { return _cardValues; } }

        public Dictionary<char, char> CardValuesWithJWild { get { return _cardValuesWithJWild; } }

        public int RunSolution1()
        {
            int totalWinnings = 0;
            List<GameRow> game = ParseTextFileToGetGameInfo();
            int handStrength = 0;

            string rank;
            for (int i = 0; i < game.Count(); i++)
            {
                handStrength = CalculateHandStrength(game[i],out rank);
                game[i].handStrength = handStrength;
                game[i].rank = rank;
            }

            var orderedList = game.OrderBy(g => g.handStrength).ThenBy(g=> g.rank).ToList();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                //Console.WriteLine(orderedList[i].cards + " " + orderedList[i].rank + " " + orderedList[i].bidAmount + " " + orderedList[i].handStrength);
                totalWinnings += int.Parse(orderedList[i].bidAmount) * (i + 1); 
            }

            return totalWinnings;
        }

        public int RunSolution2()
        {
            int totalWinnings = 0;
            List<GameRow> game = ParseTextFileToGetGameInfo();
            int handStrength = 0;

            string rank;
            for (int i = 0; i < game.Count(); i++)
            {
                handStrength = CalculateHandStrengthWithJWild(game[i], out rank);
                game[i].handStrength = handStrength;
                game[i].rank = rank;
            }

            var orderedList = game.OrderBy(g => g.handStrength).ThenBy(g => g.rank).ToList();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                //Console.WriteLine(orderedList[i].cards + " " + orderedList[i].rank + " " + orderedList[i].bidAmount + " " + orderedList[i].handStrength);
                totalWinnings += int.Parse(orderedList[i].bidAmount) * (i + 1);
            }

            return totalWinnings;
        }

        public int CalculateHandStrengthWithJWild(GameRow game, out string rank)
        {
            int handStrength = 0;
            rank = "";
            Dictionary<char, int> cardAmounts = new Dictionary<char, int>();
            //Dictionary<char, int> cardAmountsWithJs = new Dictionary<char, int>();
            int foundJs = 0;
            for (int i = 0; i < game.cards.Length; i++)
            {
                if (game.cards[i] == 'J')
                {
                    foundJs++;
                    //no additions
                }
                else if (cardAmounts.ContainsKey(game.cards[i]))
                {
                    cardAmounts[game.cards[i]] += 1;
                }
                else
                {
                    cardAmounts.Add(game.cards[i], 1);
                }
                rank += CardValuesWithJWild[game.cards[i]];
            }
            if (cardAmounts.Count() >= 1)
            {
                var keyOfMaxValue =
                    cardAmounts.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                cardAmounts[keyOfMaxValue] += foundJs;
            }
            else
            {
                cardAmounts.Add('J', foundJs);// found 5 Js
            }
            

            bool hasThreeOfAKind = false;
            bool hasTwoOfAKind = false;


            if (cardAmounts.Count() == 1) //five of a kind
            {
                handStrength = 7;
            }
            else if (cardAmounts.Count() == 2 || cardAmounts.Count() == 3)
            {
                foreach (var cardAmount in cardAmounts)
                {
                    if (cardAmount.Value == 4)//four of a kind
                    {
                        handStrength = 6;
                    }
                    if (cardAmount.Value == 3)
                    {
                        hasThreeOfAKind = true;
                    }
                    if (cardAmount.Value == 2)
                    {
                        hasTwoOfAKind = true;
                    }
                }
                if (hasThreeOfAKind && hasTwoOfAKind)// full house
                {
                    handStrength = 5;
                }
                if (hasThreeOfAKind && !hasTwoOfAKind)// three of a kind
                {
                    handStrength = 4;
                }
                if (!hasThreeOfAKind && hasTwoOfAKind && cardAmounts.Count() == 3)// 2 pairs
                {
                    handStrength = 3;
                }
            }
            else if (cardAmounts.Count() == 4)//1 pair
            {
                handStrength = 2;
            }
            else if (cardAmounts.Count() == 5)//high card
            {
                handStrength = 1;
            }


            return handStrength;
        }


        public int CalculateHandStrength(GameRow game, out string rank)
        {
            int handStrength = 0;
            rank = "";
            Dictionary<char, int> cardAmounts = new Dictionary<char, int>();
            
            for (int i = 0; i < game.cards.Length; i++)
            {
                if (cardAmounts.ContainsKey(game.cards[i]))
                {
                    cardAmounts[game.cards[i]] += 1;
                }
                else
                {
                    cardAmounts.Add(game.cards[i], 1);
                }
                rank += CardValues[game.cards[i]];
            }

            bool hasThreeOfAKind = false;
            bool hasTwoOfAKind = false;

            if (cardAmounts.Count() == 1) //five of a kind
            {
                handStrength = 7;
            }
            else if (cardAmounts.Count() == 2 || cardAmounts.Count() == 3)
            {
                foreach (var cardAmount in cardAmounts)
                {
                    if (cardAmount.Value == 4)//four of a kind
                    {
                        handStrength = 6;
                    }
                    if (cardAmount.Value == 3)
                    {
                        hasThreeOfAKind = true;
                    }
                    if (cardAmount.Value == 2)
                    {
                        hasTwoOfAKind = true;
                    }
                }
                if (hasThreeOfAKind && hasTwoOfAKind)// full house
                {
                    handStrength = 5;
                }
                if (hasThreeOfAKind && !hasTwoOfAKind)// three of a kind
                {
                    handStrength = 4;
                }
                if(!hasThreeOfAKind && hasTwoOfAKind && cardAmounts.Count() == 3)// 2 pairs
                {
                    handStrength = 3;
                }
            }
            else if(cardAmounts.Count() == 4)//1 pair
            {
                handStrength = 2;
            }
            else if (cardAmounts.Count() == 5)//high card
            {
                handStrength = 1;
            }


            return handStrength;
        }


        public List<GameRow> ParseTextFileToGetGameInfo()
        {
            const Int32 BufferSize = 128;
            List<GameRow> game = new List<GameRow>();

            GameRow gRow = new GameRow();
            using (var fileStream = File.OpenRead("C:/Git/AOC/AOC/2023/ConsoleApp1/ConsoleApp1/Day7/TextFile2.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    gRow = ExtractGameInfo(line);
                    game.Add(gRow);
                }
            }

            return game;
        }

        public class GameRow
        {
            public string cards { get; set; }
            public string bidAmount { get; set; }
            public long handStrength { get; set; }
            public string rank { get; set; }

        }

        public GameRow ExtractGameInfo(string gameLine)
        {
            GameRow gameInfo = new GameRow();

            // string[] wholeGameLine = gameLine.Split(' ');
            var info = gameLine.Split(' ').Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            gameInfo.cards = info[0];
            gameInfo.bidAmount = info[1];

            return gameInfo;
        }
    }
}
