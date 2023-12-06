namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;
    using System;

    internal class Day06PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char LINE_VALUE_SEPARATOR = ' ';
        private const char PREFIX_SYMBOL = ':';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 6;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"Get the magic number for the races' win possibilities : {await GetTheNumberOfWaysToBeatRacesRecordAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"Lost in Night City ? You can beat the record in this one much longer race in {await GetTheNumberOfWaysToBeatTheonlyOneRaceAsync(PuzzleInputsService.INPUT_FILE_NAME)} ways.";

        #endregion IPuzzleSolver members

        #region Privates methods

        private async Task<ulong> GetTheNumberOfWaysToBeatRacesRecordAsync(string inputFilename)
        {
            ulong result = 0L;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            List<RaceRecordInfos> recordInfos = new List<RaceRecordInfos>();

            if (lines.Length >= 2)
            {
                var lineParts = lines[0].Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                ulong[] times = new ulong[0];
                if (lineParts.Length > 1)
                {
                    times = lineParts[1].Split(LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(sv => ulong.TryParse(sv, out ulong iv) ? iv : 0L).Where(lv => lv > 0L).ToArray();
                }

                lineParts = lines[1].Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                ulong[] distances = new ulong[0];
                if (lineParts.Length > 1)
                {
                    distances = lineParts[1].Split(LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Select(sv => ulong.TryParse(sv, out ulong iv) ? iv : 0L).Where(lv => lv > 0L).ToArray();
                }

                if (times.Length > 0 && distances.Length == times.Length)
                {
                    List<ulong> racesWinPossibilities = new List<ulong>();
                    for (int i = 0; i < times.Length; i++)
                    {
                        var raceTime = times[i];
                        var recordToBeat = distances[i];

                        ulong possibilities = 0L;
                        ulong timeOnHoldTheButton = 1L;
                        while (timeOnHoldTheButton < raceTime)
                        {
                            var distanceForTimeOnHold = timeOnHoldTheButton * (raceTime - timeOnHoldTheButton);
                            var isAWinPossibility = distanceForTimeOnHold > recordToBeat;
                            if (isAWinPossibility)
                            {
                                possibilities++;
                            }
                            timeOnHoldTheButton++;
                        }

                        Console.WriteLine($"- {possibilities} for race n°{i + 1} with time of {raceTime} and a distance to beat of {recordToBeat}");
                        racesWinPossibilities.Add(possibilities);
                    }

                    foreach (var winPossibility in racesWinPossibilities)
                    {
                        result = result == 0L ? winPossibility : result * winPossibility;
                    }
                }
            }

            return result;
        }

        private async Task<ulong> GetTheNumberOfWaysToBeatTheonlyOneRaceAsync(string inputFilename)
        {
            ulong possibilities = 0L;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            List<RaceRecordInfos> recordInfos = new List<RaceRecordInfos>();

            if (lines.Length >= 2)
            {
                var lineParts = lines[0].Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                ulong raceTime = 0L;
                if (lineParts.Length > 1)
                {
                    string parsedValues = string.Join("", lineParts[1].Split(LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries));
                    if (ulong.TryParse(parsedValues, out var parsedTime))
                    {
                        raceTime = parsedTime;
                    }
                }

                lineParts = lines[1].Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                ulong distanceToBeat = 0L;
                if (lineParts.Length > 1)
                {
                    string parsedValues = string.Join("", lineParts[1].Split(LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries));
                    if (ulong.TryParse(parsedValues, out var parsedDistance))
                    {
                        distanceToBeat = parsedDistance;
                    }
                }

                if (raceTime > 0L && distanceToBeat > 0L)
                {
                    possibilities = 0L;
                    ulong timeOnHoldTheButton = 1L;
                    while (timeOnHoldTheButton < raceTime)
                    {
                        var distanceForTimeOnHold = timeOnHoldTheButton * (raceTime - timeOnHoldTheButton);
                        var isAWinPossibility = distanceForTimeOnHold > distanceToBeat;
                        if (isAWinPossibility)
                        {
                            possibilities++;
                        }
                        timeOnHoldTheButton++;
                    }
                }
            }

            return possibilities;
        }

        #endregion Privates methods

        #region Private classes

        private class RaceRecordInfos
        {
            #region Constructor

            public RaceRecordInfos(long time, long distance)
            {
                Time = time;
                Distance = distance;
            }

            #endregion Constructor

            #region Properties

            public long Time { get; set; }

            public long Distance { get; set; }

            #endregion Properties
        }

        #endregion Private classes
    }
}