public abstract class DecoradorProduto : Produto
{
    // Produto que está sendo decorado. Usado para acessar os métodos da classe Produto original.
    protected Produto _produto;
   
    // Construtor que recebe o produto a ser decorado e o armazena.
    public DecoradorProduto(Produto produto)
    {
        _produto = produto;
    }
   
    // Métodos que são sobrescritos. Delegam as chamadas para o produto decorado.
    public override string ObterCategoria() => _produto.ObterCategoria();
    public override decimal CalcularFrete() => _produto.CalcularFrete();
}

// Decorador para adicionar garantia ao produto. 
public class DecoradorGarantia : DecoradorProduto
{
    // Número de meses de garantia que o produto ganha.
    private int _mesesGarantia;

    // Construtor que recebe o produto e o número de meses de garantia.
    public DecoradorGarantia(Produto produto, int mesesGarantia) : base(produto)
    {
        _mesesGarantia = mesesGarantia;
        
        // Preço é aumentado de acordo com os meses de garantia. 
        // Exemplo: R$10 por cada mês adicional.
        Preco = produto.Preco + (mesesGarantia * 10);
    }

    // Método que delega a chamada à categoria do produto base.
    public override string ObterCategoria() => _produto.ObterCategoria();

    // Método que delega a chamada ao cálculo de frete do produto base.
    public override decimal CalcularFrete() => _produto.CalcularFrete();
}

// Decorador para adicionar frete expresso ao produto.
public class DecoradorFreteExpresso : DecoradorProduto
{
    // Construtor que recebe o produto e aplica o preço do frete expresso.
    public DecoradorFreteExpresso(Produto produto) : base(produto) 
    {
        Preco = produto.Preco + 20; // Adiciona R$20 ao preço do produto para o frete expresso.
    }
    
    // Sobrescreve o cálculo do frete para adicionar R$15 ao frete original.
    public override decimal CalcularFrete() => _produto.CalcularFrete() + 15;

    // Modifica a categoria para indicar que o produto tem frete expresso.
    public override string ObterCategoria() => base.ObterCategoria() + " + Frete Expresso";
}

// Decorador para adicionar embalagem especial ao produto.
public class DecoradorEmbalagem : DecoradorProduto
{
    // Construtor que recebe o produto e aplica o custo adicional da embalagem.
    public DecoradorEmbalagem(Produto produto) : base(produto) 
    {
        Preco = produto.Preco + 5; // Adiciona R$5 ao preço do produto para embalagem especial.
    }
    
    // Modifica a categoria do produto para indicar que ele tem embalagem especial.
    public override string ObterCategoria() => base.ObterCategoria() + " + Embalagem Especial";
}
