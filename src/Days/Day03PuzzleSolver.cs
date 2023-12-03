namespace AdventOfCode2023.Days
{
    using System.Drawing;
    using AdventOfCode2023.Core;

    internal class Day03PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char EXCLUDED_SYMBOL = '.';
        private const char GEAR_SYMBOL = '*';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 3;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Strange schematic engine but why not, the result is {await GetSumOfPartNumberFromEngineSchematic(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"Hey choom, you want to know the sum of gears ratios, it's {await GetSumOfGearsRatiosFromEngineSchematic(PuzzleInputsService.INPUT_FILE_NAME)}.";

        #endregion IPuzzleSolver members

        #region Privates methods

        private async Task<int> GetSumOfPartNumberFromEngineSchematic(string inputFilename)
        {
            int result = 0;
            List<Rectangle> symbolsRects = new List<Rectangle>();
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            List<NumberMapEntry> numbersMap = new List<NumberMapEntry>();
            int lineNumber = 0;
            foreach (string line in lines)
            {
                var lineChars = line.ToCharArray();
                int i = 0;
                string currentNumberWorkload = string.Empty;
                foreach (char c in lineChars)
                {
                    if (char.IsDigit(c))
                    {
                        currentNumberWorkload += c;
                        if (i < lineChars.Length - 1)
                        {
                            i++;
                            continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(currentNumberWorkload))
                    {
                        numbersMap.Add(new NumberMapEntry(currentNumberWorkload, i, lineNumber));
                        currentNumberWorkload = string.Empty;
                    }
                    if (!c.Equals(EXCLUDED_SYMBOL) && !char.IsDigit(c))
                    {
                        symbolsRects.Add(new Rectangle(i, lineNumber, 1, 1));
                    }

                    i++;
                }
                lineNumber++;
            }

            var adjacentNumbers = numbersMap.Where(nme => symbolsRects.Any(sr => sr.IntersectsWith(nme.Rectangle)));
            result = adjacentNumbers.Sum(nme => nme.Number);

            return result;
        }

        private async Task<int> GetSumOfGearsRatiosFromEngineSchematic(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            List<NumberMapEntry> gearsMap = new List<NumberMapEntry>();
            List<NumberMapEntry> numbersMap = new List<NumberMapEntry>();
            int lineNumber = 0;
            foreach (string line in lines)
            {
                var lineChars = line.ToCharArray();
                int i = 0;
                string currentNumberWorkload = string.Empty;
                foreach (char c in lineChars)
                {
                    if (char.IsDigit(c))
                    {
                        currentNumberWorkload += c;
                        if (i < lineChars.Length - 1)
                        {
                            i++;
                            continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(currentNumberWorkload))
                    {
                        numbersMap.Add(new NumberMapEntry(currentNumberWorkload, i, lineNumber));
                        currentNumberWorkload = string.Empty;
                    }
                    if (c.Equals(GEAR_SYMBOL))
                    {
                        gearsMap.Add(new NumberMapEntry(c, i, lineNumber));
                    }

                    i++;
                }
                lineNumber++;
            }

            foreach (var mapEntry in gearsMap)
            {
                var adjacentNumbers = numbersMap.Where(nme => mapEntry.Rectangle.IntersectsWith(nme.Rectangle));
                if (adjacentNumbers.Count() >= 2)
                {
                    int tempResult = 1;
                    foreach (var item in adjacentNumbers)
                    {
                        tempResult *= item.Number;
                    }
                    result += tempResult;
                }
            }

            return result;
        }

        #endregion Privates methods

        #region Privates classes

        private class NumberMapEntry
        {
            #region Constructors

            public NumberMapEntry(string numberValue, int x, int lineNumber)
            {
                var rectWidth = numberValue.Length + 1;
                Number = int.TryParse(numberValue, out var parsedValue) ? parsedValue : -1;
                Rectangle = new Rectangle(x - rectWidth, lineNumber - 1, rectWidth + 1, 3);
            }

            public NumberMapEntry(char symbol, int startX, int lineNumber)
            {
                Symbol = symbol;
                Rectangle = new Rectangle(startX, lineNumber, 1, 1);
            }

            #endregion Constructors

            #region Properties

            public int Number { get; set; }

            public char? Symbol { get; set; }

            public Rectangle Rectangle { get; set; }

            #endregion Properties
        }

        #endregion Privates classes
    }
}