namespace AdventOfCode2023.Days
{
    using System.Text.RegularExpressions;
    using AdventOfCode2023.Core;

    internal class Day01PuzzleSolver : IPuzzleSolver
    {
        #region Privates methods

        private async Task<int> GetTrebuchetCalibrationAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            Regex regex = new Regex(@"[\d]+");
            foreach (string line in lines)
            {
                var numbers = regex.Matches(line).Select(l => l.ToString().ToCharArray()).ToList();
                if (numbers.Count >= 1 && int.TryParse($"{numbers.First().First()}{numbers.Last().Last()}", out var calibration))
                {
                    result += calibration;
                }
            }
            return result;
        }

        private async Task<int> GetMoreComplexTrebuchetCalibration(string inputFilename)
        {
            int result = 0;
            var dict = new Dictionary<string, int>()
            {
                { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 }
            };
            var stringNumbersRegex = new Regex($"({string.Join('|', dict.Keys.Select(Regex.Escape))})");
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            Regex regex = new Regex(@"[\d]+");
            foreach (string line in lines)
            {
                var lineChars = line.ToCharArray();
                var numbers = new List<int>();
                List<char> workload = new List<char>();
                foreach (char ch in lineChars)
                {
                    if (int.TryParse($"{ch}", out var number))
                    {
                        numbers.Add(number);
                        workload.Clear();
                    }
                    else
                    {
                        workload.Add(ch);
                        var stringWorkload = string.Join("", workload);
                        if (stringWorkload.Length >= 3)
                        {
                            var numberInLetterMatch = stringNumbersRegex.Match(stringWorkload);
                            if (numberInLetterMatch.Success)
                            {
                                if (dict.TryGetValue(numberInLetterMatch.Value, out var dictValue))
                                {
                                    numbers.Add(dictValue);
                                    workload.Clear();
                                    workload.Add(ch); // Keep last number letter
                                }
                            }
                        }
                    }
                }
                if (numbers.Count >= 1 && int.TryParse($"{numbers.First()}{numbers.Last()}", out var calibration))
                {
                    result += calibration;
                }
            }
            return result;
        }

        #endregion Privates methods

        #region IPuzzleSolver members

        public int Day => 1;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Calibration {await GetTrebuchetCalibrationAsync(PuzzleInputsService.INPUT_FILE_NAME)}";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"Calibration a little bit more complicated : {await GetMoreComplexTrebuchetCalibration(PuzzleInputsService.INPUT_FILE_NAME)}";

        #endregion IPuzzleSolver members
    }
}