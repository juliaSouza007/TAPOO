using System;
using System.Buffers;
using System.Diagnostics;

public class ImageProcessorOtimizada
{
    private const int IMAGE_WIDTH = 800;
    private const int IMAGE_HEIGHT = 600;
    private const int TOTAL_IMAGES = 500;
    private const int IMAGE_SIZE = IMAGE_WIDTH * IMAGE_HEIGHT;

    public static void ProcessImages()
    {
        Console.WriteLine("Iniciando processamento de imagens (versão otimizada com ArrayPool)...");

        var stopwatch = Stopwatch.StartNew();
        int processedCount = 0;

        // Pool compartilhado para alugar e devolver arrays
        var pool = ArrayPool<PixelRGB>.Shared;

        for (int imageIndex = 0; imageIndex < TOTAL_IMAGES; imageIndex++)
        {
            PixelRGB[] original = null;
            PixelRGB[] blurred = null;

            try
            {
                // Alugar arrays 1D que representam imagem 2D
                original = pool.Rent(IMAGE_SIZE);
                blurred = pool.Rent(IMAGE_SIZE);

                GenerateSyntheticImage(imageIndex, original);

                ApplyBlurFilter(original, blurred);

                SaveImage(blurred, $"processed_{imageIndex}.jpg");

                processedCount++;

                if (imageIndex % 50 == 0)
                {
                    Console.WriteLine($"Processadas {imageIndex} imagens...");
                }
            }
            finally
            {
                // Garantir devolução ao pool
                if (original != null)
                    pool.Return(original, clearArray: true);
                if (blurred != null)
                    pool.Return(blurred, clearArray: true);
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"Processamento concluído!");
        Console.WriteLine($"Imagens processadas: {processedCount}");
        Console.WriteLine($"Tempo total: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Tempo médio por imagem: {stopwatch.ElapsedMilliseconds / (double)processedCount:F2} ms");
    }

    private static void GenerateSyntheticImage(int seed, PixelRGB[] buffer)
    {
        var rand = new Random(seed);

        for (int i = 0; i < IMAGE_SIZE; i++)
        {
            buffer[i] = new PixelRGB(
                (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256)
            );
        }
    }

    private static void ApplyBlurFilter(PixelRGB[] input, PixelRGB[] output)
    {
        for (int y = 0; y < IMAGE_HEIGHT - 1; y++)
        {
            for (int x = 0; x < IMAGE_WIDTH - 1; x++)
            {
                int idx = y * IMAGE_WIDTH + x;
                output[idx] = PixelRGB.Average(
                    input[idx],
                    input[idx + 1],
                    input[idx + IMAGE_WIDTH],
                    input[idx + IMAGE_WIDTH + 1]
                );
            }
        }
    }

    private static void SaveImage(PixelRGB[] image, string filename)
    {
        // Simula salvamento
    }
}
