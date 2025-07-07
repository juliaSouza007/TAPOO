namespace BatalhaRPG;

public class Personagem
{
    public int Ataque;
    public int Defesa;
    public int ChanceCritico; // 0-100
    public int MultCritico;   // multiplicador de crítico (ex: 200 = 2x)
    public int Vida;
    public bool Vivo => Vida > 0;
}