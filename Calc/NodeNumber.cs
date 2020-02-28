namespace CalcTest
{
  public class NodeNumber : Node
  {
    public NodeNumber(double number)
    {
      _number = number;
    }

    double _number;

    public override double Eval()
    {
      return _number;
    }
  }
}