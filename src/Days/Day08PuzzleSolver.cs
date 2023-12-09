namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;
    using System;

    internal class Day08PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char DIRECTION_VALUE_SEPARATOR = ',';

        private const char PREFIX_SEPARATOR = '=';

        private const char LEFT_CHAR = 'L';

        private const char RIGHT_CHAR = 'R';

        private const string TARGET_STRING = "ZZZ";

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 8;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Are you lost in Night City ? See how many steps are required to reach ZZZ {await GetRequiredStepsToReachZZZAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"Step by step choomba.";

        #endregion IPuzzleSolver members

        #region Private methods

        private async Task<ulong> GetRequiredStepsToReachZZZAsync(string inputFilename)
        {
            ulong result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            char[] directionPattern = null;
            Dictionary<string, Step> steps = new Dictionary<string, Step>();

            foreach (string line in lines)
            {
                lineNumber++;
                if (lineNumber == 1)
                {
                    directionPattern = line.ToCharArray();
                    continue;
                }

                var lineParts = line.Split(PREFIX_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                if (lineParts.Length > 1)
                {
                    var directionValues = lineParts[1].Split(DIRECTION_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(dv => dv.Replace("(", string.Empty).Replace(")", string.Empty).Trim()).ToArray();

                    if (directionValues.Length == 2)
                    {
                        steps.Add(lineParts[0].Trim(), new Step
                        {
                            ValueOnTheLeft = directionValues[0],
                            ValueOnTheRight = directionValues[1]
                        });
                    }
                }
            }

            bool hasFoundZZZ = false;
            string currentValue = "AAA"; // steps.First().Key;

            while (!hasFoundZZZ)
            {
                foreach (var direction in directionPattern)
                {
                    result++;
                    var step = steps[currentValue];
                    currentValue = direction == LEFT_CHAR ? step.ValueOnTheLeft : step.ValueOnTheRight;
                    if (currentValue == TARGET_STRING)
                    {
                        hasFoundZZZ = true;
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Private methods

        #region Private enums and classes

        private class Step
        {
            #region Properties

            public string ValueOnTheLeft { get; set; }

            public string ValueOnTheRight { get; set; }

            #endregion Properties
        }

        #endregion Private enums and classes
    }
}