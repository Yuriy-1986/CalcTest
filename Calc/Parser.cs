using System;
using System.IO;

namespace CalcTest
{
  public class Parser
  {
    public Parser(Tokenizer tokenizer)
    {
      _tokenizer = tokenizer;
    }

    Tokenizer _tokenizer;

    public Node ParseExpression()
    {
      Node expr = ParseAddSubtract();

      if (_tokenizer.Token != Token.EOF) throw new SyntaxException("Unexpected characters at the end of expression");
      return expr;
    }

    Node ParseAddSubtract()
    {
      Node lhs = ParseMultiplyDivide();

      while (true)
      {
        Func<double, double, double> op = null;
        if (_tokenizer.Token == Token.Add)
        {
          op = (a, b) => a + b;
        }
        else if (_tokenizer.Token == Token.Subtract)
        {
          op = (a, b) => a - b;
        }

        if (op == null) return lhs;

        _tokenizer.NextToken();

        Node rhs = ParseMultiplyDivide();

        lhs = new NodeBinary(lhs, rhs, op);
      }
    }

    Node ParseMultiplyDivide()
    {
      Node lhs = ParseUnary();

      while (true)
      {
        Func<double, double, double> op = null;
        if (_tokenizer.Token == Token.Multiply)
        {
          op = (a, b) => a * b;
        }
        else if (_tokenizer.Token == Token.Divide)
        {
          op = (a, b) => a / b;
        }

        if (op == null) return lhs;

        _tokenizer.NextToken();

        Node rhs = ParseUnary();

        lhs = new NodeBinary(lhs, rhs, op);
      }
    }

    Node ParseUnary()
    {
      if (_tokenizer.Token == Token.Add)
      {
        _tokenizer.NextToken();
        return ParseUnary();
      }

      if (_tokenizer.Token == Token.Subtract)
      {
        _tokenizer.NextToken();
        Node rhs = ParseUnary();

        return new NodeUnary(rhs, (a) => -a);
      }

      return ParseLeaf();
    }

    Node ParseLeaf()
    {
      if (_tokenizer.Token == Token.Number)
      {
        NodeNumber node = new NodeNumber(_tokenizer.Number);
        _tokenizer.NextToken();
        return node;
      }

      if (_tokenizer.Token == Token.OpenParens)
      {
        _tokenizer.NextToken();

        Node node = ParseAddSubtract();

        if (_tokenizer.Token != Token.CloseParens) throw new SyntaxException("Missing close parenthesis");
        _tokenizer.NextToken();

        return node;
      }

      throw new SyntaxException($"Unexpected token: {_tokenizer.Token}");
    }

    #region Helpers
    public static Node Parse(string str)
    {
      return Parse(new Tokenizer(new StringReader(str)));
    }

    public static Node Parse(Tokenizer tokenizer)
    {
      Parser parser = new Parser(tokenizer);
      return parser.ParseExpression();
    }
    #endregion
  }
}