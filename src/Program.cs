using AdventOfCode2023.Core;
using AdventOfCode2023.Days;

Console.WriteLine("---------------------------------------------------------------------------------------");
Console.WriteLine("----------------------- Advent of Code 2023 with .NET 8 'n love -----------------------");
Console.WriteLine("---------------------------------------------------------------------------------------");

List<IPuzzleSolver> solvedPuzzles = [
    new Day01PuzzleSolver(),
    new Day02PuzzleSolver(),
    new Day03PuzzleSolver(),
    new Day04PuzzleSolver(),
    new Day05PuzzleSolver(),
    new Day06PuzzleSolver(),
    new Day07PuzzleSolver()
];

foreach (var solver in solvedPuzzles)
{
    Console.WriteLine($"----------");
    Console.WriteLine($"- Puzzle's results for day {solver.Day} : ");
    Console.WriteLine($"----- Part 1 - {await solver.SolveFirstPuzzlePartAsync()}");
    Console.WriteLine($"----- Part 2 - {await solver.SolveSecondPuzzlePartAsync()}");
    Console.WriteLine($"----------------------------");
    Console.WriteLine();
}