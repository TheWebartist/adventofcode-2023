namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;

    internal class Day04PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char SEPARATOR_SYMBOL = '|';
        private const char PREFIX_END_SYMBOL = ':';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 4;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Winning card ? But Corpos always wins not Heywood's guys {await GetWinningCardsScoreAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"You want scratchcards choomba ? Ok get it : {await GetScratchCardsCountAsync(PuzzleInputsService.INPUT_FILE_NAME)}";

        #endregion IPuzzleSolver members

        #region Privates methods

        private async Task<int> GetWinningCardsScoreAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            foreach (string line in lines)
            {
                var cardParts = line.Split(PREFIX_END_SYMBOL);
                cardParts = cardParts.Length > 1 ? cardParts[1].Split(SEPARATOR_SYMBOL) : null;

                if (cardParts?.Length > 1)
                {
                    var winningCards = cardParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => int.TryParse(v, out var intValue) ? intValue : 0).Where(s => s > 0);
                    var playedCards = cardParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => int.TryParse(v, out var intValue) ? intValue : 0).Where(s => s > 0);

                    int tempResult = 0;
                    var intersectCards = playedCards.Intersect(winningCards);
                    foreach (var card in intersectCards)
                    {
                        tempResult = tempResult > 0 ? tempResult * 2 : 1;
                    }

                    // Console.WriteLine($"Score for {lineNumber.ToString("000")} : {tempResult}");
                    result += tempResult;
                }
                lineNumber++;
            }

            return result;
        }

        private async Task<int> GetScratchCardsCountAsync(string inputFilename)
        {
            int result = 0;
            Dictionary<int, int> scratchcardsDict = new Dictionary<int, int>();
            Dictionary<int, int> copySratchcards = new Dictionary<int, int>();
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            foreach (string line in lines)
            {
                var cardParts = line.Split(PREFIX_END_SYMBOL);
                cardParts = cardParts.Length > 1 ? cardParts[1].Split(SEPARATOR_SYMBOL) : null;

                if (cardParts?.Length > 1)
                {
                    var winningCards = cardParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => int.TryParse(v, out var intValue) ? intValue : 0).Where(s => s > 0);
                    var playedCards = cardParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => int.TryParse(v, out var intValue) ? intValue : 0).Where(s => s > 0);

                    var intersectCards = playedCards.Intersect(winningCards);
                    var wonScratchCards = playedCards.Intersect(winningCards).Count();

                    if (wonScratchCards > 0)
                    {
                        int loopValue = 1;
                        if (copySratchcards.TryGetValue(lineNumber, out var copyCardsCount))
                        {
                            loopValue += copyCardsCount;
                        }

                        for (int i = 0; i < loopValue; i++)
                        {
                            for (int s = 0; s < (wonScratchCards + 1); s++)
                            {
                                var lineKey = lineNumber + s;
                                if (!scratchcardsDict.ContainsKey(lineKey))
                                {
                                    scratchcardsDict[lineKey] = 0;
                                }
                                if (s > 0)
                                {
                                    if (!copySratchcards.ContainsKey(lineKey))
                                    {
                                        copySratchcards[lineKey] = 0;
                                    }
                                    copySratchcards[lineKey] += 1;
                                }
                                else
                                {
                                    scratchcardsDict[lineKey] += 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        // You win original card and copies of this card even if no winning numbers in it
                        scratchcardsDict[lineNumber] = 1 + (copySratchcards.TryGetValue(lineNumber, out var copyCardsCount) ? copyCardsCount : 0);
                    }
                }

                lineNumber++;
            }

            result = scratchcardsDict.Values.Take(lineNumber).Sum();

            return result;
        }

        #endregion Privates methods
    }
}