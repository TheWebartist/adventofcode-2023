namespace AdventOfCode2023.Days
{
    using AdventOfCode2023.Core;

    internal class Day05PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const char CONVERSION_LINE_VALUE_SEPARATOR = ' ';
        private const char PREFIX_SYMBOL = ':';
        private const char CATEGORY_FIELD_SEPARATOR = '-';

        #endregion Constants and fields

        #region IPuzzleSolver members

        public int Day => 5;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"What's this fucking Almanach ? Obviously, that's the lowest location : {await GetLowestLocationFromAlmanachAsync(PuzzleInputsService.INPUT_FILE_NAME)}.";

        public async Task<string> SolveSecondPuzzlePartAsync()
        {
            var solveResult = await StopwatchUtils.GetTaskResultWithExecutionTimeAsync(GetLowestLocationFromMoreComplexAlmanachAsync(PuzzleInputsService.SAMPLE_FILE_NAME));
            return $"Hey choom, my solution isn't good and too slow with full input entry so this is the solution of the example of part 2 : {solveResult.Item1} (executed in {solveResult.Item2}).";
        }

        #endregion IPuzzleSolver members

        #region Privates methods

        private async Task<long> GetLowestLocationFromAlmanachAsync(string inputFilename)
        {
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            List<long> seeds = new List<long>();
            List<AlmanachConversionGroup> almanachConversionGroups = new List<AlmanachConversionGroup>();
            AlmanachConversionGroup? currentAlmanachConversionGroup = null;
            var almanachLinesCount = lines.Length;

            foreach (string line in lines)
            {
                if (lineNumber == 0)
                {
                    var lineParts = line.Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1)
                    {
                        seeds = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(sv => long.TryParse(sv, out var iv) ? iv : 0).ToList();
                        lineNumber++;
                        continue;
                    }

                    if (seeds.Count == 0)
                    {
                        throw new ArgumentNullException(nameof(seeds));
                    }
                }

                if (string.IsNullOrEmpty(line))
                {
                    if (currentAlmanachConversionGroup != null)
                    {
                        almanachConversionGroups.Add(currentAlmanachConversionGroup);
                        currentAlmanachConversionGroup = null;
                    }
                }
                else
                {
                    var lineParts = line.Split(CONVERSION_LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1 && line.IndexOf(CATEGORY_FIELD_SEPARATOR) > 0)
                    {
                        currentAlmanachConversionGroup = new AlmanachConversionGroup(lineParts[0]);
                    }
                    else
                    {
                        var longValues = lineParts.Select(sv => long.TryParse(sv, out var iv) ? iv : -1).Where(iv => iv > -1).ToArray();
                        if (longValues.Length == 3 && currentAlmanachConversionGroup != null)
                        {
                            long destinationValue = longValues[0];
                            long sourceValue = longValues[1];
                            long conversionLength = longValues[2];

                            currentAlmanachConversionGroup.SeedsConversionLines.Add(new ConversionLine(sourceValue, destinationValue, conversionLength));
                        }
                    }
                }

                lineNumber++;
                if (lineNumber == almanachLinesCount && currentAlmanachConversionGroup != null)
                {
                    almanachConversionGroups.Add(currentAlmanachConversionGroup);
                    currentAlmanachConversionGroup = null;
                }
            }

            List<long> locationValues = new List<long>();

            foreach (var seed in seeds)
            {
                long locationValue = seed;
                foreach (var almanachConversionGroup in almanachConversionGroups)
                {
                    locationValue = almanachConversionGroup.ConvertValue(locationValue);
                }
                locationValues.Add(locationValue);
            }
            return locationValues.Min();
        }

        private async Task<long> GetLowestLocationFromMoreComplexAlmanachAsync(string inputFilename)
        {
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            List<long> seeds = new List<long>();
            List<AlmanachConversionGroup> almanachConversionGroups = new List<AlmanachConversionGroup>();
            AlmanachConversionGroup? currentAlmanachConversionGroup = null;
            var almanachLinesCount = lines.Length;

            foreach (string line in lines)
            {
                if (lineNumber == 0)
                {
                    var lineParts = line.Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1)
                    {
                        if (lineParts.Length > 1)
                        {
                            seeds = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(sv => long.TryParse(sv, out var iv) ? iv : 0).ToList();
                            lineNumber++;
                            continue;
                        }
                    }

                    if (seeds.Count == 0)
                    {
                        throw new ArgumentNullException(nameof(seeds));
                    }
                }

                if (string.IsNullOrEmpty(line))
                {
                    if (currentAlmanachConversionGroup != null)
                    {
                        almanachConversionGroups.Add(currentAlmanachConversionGroup);
                        currentAlmanachConversionGroup = null;
                    }
                }
                else
                {
                    var lineParts = line.Split(CONVERSION_LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1 && line.IndexOf(CATEGORY_FIELD_SEPARATOR) > 0)
                    {
                        currentAlmanachConversionGroup = new AlmanachConversionGroup(lineParts[0]);
                    }
                    else
                    {
                        var longValues = lineParts.Select(sv => long.TryParse(sv, out var iv) ? iv : -1).Where(iv => iv > -1).ToArray();
                        if (longValues.Length == 3 && currentAlmanachConversionGroup != null)
                        {
                            long destinationValue = longValues[0];
                            long sourceValue = longValues[1];
                            long conversionLength = longValues[2];

                            currentAlmanachConversionGroup.SeedsConversionLines.Add(new ConversionLine(sourceValue, destinationValue, conversionLength));
                        }
                    }
                }

                lineNumber++;
                if (lineNumber == almanachLinesCount && currentAlmanachConversionGroup != null)
                {
                    almanachConversionGroups.Add(currentAlmanachConversionGroup);
                    currentAlmanachConversionGroup = null;
                }
            }

            List<long> locationValues = new List<long>();

            var maxSeedSourceEntryValue = almanachConversionGroups.First().GetMaxSourceEntryValueForConversionGroup();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                long startSeed = seeds[i];
                long seedsLength = seeds[i + 1];
                long maxSeed = Math.Min(maxSeedSourceEntryValue, startSeed + seedsLength);

                var testPair = new Tuple<long, long>(startSeed, startSeed + seedsLength);
                for (long m = startSeed; m < maxSeed; m++)
                {
                    long locationValue = m;
                    foreach (var almanachConversionGroup in almanachConversionGroups)
                    {
                        locationValue = almanachConversionGroup.ConvertValue(locationValue);
                    }
                    locationValues.Add(locationValue);
                }

                //foreach (var almanachConversionGroup in almanachConversionGroups)
                //{
                //    locationValue = almanachConversionGroup.ConvertValue(locationValue, testPair);
                //}
            }

            return locationValues.Min();
        }

        private async Task<long> GetLowestLocationFromMoreComplexAlmanachInBadWayAsync(string inputFilename)
        {
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            int lineNumber = 0;
            List<long> seeds = new List<long>();
            List<AlmanachConversionGroup> almanachConversionGroups = new List<AlmanachConversionGroup>();
            AlmanachConversionGroup? currentAlmanachConversionGroup = null;
            var almanachLinesCount = lines.Length;

            foreach (string line in lines)
            {
                if (lineNumber == 0)
                {
                    var lineParts = line.Split(PREFIX_SYMBOL, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1)
                    {
                        var tempSeeds = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(sv => long.TryParse(sv, out var iv) ? iv : 0).ToList();
                        for (int i = 0; i < tempSeeds.Count; i += 2)
                        {
                            long seedsLength = tempSeeds[i + 1];
                            long startSeed = tempSeeds[i];
                            long t = 0;
                            while (t < seedsLength)
                            {
                                seeds.Add(startSeed + t);
                                t++;
                            }
                        }
                        lineNumber++;
                        continue;
                    }

                    if (seeds.Count == 0)
                    {
                        throw new ArgumentNullException(nameof(seeds));
                    }
                }

                if (string.IsNullOrEmpty(line))
                {
                    if (currentAlmanachConversionGroup != null)
                    {
                        almanachConversionGroups.Add(currentAlmanachConversionGroup);
                        currentAlmanachConversionGroup = null;
                    }
                }
                else
                {
                    var lineParts = line.Split(CONVERSION_LINE_VALUE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParts.Length > 1 && line.IndexOf(CATEGORY_FIELD_SEPARATOR) > 0)
                    {
                        currentAlmanachConversionGroup = new AlmanachConversionGroup(lineParts[0]);
                    }
                    else
                    {
                        var longValues = lineParts.Select(sv => long.TryParse(sv, out var iv) ? iv : -1).Where(iv => iv > -1).ToArray();
                        if (longValues.Length == 3 && currentAlmanachConversionGroup != null)
                        {
                            long destinationValue = longValues[0];
                            long sourceValue = longValues[1];
                            long conversionLength = longValues[2];

                            currentAlmanachConversionGroup.SeedsConversionLines.Add(new ConversionLine(sourceValue, destinationValue, conversionLength));
                        }
                    }
                }

                lineNumber++;
                if (lineNumber == almanachLinesCount && currentAlmanachConversionGroup != null)
                {
                    almanachConversionGroups.Add(currentAlmanachConversionGroup);
                    currentAlmanachConversionGroup = null;
                }
            }

            List<long> locationValues = new List<long>();

            foreach (var seed in seeds)
            {
                long locationValue = seed;
                foreach (var almanachConversionGroup in almanachConversionGroups)
                {
                    locationValue = almanachConversionGroup.ConvertValue(locationValue);
                }
                locationValues.Add(locationValue);
            }
            return locationValues.Min();
        }

        #endregion Privates methods

        #region Private classes

        private class ConversionLine
        {
            #region Constructor

            public ConversionLine(long sourceValue, long destinationValue, long conversionLength)
            {
                SourceValue = sourceValue;
                DestinationValue = destinationValue;
                ConversionLength = conversionLength;
            }

            #endregion Constructor

            #region Properties

            public long SourceValue { get; set; }

            public long DestinationValue { get; set; }

            public long ConversionLength { get; set; }

            #endregion Properties
        }

        private class AlmanachConversionGroup
        {
            #region Constructor

            public AlmanachConversionGroup(string resourceName)
            {
                ResourceName = resourceName;
            }

            #endregion Constructor

            #region Properties

            public string ResourceName { get; set; }

            public List<ConversionLine> SeedsConversionLines { get; set; } = new List<ConversionLine>();

            #endregion Properties

            #region Public methods

            public long ConvertValueInBadWay(long sourceValue)
            {
                var validConversionLine = SeedsConversionLines.Where(scl => scl.SourceValue <= sourceValue).OrderByDescending(scl => scl.SourceValue).FirstOrDefault();
                if (validConversionLine != null)
                {
                    for (long i = 0; i < validConversionLine.ConversionLength; i++)
                    {
                        var testSourceValue = validConversionLine.SourceValue + i;
                        if (testSourceValue == sourceValue)
                        {
                            return validConversionLine.DestinationValue + i;
                        }
                    }
                }
                return sourceValue;
            }

            public long ConvertValue(long sourceValue)
            {
                var validConversionLine = SeedsConversionLines.Where(scl => scl.SourceValue <= sourceValue).OrderByDescending(scl => scl.SourceValue).FirstOrDefault();
                if (validConversionLine != null && ((validConversionLine.SourceValue + validConversionLine.ConversionLength) > sourceValue))
                {
                    var delta = validConversionLine.DestinationValue - validConversionLine.SourceValue;
                    var result = sourceValue + delta;
                    return result;
                }
                return sourceValue;
            }

            public long ConvertValue(long locationValue, Tuple<long, long> testPair)
            {
                throw new NotImplementedException();
            }

            public long GetMinValueForConversionGroup()
            {
                var list = SeedsConversionLines.Select(scl => scl.DestinationValue - scl.ConversionLength).ToList();

                return 0L;
            }

            public long GetMaxSourceEntryValueForConversionGroup()
                => SeedsConversionLines.Select(scl => scl.SourceValue + scl.ConversionLength).Max();

            #endregion Public methods
        }

        #endregion Private classes
    }
}