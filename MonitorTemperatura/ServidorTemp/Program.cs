using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Rota GET: /temperatura/{unidade}
app.MapGet("/temperatura/{unidade}", (string unidade) =>
{
    // 1. Calcular horário atual em horas decimais (ex: 14.5 = 14h30m)
    double t = DateTime.Now.TimeOfDay.TotalHours;

    // 2. Calcular temperatura base em Celsius (25 + 5 * senoidal diária) + ruído
    double tempCBase = 25.0 + 5.0 * Math.Sin((2.0 * Math.PI / 24.0) * t);
    double ruido = Random.Shared.NextDouble();  // Ruído entre 0 e 1
    double tempC = tempCBase + ruido;

    // 3. Converter para unidade solicitada
    double resultado;
    string unidadeFormatada = unidade.ToLower();

    if (unidadeFormatada == "kelvin")
    {
        resultado = tempC + 273.15;
    }
    else if (unidadeFormatada == "fahrenheit")
    {
        resultado = tempC * 9.0 / 5.0 + 32.0;
    }
    else if (unidadeFormatada == "celsius")
    {
        resultado = tempC;
    }
    else
    {
        // Unidade inválida
        return Results.BadRequest(new { erro = "Unidade inválida. Use celsius, kelvin ou fahrenheit." });
    }

    // 4. Retornar JSON
    return Results.Ok(new
    {
        unidade = unidadeFormatada,
        valor = Math.Round(resultado, 2)
    });
});

app.Run();