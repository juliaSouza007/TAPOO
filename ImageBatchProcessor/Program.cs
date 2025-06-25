using System;

class Program
{
    static void Main()
    {
        PerformanceUtils.RunWithMetrics(ImageProcessorTrivial.ProcessImages, "Versão Trivial");
        PerformanceUtils.RunWithMetrics(ImageProcessorOtimizada.ProcessImages, "Versão Otimizada");
    }
}
