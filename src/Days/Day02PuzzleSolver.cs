namespace AdventOfCode2023.Days
{
    using System.Text.RegularExpressions;
    using AdventOfCode2023.Core;

    internal class Day02PuzzleSolver : IPuzzleSolver
    {
        #region Constants and fields

        private const string CUBE_COLORS_BLUE = "blue";
        private const string CUBE_COLORS_GREEN = "green";
        private const string CUBE_COLORS_RED = "red";
        private const char CUBE_IN_SET_SEPARATOR = ',';
        private const char GAME_PREFIX_SEPARATOR = ':';
        private const char GAME_SETS_SEPARATOR = ';';

        #endregion Constants and fields

        #region Privates methods

        private async Task<int> GetPossibleGamesAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            var cubeColorsRules = new Dictionary<string, int>()
            {
                { CUBE_COLORS_GREEN, 13 }, { CUBE_COLORS_BLUE, 14 }, { CUBE_COLORS_RED, 12 }
            };
            Regex regex = new Regex(@"[\d]+");
            foreach (string line in lines)
            {
                var firstCut = line.Split(GAME_PREFIX_SEPARATOR);
                if (firstCut.Length == 2 && int.TryParse(regex.Match(firstCut[0]).Value, out var gameNumber))
                {
                    // TODO: check rules and if game is possible, add gameNumber to result
                    var secondCut = firstCut[1].Split(GAME_SETS_SEPARATOR);
                    bool gameIsPossible = true;
                    // TODO: check rule if it's possible to have only one set for a game
                    if (secondCut.Length > 1)
                    {
                        foreach (var set in secondCut)
                        {
                            var playedCubes = set.Split(CUBE_IN_SET_SEPARATOR);
                            foreach (var item in playedCubes)
                            {
                                if (!IsGameSetIsPossible(cubeColorsRules, item))
                                {
                                    gameIsPossible = false;
                                    break;
                                }
                            }
                            if (!gameIsPossible)
                            {
                                break;
                            }
                        }
                    }
                    if (gameIsPossible)
                    {
                        result += gameNumber;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// This method isn't used, I've keep it for the legacy (a bug in my mind, I haven't read the whole statement
        /// </summary>
        private async Task<int> GetPossibleGamesWithMoreComplexeRulesAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            Regex regex = new Regex(@"[\d]+");
            foreach (string line in lines)
            {
                var firstCut = line.Split(GAME_PREFIX_SEPARATOR);
                if (firstCut.Length == 2 && int.TryParse(regex.Match(firstCut[0]).Value, out var gameNumber))
                {
                    var cubeColorsRules = new Dictionary<string, int>()
                    {
                        { CUBE_COLORS_GREEN, 13 }, { CUBE_COLORS_BLUE, 14 }, { CUBE_COLORS_RED, 12 }
                    };
                    // TODO: check rules and if game is possible, add gameNumber to result
                    var secondCut = firstCut[1].Split(GAME_SETS_SEPARATOR);
                    // TODO: check rule if it's possible to have only one set for a game
                    if (secondCut.Length > 1)
                    {
                        foreach (var set in secondCut)
                        {
                            var playedCubes = set.Split(CUBE_IN_SET_SEPARATOR);
                            foreach (var item in playedCubes)
                            {
                                AddOrRemoveCubeFromGame(cubeColorsRules, item);
                            }
                        }
                    }
                    if (cubeColorsRules.All(cr => cr.Value > -1))
                    {
                        result += gameNumber;
                    }
                }
            }
            return result;
        }

        private async Task<int> GetCubesPowerAsync(string inputFilename)
        {
            int result = 0;
            string[] lines = await PuzzleInputsService.GetPuzzleInputLinesAsync(Day, inputFilename);
            Regex regex = new Regex(@"[\d]+");
            foreach (string line in lines)
            {
                var firstCut = line.Split(GAME_PREFIX_SEPARATOR);
                if (firstCut.Length == 2 && int.TryParse(regex.Match(firstCut[0]).Value, out var gameNumber))
                {
                    var cubeColorsRules = new Dictionary<string, int>()
                    {
                        { CUBE_COLORS_GREEN, 0 }, { CUBE_COLORS_BLUE, 0 }, { CUBE_COLORS_RED, 0 }
                    };
                    var secondCut = firstCut[1].Split(GAME_SETS_SEPARATOR);
                    if (secondCut.Length > 1)
                    {
                        foreach (var set in secondCut)
                        {
                            var playedCubes = set.Split(CUBE_IN_SET_SEPARATOR);
                            foreach (var item in playedCubes)
                            {
                                ManageCubePowerForGame(cubeColorsRules, item);
                            }
                        }
                    }
                    int cubePowerInGame = 0;
                    foreach (var item in cubeColorsRules)
                    {
                        cubePowerInGame = cubePowerInGame == 0 ? item.Value : cubePowerInGame * item.Value;
                    }

                    result += cubePowerInGame;
                }
            }
            return result;
        }

        private void AddOrRemoveCubeFromGame(Dictionary<string, int> gamePlayableCubes, string cubeInSetInput)
        {
            foreach (var item in gamePlayableCubes)
            {
                if (cubeInSetInput.EndsWith(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var finalCut = cubeInSetInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (finalCut.Length > 1 && int.TryParse(finalCut[0], out var numberOfCubes))
                    {
                        gamePlayableCubes[item.Key] -= numberOfCubes;
                    }
                }
            }
        }

        private bool IsGameSetIsPossible(Dictionary<string, int> gamePlayableCubes, string cubeInSetInput)
        {
            foreach (var item in gamePlayableCubes)
            {
                if (cubeInSetInput.EndsWith(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var finalCut = cubeInSetInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (finalCut.Length > 1 && int.TryParse(finalCut[0], out var numberOfCubes) && numberOfCubes <= item.Value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ManageCubePowerForGame(Dictionary<string, int> gamePlayableCubes, string cubeInSetInput)
        {
            foreach (var item in gamePlayableCubes)
            {
                if (cubeInSetInput.EndsWith(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var finalCut = cubeInSetInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (finalCut.Length > 1 && int.TryParse(finalCut[0], out var numberOfCubes) && numberOfCubes > item.Value)
                    {
                        gamePlayableCubes[item.Key] = numberOfCubes;
                    }
                }
            }
        }

        #endregion Privates methods

        #region IPuzzleSolver members

        public int Day => 2;

        public async Task<string> SolveFirstPuzzlePartAsync()
            => $"What is the count of possible games for this strange game ? I'm sure that is {await GetPossibleGamesAsync(PuzzleInputsService.INPUT_FILE_NAME)}";

        public async Task<string> SolveSecondPuzzlePartAsync()
            => $"The power of the cubes is {await GetCubesPowerAsync(PuzzleInputsService.INPUT_FILE_NAME)}, it's nova !";

        #endregion IPuzzleSolver members
    }
}