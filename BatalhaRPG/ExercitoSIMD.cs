namespace BatalhaRPG
{
public class ExercitoSIMD
{
    public int[] Ataques;
    public int[] Defesas;
    public int[] ChancesCritico;
    public int[] MultCriticos;
    public int[] Vidas;
    public bool[] Vivos;

    public ExercitoSIMD(int tamanho)
    {
        Ataques = new int[tamanho];
        Defesas = new int[tamanho];
        ChancesCritico = new int[tamanho];
        MultCriticos = new int[tamanho];
        Vidas = new int[tamanho];
        Vivos = new bool[tamanho];
    }

    public void ConverterDePersonagens(Personagem[] personagens)
    {
        for (int i = 0; i < personagens.Length; i++)
        {
            Ataques[i] = personagens[i].Ataque;
            Defesas[i] = personagens[i].Defesa;
            ChancesCritico[i] = personagens[i].ChanceCritico;
            MultCriticos[i] = personagens[i].MultCritico;
            Vidas[i] = personagens[i].Vida;
            Vivos[i] = personagens[i].Vivo;
        }
    }
}
}
