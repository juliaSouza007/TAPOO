public interface IObservadorPedido
{
	void AoMudarStatusPedido(Pedido pedido, string novoStatus);
}

// Classe principal que representa um pedido observável.
public class Pedido
{
	private List<IObservadorPedido> _listaObservadores = new List<IObservadorPedido>();
	private string? _statusAtual;

	// Propriedade que define o status do pedido e notifica os observadores ao alterá-lo.
	public string Status
	{
		get => _statusAtual;
		set
		{
			_statusAtual = value;
			NotificarObservadores();
		}
	}

	// Inscreve um novo observador.
	public void Inscrever(IObservadorPedido observador)
	{
		_listaObservadores.Add(observador);
	}

	// Notifica todos os observadores da mudança de status.
	private void NotificarObservadores()
	{
		foreach (var observador in _listaObservadores)
		{
			observador.AoMudarStatusPedido(this, _statusAtual);
		}
	}
}

public class NotificadorEmail : IObservadorPedido
{
	public void AoMudarStatusPedido(Pedido pedido, string novoStatus)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("Enviando e‑mail...");
		Console.ResetColor();

		Console.WriteLine($"Status alterado para {novoStatus}.");
		Console.WriteLine("E‑mail enviado!\n");
	}
}

public class NotificadorSMS : IObservadorPedido
{
	public void AoMudarStatusPedido(Pedido pedido, string novoStatus)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("Enviando SMS...");
		Console.ResetColor();

		Console.WriteLine($"Seu pedido agora está {novoStatus}.");
		Console.WriteLine("SMS enviado!\n");
	}
}
