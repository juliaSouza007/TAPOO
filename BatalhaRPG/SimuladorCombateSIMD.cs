using System;
using System.Numerics;

namespace BatalhaRPG
{
    public class SimuladorCombateSIMD
    {
        private static Random gerador = new Random(42);

        // Gera um array de aleatórios fora do loop principal
        private static void PreencherAleatorios(int[] destino)
        {
            for (int i = 0; i < destino.Length; i++)
                destino[i] = gerador.Next(0, 100);
        }

        public static int CalcularDanoVetorizado(ExercitoSIMD atacantes, ExercitoSIMD defensores)
        {
            int tamanho = atacantes.Ataques.Length;
            int tamanhoVetor = Vector<int>.Count;
            int danoTotal = 0;

            // Pré-cálculo dos aleatórios
            int[] aleatorios = new int[tamanho];
            PreencherAleatorios(aleatorios);

            Vector<int> vetorUm = Vector<int>.One;
            Vector<int> vetorCem = new Vector<int>(100);
            int limite = tamanho - (tamanho % tamanhoVetor);

            for (int i = 0; i < limite; i += tamanhoVetor)
            {
                var ataque = new Vector<int>(atacantes.Ataques, i);
                var defesa = new Vector<int>(defensores.Defesas, i);
                var chanceCrit = new Vector<int>(atacantes.ChancesCritico, i);
                var multCrit = new Vector<int>(atacantes.MultCriticos, i);
                var vidaAtac = new Vector<int>(atacantes.Vidas, i);
                var vidaDef = new Vector<int>(defensores.Vidas, i);
                var aleat = new Vector<int>(aleatorios, i);

                // Dano base
                var danoBase = Vector.Max(Vector.Subtract(ataque, defesa), vetorUm);

                // Máscaras de vivos
                var maskAtacVivo = Vector.GreaterThan(vidaAtac, Vector<int>.Zero);
                var maskDefVivo = Vector.GreaterThan(vidaDef, Vector<int>.Zero);
                var maskVivos = Vector.BitwiseAnd(maskAtacVivo, maskDefVivo);

                // Crítico
                var maskCrit = Vector.LessThan(aleat, chanceCrit);
                var danoCrit = Vector.Divide(Vector.Multiply(danoBase, multCrit), vetorCem);
                var danoComCrit = Vector.ConditionalSelect(maskCrit, danoCrit, danoBase);

                // Aplica vivos
                var danoFinal = Vector.ConditionalSelect(maskVivos, danoComCrit, Vector<int>.Zero);

                danoTotal += Vector.Dot(danoFinal, vetorUm);
            }

            // Loop escalar para o resto
            for (int i = limite; i < tamanho; i++)
            {
                if (atacantes.Vidas[i] > 0 && defensores.Vidas[i] > 0)
                {
                    int danoBase = Math.Max(1, atacantes.Ataques[i] - defensores.Defesas[i]);
                    bool critico = aleatorios[i] < atacantes.ChancesCritico[i];
                    int dano = critico ? (danoBase * atacantes.MultCriticos[i]) / 100 : danoBase;
                    danoTotal += dano;
                }
            }

            return danoTotal;
        }
    }
}
