using System;
using System.Threading;

class Program
{
    static void Main()
    {
        int numFilosofos = 5;
        Filosofo[] filosofos = new Filosofo[numFilosofos];
        SemaphoreSlim[] garfos = new SemaphoreSlim[numFilosofos];
        SemaphoreSlim garcom = new SemaphoreSlim(numFilosofos - 1); // evita deadlock

        for (int i = 0; i < numFilosofos; i++)
            garfos[i] = new SemaphoreSlim(1, 1);

        for (int i = 0; i < numFilosofos; i++)
        {
            filosofos[i] = new Filosofo(i, garfos, garcom);
            new Thread(filosofos[i].Viver).Start();
        }
    }
}

class Filosofo
{
    private static int comendoAgora = 0;
    private static object contadorLock = new object();

    private int id;
    private SemaphoreSlim[] garfos;
    private SemaphoreSlim garcom;
    private Random random = new Random();

    public Filosofo(int id, SemaphoreSlim[] garfos, SemaphoreSlim garcom)
    {
        this.id = id;
        this.garfos = garfos;
        this.garcom = garcom;
    }

    public void Viver()
    {
        while (true)
        {
            Pensar();

            garcom.Wait(); // tenta entrar na sala de jantar

            int garfoEsquerdo = id;
            int garfoDireito = (id + 1) % garfos.Length;

            int primeiro = Math.Min(garfoEsquerdo, garfoDireito);
            int segundo = Math.Max(garfoEsquerdo, garfoDireito);

            garfos[primeiro].Wait();
            garfos[segundo].Wait();

            Comer();

            garfos[segundo].Release();
            garfos[primeiro].Release();

            garcom.Release(); // sai da sala de jantar
        }
    }

    private void Pensar()
    {
        Console.ResetColor(); // branco (padrão)
        Console.WriteLine($"Filósofo {id} está pensando...");
        Thread.Sleep(random.Next(1000, 3000));
    }

    private void Comer()
    {
        lock (contadorLock)
        {
            comendoAgora++;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Filósofo {id} está comendo [🐟🥗] | ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Total comendo agora: {comendoAgora}");

            Console.ResetColor();
        }

        Thread.Sleep(random.Next(1000, 2000));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Filósofo {id} terminou de comer.");
        Console.ResetColor();

        lock (contadorLock)
        {
            comendoAgora--;
        }
    }
}