using System;
using System.Numerics;

namespace CAS;

public abstract class Expressao
{
    public abstract override string ToString();
    public abstract Expressao Derivar(Simbolo x);
    public abstract Expressao Simplificar();
    
    public static Expressao operator +(Expressao a, Expressao b) => new Soma(a, b).Simplificar();
    public static Expressao operator -(Expressao a, Expressao b) => new Subtracao(a, b).Simplificar();
    public static Expressao operator *(Expressao a, Expressao b) => new Multiplicacao(a, b).Simplificar();
    public static Expressao operator /(Expressao a, Expressao b) => new Divisao(a, b).Simplificar();
    
    public static implicit operator Expressao(double v) => new Numero(v);
    public static implicit operator Expressao(Complex v) => new NumeroComplexo(v);
    public static implicit operator Expressao(string s) => new Simbolo(s);
}

// numero real
public class Numero : Expressao
{
    public double valor;
    public Numero(double v) => this.valor = v;
    public override string ToString() => valor.ToString();
    public override Expressao Derivar(Simbolo x) => new Numero(0);
    public override Expressao Simplificar() => this;
}

// numero complexo
public class NumeroComplexo : Expressao
{
    public Complex valor;
    public NumeroComplexo(Complex v) => this.valor = v;
    public override string ToString() => $"{valor.Real} + {valor.Imaginary}i";
    public override Expressao Derivar(Simbolo x) => new NumeroComplexo(Complex.Zero);
    public override Expressao Simplificar() => this;
}

// simbolo
public class Simbolo : Expressao
{
    public string Nome;
    public Simbolo(string nome) => Nome = nome;
    public override string ToString() => Nome;
    public override Expressao Derivar(Simbolo x) => Nome == x.Nome ? new Numero(1) : new Numero(0);
    public override Expressao Simplificar() => this;
    
    // Método de substituição
    public Expressao Substituir(string antigo, Expressao novoValor)
    {
        return Nome == antigo ? novoValor : this;
    }
}

// operações
public class Soma : Expressao
{
    public Expressao Esq, Dir;
    public Soma(Expressao esq, Expressao dir) { Esq = esq; Dir = dir; }
    public override string ToString() => $"({Esq} + {Dir})";
    public override Expressao Derivar(Simbolo x) => new Soma(Esq.Derivar(x), Dir.Derivar(x));
    
    public override Expressao Simplificar()
    {
        if (Esq is Numero n1 && n1.valor == 0) return Dir;
        if (Dir is Numero n2 && n2.valor == 0) return Esq;
        return this;
    }
}

public class Subtracao : Expressao
{
    Expressao a, b; // a - b
    public Subtracao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} - {b.ToString()})";
    public override Expressao Derivar(Simbolo x) => 
        new Subtracao(a.Derivar(x), b.Derivar(x));
    public override Expressao Simplificar()
    {
        if(a is Numero numA && b is Numero numB)
        {
            return new Numero(numA.valor - numB.valor);
        }
        return this;
    }
}

public class Multiplicacao : Expressao
{
    Expressao a, b; // a * b
    public Multiplicacao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} * {b.ToString()})";
    public override Expressao Derivar(Simbolo x) =>
        new Soma(
            new Multiplicacao(a.Derivar(x), b),
            new Multiplicacao(a, b.Derivar(x)));
    public override Expressao Simplificar()
    {
        if (a is Numero numA && b is Numero numB)
        {
            return new Numero(numA.valor * numB.valor);
        }
        return this;
    }
}

public class Divisao : Expressao
{
    Expressao a, b; // a / b
    public Divisao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public override string ToString() => $"({a.ToString()} / {b.ToString()})";
    public override Expressao Derivar(Simbolo x) =>
        new Divisao(
            new Subtracao(
                new Multiplicacao(a.Derivar(x), b), 
                new Multiplicacao(a, b.Derivar(x))),
            new Multiplicacao(b, b));
    public override Expressao Simplificar()
    {
        return this;
    }
}
