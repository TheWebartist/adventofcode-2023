namespace AdventOfCode2023.Core
{
    internal static class PuzzleInputsService
    {
        #region Constants and fields

        private const string INPUTS_FOLDER_NAME = "Inputs";

        private const string INPUTS_LINE_SEPARATOR = "\r\n";

        internal const string INPUT_FILE_NAME = "input";

        internal const string SAMPLE_PART_ONE_FILE_NAME = "sample-part-1";

        internal const string SAMPLE_PART_TWO_FILE_NAME = "sample-part-2";

        #endregion Constants and fields

        #region Private methods

        private static string GetFolderPath(int day)
            => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), INPUTS_FOLDER_NAME, day.ToString("00"));

        #endregion Private methods

        #region Exposed methods

        internal static async Task<string> GetPuzzleInputFileContentAsync(int day, string inputFileName)
            => await File.ReadAllTextAsync(Path.Combine(GetFolderPath(day), inputFileName));

        internal static async Task<string[]> GetPuzzleInputLinesAsync(int day, string inputFileName)
            => (await GetPuzzleInputFileContentAsync(day, inputFileName)).Split(INPUTS_LINE_SEPARATOR);

        #endregion Exposed methods
    }
}