using System;
using System.Diagnostics;
using BatalhaRPG;

namespace BatalhaRPG;

class Program
{
    static void Main()
    {
        const int tamanhoExercito = 1_000_000;

        Personagem[] atacantes = SimuladorCombate.GerarExercito(tamanhoExercito, "atacante");
        Personagem[] defensores = SimuladorCombate.GerarExercito(tamanhoExercito, "defensor");

        Console.WriteLine("=== SIMULAÇÃO DE BATALHA ===");
        Console.WriteLine($"Exércitos: {tamanhoExercito:N0} vs {tamanhoExercito:N0}");

        Stopwatch cronometro = Stopwatch.StartNew();
        int danoSemSIMD = SimuladorCombate.SimularRodadaCombate(atacantes, defensores);
        cronometro.Stop();

        Console.WriteLine($"[SEM SIMD] Dano total causado: {danoSemSIMD:N0}");
        Console.WriteLine($"[SEM SIMD] Tempo: {cronometro.ElapsedMilliseconds}ms");
        Console.WriteLine($"[SEM SIMD] DPS: {danoSemSIMD * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");

        var exercitoAtacantesSIMD = new ExercitoSIMD(atacantes.Length);
        exercitoAtacantesSIMD.ConverterDePersonagens(atacantes);

        var exercitoDefensoresSIMD = new ExercitoSIMD(defensores.Length);
        exercitoDefensoresSIMD.ConverterDePersonagens(defensores);

        cronometro = Stopwatch.StartNew();
        int danoComSIMD = SimuladorCombateSIMD.CalcularDanoVetorizado(exercitoAtacantesSIMD, exercitoDefensoresSIMD);
        cronometro.Stop();

        Console.WriteLine($"[COM SIMD] Dano total causado: {danoComSIMD:N0}");
        Console.WriteLine($"[COM SIMD] Tempo: {cronometro.ElapsedMilliseconds}ms");
        Console.WriteLine($"[COM SIMD] DPS: {danoComSIMD * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");
    }
}
