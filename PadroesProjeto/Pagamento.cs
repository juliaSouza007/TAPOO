public interface IEstrategiaPagamento
{
    bool ProcessarPagamento(decimal valor);
    string ObterDetalhesPagamento();
}

public class ContextoPagamento
{
    public IEstrategiaPagamento? _estrategiaPagamento;

    public void DefinirEstrategiaPagamento(IEstrategiaPagamento estrategia)
    {
        _estrategiaPagamento = estrategia;
    }

    public bool ExecutarPagamento(decimal valor)
    {
        return _estrategiaPagamento?.ProcessarPagamento(valor) ?? false;
    }
}

public class PagamentoCartaoCredito : IEstrategiaPagamento
{
    public string? NumeroCartao { get; set; }
    public string? NomeTitular { get; set; }

    public bool ProcessarPagamento(decimal valor)
    {
        // Pagamento aceito apenas se o valor for maior que 0 e menor que 5000
        return valor > 0 && valor < 5000;
    }

    public string ObterDetalhesPagamento()
    {
        // Exibe os últimos 4 dígitos do número do cartão
        return $"Cartão de Crédito\n**** {NumeroCartao.Substring(NumeroCartao.Length - 4)}";
    }
}

public class PaymentPayPal : IEstrategiaPagamento
{
    public string? Email { get; set; }

    public bool ProcessarPagamento(decimal valor)
    {
        // Pagamento aceito apenas se o valor for positivo e o e-mail do PayPal estiver definido
        return (valor > 0 && !string.IsNullOrEmpty(Email));
    }

    public string ObterDetalhesPagamento()
    {
        return $"PayPal\nEmail: {Email}";
    }
}

public class PagamentoPix : IEstrategiaPagamento
{
    public string? Chave { get; set; }

    public bool ProcessarPagamento(decimal valor)
    {
        // Pagamento aceito se o valor for maior que 0
        return valor > 0;
    }

    public string ObterDetalhesPagamento()
    {
        return $"PIX\nChave: {Chave}";
    }
}
