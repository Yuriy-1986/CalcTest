namespace CalcTest
{
  public abstract class Operator
  {
    int _priority;
    public int Priority { get { return _priority; } }

    string _tokenString;
    public string TokenString { get { return _tokenString; } }

    public Operator(string tokenString, int priority)
    {
      _priority = priority;
      _tokenString = tokenString;
    }
  }
}