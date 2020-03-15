using System;

namespace CalcTest
{
  public class OperatorBinary : Operator
  {
    protected Associativity _associativity;
    public Associativity Associativity { get { return _associativity; } }

    Func<double, double, double> _op;
    public Func<double, double, double> Op { get { return _op; } }

    public OperatorBinary(string tokenString, int priority, Associativity associativity, Func<double, double, double> op) : base (tokenString, priority)
    {
      _associativity = associativity;
      _op = op;
    }
  }
}