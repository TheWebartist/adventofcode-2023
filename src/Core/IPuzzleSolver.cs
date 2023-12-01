namespace AdventOfCode2023.Core
{
    internal interface IPuzzleSolver
    {
        int Day { get; }

        Task<string> SolveFirstPuzzlePartAsync();

        Task<string> SolveSecondPuzzlePartAsync();
    }
}