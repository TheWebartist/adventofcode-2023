namespace AdventOfCode2023.Core
{
    using System.Diagnostics;

    internal class StopwatchUtils
    {
        internal static async Task<Tuple<T, TimeSpan>> GetTaskResultWithExecutionTimeAsync<T>(Task<T> task)
        {
            Stopwatch sw = Stopwatch.StartNew();
            T result = await task;
            sw.Stop();
            return new Tuple<T, TimeSpan>(result, sw.Elapsed);
        }
    }
}