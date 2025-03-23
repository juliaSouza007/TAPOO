using ConsoleDump;

var cotas = CotaParlamentar.LerCotasParlamentares("cota_parlamentar.csv").ToList();
cotas.OrderByDescending(c => c.ValorLiquido).Take(10).ToArray().Dump();

Console.WriteLine("1) Total gasto por partido");
var gastoPorPartido = cotas.GroupBy(c => c.Partido)
    .Select(g => new { Partido = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .OrderByDescending(g => g.TotalGasto);
gastoPorPartido.Dump();

Console.WriteLine("2) Deputados com maior gasto individual (top 5)");
var top5Deputados = cotas.GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Deputado = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .OrderByDescending(g => g.TotalGasto)
    .Take(5);
top5Deputados.Dump();

Console.WriteLine("3) Gasto medio por mes");
var gastoPorMes = cotas.GroupBy(c => c.DataEmissao?.Month)
    .Select(g => new { Mes = g.Key, GastoMedio = g.Average(c => c.ValorLiquido ?? 0) })
    .OrderBy(g => g.Mes);
gastoPorMes.Dump();

Console.WriteLine("4) Total gasto em alimentacao por deputado");
var gastoAlimentacao = cotas.Where(c => c.Descricao != null && c.Descricao.Contains("ALIMENTAÇÃO"))
    .GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Deputado = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) });
gastoAlimentacao.Dump();

Console.WriteLine("5) Lista de fornecedores mais utilizados");
var fornecedoresMaisUsados = cotas.GroupBy(c => c.Fornecedor)
    .Select(g => new { Fornecedor = g.Key, Quantidade = g.Count() })
    .OrderByDescending(g => g.Quantidade);
fornecedoresMaisUsados.Dump();

Console.WriteLine("6) Gasto total por UF");
var gastoPorUF = cotas.GroupBy(c => c.UF)
    .Select(g => new { UF = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .OrderByDescending(g => g.TotalGasto);
gastoPorUF.Dump();

Console.WriteLine("7) Meses com maior numero de documentos emitidos");
var mesesMaisDocumentos = cotas.GroupBy(c => c.DataEmissao?.Month)
    .Select(g => new { Mes = g.Key, Quantidade = g.Count() })
    .OrderByDescending(g => g.Quantidade);
mesesMaisDocumentos.Dump();

Console.WriteLine("8) Deputados com despesas acima de R$ 10.000,00");
var deputadosAcima10000 = cotas.GroupBy(c => c.NomeParlamentar)
    .Select(g => new { Deputado = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .Where(g => g.TotalGasto > 10000)
    .OrderByDescending(g => g.TotalGasto);
deputadosAcima10000.Dump();

Console.WriteLine("9) Total gasto por tipo de despesa");
var gastoPorDescricao = cotas.GroupBy(c => c.Descricao)
    .Select(g => new { TipoDespesa = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .OrderByDescending(g => g.TotalGasto);
gastoPorDescricao.Dump();

Console.WriteLine("10) Total de gastos por ano");
var gastoPorAno = cotas.GroupBy(c => c.Ano)
    .Select(g => new { Ano = g.Key, TotalGasto = g.Sum(c => c.ValorLiquido ?? 0) })
    .OrderByDescending(g => g.Ano);
gastoPorAno.Dump();