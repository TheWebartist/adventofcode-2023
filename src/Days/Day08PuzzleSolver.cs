namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;
    using System;
    using System.IO;

    internal class Day08PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char DIRECTION_VALUE_SEPARATOR = ',';

        private const char PREFIX_SEPARATOR = '=';

        private const char LEFT_CHAR = 'L';

        private const char RIGHT_CHAR = 'R';

        private const string TARGET_STRING = "ZZZ";

        private const char START_CHAR = 'A';

        private const char TARGET_CHAR = 'Z';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 8;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Are you lost in Night City ? See how many steps are required to reach ZZZ {await GetRequiredStepsToReachZZZAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
             => $"Are you lost in Night City ? See how many steps are required to reach all **Z steps {await GetRequiredSimultaneousStepsToReachZZZAsync(PuzzleInputsService.SAMPLE_PART_TWO_FILE_NAME)}.";

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
            string currentValue = "AAA";

            while (!hasFoundZZZ)
            {
                foreach (var direction in directionPattern)
                {
                    result++;
                    hasFoundZZZ = HasArrivedToZZZ(ref currentValue, direction, steps);
                    if (hasFoundZZZ)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private async Task<ulong> GetRequiredSimultaneousStepsToReachZZZAsync(string inputFilename)
        {
            Console.WriteLine("Search for the smallest ways to Z, choom !");
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
            string[] currentValues = steps.Keys.Where(s => s.EndsWith(START_CHAR)).ToArray();

            while (!hasFoundZZZ)
            {
                foreach (var direction in directionPattern)
                {
                    result++;
                    var valuesKP = currentValues.Select(v => HasArrivedToPartialZStep(v, direction, steps));
                    hasFoundZZZ = valuesKP.All(kp => kp.Item2 == true);
                    currentValues = valuesKP.Select(kp => kp.Item1).ToArray();
                    if (hasFoundZZZ)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private bool HasArrivedToZZZ(ref string currentStepValue, char direction, Dictionary<string, Step> steps)
        {
            var step = steps[currentStepValue];
            currentStepValue = direction == LEFT_CHAR ? step.ValueOnTheLeft : step.ValueOnTheRight;
            return currentStepValue == TARGET_STRING;
        }

        private Tuple<string, bool> HasArrivedToPartialZStep(string currentStepValue, char direction, Dictionary<string, Step> steps)
        {
            var step = steps[currentStepValue];
            currentStepValue = direction == LEFT_CHAR ? step.ValueOnTheLeft : step.ValueOnTheRight;
            return new Tuple<string, bool>(currentStepValue, currentStepValue.EndsWith(TARGET_CHAR));
        }

        #endregion Private methods

        #region Private classes

        private class Step
        {
            #region Properties

            public string ValueOnTheLeft { get; set; }

            public string ValueOnTheRight { get; set; }

            #endregion Properties
        }

        #endregion Private classes
    }
}