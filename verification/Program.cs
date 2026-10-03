extern alias baseline;
using System;
using System.Collections.Generic;
using System.IO;
using Old = baseline::SPSO_2007;
using Current = SPSO_2007;

class Runner
{
    static void Main(string[] args)
    {
        Study.PublicApi(typeof(Old.Problem).Assembly, typeof(Current.Problem).Assembly);
        var random = new Random(1937);
        int checks = 0;
        var measurements = new List<object>();
        foreach (int function in new[] { 100, 102, 103, 104, 105, 106 })
        {
            var a = new Old.Position(114) { size = 30 };
            var b = new Current.Position(114) { size = 30 };
            var input = new double[30];
            for (int sample = 0; sample < 1000; sample++)
            {
                for (int d = 0; d < 30; d++) input[d] = sample == 0 ? 0 : sample == 1 ? 100 : sample == 2 ? -100 : (random.NextDouble() * 200 - 100);
                Array.Copy(input, a.x, 30); Array.Copy(input, b.x, 30);
                Study.Equal(Old.Problem.perf(a, function, 0), Current.Problem.perf(b, function, 0)); checks++;
                for (int d = 0; d < 30; d++) { Study.Equal(a.x[d], b.x[d]); checks++; }
            }
            measurements.Add(Study.Measure($"Function {function}: constant-table reuse and evaluator extraction",
                () => Old.Problem.perf(a, function, 0), () => Current.Problem.perf(b, function, 0)));
        }
        foreach (int function in new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 99 })
        {
            var oldProblem = Old.Problem.problemDef(function);
            var problem = Current.Problem.problemDef(function);
            if (oldProblem.SS.D != problem.SS.D) throw new Exception("Problem dimensions changed");
            Study.Equal(oldProblem.objective, problem.objective); checks++;
            var a = new Old.Position(114) { size = oldProblem.SS.D };
            var b = new Current.Position(114) { size = problem.SS.D };
            for (int sample = 0; sample < 100; sample++)
            {
                for (int d = 0; d < a.size; d++)
                {
                    Study.Equal(oldProblem.SS.min[d], problem.SS.min[d]); checks++;
                    Study.Equal(oldProblem.SS.max[d], problem.SS.max[d]); checks++;
                    double value = oldProblem.SS.min[d] + (oldProblem.SS.max[d] - oldProblem.SS.min[d]) * random.NextDouble();
                    a.x[d] = value; b.x[d] = value;
                }
                Study.Equal(Old.Problem.perf(a, function, oldProblem.objective), Current.Problem.perf(b, function, problem.objective)); checks++;
                for (int d = 0; d < a.size; d++) { Study.Equal(a.x[d], b.x[d]); checks++; }
            }
        }
        Study.Save(args[0], checks, measurements, "Pinned Git baseline; constant-table reuse and responsibility extraction; nine alternating trials after warmup. Compare fitness and input mutation exactly. Existing shifted-sphere formula behavior is preserved.");
    }
}
static class Study
{
    public static void PublicApi(System.Reflection.Assembly baseline, System.Reflection.Assembly current)
    {
        string[] Describe(System.Reflection.Assembly assembly)
        {
            var members = new List<string>();
            foreach (var type in assembly.GetExportedTypes())
            {
                members.Add(type.FullName);
                foreach (var member in type.GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly))
                    members.Add(type.FullName + ":" + member.MemberType + ":" + member);
            }
            members.Sort(StringComparer.Ordinal);
            return members.ToArray();
        }
        var expected = Describe(baseline); var actual = Describe(current);
        if (expected.Length != actual.Length) throw new Exception("Public API member count changed");
        for (int i = 0; i < expected.Length; i++)
            if (expected[i] != actual[i]) throw new Exception($"Public API changed: {expected[i]} versus {actual[i]}");
        Console.WriteLine($"PASS: {expected.Length} public type/member signatures unchanged");
    }

    public static void Equal(double expected, double actual)
    {
        if (BitConverter.DoubleToInt64Bits(expected) != BitConverter.DoubleToInt64Bits(actual))
            throw new Exception($"Numerical mismatch: {expected:R} versus {actual:R}");
    }
    public static object Measure(string name, Func<double> baseline, Func<double> candidate, int iterations = 20000)
    {
        for (int i = 0; i < 10000; i++) { baseline(); candidate(); }
        var oldTimes = new double[9]; var newTimes = new double[9];
        var oldBytes = new long[9]; var newBytes = new long[9];
        double sink = 0;
        void Batch(Func<double> call, double[] times, long[] bytes, int trial)
        {
            long before = GC.GetAllocatedBytesForCurrentThread();
            long start = System.Diagnostics.Stopwatch.GetTimestamp();
            for (int i = 0; i < iterations; i++) sink += call();
            times[trial] = System.Diagnostics.Stopwatch.GetElapsedTime(start).TotalMilliseconds;
            bytes[trial] = GC.GetAllocatedBytesForCurrentThread() - before;
        }
        for (int trial = 0; trial < 9; trial++)
        {
            if (trial % 2 == 0) { Batch(baseline, oldTimes, oldBytes, trial); Batch(candidate, newTimes, newBytes, trial); }
            else { Batch(candidate, newTimes, newBytes, trial); Batch(baseline, oldTimes, oldBytes, trial); }
        }
        GC.KeepAlive(sink);
        double[] sortedOld = (double[])oldTimes.Clone(), sortedNew = (double[])newTimes.Clone();
        Array.Sort(sortedOld); Array.Sort(sortedNew);
        return new { name, iterations, baselineMilliseconds = oldTimes, candidateMilliseconds = newTimes,
            baselineMedianMilliseconds = sortedOld[4], candidateMedianMilliseconds = sortedNew[4],
            speedup = sortedOld[4] / sortedNew[4], baselineBytesPerCall = oldBytes[4] / (double)iterations,
            candidateBytesPerCall = newBytes[4] / (double)iterations };
    }
    public static void Save(string path, int checks, List<object> measurements, string notes)
    {
        File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(new {
            runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            architecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
            numericalChecks = checks, maximumAbsoluteError = 0, maximumRelativeError = 0,
            measurements, notes
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }) + "\n");
        Console.WriteLine($"PASS: {checks} exact numerical checks; results: {path}");
    }
}
