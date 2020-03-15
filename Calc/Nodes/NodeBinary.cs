using System;

namespace CalcTest
{
  public class NodeBinary : Node
  {
    public NodeBinary(Node lhs, Node rhs, Func<double, double, double> op)
    {
      _lhs = lhs;
      _rhs = rhs;
      _op = op;
    }

    Node _lhs;
    Node _rhs;
    Func<double, double, double> _op;

    public override double Eval()
    {
      double lhsVal = _lhs.Eval();
      double rhsVal = _rhs.Eval();

      double result = _op(lhsVal, rhsVal);
      return result;
    }
  }
}