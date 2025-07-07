namespace BatalhaRPG;

public class SimuladorCombate
{
    private static Random gerador = new Random(42);

    public static int CalcularDano(Personagem atacante, Personagem defensor)
    {
        if (!atacante.Vivo || !defensor.Vivo)
            return 0;

        int danoBase = Math.Max(1, atacante.Ataque - defensor.Defesa);
        bool ehCritico = gerador.Next(0, 100) < atacante.ChanceCritico;

        if (ehCritico)
        {
            danoBase = (danoBase * atacante.MultCritico) / 100;
        }

        return danoBase;
    }

    public static int SimularRodadaCombate(Personagem[] atacantes, Personagem[] defensores)
    {
        int danoTotal = 0;

        for (int i = 0; i < atacantes.Length && i < defensores.Length; i++)
        {
            danoTotal += CalcularDano(atacantes[i], defensores[i]);
        }

        return danoTotal;
    }

    public static Personagem[] GerarExercito(int tamanho, string tipo)
    {
        Personagem[] exercito = new Personagem[tamanho];

        for (int i = 0; i < tamanho; i++)
        {
            if (tipo == "atacante")
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(80, 120),
                    Defesa = gerador.Next(20, 40),
                    ChanceCritico = gerador.Next(15, 25),
                    MultCritico = gerador.Next(180, 220),
                    Vida = gerador.Next(100, 150)
                };
            }
            else
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(60, 80),
                    Defesa = gerador.Next(40, 70),
                    ChanceCritico = gerador.Next(10, 20),
                    MultCritico = gerador.Next(150, 200),
                    Vida = gerador.Next(120, 180)
                };
            }
        }

        return exercito;
    }
}
