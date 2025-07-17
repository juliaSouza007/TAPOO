public class SistemaECommerce
{
	public static void Main()
	{
		var configuracao = GerenciadorConfiguracao.Instancia;

		// FACTORY
		var fabricaEletronicos = new FabricaEletronicos();
		var tablet = fabricaEletronicos.CriarProduto("Tablet Samsung", 1142.99m);

		var fabricaRoupas = new FabricaRoupa();
		var camiseta = fabricaRoupas.CriarProduto("Camiseta Preta", 49.90m, "PP");
		var calca = fabricaRoupas.CriarProduto("Calça Cargo", 129.90m, "M");

		var fabricaLivros = new FabricaLivro();
		var livro = fabricaLivros.CriarProduto("A gente mira no amor e acerta na solidão", 34.93m, "Ana Suy", 160);

		// PRODUTOS CRIADOS (AZUL)
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("-- PRODUTOS CRIADOS: --");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
		ExibirProduto(tablet);
		ExibirProduto(camiseta);
		ExibirProduto(calca);
		ExibirProduto(livro);
		Console.ResetColor();

		// DECORADORES
		var tabletComGarantia = new DecoradorGarantia(tablet, 12);
		var camisetaComEmbalagem = new DecoradorEmbalagem(camiseta);
		var calcaComFreteExpresso = new DecoradorFreteExpresso(calca);
		var livroComFreteExpresso = new DecoradorFreteExpresso(livro);

		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("-- PRODUTOS COM DECORADORES: --");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Blue;
		ExibirProdutoDecorado(tablet, tabletComGarantia);
		ExibirProdutoDecorado(camiseta, camisetaComEmbalagem);
		ExibirProdutoDecorado(calca, calcaComFreteExpresso);
		ExibirProdutoDecorado(livro, livroComFreteExpresso);
		Console.ResetColor();

		// OBSERVER
		var pedido = new Pedido();
		pedido.Inscrever(new NotificadorEmail());
		pedido.Inscrever(new NotificadorSMS());

		// STRATEGY - PAGAMENTO CARTÃO
		var contextoPagamento = new ContextoPagamento();
		var pagamento = new PagamentoCartaoCredito { NumeroCartao = "1234567890123456" };
		contextoPagamento.DefinirEstrategiaPagamento(pagamento);

		Console.WriteLine("Processando pagamento...");
		var sucesso = contextoPagamento.ExecutarPagamento(tabletComGarantia.Preco);
		if (sucesso)
		{
            Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Pagamento aprovado com sucesso!");
            Console.ResetColor();
			Console.WriteLine(pagamento.ObterDetalhesPagamento() + "\n");

			AlterarStatusPedido(pedido);
		}

		// STRATEGY - PAGAMENTO PAYPAL
		var pagamentoPaypal = new PaymentPayPal { Email = "julinha@gmail.com" };
		contextoPagamento.DefinirEstrategiaPagamento(pagamentoPaypal);

		Console.WriteLine("Processando pagamento...");
		sucesso = contextoPagamento.ExecutarPagamento(camisetaComEmbalagem.Preco);
		if (sucesso)
		{
			Console.WriteLine("Pagamento aprovado com sucesso!");
			Console.WriteLine(pagamentoPaypal.ObterDetalhesPagamento() + "\n");

			AlterarStatusPedido(pedido);
		}

		// STRATEGY - PAGAMENTO PIX
		var pagamentoPix = new PagamentoPix { Chave = "31950500505" };
		contextoPagamento.DefinirEstrategiaPagamento(pagamentoPix);

		Console.WriteLine("Processando pagamento...");
		sucesso = contextoPagamento.ExecutarPagamento(livroComFreteExpresso.Preco);
		if (sucesso)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Pagamento aprovado com sucesso!");
            Console.ResetColor();
			Console.WriteLine(pagamentoPix.ObterDetalhesPagamento() + "\n");

			AlterarStatusPedido(pedido);
		}
	}

	// Método auxiliar para exibir produto comum
	private static void ExibirProduto(Produto produto)
	{
		Console.WriteLine($"Nome: {produto.Nome}");
		Console.WriteLine($"Categoria: {produto.ObterCategoria()}");
		Console.WriteLine($"Preço: R$ {produto.Preco:F2}\n");
	}

	// Método auxiliar para exibir produto decorado
	private static void ExibirProdutoDecorado(Produto original, Produto decorado)
	{
		Console.WriteLine($"Nome: {original.Nome}");
		Console.WriteLine($"Preço com extras: R$ {decorado.Preco:F2}");
		Console.WriteLine($"Frete calculado: R$ {decorado.CalcularFrete():F2}\n");
	}

	// Método auxiliar para simular mudança de status com cor verde
	private static void AlterarStatusPedido(Pedido pedido)
	{
		var statusList = new[] { "Confirmado", "Em transporte", "Entregue" };

		foreach (var status in statusList)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"[Status atualizado] Pedido agora está: {status}\n");
			Console.ResetColor();

			pedido.Status = status;
		}
	}
}