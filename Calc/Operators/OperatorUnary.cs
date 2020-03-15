using System;

namespace CalcTest
{
  public class OperatorUnary : Operator
  {
    Func<double, double> _op;
    public Func<double, double> Op { get { return _op; } }

    public OperatorUnary (string tokenString, int priority, Func<double, double> op) : base (tokenString, priority)
    {
      _op = op;
    }
  }
}