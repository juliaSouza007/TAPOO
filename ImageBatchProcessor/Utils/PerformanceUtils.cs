using System;
using System.Diagnostics;

public static class PerformanceUtils
{
    public static void RunWithMetrics(Action action, string label)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var sw = Stopwatch.StartNew();
        long memBefore = GC.GetTotalMemory(true);

        int gc0Before = GC.CollectionCount(0);
        int gc1Before = GC.CollectionCount(1);
        int gc2Before = GC.CollectionCount(2);

        action();

        sw.Stop();
        long memAfter = GC.GetTotalMemory(true);

        int gc0After = GC.CollectionCount(0);
        int gc1After = GC.CollectionCount(1);
        int gc2After = GC.CollectionCount(2);

        Console.WriteLine($"---- {label} ----");
        Console.WriteLine($"Memória inicial: {memBefore / 1024 / 1024:F2} MB");
        Console.WriteLine($"Memória final: {memAfter / 1024 / 1024:F2} MB");
        Console.WriteLine($"Diferença de memória: {(memAfter - memBefore) / 1024 / 1024:F2} MB");
        Console.WriteLine($"Tempo: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"GC Gen0: {gc0After - gc0Before}");
        Console.WriteLine($"GC Gen1: {gc1After - gc1Before}");
        Console.WriteLine($"GC Gen2: {gc2After - gc2Before}");
        Console.WriteLine();
    }
}
