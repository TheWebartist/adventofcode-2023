namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;
    using System;

    internal class Day07PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        /// <summary>
        /// In Camel Cards, you get a list of hands, and your goal is to order them based on the strength of each hand.
        /// A hand consists of five cards labeled one of A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2.
        /// The relative strength of each card follows this order, where A is the highest and 2 is the lowest.
        /// </summary>
        private static readonly List<char> CAMEL_CARDS_TYPES = new List<char> { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

        private const char LINE_VALUE_SEPARATOR = ' ';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 7;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"This camel cards game is like Poker, no ? The total winnings are : {await GetCamelCardsGameTotalWinningsAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"Be patient choom.";

        #endregion IPuzzleSolver members

        #region Private methods

        private async Task<int> GetCamelCardsGameTotalWinningsAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            List<HandPlayEntry> handsEntries = new List<HandPlayEntry>();
            int lineNumber = 0;

            foreach (string line in lines)
            {
                var lineParts = line.Split(LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                if (lineParts.Length > 1 && int.TryParse(lineParts[1], out var bid))
                {
                    var hand = lineParts[0];
                    var handType = GetHandTypeForHand(hand);
                    handsEntries.Add(new HandPlayEntry
                    {
                        Bid = bid,
                        Hand = hand,
                        HandType = handType
                    });
                }

                lineNumber++;
            }

            handsEntries = handsEntries.OrderByDescending(x => x.Rank).ToList();

            int rank = handsEntries.Count;
            foreach (var handsEntry in handsEntries)
            {
                //Console.WriteLine($"hand {handsEntry.Hand} is a hand of type {handsEntry.HandType} and  has rank n°{rank} (of {handsEntry.RankStr})");
                result += handsEntry.Bid * rank;
                rank--;
            }

            return result;
        }

        private HandType GetHandTypeForHand(string handRepresentation)
        {
            var cards = handRepresentation.ToCharArray().GroupBy(x => x);
            var groupsCount = cards.Count();
            switch (groupsCount)
            {
                case 1:
                    return HandType.FiveOfAKind;

                case 2:
                    return cards.Any(cg => cg.Count() == 2) ? HandType.FullHouse : HandType.FourOfAKind;

                case 3:
                    return cards.Any(cg => cg.Count() == 3) ? HandType.ThreeOfAKind : HandType.TwoPairs;

                case 4:
                    return HandType.OnePair;

                case 5:
                default:
                    return HandType.HighCard;
            }
        }

        #endregion Private methods

        #region Private enums and classes

        private enum HandType
        {
            HighCard = 1,
            OnePair = 10,
            TwoPairs = 100,
            ThreeOfAKind = 1000,
            FullHouse = 10000,
            FourOfAKind = 100000,
            FiveOfAKind = 1000000
        }

        private class HandPlayEntry
        {
            #region Properties

            public int Bid { get; set; }

            public string Hand { get; set; }

            public HandType HandType { get; set; }

            public ulong Rank
            {
                get
                {
                    if (!ulong.TryParse(RankStr, out var rank))
                    {
                        rank = 0;
                    }
                    return rank;
                }
            }

            public string RankStr
                => $"{(int)HandType}{HandStrengh}";

            public string HandStrengh
            {
                get
                {
                    var handCards = Hand.ToCharArray();
                    var rankSeries = handCards.Select(c => CAMEL_CARDS_TYPES.IndexOf(c));
                    return string.Join("", rankSeries.Select(v => v.ToString("00")));
                }
            }

            #endregion Properties
        }

        #endregion Private enums and classes
    }
}