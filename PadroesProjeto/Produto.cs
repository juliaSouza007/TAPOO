public abstract class Produto
{
	public string? Nome { get; set; }
	public decimal Preco { get; set; }

	public abstract string ObterCategoria();     // Categoria do produto (ex: Eletrônicos, Roupas)
	public abstract decimal CalcularFrete();     // Método específico de cálculo de frete
}

// Produto do tipo eletrônico
public class Eletronico : Produto
{
	public Eletronico(string nome, decimal preco)
	{
		Nome = nome;
		Preco = preco;
	}

	public override string ObterCategoria() => "Eletrônicos";

	public override decimal CalcularFrete() => Preco * 0.05m;  // 5% do preço como frete
}

// Produto do tipo roupa
public class Roupa : Produto
{
	public string Tamanho { get; set; }

	public Roupa(string nome, decimal preco, string tamanho)
	{
		Nome = nome;
		Preco = preco;
		Tamanho = tamanho;
	}

	public override string ObterCategoria() => "Roupas";

	public override decimal CalcularFrete() => 12.50m; // Frete fixo
}

// Produto do tipo livro
public class Livro : Produto
{
	public string Autor { get; set; }
	public int NumeroPaginas { get; set; }

	public Livro(string nome, decimal preco, string autor, int numeroPaginas)
	{
		Nome = nome;
		Preco = preco;
		Autor = autor;
		NumeroPaginas = numeroPaginas;
	}

	public override string ObterCategoria() => "Livros";

	public override decimal CalcularFrete() =>
		NumeroPaginas > 300 ? 8.0m : 5.0m; // Frete baseado na quantidade de páginas
}

// Fábrica abstrata que define o método de criação de produtos
public abstract class FabricaProduto
{
	public abstract Produto CriarProduto(string nome, decimal preco);
}

// Fábrica para eletrônicos
public class FabricaEletronicos : FabricaProduto
{
	public override Produto CriarProduto(string nome, decimal preco)
	{
		return new Eletronico(nome, preco);
	}
}

// Fábrica para roupas
public class FabricaRoupa : FabricaProduto
{
	// Criação com tamanho padrão
	public override Produto CriarProduto(string nome, decimal preco)
	{
		return new Roupa(nome, preco, "M");
	}

	// Sobrecarga para criar com tamanho específico
	public Produto CriarProduto(string nome, decimal preco, string tamanho)
	{
		return new Roupa(nome, preco, tamanho);
	}
}

// Fábrica para livros
public class FabricaLivro : FabricaProduto
{
	// Criação com valores padrão
	public override Produto CriarProduto(string nome, decimal preco)
	{
		return new Livro(nome, preco, "Desconhecido", 100);
	}

	// Sobrecarga para criar livro com autor e páginas personalizados
	public Produto CriarProduto(string nome, decimal preco, string autor, int numeroPaginas)
	{
		return new Livro(nome, preco, autor, numeroPaginas);
	}
}