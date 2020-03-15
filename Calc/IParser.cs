namespace CalcTest
{
  public interface IParser
  {
    public void AddOperator(Operator op);
    public Node ParseExpression(string expr);
  }
}
