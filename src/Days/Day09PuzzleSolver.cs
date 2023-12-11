namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;
    using System;

    internal class Day09PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char SENSOR_LINE_VALUE_SEPARATOR = ' ';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 9;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Can you predict the end of your story in Night City ? No but you can predict the next values of each sensor's line : {await PredictNextValuesOfEachSensorLineAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
             => $"Do you remember your background at the start of your story in Night City ? No but you can predict the previous values of each sensor's line : {await PredictPreviousValuesOfEachSensorLineAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        #endregion IPuzzleSolver members

        #region Private methods

        private async Task<long> PredictNextValuesOfEachSensorLineAsync(string inputFilename)
        {
            long result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;

                List<long> lineSeq = line.Split(SENSOR_LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(lv => long.Parse(lv)).ToList();
                List<long> lastValues = [lineSeq.Last()];

                while (lineSeq.Count == 0 || !lineSeq.All(s => s == 0L))
                {
                    var tempSeq = new List<long>();
                    for (int i = 0; i < lineSeq.Count; i++)
                    {
                        if (i < lineSeq.Count - 1)
                        {
                            var nextColumn = lineSeq[i + 1];
                            tempSeq.Add(nextColumn - lineSeq[i]);
                        }
                    }

                    lastValues.Add(tempSeq.Last());
                    lineSeq = tempSeq;
                }

                long nextValue = lastValues.Sum();

                result += nextValue;
            }

            return result;
        }

        private async Task<long> PredictPreviousValuesOfEachSensorLineAsync(string inputFilename)
        {
            long result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;

                List<long> lineSeq = line.Split(SENSOR_LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(lv => long.Parse(lv)).ToList();

                List<long> firstValues = new List<long>();
                List<long> lastValues = [lineSeq.Last()];

                while (lineSeq.Count == 0 || !lineSeq.All(s => s == 0L))
                {
                    var tempSeq = new List<long>();
                    for (int i = 0; i < lineSeq.Count; i++)
                    {
                        if (i < lineSeq.Count - 1)
                        {
                            var nextColumn = lineSeq[i + 1];
                            tempSeq.Add(nextColumn - lineSeq[i]);
                        }
                    }

                    firstValues.Add(tempSeq.First());
                    lastValues.Add(tempSeq.Last());
                    lineSeq = tempSeq;
                }

                long previousValue = 0L;
                firstValues.Reverse();
                for (int i = 0; i < firstValues.Count; i++)
                {
                    previousValue += firstValues[i] - previousValue;
                }

                // TODO : uncomment when i will come back to the part 2 (sample are validated but not the full input)
                //Console.WriteLine($"Previous value for line n°{lineNumber.ToString("000")} : {previousValue}");

                result += previousValue;
            }

            return result;
        }

        #endregion Private methods
    }
}