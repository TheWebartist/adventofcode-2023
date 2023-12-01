using AdventOfCode2023.Core;
using AdventOfCode2023.Days;

Console.WriteLine("---------------------------------------------------------------------------------------");
Console.WriteLine("----------------------- Advent of Code 2023 with .NET 8 'n love -----------------------");
Console.WriteLine("---------------------------------------------------------------------------------------");

List<IPuzzleSolver> solvedPuzzles = new List<IPuzzleSolver>();

solvedPuzzles.Add(new Day01PuzzleSolver());

foreach (var solver in solvedPuzzles)
{
    Console.WriteLine($"----------");
    Console.WriteLine($"- Puzzle's results for day {solver.Day} : ");
    Console.WriteLine($"----- Part 1 - {await solver.SolveFirstPuzzlePartAsync()}");
    Console.WriteLine($"----- Part 2 - {await solver.SolveSecondPuzzlePartAsync()}");
    Console.WriteLine($"----------------------------");
    Console.WriteLine();
}