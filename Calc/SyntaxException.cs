using System;

namespace CalcTest
{
  public class SyntaxException : Exception
  {
    public SyntaxException(string message) : base(message)
    {
    }
  }
}