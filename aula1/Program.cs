using CAS;
using System.Numerics;

Expressao a = 10;
Expressao b = "b";

Expressao soma = a + b;
Expressao c = 50;

Console.WriteLine(a+c); // 10 + 50

//implementacao complexos
Expressao real = new Numero(5);
Expressao complexo = new NumeroComplexo(new Complex(6, 4));

Console.WriteLine(real);     // 5
Console.WriteLine(complexo); // 6 + 4i

//substituicao de simbolos
Simbolo x = new Simbolo("x");
Expressao expr = new Soma(x, new Numero(10)); // x + 10

Expressao resultado = x.Substituir("x", new Numero(21));

Console.WriteLine(resultado); // 21

//simplificacao
Expressao expr1 = new Soma(new Numero(0), new Simbolo("x"));
Expressao simplificada1 = expr1.Simplificar();

Console.WriteLine(simplificada1); // x