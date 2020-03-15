namespace CalcTest
{
  public interface ITokenizer
  {
    void NextToken();
    string Token { get; }

    TokenType TokenType { get; }

    double Number { get; }
  }
}
